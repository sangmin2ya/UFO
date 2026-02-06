using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ToggleGuide : MonoBehaviour
{
    // Reference to the Canvas component
    public GameObject targetCanvas;

    // Method to toggle the canvas and adjust time scale
    public void ToggleCanvas()
    {
        // Check if the canvas is currently enabled
        bool isCanvasEnabled = targetCanvas.activeSelf;

        // Toggle the canvas state
        targetCanvas.SetActive(!isCanvasEnabled);

        // Adjust the time scale based on the canvas state
        Time.timeScale = targetCanvas.activeSelf ? 0 : 1;
    }
    void OnGuide(InputValue value)
    {
        ToggleCanvas();
    }
}
