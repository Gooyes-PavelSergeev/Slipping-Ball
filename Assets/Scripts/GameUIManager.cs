using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameUIManager : MonoBehaviour
{
    [Header("Background")]
    [SerializeField] private GameObject _background;

    [Header("Text Objects")]
    [SerializeField] private GameObject _timerTextGO;
    [SerializeField] private TextMeshProUGUI _counterText;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _recordText;

    [Header("Buttons")]
    [SerializeField] private GameObject _difficultyChangeButton;
    [SerializeField] private GameObject _restartButton;
    [SerializeField] private GameObject _pauseButton;

    [Header("Game Stop UI")]
    [SerializeField] private GameObject _endGameElements;
    [SerializeField] private GameObject _pauseElements;

    [Header("Difficulty Selector")]
    [SerializeField] private GameObject _difficultySelector;

    public void UIStart()
    {
        TurnOffUI();
        _pauseElements.SetActive(false);
        _difficultySelector.SetActive(false);
    }

    public void UpdateScore(int score)
    {
        _scoreText.text = score.ToString();
    }

    public void TurnOffUI()
    {
        _endGameElements.SetActive(false);
        _background.SetActive(false);
        _pauseButton.SetActive(true);
    }

    public void TurnOnUI()
    {
        _endGameElements.SetActive(true);
        _background.SetActive(true);
        _pauseButton.SetActive(false);
    }

    public void OnGameEnd(float time)
    {
        var tryCount = PlayerPrefs.GetInt("TryCounter");
        _counterText.text = "That was try number " + tryCount.ToString();

        var recordScore = PlayerPrefs.GetInt("RecordScore");
        _recordText.text = "Record is " + recordScore.ToString();

        var timerText = TimeSpan.FromSeconds(time).Hours.ToString("00") +
                        ":" + TimeSpan.FromSeconds(time).Minutes.ToString("00") +
                        ":" + TimeSpan.FromSeconds(time).Seconds.ToString("00");
        _timerTextGO.GetComponent<TextMeshProUGUI>().text = timerText;

        TurnOnUI();
    }

    public void SetDifficulty(int diff)
    {
        GameManager.instance.SetDifficulty((DifficultyLevel)diff);
        BackFromDifficultySelector();
    }

    public void GoToDifficultySelector()
    {
        _difficultySelector.SetActive(true);
        TurnOffUI();
        _pauseButton.SetActive(false);
        _background.SetActive(true);
        _scoreText.gameObject.SetActive(false);
    }

    public void BackFromDifficultySelector()
    {
        _difficultySelector.SetActive(false);
        TurnOnUI();
        _scoreText.gameObject.SetActive(true);
    }

    public void ChangePauseState(bool isPaused)
    {
        _background.SetActive(isPaused);
        _pauseButton.SetActive(!isPaused);
        GameManager.instance.ChangePauseState(isPaused);
        _pauseElements.SetActive(isPaused);
    }

    public void GoToMainMenu()
    {
        GameManager.instance.GoToMainMenu();
    }
}
