using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    Transform cameraTransform;  // ī�޶��� Ʈ������
    public float shakeDuration = 0.5f; // ��鸮�� �ð�
    public float shakeAmount = 0.7f;   // ��鸮�� ����
    public float decreaseFactor = 1.0f; // ��鸲�� �پ��� �ӵ�
    bool isShakeStart = false;

    private void Start()
    {
        cameraTransform = transform;
    }

    public void startCameraShake(float shakeAmount, float shakeTime)
    {
        if (isShakeStart) return;
        StartCoroutine(ShakeCoroutine(shakeAmount, shakeTime));
    }
    public void StopCameraShake()
    {
        StopAllCoroutines();
        cameraTransform.position = new Vector3(0, 0, -10);
        isShakeStart = false;
    }
    IEnumerator ShakeCoroutine(float shakeAmount, float shakeTime)
    {
        isShakeStart = true;
        float elapsed = 0.0f;

        while (elapsed < shakeTime)
        {
            float x = Random.Range(-1f, 1f) * shakeAmount;
            float y = Random.Range(-1f, 1f) * shakeAmount;

            cameraTransform.localPosition = new Vector3(transform.position.x + x, transform.position.y + y, -10);

            elapsed += Time.deltaTime;

            yield return null;
        }
        cameraTransform.position = new Vector3(0,0, -10);
        isShakeStart = false;
    }
}
