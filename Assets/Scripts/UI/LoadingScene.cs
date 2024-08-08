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
    public int idx = 0;
    public TMP_Text[] Ments;
    [TextArea]
    public string[] Level_Ments = {
    "일론머스크", "머스크 ㅋㅋ", "스크 ㅋㅋ",
    "일론머스크", "머스크 ㅋㅋ", "스크 ㅋㅋ",
    "일론머스크", "머스크 ㅋㅋ", "스크 ㅋㅋ",
    "일론머스크", "머스크 ㅋㅋ", "스크 ㅋㅋ",
    "일론머스크", "머스크 ㅋㅋ", "스크 ㅋㅋ"};
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
    }
    public void loadScene()
    {
        SceneManager.LoadSceneAsync(level_Sequence[idx]);
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
        anim.SetTrigger("Load");
    }
}