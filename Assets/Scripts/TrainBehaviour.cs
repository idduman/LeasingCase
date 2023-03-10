using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Cinemachine;
using UnityEngine;

namespace LeasingCase
{
    public enum TrainColor
    {
        White,
        Black,
        Red,
        Green,
        Blue,
        Yellow,
        Purple,
    }
    
    public class TrainBehaviour : MonoBehaviour
    {
        public static event Action<bool> DestinationReached;
        
        [SerializeField] private TrainColor _colorA;
        [SerializeField] private TrainColor _colorB;
        [SerializeField] private RailBehaviour _currentRail;
        
        private float _moveSpeed;
        private RailEntryType _currentRailEntryType = RailEntryType.Forward;
        private LayerMask _tileMask;

        private float _pathProgress;

        private bool _started, _finished;


        private void Awake()
        {
            _moveSpeed = GameManager.Instance.GameConfig.TrainMoveSpeed;
            _tileMask = LayerMask.GetMask("Rail");
            _started = true;
            _finished = false;
            
        }

        private void Update()
        {
            if (!_started || _finished || !_currentRail)
                return;

            if (_pathProgress < 1f)
            {
                var fw = _currentRailEntryType is RailEntryType.Forward;
                var p = fw ? _pathProgress : (1f - _pathProgress);
                var angle = fw ? 0f : 180f;
                
                transform.position = _currentRail.Path.EvaluatePositionAtUnit(p,
                    CinemachinePathBase.PositionUnits.Normalized);
                transform.rotation = _currentRail.Path.EvaluateOrientationAtUnit(p,
                    CinemachinePathBase.PositionUnits.Normalized)
                    * Quaternion.AngleAxis(angle, Vector3.up);
                _pathProgress += _moveSpeed * Time.deltaTime;
            }
            else if(_currentRail)
            {
                if(!_finished && _currentRail.TryGetComponent<TrainDestination>(out var dest))
                {
                    _finished = true;
                    var success = dest.CompareColors(_colorA, _colorB);
                    DestinationReached?.Invoke(success);
                    Destroy(gameObject,0.1f);
                    return;
                }
                _currentRail = null;
                GetNextRail();
            }
        }

        public void SetRail(RailBehaviour rail)
        {
            _currentRailEntryType = rail.CheckEntryType(transform.position);
            //Debug.Log("Entry Type: " + _currentRailEntryType);
            if (_currentRailEntryType == RailEntryType.None)
            {
                //TODO fail;
                _finished = true;
                _currentRail = null;
                return;
            }
            
            _currentRail = rail;
            _pathProgress = 0f;
        }

        private void GetNextRail()
        {
            var ray = new Ray(transform.position, transform.forward);
            if (!Physics.Raycast(ray, out var hit, 1f, _tileMask)
                || !hit.collider.TryGetComponent<RailBehaviour>(out var rail))
            {
                //TODO fail;
                _finished = true;
                _currentRail = null;
                return;
            }
            
            SetRail(rail);
        }

        public bool CompareColors(TrainColor colorA, TrainColor colorB)
        {
            return colorA == _colorA && colorB == _colorB
                   || colorA == _colorB && colorB == _colorA;
        }
    }

}
