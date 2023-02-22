using System;
using System.Collections;
using System.Collections.Generic;
using LeasingCase;
using TMPro;
using UnityEngine;

public class UIController : SingletonBehaviour<UIController>
{
    [SerializeField] private RectTransform _HUDPanel;
    [SerializeField] private RectTransform _startPanel;
    [SerializeField] private RectTransform _successPanel;
    [SerializeField] private RectTransform _failPanel;
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private TMP_Text _trainCountText;
    [SerializeField] private TMP_Text _successCountText;

    private void Awake()
    {
        _HUDPanel.gameObject.SetActive(false);
        _startPanel.gameObject.SetActive(false);
    }

    public void ActivateEndgamePanel(bool success)
    {
        var panel = success ? _successPanel : _failPanel;

        panel.gameObject.SetActive(true);
    }

    public void Initialize()
    {
        _successPanel.gameObject.SetActive(false);
        _failPanel.gameObject.SetActive(false);
        _startPanel.gameObject.SetActive(true);
        _HUDPanel.gameObject.SetActive(true);
    }

    public void RestartLevel()
    {
        GameManager.Instance.EndLevel(false);
    }

    public void NextLevel()
    {
        GameManager.Instance.EndLevel(true);
    }

    public void ToggleStartPanel(bool active)
    {
        _startPanel.gameObject.SetActive(active);
    }

    public void SetTrainCount(int correctCount, int totalCount)
    {
        _successCountText.text = 
            _trainCountText.text = $"{correctCount} of {totalCount}";
    }

    public void SetLevelText(int level)
    {
        _levelText.text = $"Level {level}";
    }

    public void SetTimeText(int time)
    {
        var minutes = time / 60;
        var seconds = time % 60;
        var timeText = $"{minutes}:{seconds:00}";
        _timeText.text = $"Time {timeText}]";
    }
}
