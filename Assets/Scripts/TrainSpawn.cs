using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LeasingCase
{
    [RequireComponent(typeof(RailBehaviour))]
    public class TrainSpawn : MonoBehaviour
    {
        private RailBehaviour _rail;

        private void Awake()
        {
            _rail = GetComponent<RailBehaviour>();
        }

        private void Spawn()
        {
            
        }
    }

}
