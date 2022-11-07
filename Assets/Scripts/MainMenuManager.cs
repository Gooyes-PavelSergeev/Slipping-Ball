using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Text Objects")]
    [SerializeField] private TextMeshProUGUI _counterText;
    [SerializeField] private TextMeshProUGUI _recordText;

    [Header("Main Elements")]
    [SerializeField] private GameObject _mainElements;

    [Header("Difficulty Selector")]
    [SerializeField] private GameObject _difficultySelector;

    private void Start()
    {
        var recordScore = PlayerPrefs.GetInt("RecordScore", 0);
        _recordText.text = "Your best score is " + recordScore.ToString();

        var tryCount = PlayerPrefs.GetInt("TryCounter", 0);
        _counterText.text = "You played " + tryCount.ToString() + " times";

        _mainElements.SetActive(true);
        _difficultySelector.SetActive(false);
    }

    public void StartGame(int diff)
    {
        DataKeeper.instance.Difficulty = (DifficultyLevel)diff;
        SceneManager.LoadScene("GameScene");
    }

    public void TransferDifficultySelector(bool toSelector)
    {
        _mainElements.SetActive(!toSelector);
        _difficultySelector.SetActive(toSelector);
    }
}
