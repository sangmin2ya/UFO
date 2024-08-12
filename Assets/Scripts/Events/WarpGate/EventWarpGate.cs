using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EventWarpGate : MonoBehaviour
{
    [SerializeField] private float minChange = 0.2f;
    [SerializeField] private float maxChange = 0.3f;

    [SerializeField] private float moveSpeed = 2f;

    ObstacleManager obstacleManager;
    private GameObject _cutScene;
    private float progressChange;

    void Start()
    {
        obstacleManager = GameObject.Find("ObstacleManager").GetComponent<ObstacleManager>();
        _cutScene = GameObject.Find("CutScene");
    }
    private void FixedUpdate()
    {
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            float currentProgress = GameManager.Instance._currentProgress;

            if (Random.value > 0.3f)
            {
                // 70% Increase progress
                progressChange = Random.Range(minChange, maxChange);
            }
            else
            {
                // 30% Decrease progress
                progressChange = -Random.Range(minChange, maxChange);
            }

            // Delete all surrounding objects
            StartCoroutine(DestroyWarpGate());
            StartCoroutine(ShowCutscene(progressChange > 0 ? 1 : -1));
        }
    }
    IEnumerator DestroyWarpGate()
    {
        yield return new WaitForSeconds(0.5f);
        obstacleManager.OnBomb();
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
        // Update the progress in the GameManager
        float currentProgress = GameManager.Instance._currentProgress;
        GameManager.Instance.SetProgress(Mathf.Clamp(currentProgress + progressChange, 0.1f, 0.9f));

        yield return new WaitForSeconds(1.0f);
        // Delete warp gate
        Destroy(gameObject);
    }
    IEnumerator ShowCutscene(int direction)
    {
        float elapsedTime = 0;

        // 초기 위치 설정
        _cutScene.transform.localPosition = new Vector3(5200 * direction, 0, 0);

        while (elapsedTime < 1.5)
        {
            _cutScene.transform.localPosition = Vector3.Lerp(new Vector3(5200 * direction, 0, 0), new Vector3(-5200 * direction, 0, 0), elapsedTime / 1.5f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        GameObject spaceShip = GameObject.FindWithTag("SpaceStation");
        if (spaceShip != null)
        {
            Destroy(spaceShip);
        }

        // 마지막 위치 설정
        _cutScene.transform.localPosition = new Vector3(-5200 * direction, 0, 0);
    }
}
