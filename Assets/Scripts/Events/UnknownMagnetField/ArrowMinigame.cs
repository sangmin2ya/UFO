using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.PostProcessing;

public class ArrowMinigame : MonoBehaviour
{
    [SerializeField] private List<GameObject> _arrows;
    [SerializeField] private Sprite _rightArrow;
    [SerializeField] private Sprite _leftArrow;
    [SerializeField] private Sprite _upArrow;
    [SerializeField] private Sprite _downArrow;
    private List<KeyCode> _codes = new List<KeyCode>();
    private int _count = 0;
    private KeyCode _inputKey;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < _arrows.Count; i++)
        {
            _arrows[i].SetActive(false);
        }
        StartCoroutine("ResetCode");
        Time.timeScale = 0.5f;
        _inputKey = KeyCode.None;
        GameObject.Find("PostProcessVolume").GetComponent<PostProcessVolume>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        SpecialMission();
    }

    private void SpecialMission()
    {
        //키 입력 게임
        if (_count < 5)
        {
            KeyCode inputKey = _inputKey;
            //키 입력이 들어왔을때
            if (inputKey != KeyCode.None)
            {
                //첫 키코드와 같다면 해당 키코드 제거
                if (_codes[_count] == inputKey)
                {
                    _arrows[_count].SetActive(false);
                    _count++;
                }
                _inputKey = KeyCode.None;
            }
        }
        //모두 맞출경우 코드 리셋 후 성공 카운트
        else if (_count == 5)
        {
            GameObject.Find("UFO").GetComponent<UfoController>().ExitMagnetField();
            Time.timeScale = 1f;
            Destroy(gameObject);
        }
    }
    private void RandomPrint()
    {
        for (int i = 0; i < 5; i++)
        {
            _arrows[i].SetActive(true);
            switch (Random.Range(0, 4))
            {
                case 0:
                    _arrows[i].GetComponent<UnityEngine.UI.Image>().sprite = _leftArrow;
                    break;
                case 1:
                    _arrows[i].GetComponent<UnityEngine.UI.Image>().sprite = _upArrow;
                    break;
                case 2:
                    _arrows[i].GetComponent<UnityEngine.UI.Image>().sprite = _downArrow;
                    break;
                case 3:
                    _arrows[i].GetComponent<UnityEngine.UI.Image>().sprite = _rightArrow;
                    break;
                case 4:
                    _arrows[i].GetComponent<UnityEngine.UI.Image>().sprite = null;
                    break;
                default:
                    break;
            }
        }
    }
    IEnumerator ResetCode()
    {
        float time = 0;
        while (time < 1)
        {
            time += Time.fixedDeltaTime;
            RandomPrint();
            yield return new WaitForFixedUpdate();
        }
        //_hacking.text = "[ PRESS ARROWS TO HACK ]";
        _codes.Clear();
        for (int i = 0; i < 5; i++)
        {
            _arrows[i].SetActive(true);
            switch (Random.Range(0, 4))
            {
                case 0:
                    _codes.Add(KeyCode.LeftArrow);
                    _arrows[i].GetComponent<UnityEngine.UI.Image>().sprite = _leftArrow;
                    break;
                case 1:
                    _codes.Add(KeyCode.UpArrow);
                    _arrows[i].GetComponent<UnityEngine.UI.Image>().sprite = _upArrow;
                    break;
                case 2:
                    _codes.Add(KeyCode.DownArrow);
                    _arrows[i].GetComponent<UnityEngine.UI.Image>().sprite = _downArrow;
                    break;
                case 3:
                    _codes.Add(KeyCode.RightArrow);
                    _arrows[i].GetComponent<UnityEngine.UI.Image>().sprite = _rightArrow;
                    break;
                default:
                    break;
            }
        }
        yield return null;
    }
    //Player Input-----------------------------------------------------------
    void OnLeftArrow(InputValue value)
    {
        _inputKey = KeyCode.LeftArrow;
    }
    void OnRightArrow(InputValue value)
    {
        _inputKey = KeyCode.RightArrow;
    }
    void OnUpArrow(InputValue value)
    {
        _inputKey = KeyCode.UpArrow;
    }
    void OnDownArrow(InputValue value)
    {
        _inputKey = KeyCode.DownArrow;
    }
}
