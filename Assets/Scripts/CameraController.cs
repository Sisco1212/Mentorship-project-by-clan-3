using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Transform enemy;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    public float minFOV = 40f; // Minimum field of view (closer to targets)
    public float maxFOV = 60f; // Maximum field of view (farther from targets)
    public float zoomLimiter = 10f; // Controls zoom sensitivity to distance

    private Vector3 originalPosition;
    private bool isShaking = false;

    public float duration = 0.5f; // Duration of the shake
    public float magnitude = 0.1f; // Shake intensity

    public Vector3 ellipseAxes = new Vector3(5f, 1f, 3f); // Semi-major and semi-minor axes of the ellipse
    public float rotationDuration = 6f; // Duration of the rotation in seconds

    private bool isRotatingAroundPlayer = false;

    void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        enemy = GameObject.FindWithTag("Enemy").transform;
    }

    void LateUpdate()
    {
        if (player == null || enemy == null) return;
        if(!GameManager.Instance.fightStarted) return;
        Vector3 centerPoint = GetCenterPoint();
        Vector3 desiredPosition = centerPoint + offset;
        float verticalOffset = CalculateVerticalOffset();
        desiredPosition.y += verticalOffset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
        float distance = Vector3.Distance(player.position, enemy.position);
        float targetFOV = Mathf.Lerp(minFOV, maxFOV, distance / zoomLimiter);
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, targetFOV, smoothSpeed);
    }

    public void StartEllipseRotation()
    {
        if (!isRotatingAroundPlayer && player != null)
        {
            StartCoroutine(EllipseRotationAroundPlayer());
        }
    }

    private IEnumerator EllipseRotationAroundPlayer()
    {
        isRotatingAroundPlayer = true;

        Vector3 initialPosition = player.position + new Vector3(0, ellipseAxes.y, -ellipseAxes.z);
        float elapsedTime = 0f;

        Vector3 lookAtOffset = new Vector3(0f, 1.5f, 0f);

        while (elapsedTime < rotationDuration)
        {

            float angle = (elapsedTime / rotationDuration) * 2 * Mathf.PI;

            Vector3 offset = new Vector3(
                Mathf.Sin(angle) * ellipseAxes.x,
                ellipseAxes.y,
                Mathf.Cos(angle) * ellipseAxes.z
            );

            transform.position = player.position + offset;
            transform.LookAt(player.position + lookAtOffset);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = player.position + new Vector3(0, ellipseAxes.y, -ellipseAxes.z);
        transform.LookAt(player.position + lookAtOffset);

        isRotatingAroundPlayer = false;
    }

    Vector3 GetCenterPoint()
    {
        return (player.position + enemy.position) / 2;
    }

    float CalculateVerticalOffset()
    {
        float lowestY = Mathf.Min(player.position.y, enemy.position.y);
        float offsetNeeded = Mathf.Abs(lowestY - GetCenterPoint().y);
        return offsetNeeded;
    }

    public void StartShake(float mnt, float dur)
    {
        magnitude = mnt;
        duration = dur;
        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        float elapsed = 0.0f;
        if (isShaking)
        {
            transform.position = originalPosition;
        }
        originalPosition = transform.position;
        isShaking = true;
        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }
        transform.localPosition = originalPosition;
        isShaking = false;
    }
}
