using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LeasingCase
{
    public class LevelBehaviour : MonoBehaviour
    {
        [SerializeField] private float _spawnInterval = 2f;

        public bool Spawning;
        
        private bool _started;
        private bool _finished;

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
        }

        public void Update()
        {
            if (!_started || _finished)
                return;
            
        }

        private void Subscribe()
        {
            InputController.Pressed += OnPressed;
        }

        private void Unsubscribe()
        {
            InputController.Pressed -= OnPressed;
        }
        
        public void Load(LevelBehaviour previousLevel)
        {
            _started = false;
            _finished = false;
            Subscribe();
        }

        private void FinishLevel(bool success)
        {
            if (_finished)
                return;
            
            _finished = true;
            InputController.Pressed -= OnPressed;
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
                    yield return new WaitForSeconds(_spawnInterval);
                }
                yield return null;
            }
        }
    }
}
