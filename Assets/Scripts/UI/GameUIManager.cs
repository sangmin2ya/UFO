using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    GameObject Player;

    [SerializeField] TMP_Text AlertText;


    public void showAlert(string message)
    {
        AlertText.text = message;
        AlertText.gameObject.SetActive(true);
        AlertText.GetComponent<Animator>().Play("Text_PopUp");
    }
}
