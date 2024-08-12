using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageController : MonoBehaviour
{
    [SerializeField] EventManager eventManager;
    private int currStageLv;
    [SerializeField] private List<Sprite> backgroundSprites;
    private GameObject _UFO;
    private bool clearOnce;

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
    private bool checkMiJiJangAlert;

    [Header("AlertTime")]
    public float blackHoleAlertTime;
    public float spaceStationAlertTime;
    public float miJiJangAlertTime;


    // Start is called before the first frame update
    void Start()
    {
        _UFO = GameObject.Find("UFO");
        currStageLv = GameManager.Instance._stage;
        eventManager.MapSetting(currStageLv);

        checkCometShower = false;
        checkBlackhole = false;
        checkSpaceStation = false;
        checkBlakcholeAlert = false;
        checkSpaceStationAlert = false;
        checkMiJiJangAlert = false;
        clearOnce = true;
        if (currStageLv == 1)
        {
            eventManager.OffEpicItems();
        }
        if (currStageLv == 3)
        {
            eventManager.OnEpicItems();
        }

        if (currStageLv == 2 || currStageLv == 4)
        {
            eventManager.OffEpicItems();
        }
        for (int i = 0; i < 4; i++)
        {
            GameObject.Find("Backgrounds").transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = backgroundSprites[currStageLv];
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

        if (currStageLv == 2)
        {
            CheckStage3Event();
        }

        else if (currStageLv == 3)
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
        if (GameManager.Instance._currentProgress >= 1 && clearOnce)
        {
            clearOnce = false;
            StageClear();
        }
    }

    private void StageClear()
    {
        _UFO.GetComponent<UfoManager>().DestroyEveryObstacle();
        _UFO.GetComponent<UfoController>().SetUnmovable();
        GameObject.Find("EventController").GetComponent<EventManager>().OffEpicItems();
        GameObject.Find("EventController").GetComponent<EventManager>().OffObstacles();

        StartCoroutine(MoveToNextStage());
    }
    IEnumerator MoveToNextStage()
    {
        BackgroundRepeating[] backgrounds = GameObject.Find("Backgrounds").transform.GetComponentsInChildren<BackgroundRepeating>();
        foreach (var obj in backgrounds)
        {
            obj.Stop();
        }

        Vector3 startPosition = _UFO.transform.position;
        yield return new WaitForSeconds(1);
        // 화면 오른쪽 끝의 월드 좌표를 계산합니다.
        Vector3 screenRightEdge = Camera.main.ViewportToWorldPoint(new Vector3(2, 0.5f, Camera.main.nearClipPlane));
        screenRightEdge.z = startPosition.z;

        float elapsedTime = 0;

        while (elapsedTime < 2.5f)
        {
            _UFO.transform.position = Vector3.Lerp(startPosition, screenRightEdge, elapsedTime / 2.5f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // 정확한 위치를 보장하기 위해 루프가 끝난 후 위치를 설정합니다.
        _UFO.transform.position = screenRightEdge;

        _UFO.GetComponent<UfoManager>().AddBombCount();
        GameManager.Instance.AddStage();
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
            checkCometShower = true;
            eventManager.OnCometShower();
        }
        if (!checkBlackhole && blackholeTime <= GameManager.Instance._currentProgress)
        {
            checkBlackhole = true;
            eventManager.OnBlackhole(onBlackholeTime);
        }
        if (!checkBlakcholeAlert && blackHoleAlertTime <= GameManager.Instance._currentProgress)
        {
            checkBlakcholeAlert = true;
            StartCoroutine(ShowAlert("블랙홀 접근 중. 끌려온 혜성을 조심하세요!"));
        }
    }

    public void CheckStage3Event()
    {
        if (!checkSpaceStationAlert && spaceStationAlertTime <= GameManager.Instance._currentProgress)
        {
            GameObject.Find("Canvas").GetComponent<GameUIManager>().showDescription(0);
            checkSpaceStationAlert = true;
            StartCoroutine(ShowAlert("우주 정거장을 운석으로부터 지켜내세요!"));
        }
        if (!checkSpaceStation && SpaceStationTime <= GameManager.Instance._currentProgress)
        {
            checkSpaceStation = true;
            eventManager.OffEpicItems();
            eventManager.OnSpaceStation();
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