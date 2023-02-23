using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

namespace LeasingCase
{
    public class GameManager : SingletonBehaviour<GameManager>
    {
        [SerializeField] private TrainPrefabData _trainPrefabData;
        [SerializeField] private GameConfig _gameConfig;
        [SerializeField] private List<LevelBehaviour> _levels;
        
        private LevelBehaviour _currentLevel;
        private int _playerLevel;

        public TrainPrefabData TrainPrefabs => _trainPrefabData;
        public GameConfig GameConfig => _gameConfig;
        public LevelBehaviour CurrentLevel => _currentLevel;

        private void Start()
        {
            _playerLevel = PlayerPrefs.GetInt("PlayerLevel", 0);
            Load();
        }

        private void Load()
        {
            if (_levels.Count < 1)
            {
                Debug.LogError("No levels are present in GameManager");
                return;
            }
            
            UIController.Instance.Initialize();
            UIController.Instance.SetLevelText(_playerLevel + 1);
            StartCoroutine(LoadRoutine());
        }

        private IEnumerator LoadRoutine()
        {
            if (_currentLevel)
            {
                Destroy(_currentLevel.gameObject);
                _currentLevel = null;
            }

            _currentLevel = Instantiate(_levels[_playerLevel % _levels.Count]);
            _currentLevel.Load();
            yield return new WaitForEndOfFrame();
        }
        
        public void FinishLevel(bool success)
        {
            UIController.Instance.ActivateEndgamePanel(success);
        }

        public void EndLevel(bool success)
        {
            if (success)
                ++_playerLevel;

            PlayerPrefs.SetInt("PlayerLevel", _playerLevel);
            Load();
        }
    }
}
