using UnityEngine;

namespace LeasingCase
{
    [RequireComponent(typeof(RailBehaviour))]
    public class TrainSpawn : MonoBehaviour
    {
        private RailBehaviour _rail;

        private Transform _levelTransform;

        private void Awake()
        {
            _rail = GetComponent<RailBehaviour>();
        }

        public void Initialize()
        {
            _levelTransform = GameManager.Instance.CurrentLevel.transform;
        }

        public void Spawn(TrainColor colorA, TrainColor colorB)
        {
            var trainPrefab = GameManager.Instance.TrainPrefabs.GetTrain(colorA, colorB);
            if(!trainPrefab)
            {
                Debug.LogError($"Train of {colorA},{colorB} not found in Config!");
                return;
            }

            var train = Instantiate(trainPrefab,
                _rail.Path.EvaluatePosition(0), _rail.Path.EvaluateOrientation(0)
                , _levelTransform);
            train.SetRail(_rail);
        }
    }

}
