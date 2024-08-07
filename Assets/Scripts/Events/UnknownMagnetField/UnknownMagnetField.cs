using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnknownMagnetField : MonoBehaviour
{
    [SerializeField] private float _magnetFieldDuration;
    // Start is called before the first frame update
    void Start()
    {
        StartMagnetField();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void StartMagnetField()
    {
        // Start magnet field
        GetComponent<BoxCollider2D>().enabled = false;
        StartCoroutine(ScaleObject());
    }
    private IEnumerator ScaleObject()
    {
        Vector3 originalScale = transform.localScale;

        // 첫 번째 전환
        for (int i = 1; i < 4; i++)
        {
            transform.localScale = new Vector3(0, originalScale.y, originalScale.z);
            yield return null;
            transform.localScale = new Vector3(0.1f * i, originalScale.y, originalScale.z);
            yield return new WaitForSeconds(0.1f);
            transform.localScale = new Vector3(0, originalScale.y, originalScale.z);
            yield return new WaitForSeconds(1.5f - (i * 0.4f));
        }

        // 서서히 증가: 0에서 5로 2초 동안
        float duration = 2.0f;
        float elapsedTime = 0.0f;
        while (elapsedTime < duration)
        {
            float newXScale = Mathf.Lerp(0, 5, elapsedTime / duration);
            transform.localScale = new Vector3(newXScale, originalScale.y, originalScale.z);
            elapsedTime += Time.fixedDeltaTime;
            yield return null;
        }

        // 마지막 스케일 설정: 5
        GetComponent<BoxCollider2D>().enabled = true;
        transform.localScale = new Vector3(5, originalScale.y, originalScale.z);

        // Magnet field duration
        yield return new WaitForSeconds(_magnetFieldDuration);
        StartCoroutine(DestroyMagnetField());
    }
    private IEnumerator DestroyMagnetField()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        Vector3 originalScale = transform.localScale;
        yield return new WaitForSeconds(0.5f);
        float duration = 2.0f;
        float elapsedTime = 0.0f;
        while (elapsedTime < duration)
        {
            float newXScale = Mathf.Lerp(2, 0, elapsedTime / duration);
            transform.localScale = new Vector3(newXScale, originalScale.y, originalScale.z);
            elapsedTime += Time.fixedDeltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
    public void StopDestroyMagnetField()
    {
        StopAllCoroutines();
    }
    public void StartDestroyMagnetField()
    {
        StartCoroutine(DestroyMagnetField());
    }
}
