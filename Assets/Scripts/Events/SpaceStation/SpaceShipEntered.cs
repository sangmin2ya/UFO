using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpaceShipEntered : MonoBehaviour
{
    public float requiredTime = 3f;
    private float timeInside = 0f;
    private bool isInside = false;
    public Image spaceShipCoolDown;
    
    [SerializeField] private UfoManager ufoManager;
    [SerializeField] HPManager hpManager;

    void Update()
    {
        if (isInside)
        {
            timeInside += Time.deltaTime;
            if (timeInside >= requiredTime)
            {
                OnSpaceStation();
                timeInside = 0f;
            }
        }
        else
        {
            timeInside = 0f;
        }
        float gaugeValue = Mathf.Clamp01(timeInside / requiredTime);
        spaceShipCoolDown.fillAmount = gaugeValue;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            isInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isInside = false;
        }
    }

    private void OnSpaceStation()
    {
        ufoManager.DestroyAttachedObject();
        hpManager.Repaired(100.0f);
    }
}
