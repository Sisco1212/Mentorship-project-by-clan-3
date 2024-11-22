using System.Collections;
using UnityEngine;

public class GestureDetector : MonoBehaviour
{
    public static GestureDetector Instance { get; private set; }

    public float tapMaxDuration = 0.1f; // Max duration to consider a tap
    public float doubleTapMaxInterval = 0.3f; // Max interval for double tap
    public float holdThreshold = 0.1f; // Duration required to consider a hold
    public float swipeThreshold = 50f; // Minimum distance for a swipe
    public float swipeDetectionThreshold = 15f; // Minimum X distance to detect ongoing swipes left/right

    private float leftTapTime, rightTapTime;
    private Vector2 startTouchPos;
    private float startTouchTime;
    private bool leftHoldActive, rightHoldActive;

    // Gesture states
    private bool isDoubleTapLeft, isDoubleTapRight, isSingleTapLeft, isSingleTapRight;
    private bool isHoldLeft, isHoldRight;
    private bool isSwipeUp, isSwipeDown, isSwipeLeft, isSwipeRight;
    private bool isSwipeUpRight, isSwipeUpLeft, isSwipeDownRight, isSwipeDownLeft;
    private bool isSwipingLeft, isSwipingRight;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        ResetGestureStates();

        #if UNITY_EDITOR || UNITY_STANDALONE // Only check mouse input in the editor or standalone build
            DetectMouseInput();
        #endif
        DetectTouches();
    }

    private void ResetGestureStates()
    {
        isDoubleTapLeft = isDoubleTapRight = isSingleTapLeft = isSingleTapRight = false;
        isHoldLeft = isHoldRight = false;
        isSwipeUp = isSwipeDown = isSwipeLeft = isSwipeRight = false;
        isSwipeUpRight = isSwipeUpLeft = isSwipeDownRight = isSwipeDownLeft = false;
        isSwipingLeft = isSwipingRight = false; // Reset swiping states
    }
    private void HandleTap(ref bool isSingleTap, ref float lastTapTime, ref bool isDoubleTap)
    {
        isSingleTap = true;
        /* if (Time.time - lastTapTime <= doubleTapMaxInterval)
        {
            isDoubleTap = true;
        }
        else
        {
            isSingleTap = true;
        } */
        lastTapTime = Time.time;
    }

    private void DetectMouseInput()
{
    if (Input.GetMouseButtonDown(0))
    {
        startTouchPos = Input.mousePosition;
        startTouchTime = Time.time;

        if (Input.mousePosition.x < Screen.width / 2) leftHoldActive = true;
        else rightHoldActive = true;
    }
    else if (Input.GetMouseButton(0))
    {
        Vector2 currentTouchPos = Input.mousePosition;
        DetectOngoingSwipe(currentTouchPos); // Detect real-time swiping

        // Calculate the movement distance
        float distanceMoved = Vector2.Distance(startTouchPos, currentTouchPos);

        // Only consider as hold if the distance moved is below the threshold (indicating no swipe)
        if (distanceMoved < 15f) // Adjust this threshold as necessary
        {
            if (Input.mousePosition.x < Screen.width / 2 && leftHoldActive && Time.time - startTouchTime >= holdThreshold)
                isHoldLeft = true;
            if (Input.mousePosition.x >= Screen.width / 2 && rightHoldActive && Time.time - startTouchTime >= holdThreshold)
                isHoldRight = true;
        }
    }
    else if (Input.GetMouseButtonUp(0))
    {
        float touchDuration = Time.time - startTouchTime;
        Vector2 endTouchPos = Input.mousePosition;

        // Calculate the movement distance
        float distanceMoved = Vector2.Distance(startTouchPos, endTouchPos);

        // Check for single tap only if movement is minimal (not a swipe)
        if (touchDuration <= tapMaxDuration && distanceMoved < 15f)
        {
            if (Input.mousePosition.x < Screen.width / 2) HandleTap(ref isSingleTapLeft, ref leftTapTime, ref isDoubleTapLeft);
            else HandleTap(ref isSingleTapRight, ref rightTapTime, ref isDoubleTapRight);
        }

        // Reset hold states after releasing the button
        leftHoldActive = false;
        rightHoldActive = false;

        // Detect swipe only if the movement exceeded the threshold
        if (distanceMoved >= 15f)
            DetectSwipe(endTouchPos);
    }
}

    private void DetectTouches()
    {
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                Vector2 touchPos = touch.position;
                bool isLeftSide = touchPos.x < Screen.width / 2;
                bool isRightSide = touchPos.x >= Screen.width / 2;

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        startTouchPos = touchPos;
                        startTouchTime = Time.time;

                        if (isLeftSide) leftHoldActive = true;
                        if (isRightSide) rightHoldActive = true;
                        break;

                    case TouchPhase.Moved:
                        // Calculate the movement distance
                        float distanceMoved = Vector2.Distance(startTouchPos, touch.position);

                        // Detect swipe if movement exceeds threshold
                        if (distanceMoved >= 15f)
                        {
                            DetectOngoingSwipe(touch.position);
                        }
                        break;

                    case TouchPhase.Stationary:
                        float timeHeld = Time.time - startTouchTime;
                        // Only consider as hold if the distance moved is below the threshold (indicating no swipe)
                        if (isLeftSide && leftHoldActive && timeHeld >= holdThreshold && Vector2.Distance(startTouchPos, touchPos) < 15f)
                            isHoldLeft = true;
                        if (isRightSide && rightHoldActive && timeHeld >= holdThreshold && Vector2.Distance(startTouchPos, touchPos) < 15f)
                            isHoldRight = true;
                        break;

                    case TouchPhase.Ended:
                        float touchDuration = Time.time - startTouchTime;
                        float endDistanceMoved = Vector2.Distance(startTouchPos, touchPos);

                        // Check for single tap if duration and movement are within threshold (indicating no swipe)
                        if (touchDuration <= tapMaxDuration && endDistanceMoved < 15f)
                        {
                            if (isLeftSide) HandleTap(ref isSingleTapLeft, ref leftTapTime, ref isDoubleTapLeft);
                            if (isRightSide) HandleTap(ref isSingleTapRight, ref rightTapTime, ref isDoubleTapRight);
                        }

                        // Reset hold states after touch ends
                        leftHoldActive = false;
                        rightHoldActive = false;

                        // Detect swipe only if the movement exceeded the threshold
                        if (endDistanceMoved >= 15f)
                            DetectSwipe(touchPos);
                        break;
                }
            }
        }
    }

    private void DetectSwipe(Vector2 endTouchPos)
    {
        float distance = Vector2.Distance(startTouchPos, endTouchPos);
        if (distance >= swipeThreshold)
        {
            Vector2 direction = endTouchPos - startTouchPos;
            float angle = Vector2.SignedAngle(Vector2.up, direction);

            if (angle >= -22.5f && angle < 22.5f) isSwipeUp = true;
            else if (angle >= 22.5f && angle < 67.5f) isSwipeUpRight = true;
            else if (angle >= 67.5f && angle < 112.5f) isSwipeRight = true;
            else if (angle >= 112.5f && angle < 157.5f) isSwipeDownRight = true;
            else if (angle >= 157.5f || angle < -157.5f) isSwipeDown = true;
            else if (angle >= -157.5f && angle < -112.5f) isSwipeDownLeft = true;
            else if (angle >= -112.5f && angle < -67.5f) isSwipeLeft = true;
            else if (angle >= -67.5f && angle < -22.5f) isSwipeUpLeft = true;
        }
    }

    private void DetectOngoingSwipe(Vector2 currentTouchPos)
    {
        Vector2 swipeDelta = currentTouchPos - startTouchPos;

        if (Mathf.Abs(swipeDelta.x) > swipeDetectionThreshold)
        {
            if (swipeDelta.x > 0)
            {
                isSwipingRight = true;
                isSwipingLeft = false;
            }
            else
            {
                isSwipingLeft = true;
                isSwipingRight = false;
            }
        }
    }

    // Exposed functions for external checks
    public bool IsDoubleTapLeft() => isDoubleTapLeft;
    public bool IsDoubleTapRight() => isDoubleTapRight;
    public bool IsSingleTapLeft() => isSingleTapLeft;
    public bool IsSingleTapRight() => isSingleTapRight;
    public bool IsHoldLeft() => isHoldLeft;
    public bool IsHoldRight() => isHoldRight;
    public bool IsTouchingLeft() => leftHoldActive;
    public bool IsTouchingRight() => rightHoldActive;
    public bool IsSwipeUp() => isSwipeUp;
    public bool IsSwipeDown() => isSwipeDown;
    public bool IsSwipeLeft() => isSwipeLeft;
    public bool IsSwipeRight() => isSwipeRight;
    public bool IsSwipeUpRight() => isSwipeUpRight;
    public bool IsSwipeUpLeft() => isSwipeUpLeft;
    public bool IsSwipeDownRight() => isSwipeDownRight;
    public bool IsSwipeDownLeft() => isSwipeDownLeft;

    // New exposed functions for ongoing swipes
    public bool IsSwipingLeft() => isSwipingLeft;
    public bool IsSwipingRight() => isSwipingRight;

    public bool IsSwiping() => isSwipingLeft || isSwipingRight;
}
