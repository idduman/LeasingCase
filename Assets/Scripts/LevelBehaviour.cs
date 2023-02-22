using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LeasingCase
{
    public class LevelBehaviour : MonoBehaviour
    {
        [SerializeField] private int _trainCount;
        [SerializeField] private int _trainsToWin;
        [SerializeField] private float _spawnInterval = 2f;

        public bool Spawning;
        
        private bool _started;
        private bool _finished;

        private int _currentTrainCount;
        private int _correctCount;
        private int _completedCount;
        public int CompletedCount
        {
            get => _completedCount;
            private set
            {
                _completedCount = value;
                if (!UIController.Instance)
                    return;
                
                UIController.Instance.SetTrainCount(_correctCount, _completedCount);
            }
        }

        private TrainSpawn _spawn;
        private List<TrainDestination> _destinations;

        private Coroutine _spawnRoutine;

        private void Awake()
        {
            _spawn = GetComponentInChildren<TrainSpawn>();
            if (!_spawn)
                Debug.LogError($"No train spawn found in level \"{name}\"");
            _destinations = GetComponentsInChildren<TrainDestination>().ToList();
            if (_destinations.Count < 1)
                Debug.LogError($"No train destinations found in level \"{name}\"");
        }

        private void OnDestroy()
        {
            Unsubscribe();
            StopAllCoroutines();
        }

        public void Update()
        {
            if (!_started || _finished)
                return;
            
        }

        private void Subscribe()
        {
            InputController.Pressed += OnPressed;
            TrainBehaviour.DestinationReached += OnTrainDestinationReached;
        }

        private void Unsubscribe()
        {
            InputController.Pressed -= OnPressed;
        }
        
        public void Load()
        {
            _currentTrainCount = 0;
            _correctCount = 0;
            CompletedCount = 0;
            _started = false;
            _finished = false;
            
            _spawn.Initialize();
            Subscribe();
        }

        private void FinishLevel(bool success)
        {
            if (_finished)
                return;
            
            _finished = true;
            Unsubscribe();
            
            if (_spawnRoutine != null)
                StopCoroutine(_spawnRoutine);
            StartCoroutine(FinishRoutine(success));
        }
        
        private void OnPressed(Vector3 pos)
        {
            if (!_started && !_finished)
            {
                _started = true;
                UIController.Instance.ToggleStartPanel(false);
                Spawning = true;
                StartCoroutine(SpawnRoutine());
                return;
            }
        }
        
        private void OnTrainDestinationReached(bool success)
        {
            if (_finished)
                return;
            
            if (success)
                _correctCount++;
            
            CompletedCount++;
            
            if(CompletedCount == _trainCount)
                FinishLevel(_completedCount >= _trainsToWin);
        }

        private IEnumerator FinishRoutine(bool success)
        {
            yield return new WaitForSeconds(0.25f);
            GameManager.Instance.FinishLevel(success);
        }

        private IEnumerator SpawnRoutine()
        {
            while (true)
            {
                if (Spawning)
                {
                    var destination = _destinations[Random.Range(0, _destinations.Count)];
                    _spawn.Spawn(destination.ColorA, destination.ColorB);
                    _currentTrainCount++;
                    yield return new WaitForSeconds(_spawnInterval);
                }
                yield return null;
            }
        }
    }
}
