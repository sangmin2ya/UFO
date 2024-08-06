using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handle : MonoBehaviour
{
    public float Rotate_Time = .5f;
    bool isDirDifferent;
    float t_Rotate = 0, dir = 0;

    RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        InvokeRepeating("checkDir", 0, .1f);
    }

    private void Update()
    {
        float xHor = Input.GetAxisRaw("Horizontal");
        if (dir != xHor) t_Rotate = 0;

        if (xHor > 0) //¿ìÈ¸Àü
        {
            t_Rotate += Time.deltaTime;
            rectTransform.localRotation = Quaternion.Euler(Vector3.Lerp(new Vector3(0, 0, rectTransform.localRotation.z), new Vector3(0, 0, -20), t_Rotate / Rotate_Time));
        }
        else if (xHor < 0) {
            t_Rotate += Time.deltaTime;
            rectTransform.localRotation = Quaternion.Euler(Vector3.Lerp(new Vector3(0, 0, rectTransform.localRotation.z), new Vector3(0, 0, 20), t_Rotate / Rotate_Time));
        }
        else
        {
            t_Rotate += Time.deltaTime;
            rectTransform.localRotation = Quaternion.Euler(Vector3.Lerp(new Vector3(0, 0, rectTransform.localRotation.z), new Vector3(0, 0, 0), t_Rotate / Rotate_Time));
        }

    }

    void checkDir() => dir = Input.GetAxisRaw("Horizontal");
}
