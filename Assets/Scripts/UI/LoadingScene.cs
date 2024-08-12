using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LoadingScene : MonoBehaviour
{
    int[] level_Sequence = new int[5] { 1, 1, 1, 1, 3 };
    public static LoadingScene instance;
    public Animator anim;
    public int idx = -1;
    public TMP_Text[] Ments;
    [TextArea]
    public string[] Level_Ments = {
    "일론머스크", "머스크 ㅋㅋ", "스크 ㅋㅋ",
    "일론머스크", "머스크 ㅋㅋ", "스크 ㅋㅋ",
    "일론머스크", "머스크 ㅋㅋ", "스크 ㅋㅋ",
    "일론머스크", "머스크 ㅋㅋ", "스크 ㅋㅋ",
    "일론머스크", "머스크 ㅋㅋ", "스크 ㅋㅋ"};

    AudioSource source;
    [SerializeField] AudioClip[] clips;
    int soundidx = 1;
    [SerializeField] TMP_Text StageText;
    [SerializeField] Button SkipButton;

    public bool canSkip = false;
    public bool canBomb;

    // Input 관련 필드
    public InputActionAsset inputActions;
    private InputAction submitAction;

    private void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += onsceneload;
        source = GetComponent<AudioSource>();

        // InputAction 설정 및 이벤트 등록
        submitAction = inputActions.FindActionMap("Player").FindAction("Submit");
        submitAction.Enable();
        submitAction.started += OnSubmitPerformed;
    }

    private void OnDestroy()
    {
        // 이벤트 해제
        submitAction.canceled -= OnSubmitPerformed;
    }

    private void OnSubmitPerformed(InputAction.CallbackContext context)
    {
        // 스킵 버튼이 활성화되어 있고, 스킵이 가능한 상태일 때만 스킵 처리
        if (SkipButton.interactable && canSkip)
        {
            Debug.Log("스킵 버튼 눌림");
            SendMessage("SkipStory");
        }
    }

    public void SkipStory()
    {
        if (!canSkip) return;
        
        canSkip = false;
        Debug.Log(canSkip);

        anim.Play("LoadEnd");
        loadScene();
        StartCoroutine(SkipBtn());
    }

    public void loadScene()
    {
        SceneManager.LoadScene(level_Sequence[idx]);
        GameManager.Instance.ResetProgress();
        int temp = soundidx;
        soundidx = (idx == 4 || idx == -1) ? 1 : 0;
        source.pitch = (idx == 4 || idx == -1) ? 1.5f : 1f;
        source.clip = clips[soundidx];
        if (temp != soundidx) source.Play();
    }

    public void Restart()
    {
        SceneManager.LoadScene(level_Sequence[idx]);
        int temp = soundidx;
        soundidx = (idx == 4 || idx == -1) ? 1 : 0;
        source.pitch = (idx == 4 || idx == -1) ? 1.5f : 1f;
        source.clip = clips[soundidx];
        if (temp != soundidx) source.Play();
    }

    private void onsceneload(Scene arg0, LoadSceneMode arg1)
    {
        anim.SetTrigger("LoadEnd");
    }
    public void LoadingStart()
    {
        idx += 1;
        for (int i = 0; i < Ments.Length; i++)
        {
            Ments[i].text = Level_Ments[(idx == -1 ? 0 : idx) * 3 + i];
        }
        StageText.text = $"{idx + 1}장";
        anim.SetTrigger("Load");
    }

    public void skipTrue() => canSkip = true;

    public void skipFalse() => canSkip = false;

    public void BombTrue() => canBomb = true;

    public void BombFalse() => canBomb = false;

    IEnumerator SkipBtn()
    {
        yield return new WaitForSeconds(5);
        SkipButton.interactable = true;
    }
}