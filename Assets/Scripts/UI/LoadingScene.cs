using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

    public bool canSkip = true;

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
    }

    public void SkipStory()
    {
        if (!canSkip) return;
        canSkip = false;
        anim.Play("Default");
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

    IEnumerator SkipBtn()
    {
        yield return new WaitForSeconds(5);
        SkipButton.interactable = true;
    }
}