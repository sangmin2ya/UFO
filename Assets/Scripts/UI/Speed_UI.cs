using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class Speed_UI : MonoBehaviour
{
    [Header("이미지")]
    public Image[] Handle = new Image[2];


    [Header("변수 설정")]
    [SerializeField] bool isFuel = false;
    [SerializeField] bool isSwap = false;
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
        if (isFuel)
        {
            t_fuel += Time.deltaTime / fuel_Time;
            speed_Slider.fillAmount = 1 - t_fuel <= 0 ? 0 : 1 - t_fuel;

            if (1 - t_fuel <= 0) t_fuel = 0;

            speed_Slider.color = new Color(1, 1 - t_fuel, 1 - t_fuel);

            return;
        }

        else if (isSwap)
        {
            t_Swap += Time.deltaTime / swap_Time;
            speed_Slider.fillAmount = t_Swap >= 1 ? 1 : t_Swap;

            if(t_Swap >= 1) t_Swap = 0;
            return;
        }

        t_Speed += Time.deltaTime / speed_Time;
        Camera.main.GetComponent<CameraShake>().shakeMagnitude = .02f * t_Speed;

        foreach(var x in Handle)
        {
            x.color = new Color(1, 1 - t_Speed, 1 - t_Speed);
        }
        speed_Slider.fillAmount = t_Speed >= 1 ? 1 : t_Speed;

        if(t_Speed >= 1) t_Speed = 0;
    }
}
