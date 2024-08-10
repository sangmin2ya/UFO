using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossStageController : MonoBehaviour
{
    [SerializeField] BossEventManager eventManager;
    [SerializeField] private int currStageLv;

    // Start is called before the first frame update
    void Start()
    {
        currStageLv = GameManager.Instance._stage;
        eventManager.MapSetting(currStageLv);
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
        GameManager.Instance.SetProgressSpeed(0.002f);
        LoadingScene.instance.LoadingStart();
    }

    private void AddProgress()
    {
        GameManager.Instance.AddProgress();
    }
}