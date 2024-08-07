using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CheckStageClear();
    }
    void FixedUpdate()
    {
        AddProgress();
    }
    private void CheckStageClear()
    {
        if (GameManager.Instance._currentProgress >= 1)
        {
            StageClear();
        }
    }
    private void StageClear()
    {
        GameManager.Instance.ResetProgress();
        GameManager.Instance.AddStage();
        GameManager.Instance.SetProgressSpeed(0.01f);
        SceneManager.LoadScene("PSM_GameClear");
        // SceneManager.LoadScene("Main");
    }
    private void AddProgress()
    {
        GameManager.Instance.AddProgress();
    }
}
