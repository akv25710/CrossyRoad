using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Hopper {
    public class GenerateVehicle : MonoBehaviour {

        [SerializeField] private float vehicleGenerationTime;
        [SerializeField] private bool isLeft;
        [SerializeField] private Transform initialPosition;
        [SerializeField] private Transform finalPosition;
        [SerializeField] private VehicleType vehicleType;

        [SerializeField] private List<TrafficLights> trafficLights;

        private float _elaspedTime;
        private int _speed;

        private bool _isRedLightOn;
        private bool _isGreenLightOn;

        private void Start() {
            int currentLevel = HopperGameManager.GetInstance().GetCurrentLevel();
            _speed = 10 + Random.Range( 0 , currentLevel * 10);
            _speed = Math.Min(_speed, 40);
        }

        private void Update() {
            
            _elaspedTime += Time.deltaTime;

            if (vehicleType == VehicleType.Train) {
                if (!_isRedLightOn && _elaspedTime > vehicleGenerationTime - 0.5f) {
                    foreach (var light in trafficLights) {
                        light.TurnGreenLightOff();
                        light.TurnRedLightOn();
                    }

                    _isRedLightOn = true;
                    _isGreenLightOn = false;
                }
                
                if (!_isGreenLightOn && _elaspedTime > vehicleGenerationTime + 2.0f || _elaspedTime < vehicleGenerationTime - 0.5f) {
                    foreach (var light in trafficLights) {
                        light.TurnGreenLightOn();
                        light.TurnRedLightOff();
                    }
                    
                    _isRedLightOn = false;
                    _isGreenLightOn = true;
                }
            }
                 

            if (_elaspedTime < vehicleGenerationTime) {
                return;
            }
            _elaspedTime = 0.0f;
            
            var vehicle = VehiclesPool.GetInstance()?.GetObjectFromPool(vehicleType);
            vehicle.SetActive(true);

            if (isLeft) {
                vehicle.GetComponent<MoveVehicle>().SetMovingSide(isLeft, initialPosition.position, finalPosition.position , _speed);
            } else {
                vehicle.GetComponent<MoveVehicle>().SetMovingSide(isLeft, finalPosition.position, initialPosition.position , _speed);
            }

            switch (vehicleType) {
                case VehicleType.Thela :
                    vehicleGenerationTime = 15.0f;
                    break;
                case  VehicleType.Train:
                    vehicleGenerationTime = Random.Range(4.0f,9.0f);
                    break;
                case VehicleType.Vehicle :
                    float maxGenerationTime = Math.Max(8 - HopperGameManager.GetInstance().GetCurrentLevel(), 4.0f);
                    vehicleGenerationTime = Random.Range(1.5f, maxGenerationTime);
                    break;
            }
        }
    }
}
