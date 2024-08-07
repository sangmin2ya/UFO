  using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Singleton
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(GameManager).Name);
                    _instance = singletonObject.AddComponent<GameManager>();
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return _instance;
        }
    }


    //Game State
    public int _stage { get; private set; } //Current Stage
    public int _score { get; private set; } //Obsatcle Count?
    public float _currentProgress { get; private set; } //Current Position of stage
    [Tooltip("Set Speed of progress Depending on the stage")]
    [SerializeField] private float _progressSpeed = 0.01f; //Speed of progress
    protected void Awake()
    {
        if (_instance == null)
        {
            _instance = this as GameManager;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// Set Progress of the stage for warpgate
    /// </summary>
    /// <param name="progress">float that between 0~1</param>
    public void SetProgress(float progress)
    {
        _currentProgress = progress;
    }
    public void SetProgressSpeed(float speed)
    {
        _progressSpeed = speed;
    }
    public void ResetProgress()
    {
        _currentProgress = 0;
    }
    public void AddProgress()
    {
        _currentProgress += _progressSpeed * Time.fixedDeltaTime;
    }
    public void ResetStage()
    {
        _stage = 0;
    }
    public void AddStage()
    {
        _stage++;
    }
    public void AddScore(int score)
    {
        _score += score;
    }
    public void ResetScore()
    {
        _score = 0;
    }
    public void ResetGame()
    {
        ResetStage();
        ResetScore();
        ResetProgress();
    }
}
