using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadingScene : MonoBehaviour
{
    int[] level_Sequence = new int[5] {1,1,2,1,3};
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
    public void loadScene()
    {
        SceneManager.LoadScene(level_Sequence[idx]);
        int temp = soundidx;
        soundidx = (idx == 2 || idx == 4 || idx == -1) ? 1 : 0;
        source.clip = clips[soundidx];
        if(temp != soundidx) source.Play();
        idx += 1;
    }
    private void onsceneload(Scene arg0, LoadSceneMode arg1)
    {
        anim.SetTrigger("LoadEnd");
    }
    public void LoadingStart()
    {
        for (int i = 0; i < Ments.Length; i++) {
            Ments[i].text = Level_Ments[idx*3+i];
        }
        StageText.text = $"{idx + 1}장";
        anim.SetTrigger("Load");
    }
}