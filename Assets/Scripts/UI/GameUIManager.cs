using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    GameObject Player;

    [SerializeField] TMP_Text AlertText;
    [SerializeField] TMP_Text EventText;
    [SerializeField] TMP_Text BombText;
    [SerializeField] GameObject Damaged;
    public GameObject TimeBubbleEffect;

    [SerializeField] GameObject DescriptionPanel;

    public void showBombText() => BombText.GetComponent<Animator>().Play("Text_PopUp");

    public void showAlert(string message)
    {
        AlertText.text = message;
        AlertText.GetComponent<Animator>().Play("Text_PopUp");
    }

    public void showEvent(string message)
    {
        EventText.text = message;
        EventText.GetComponent<Animator>().Play("Event_Show");
    }

    public void showDamaged()
    {
        Damaged.GetComponent<Animator>().Play("Damaged");
        Camera.main.GetComponent<CameraShake>().startCameraShake(.05f, .1f);
    }



    public void showTimeBubbleEffect() => TimeBubbleEffect.GetComponent<Animator>().Play("TimeBubble");

    public void HideAlertText() => AlertText.GetComponent<Animator>().Play("Default");
    public void HideEventText() => EventText.GetComponent<Animator>().Play("Default");

    public void showDescription(int idx)
    {
        DescriptionPanel.SetActive(true);
        for (int i = 0; i < DescriptionPanel.transform.childCount; i++) {
            DescriptionPanel.transform.GetChild(i).gameObject.SetActive(false);
        }
        DescriptionPanel.transform.GetChild(idx).gameObject.SetActive(true);
        Time.timeScale = 0;

        StartCoroutine(delay(5));
    }

    IEnumerator delay(float amount)
    {
        yield return new WaitForSecondsRealtime(amount);
        Time.timeScale = 1;
        DescriptionPanel.SetActive(false);
    }
}
