using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace LeasingCase
{
    public class LevelBehaviour : MonoBehaviour
    {

        private bool _started;
        private bool _finished;

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
            StartCoroutine(FinishRoutine(success));
        }
        
        private void OnPressed(Vector3 pos)
        {
            if (!_started && !_finished)
            {
                _started = true;
                UIController.Instance.ToggleStartPanel(false);
                return;
            }
        }

        private IEnumerator FinishRoutine(bool success)
        {
            yield return new WaitForSeconds(0.25f);
            GameManager.Instance.FinishLevel(success);
        }
    }
}
