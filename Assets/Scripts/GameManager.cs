using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("UI Manager")]
    [SerializeField] private GameUIManager _UIManager;

    [Header("Level Manager")]
    [SerializeField] private LevelManager _levelManager;

    [Header("Ball Prefab")]
    [SerializeField] private GameObject _ballPrefab;
    private Ball _ball;

    [Header("Ball Settings")]
    [SerializeField] private float _ballSpeed;
    [SerializeField] private float _fallSpeed;
    private float _fallSpeedBuffer;
    [SerializeField] private float _fallSpeedIncrease;
    [SerializeField] private int _timeToIncreaseSpeed = 15;

    [Header("Level Settings")]
    [SerializeField] private int _tilesToSpawnObstacle;
    [SerializeField] private DifficultyLevel _difficultyLevel;

    private float _distanceSinceObstacle;
    private float _totalDistance;
    private float _timeSinceIncrease;
    private float _totalTime;

    private bool _isPlaying;
    private bool _isFirstTry = true;

    #region Singleton
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion

    private void Start()
    {
        _UIManager.UIStart();
        if (DataKeeper.instance != null) SetDifficulty();

        _isPlaying = true;
        if (_isFirstTry) _fallSpeedBuffer = _fallSpeed;
        else _fallSpeed = _fallSpeedBuffer;

        _levelManager.ClearLevel();
        _levelManager.CreateLevel();

        if (_ball != null)
        {
            Destroy(_ball);
            if (_ball.gameObject != null) Destroy(_ball.gameObject);
        }

        var ball = Instantiate(_ballPrefab);
        _ball = ball.GetComponent<Ball>();
        _ball.BallStart();

        _totalDistance = 0;
        _totalTime = 0;
    }

    private void Update()
    {
        if (_isPlaying)
        {
            var distance = _ballSpeed * Time.deltaTime;
            _totalDistance += distance;
            _distanceSinceObstacle += distance;
            _UIManager.UpdateScore((int)(_totalDistance * 10));

            _totalTime += Time.deltaTime;
            _timeSinceIncrease += Time.deltaTime;

            _levelManager.MoveWall(distance);
            _ball.Move(_fallSpeed);

            if (_timeSinceIncrease >= _timeToIncreaseSpeed)
            {
                _timeSinceIncrease = 0;
                _fallSpeed += _fallSpeedIncrease;
            }
            if (_distanceSinceObstacle >= _tilesToSpawnObstacle)
            {
                _distanceSinceObstacle = 0;
                _levelManager.AddObstacle();
            }
        }
    }

    private void SetDifficulty()
    {
        var difficulty = DataKeeper.instance.Difficulty;
        if (difficulty == DifficultyLevel.Easy) _ballSpeed = 3;
        else if (difficulty == DifficultyLevel.Medium) _ballSpeed = 4;
        else if (difficulty == DifficultyLevel.Hard) _ballSpeed = 5;
    }

    public void SetDifficulty(DifficultyLevel difficulty)
    {
        if (DataKeeper.instance != null)
        {
            DataKeeper.instance.Difficulty = difficulty;
            SetDifficulty();
        }
        else
        {
            if (difficulty == DifficultyLevel.Easy) _ballSpeed = 3;
            else if (difficulty == DifficultyLevel.Medium) _ballSpeed = 4;
            else if (difficulty == DifficultyLevel.Hard) _ballSpeed = 5;
        }
    }

    public void RestartGame()
    {
        _isFirstTry = false;
        Start();
    }

    public void OnBallCollided()
    {
        _isPlaying = false;
        var tryCount = PlayerPrefs.GetInt("TryCounter", 0);
        tryCount++;
        PlayerPrefs.SetInt("TryCounter", tryCount);

        UpdateRecordScore();

        _UIManager.OnGameEnd(_totalTime);
    }

    private void UpdateRecordScore()
    {
        var recordDistance = PlayerPrefs.GetInt("RecordScore", 0);
        var score = (int)(_totalDistance * 10);
        if (recordDistance < score) PlayerPrefs.SetInt("RecordScore", score);
    }

    public void ChangePauseState(bool pause)
    {
        _isPlaying = !pause;
    }

    public void GoToMainMenu()
    {
        UpdateRecordScore();
        SceneManager.LoadScene("MainMenu");
    }
}

public enum DifficultyLevel
{
    Easy,
    Medium,
    Hard
}
