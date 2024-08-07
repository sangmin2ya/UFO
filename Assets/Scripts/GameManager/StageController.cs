using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageController : MonoBehaviour
{
    [SerializeField] EventManager eventManager;
    private int currStageLv;
    
    private bool checkCometShower;
    private bool checkBlackhole;
    private bool checkSpaceStation;

    public float cometShowerTime;
    public float blackholeTime;
    public float SpaceStationTime;

    [Range(0,60)]
    public float onBlackholeTime;


    // Start is called before the first frame update
    void Start()
    {
        currStageLv = GameManager.Instance._stage;
        eventManager.MapSetting(currStageLv);
        checkCometShower = false;
        checkBlackhole = false;
        checkSpaceStation = false;
}

    // Update is called once per frame
    void Update()
    {
        CheckStageClear();
        CheckStageStatus();
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
        SceneManager.LoadScene("Main");
    }

    private void AddProgress()
    {
        GameManager.Instance.AddProgress();
    }

    private void CheckStageStatus()
    {
        switch (currStageLv)
        {
            case 1:
                CheckStage2Event();
                break;
            case 3:
                break;
        }
    }

    public void CheckStage2Event()
    {
        if (!checkCometShower && cometShowerTime>= GameManager.Instance._currentProgress)
        {
            eventManager.OnCometShower();
        }

        if (!checkBlackhole && blackholeTime >= GameManager.Instance._currentProgress)
        {
            eventManager.OnBlackhole(onBlackholeTime);
        }

        if (!checkSpaceStation && SpaceStationTime >= GameManager.Instance._currentProgress)
        {
            eventManager.OnSpaceStation();
        }
    }

    public void CheckStage4Event()
    {

    }
}
