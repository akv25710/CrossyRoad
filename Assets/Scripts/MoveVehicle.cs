using UnityEngine;

namespace Hopper {
    public class MoveVehicle : MonoBehaviour {

        [SerializeField] private VehicleType vehicleType;
        [SerializeField] private int speed;
        
        private Vector3 _endingPosition;

        [HideInInspector] public bool isLeft;
        private bool _areValuesSet;
    
        private void Update() {
            if (!_areValuesSet) {
                return;
            }
            var dir = isLeft ? -1 : 1;
        
            transform.position = new Vector3(transform.position.x , transform.position.y , transform.position.z + dir * speed * Time.deltaTime);

            if (isLeft && transform.position.z < _endingPosition.z) {
                VehiclesPool.GetInstance()?.PutObjectInPool(gameObject, vehicleType);
            }

            if (!isLeft && transform.position.z > _endingPosition.z) {
                VehiclesPool.GetInstance()?.PutObjectInPool(gameObject, vehicleType);
            }
        }

        public void SetMovingSide(bool isLeft, Vector3 start, Vector3 end, int generatedSpeed) {
            this.isLeft = isLeft;
            transform.position = start;
            _endingPosition = end;
            if (vehicleType == VehicleType.Vehicle) {
                speed = generatedSpeed;
            }
            _areValuesSet = true;

            if (vehicleType == VehicleType.Thela) {
                transform.rotation = Quaternion.Euler(-90, isLeft ? -90 : 90, transform.rotation.z);
            } else if (vehicleType == VehicleType.Train) {
                transform.rotation = Quaternion.Euler(0,0,0);
            } else if (vehicleType == VehicleType.Vehicle) {
                transform.rotation = Quaternion.Euler(transform.eulerAngles.x, isLeft ? 180 : 0, transform.eulerAngles.z);
            }

        }
    }
}
