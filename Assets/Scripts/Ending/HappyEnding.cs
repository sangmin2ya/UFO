using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HappyEnding : MonoBehaviour
{
    public GameObject[] Bursts;

    public void Burst1()
    {
        Bursts[0].gameObject.SetActive(true);
    }

    public void Burst2()
    {
        Bursts[1].gameObject.SetActive(true);
    }

    public void Burst3()
    {
        Bursts[2].gameObject.SetActive(true);
    }

    public void MainMenu()
    {
        LoadingScene.instance.idx = -1;
        SceneManager.LoadScene(0);
    }
}
