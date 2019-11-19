using UnityEngine;

namespace Hopper {
    public class CameraMovement : MonoBehaviour {
    
        [SerializeField] private CharacterMovement character;
        [SerializeField] private float speed;
        [SerializeField] private float maxCameraDistance;
        [SerializeField] private float cameraXOffset;
        

        private Vector3 _offSet;
        private bool _moveCamera;
        private float _initialXPosCamera;

        private void Awake() {
            _offSet = transform.position - character.transform.position;
            _initialXPosCamera = transform.position.x;
        }
        
        private void LateUpdate() {
            if (!_moveCamera) {
                return;
            }

            if (transform.position.x > character.transform.position.x + _initialXPosCamera + cameraXOffset) {
                character.SetDeadState();
                return;
            }
            
            Vector3 pos = character.transform.position + _offSet;

            if (transform.position.x < pos.x  - maxCameraDistance) {
                transform.position += new Vector3(4 * speed * Time.deltaTime, 0, 0);
            } else if (transform.position.x < pos.x) {
                transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
            } else {
                transform.position += new Vector3(speed/3 * Time.deltaTime, 0 , 0);
            }
            
            transform.position = new Vector3(transform.position.x, transform.position.y, pos.z);
        }
        
        public void SetCameraMovement(bool enableCameraMovement) {
            _moveCamera = enableCameraMovement;
        }
    }
}
