using System.Collections;
using System.Collections.Generic;
using LeasingCase;
using UnityEngine;

namespace LeasingCase
{
    [RequireComponent(typeof(RailBehaviour))]
    public class TrainDestination : MonoBehaviour
    {
        [SerializeField] private TrainColor _colorA;
        [SerializeField] private TrainColor _colorB;

        public bool CompareColors(TrainColor colorA, TrainColor colorB)
        {
            return colorA == _colorA && colorB == _colorB
                   || colorA == _colorB && colorB == _colorA;
        }
    }
}