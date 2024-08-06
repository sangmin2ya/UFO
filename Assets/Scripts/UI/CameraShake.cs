using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeMagnitude = 0.1f; // Èçµé¸² °­µµ
    private Vector3 originalPos;

    void Start()
    {
        originalPos = transform.localPosition;
    }

    void Update()
    {
        float x = Random.Range(-1f, 1f) * shakeMagnitude;
        float y = Random.Range(-1f, 1f) * shakeMagnitude;

        transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);
    }
}
