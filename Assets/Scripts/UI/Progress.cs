using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Progress : MonoBehaviour
{
    private Slider _progressBar;
    void Start()
    {
        _progressBar = GetComponent<Slider>();
    }
    void Update()
    {
        UpdateProgressUI();
    }
    private void UpdateProgressUI()
    {
        _progressBar.value = GameManager.Instance._currentProgress;
    }
}
