using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingMenu : MonoBehaviour
{
    public void ExitMenu() {
        LoadingScene.instance.idx = 0;
        SceneManager.LoadScene(0);
    }

    public void GameExit() => Application.Quit();
}
