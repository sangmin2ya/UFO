using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class Speed_UI : MonoBehaviour
{
    [Header("�̹���")]
    public Image[] Handle = new Image[2];


    [Header("���� ����")]
    [SerializeField] bool isFuel = false;
    public bool isSwap = false;
    public float fuel_Time = 10;
    float t_fuel = 0;

    public float speed_Time = 3;
    float t_Speed = 0;
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
            Debug.Log("Swap");
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
