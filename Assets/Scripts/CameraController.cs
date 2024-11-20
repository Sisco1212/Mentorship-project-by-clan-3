using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public Transform[] targets;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    private Vector3 originalPosition;

    public float duration = 0.5f; // Duration of the shake
    public float magnitude = 0.1f; // Shake intensity
    private bool isShaking = false;

    void LateUpdate(){
        if(targets == null || targets.Length ==0){
            return;
        }
        Transform activeTarget = FindActiveTarget();
        if(activeTarget == null){
            return;
        }
        Vector3 desiredPosition = activeTarget.position + offset;
        desiredPosition.y = transform.position.y;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }

    Transform FindActiveTarget(){
        foreach(Transform target in targets){
            if(target.gameObject.activeInHierarchy){
                return target;
            }
        }
        return null;
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
        if(isShaking){
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
