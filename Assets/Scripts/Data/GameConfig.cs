using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace LeasingCase
{
    [CreateAssetMenu(fileName = TypeName, menuName = MenuName)]
    public class GameConfig : ScriptableObject
    {
        private const string TypeName = nameof(GameConfig);
        private const string MenuName = "Data/" + TypeName;

        public float TrainMoveSpeed = 2f;
    }
}

