using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace LeasingCase
{
    [CreateAssetMenu(fileName = TypeName, menuName = MenuName)]
    public class TrainPrefabData : ScriptableObject
    {
        private const string TypeName = nameof(TrainPrefabData);
        private const string MenuName = "Data/" + TypeName;
        // Start is called before the first frame update

        [SerializeField] private List<TrainBehaviour> TrainPrefabs = new();

        public TrainBehaviour GetTrain(TrainColor colorA, TrainColor colorB)
        {
            //Debug.Log($"ColorA {colorA} ColorB {colorB}");
            var filteredList = TrainPrefabs.Where(x => x.CompareColors(colorA, colorB)).ToList();
            return filteredList[Random.Range(0, filteredList.Count)];
        }
    }
}

