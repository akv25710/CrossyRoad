using UnityEngine;

namespace Hopper {
    public class MoveVehicle : MonoBehaviour {

        [SerializeField] private VehicleType vehicleType;
        [SerializeField] private int speed;
        
        private float _yPos = 1.2f;
    
        private Vector3 _startingPosition;
        private Vector3 _endingPosition;

        private bool _isLeft;
        private bool _areValuesSet;
    
        private void Update() {
            if (!_areValuesSet) {
                return;
            }
            var dir = _isLeft ? -1 : 1;
        
            transform.position = new Vector3(transform.position.x , _yPos , transform.position.z + dir * speed * Time.deltaTime);

            if (_isLeft && transform.position.z < _endingPosition.z) {
                VehiclesPool.GetInstance().PutObjectInPool(gameObject, vehicleType);
            }

            if (!_isLeft && transform.position.z > _endingPosition.z) {
                VehiclesPool.GetInstance().PutObjectInPool(gameObject, vehicleType);
            }
        }

        public void SetMovingSide(bool isLeft, Vector3 start, Vector3 end, int generatedSpeed) {
            _isLeft = isLeft;
            _startingPosition = start;
            _endingPosition = end;
            if (vehicleType == VehicleType.Vehicle) {
                speed = generatedSpeed;
            }
            _areValuesSet = true;

            if (vehicleType == VehicleType.Thela || vehicleType == VehicleType.Train) {
                transform.rotation = Quaternion.Euler(-90, isLeft ? -90 : 90, transform.rotation.z);
            }

        }
    }
}
