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

    void Start()
    {
        obstacleManager = GameObject.Find("ObstacleManager").GetComponent<ObstacleManager>();
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
            float progressChange;
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

            // Update the progress in the GameManager
            GameManager.Instance.SetProgress(Mathf.Clamp(currentProgress + progressChange, 0.1f, 0.9f));

            // Delete all surrounding objects
            obstacleManager.OnBomb();
            // Delete warp gate
            Destroy(gameObject);

        }
    }
}
