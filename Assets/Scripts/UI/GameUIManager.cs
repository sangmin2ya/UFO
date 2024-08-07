using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    GameObject Player;

    [SerializeField] TMP_Text AlertText;
    [SerializeField] TMP_Text EventText;
    [SerializeField] GameObject Damaged;
    public GameObject TimeBubbleEffect;


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
}
