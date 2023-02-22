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

        private LevelBehaviour _previousLevel;
        private LevelBehaviour _currentLevel;
        private int _playerLevel;

        public TrainPrefabData TrainPrefabs => _trainPrefabData;
        public GameConfig GameConfig => _gameConfig;

        private void Start()
        {
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
            StartCoroutine(LoadRoutine());
        }

        private IEnumerator LoadRoutine()
        {
            if (_currentLevel)
            {
                _previousLevel = _currentLevel;
            }

            _currentLevel = Instantiate(_levels[_playerLevel % _levels.Count]);
            _currentLevel.Load(_previousLevel);
            yield return new WaitForEndOfFrame();
        }
        
        public void FinishLevel(bool success)
        {
            UIController.Instance.ActivateEndgamePanel(success);
        }

        public void EndLevel(bool success)
        {
            if (success)
            {
                if (_previousLevel)
                {
                    Destroy(_previousLevel.gameObject);
                    _previousLevel = null;
                }
                ++_playerLevel;
            }
            else
            {
                Destroy(_currentLevel.gameObject);
                _currentLevel = null;
            }
            
            Load();
        }
    }
}
