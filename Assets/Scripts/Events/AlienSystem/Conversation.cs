using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class Conversation : MonoBehaviour
{
    void OnSubmit(InputValue value)
    {
        if (value.isPressed)
        {
            if (Time.timeScale == 0f)
            {
                Time.timeScale = 1f;
                gameObject.SetActive(false);
            }
        }
    }
}


