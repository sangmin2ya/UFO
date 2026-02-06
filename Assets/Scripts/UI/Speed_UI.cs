using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class Speed_UI : MonoBehaviour
{
    public bool isSwap = false;


    Image speed_Slider;

    public float swap_Time = 2;
    float t_Swap = 0;

    private void Start()
    {
        speed_Slider = GetComponent<Image>();
    }

    private void FixedUpdate()
    {

        if (isSwap)
        {
            t_Swap += Time.deltaTime / swap_Time;
            speed_Slider.fillAmount = t_Swap >= 0.5 ? 1 : (t_Swap / 0.5f);

            if (t_Swap >= 0.5)
            {
                t_Swap = 0;
                isSwap = false;
            }
            return;
        }
    }
}
