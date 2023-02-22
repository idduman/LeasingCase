using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;

namespace LeasingCase
{
    public enum RailEntryType
    {
        None,
        Forward,
        Backward,
    }

    public enum RailRenderMode
    {
        Opaque,
        Transparent,
        Hidden,
    }
    

    [RequireComponent(typeof(CinemachinePath))]
    [RequireComponent(typeof(Collider))]
    public class RailBehaviour : MonoBehaviour
    {
        public CinemachinePath Path { get; private set; }
        public Collider Collider { get; private set; }

        private Transform _model;
        private Renderer[] _renderers;
        
        private static readonly float Treshold = 0.15f;
        private static readonly float TransparentModeAlpha = 0.5f;
        private static readonly float TransparentModeOffset = 0.1f;

        private Vector3 _pos1;
        private Vector3 _pos2;
        // Start is called before the first frame update
        void Awake()
        {
            Path = GetComponent<CinemachinePath>();
            Collider = GetComponent<Collider>();
            _model = transform.Find("Model");
            _renderers = _model.GetComponentsInChildren<Renderer>();
            if (Path.m_Waypoints.Length < 2)
            {
                Debug.LogError($"Waypoint on {name} has less than 2 waypoints!");
                return;
            }
            _pos1 = Path.m_Waypoints[0].position;
            _pos2 = Path.m_Waypoints[1].position;
        }

        public RailEntryType CheckEntryType(Vector3 position)
        {
            var pos = transform.position;
            
            var pos1World = transform.TransformPoint(_pos1);
            pos1World.y = pos.y;
            if (Vector3.Distance(position, pos1World) < Treshold)
            {
                return RailEntryType.Forward;
            }
            var pos2World = transform.TransformPoint(_pos2);
            pos2World.y = pos.y;
            if (Vector3.Distance(position, pos2World) < Treshold)
            {
                return RailEntryType.Backward;
            }
            return RailEntryType.None;
        }

        public void SetRenderMode(RailRenderMode renderMode)
        {
            var alpha = renderMode is RailRenderMode.Opaque ? 1f :
                renderMode is RailRenderMode.Hidden ? 0f : TransparentModeAlpha;

            var offset = renderMode is RailRenderMode.Transparent ? TransparentModeOffset : 0f;
            _model.localPosition = new Vector3(0f, offset, 0f);
            
            _model.gameObject.SetActive(renderMode is not RailRenderMode.Hidden);

            foreach (var r in _renderers)
            {
                var color = r.material.color;
                var colorNew = new Color(color.r, color.g, color.b, alpha);
                r.material.SetColor("_Color", colorNew);
            }
        }
        
    }
}