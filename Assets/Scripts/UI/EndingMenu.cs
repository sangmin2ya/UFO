using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingMenu : MonoBehaviour
{
    int[] level_Sequence = new int[5] { 1, 1, 2, 1, 3 };
    public void GameStartClick()
    {
        LoadingScene.instance.LoadingStart();
    }
    public void ExitMenu() {
        LoadingScene.instance.idx = -1;
        SceneManager.LoadScene(0);
    }
    public void RestartClick() {
        SceneManager.LoadScene(level_Sequence[LoadingScene.instance.idx]);
    }
    public void GameExit() => Application.Quit();
}
