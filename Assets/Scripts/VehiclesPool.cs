using System.Collections.Generic;
using UnityEngine;

namespace Hopper {
    public class VehiclesPool : MonoBehaviour {
    
        [SerializeField] private List<GameObject> vehiclesPrefab;
        [SerializeField] private List<GameObject> trainPrefab;
        [SerializeField] private List<GameObject> thelaPrefab;

        private Queue<GameObject> _vehicleQueue;
        private Queue<GameObject> _trainQueue;
        private Queue<GameObject> _thelaQueue;

        private static VehiclesPool _instance;

        public static VehiclesPool GetInstance() {
            return _instance;
        }
        
        private void Awake() {
            if (_instance == null) {
                _instance = this;
            } else {
                Debug.LogWarning("Another instance of vehicles pool present");
            }
            _vehicleQueue = new Queue<GameObject>(); 
            _thelaQueue = new Queue<GameObject>();
            _trainQueue = new Queue<GameObject>();
        }

        public GameObject GetObjectFromPool(VehicleType type) {

            switch (type) {
                case VehicleType.Train:
                    if (_trainQueue.Count == 0) {
                        GameObject poolObject = Instantiate(trainPrefab[Random.Range(0, trainPrefab.Count)], transform);
                        poolObject.SetActive(false);
                        _trainQueue.Enqueue(poolObject);
                    }
                    return _trainQueue.Dequeue();
                
                case VehicleType.Thela:
                    if (_thelaQueue.Count == 0) {
                        GameObject poolObject = Instantiate(thelaPrefab[Random.Range(0, thelaPrefab.Count)], transform);
                        poolObject.SetActive(false);
                        _thelaQueue.Enqueue(poolObject);
                    }
                    return _thelaQueue.Dequeue();
                
                case VehicleType.Vehicle:
                    if (_vehicleQueue.Count == 0) {
                        GameObject poolObject = Instantiate(vehiclesPrefab[Random.Range(0, vehiclesPrefab.Count)], transform);
                        poolObject.SetActive(false);
                        _vehicleQueue.Enqueue(poolObject);
                    }
                    return _vehicleQueue.Dequeue();
            }
            return null;
        }

        public void PutObjectInPool(GameObject poolObject, VehicleType type) {
            poolObject.SetActive(false);
            switch (type) {
                case VehicleType.Train:
                    _trainQueue.Enqueue(poolObject);
                    break;
                
                case VehicleType.Thela:
                    _thelaQueue.Enqueue(poolObject);
                    break;
                
                case VehicleType.Vehicle:
                    _vehicleQueue.Enqueue(poolObject);
                    break;
            }
        }
    }
}
