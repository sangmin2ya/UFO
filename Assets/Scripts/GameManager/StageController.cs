using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageController : MonoBehaviour
{
    [SerializeField] EventManager eventManager;
    [SerializeField] private int currStageLv;

    private bool checkCometShower;
    private bool checkBlackhole;
    private bool checkSpaceStation;
    bool checkAlert;

    public float cometShowerTime;
    public float blackholeTime;
    public float SpaceStationTime;

    [Range(0, 60)]
    public float onBlackholeTime;

    public float blackHoleAlertTime;


    // Start is called before the first frame update
    void Start()
    {
        currStageLv = GameManager.Instance._stage;
        eventManager.MapSetting(currStageLv);
        checkCometShower = false;
        checkBlackhole = false;
        checkSpaceStation = false;
        if (currStageLv == 3)
        {
            eventManager.OnEpicItems();
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckStageClear();
        if (currStageLv == 1)
        {
            CheckStage2Event();
        }
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
    public void CheckStage2Event()
    {
        if (!checkCometShower && cometShowerTime <= GameManager.Instance._currentProgress)
        {
            Debug.Log("CometShower");
            checkCometShower = true;
            eventManager.OnCometShower();
        }

        if (!checkBlackhole && blackholeTime <= GameManager.Instance._currentProgress)
        {
            checkBlackhole = true;

            eventManager.OnBlackhole(onBlackholeTime);
        }

        if (!checkSpaceStation && SpaceStationTime <= GameManager.Instance._currentProgress)
        {
            checkSpaceStation = true;
            eventManager.OnSpaceStation();
        }

        if(!checkAlert && blackHoleAlertTime <= GameManager.Instance._currentProgress)
        {
            checkAlert = true;
            StartCoroutine(showBlackHoleAlert("블랙홀 접근 중"));
        }


    }

    IEnumerator showBlackHoleAlert(string message)
    {
        GameObject.Find("Canvas").GetComponent<GameUIManager>().showEvent(message);
        yield return new WaitForSeconds(3);
        GameObject.Find("Canvas").GetComponent<GameUIManager>().HideEventText();
    }
}
