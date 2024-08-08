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

    [Header("EventTime")]
    public float cometShowerTime;
    public float blackholeTime;
    public float SpaceStationTime;


    [Range(0, 60)]
    public float onBlackholeTime;

    private bool checkBlakcholeAlert;
    private bool checkSpaceStationAlert;
    private bool checkSpaceStationOnAlert;
    private bool checkMiJiJangAlert;

    [Header("AlertTime")]
    public float blackHoleAlertTime;
    public float spaceStationAlertTime;
    public float SpaceStationOnAlertTime;
    public float miJiJangAlertTime;


    // Start is called before the first frame update
    void Start()
    {
        currStageLv = GameManager.Instance._stage;
        eventManager.MapSetting(currStageLv);

        checkCometShower = false;
        checkBlackhole = false;
        checkSpaceStation = false;
        checkBlakcholeAlert = false;
        checkSpaceStationAlert = false;
        checkMiJiJangAlert = false;

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
        }else if (currStageLv == 3)
        {
            CheckStage4Event();
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

        if(!checkBlakcholeAlert && blackHoleAlertTime <= GameManager.Instance._currentProgress)
        {
            checkBlakcholeAlert = true;
            StartCoroutine(ShowAlert("블랙홀 접근 중 빨려들어가지않게 조심하세요!"));
        }

        if (!checkSpaceStationAlert && spaceStationAlertTime <= GameManager.Instance._currentProgress)
        {
            checkSpaceStationAlert = true;
            StartCoroutine(ShowAlert("우주 정거장 접근 중. 가운데로 도킹하세요."));
        }

        if (!checkSpaceStationOnAlert && SpaceStationOnAlertTime <= GameManager.Instance._currentProgress)
        {
            checkSpaceStationOnAlert = true;
            StartCoroutine(ShowAlert("우주정거장 도킹으로 연료가 회복되었습니다!"));
        }
    }


    public void CheckStage4Event()
    {
        if (!checkMiJiJangAlert && miJiJangAlertTime <= GameManager.Instance._currentProgress)
        {
            checkMiJiJangAlert = true;
            StartCoroutine(ShowAlert("에일리온 머스크의 방해로 미지의 에너지 필드가 생성됩니다. 조심하세요!"));
        }
    }

    public IEnumerator ShowAlert(string message)
    {
        GameObject.Find("Canvas").GetComponent<GameUIManager>().showEvent(message);
        yield return new WaitForSeconds(3);
        GameObject.Find("Canvas").GetComponent<GameUIManager>().HideEventText();
    }
}
