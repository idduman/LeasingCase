using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LeasingCase
{
    public class ToggleRail : MonoBehaviour
    {
        [SerializeField] private RailBehaviour _railA;
        [SerializeField] private RailBehaviour _railB;

        private RailBehaviour _activeRail;
        private RailBehaviour _inactiveRail;

        private bool _hover;
        private List<Transform> _trains = new();

        public bool Hover
        {
            get => _hover;
            private set
            {
                _hover = value;
                _inactiveRail.SetRenderMode(
                    _hover ? RailRenderMode.Transparent : RailRenderMode.Hidden);
            }
        }

        private void Start()
        {
            if (!_railA || !_railB)
            {
                Debug.LogError($"Rail missing in rail toggler {name}");
                return;
            }

            _activeRail = _railA;
            _activeRail.Collider.enabled = true;
            _activeRail.SetRenderMode(RailRenderMode.Opaque);
            _inactiveRail = _railB;
            _inactiveRail.Collider.enabled = false;
            _inactiveRail.SetRenderMode(RailRenderMode.Hidden);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Train") && !_trains.Contains(other.transform))
                _trains.Add(other.transform);
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.CompareTag("Train") && _trains.Contains(other.transform))
                _trains.Remove(other.transform);
        }

        private void OnMouseEnter()
        {
            Hover = true;
        }

        private void OnMouseExit()
        {
            Hover = false;
        }

        private void OnMouseDown()
        {
            if (_trains.Count > 0)
                return;
            
            _activeRail.Collider.enabled = false;
            _activeRail.SetRenderMode(RailRenderMode.Transparent);
            _inactiveRail.Collider.enabled = true;
            _inactiveRail.SetRenderMode(RailRenderMode.Opaque);

            var temp = _activeRail;
            _activeRail = _inactiveRail;
            _inactiveRail = temp;
        }
    }

}
