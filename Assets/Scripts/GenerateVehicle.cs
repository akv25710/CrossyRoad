using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Hopper {
    public class GenerateVehicle : MonoBehaviour {

        [SerializeField] private float vehicleGenerationTime;
        [SerializeField] private bool isLeft;
        [SerializeField] private Transform initialPosition;
        [SerializeField] private Transform finalPosition;
        [SerializeField] private VehicleType vehicleType;

        private float _elaspedTime;
        private int _speed;

        private void Start() {
            int currentLevel = HopperGameManager.GetInstance().GetCurrentLevel();
            _speed = 10 + Random.Range( 0 , currentLevel * 10);
            _speed = Math.Min(_speed, 60);
        }

        private void Update() {
            
            _elaspedTime += Time.deltaTime;

            if (_elaspedTime < vehicleGenerationTime) {
                return;
            }
            _elaspedTime = 0.0f;
            
            var vehicle = VehiclesPool.GetInstance().GetObjectFromPool(vehicleType);
            vehicle.SetActive(true);

            if (isLeft) {
                vehicle.transform.position = initialPosition.position;
                vehicle.transform.rotation = Quaternion.Euler(0,90,0);
                vehicle.GetComponent<MoveVehicle>().SetMovingSide(isLeft, initialPosition.position, finalPosition.position , _speed);
            } else {
                vehicle.transform.position = finalPosition.position;
                vehicle.transform.rotation = Quaternion.Euler(0,-90,0);
                vehicle.GetComponent<MoveVehicle>().SetMovingSide(isLeft, finalPosition.position, initialPosition.position , _speed);
            }

            switch (vehicleType) {
                case VehicleType.Thela :
                    vehicleGenerationTime = Random.Range(3.0f, 20.0f);
                    break;
                case  VehicleType.Train:
                    vehicleGenerationTime = Random.Range(7.0f,15.0f);
                    break;
                case VehicleType.Vehicle :
                    float maxGenerationTime = Math.Max(10 - HopperGameManager.GetInstance().GetCurrentLevel(), 3.5f);
                    vehicleGenerationTime = Random.Range(1.5f, maxGenerationTime);
                    break;
            }
        }
    }
}
