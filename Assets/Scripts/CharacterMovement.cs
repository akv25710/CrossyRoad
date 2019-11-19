using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hopper {
    public class CharacterMovement : MonoBehaviour {

        [SerializeField] private Transform playerMesh;
        [SerializeField] private CameraMovement cameraMovement;
    
        [SerializeField] private float verticalTraversalDistance;
        [SerializeField] private float horizontalTraversalDistance;
        [SerializeField] private float speed;
        [SerializeField] private float minSwipeDistance;
        [SerializeField] private float jumpSpeedVertical;
        [SerializeField] private float jumpSpeedHorizontal;

        private const string TAG_ENEMY = "enemy";
        private const string TAG_GRASS_PLAIN = "grass_plain";
        private const string TAG_ROAD_PLAIN = "road_plain";
        private const string TAG_OBSTACLE = "obstacle";
        private const string TAG_BOUNDARY = "boundary";

        private bool _isMovingUp, _isMovingDown, _isMovingLeft, _isMovingRight;
        private bool _isDead;
        private bool _isSwipe;
        private bool _hasGameStarted;

        private float _midwayPoint;
        private float _initialYPos;
        private float _lastYPos;
        
        private int _characterMovedAhead = -3;

        private Vector2 _fingerStartPos;

        private Vector3 _finalPosition;

        private Movement _movementType;

        private void Awake() {
            _initialYPos = transform.position.y;
        }

        private void Update() {
            if (_isDead || !_hasGameStarted) {
                return;
            }

#if UNITY_EDITOR
           KeyboardControls(); 
#endif
            CharacterController();

            MoveCharacters();
        }


        private bool IsCharacterMoving() {
            return _isMovingUp || _isMovingDown || _isMovingLeft || _isMovingRight;
        }

        private void MoveCharacters() {
            if (!IsCharacterMoving()) {
                return;
            }
            
            if (_isMovingUp && transform.position.x < _finalPosition.x) {
                if (transform.position.x < _midwayPoint) {
                    transform.position += new Vector3(speed*Time.deltaTime, jumpSpeedVertical*Time.deltaTime ,0);
                } else {
                    transform.position += new Vector3(speed*Time.deltaTime, -jumpSpeedVertical*Time.deltaTime ,0);
                }
            } else {
                if (_isMovingUp) {
                    transform.position = _finalPosition;
                }
                _isMovingUp = false;
            }

            if (_isMovingDown && transform.position.x > _finalPosition.x) {
                if (transform.position.x > _midwayPoint) {
                    transform.position -= new Vector3(speed*Time.deltaTime, -jumpSpeedVertical*Time.deltaTime, 0);
                } else {
                    transform.position -= new Vector3(speed*Time.deltaTime, jumpSpeedVertical*Time.deltaTime, 0);
                }
            } else {
                if (_isMovingDown) {
                    transform.position = _finalPosition;
                }
                _isMovingDown = false;
            }
        
            if (_isMovingLeft && transform.position.z < _finalPosition.z) {
                if (transform.position.z < _midwayPoint) {
                    transform.position += new Vector3(0, jumpSpeedHorizontal*Time.deltaTime ,speed * Time.deltaTime );
                } else {
                    transform.position += new Vector3(0, jumpSpeedHorizontal*Time.deltaTime ,speed * Time.deltaTime );
                }
            } else {
                if (_isMovingLeft) {
                    transform.position = _finalPosition;
                }
                _isMovingLeft = false;
            }
        
            if (_isMovingRight && transform.position.z > _finalPosition.z) {
                if (transform.position.z > _midwayPoint) {
                    transform.position -= new Vector3(0, -jumpSpeedHorizontal*Time.deltaTime ,speed * Time.deltaTime );
                } else {
                    transform.position -= new Vector3(0, jumpSpeedHorizontal*Time.deltaTime ,speed * Time.deltaTime );
                }
            } else {
                if (_isMovingRight) {
                    transform.position = _finalPosition;
                }
                _isMovingRight = false;
            }
        }
        
        private void KeyboardControls() {
            if (IsCharacterMoving()) {
                return;
            }
            
            if (Input.GetKeyDown(KeyCode.A)) {
                MoveCharacterLeft();
            }

            if (Input.GetKeyDown(KeyCode.D)) {
                MoveCharacterRight();
            }
            
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKey(KeyCode.Mouse0)) {
                MoveCharacterUp();
            }

            if (Input.GetKeyDown(KeyCode.S)) {
                MoveCharacterDown();
            }
        }

        private void CharacterController() {
            if (Input.touchCount > 0 ) {

                foreach (Touch touch in Input.touches) {
                
                    switch (touch.phase) {
                
                    case TouchPhase.Began :
                        _isSwipe = true;
                        _fingerStartPos = touch.position;
                        break;
                    
                    case TouchPhase.Canceled :
                        _isSwipe = false;
                        break;
                    
                    case TouchPhase.Ended :
                        float horizontalSwipeDistance = touch.position.x - _fingerStartPos.x;
                        float verticalSwipeDistance = touch.position.y - _fingerStartPos.y;

                        if (_isSwipe && !IsCharacterMoving()){
                            if(Math.Abs(horizontalSwipeDistance) > minSwipeDistance ) {
                                if (horizontalSwipeDistance > 0) {
                                    MoveCharacterRight();
                                } else {
                                    MoveCharacterLeft();
                                }
                            } else if(Math.Abs(verticalSwipeDistance) > minSwipeDistance) {
                                if (verticalSwipeDistance > 0) {
                                    MoveCharacterUp();
                                } else {
                                    MoveCharacterDown();
                                }
                            } else {
                                MoveCharacterUp();
                            }
                        }
                        break;
                    }
                }
            }
        }

        private void MoveCharacterUp() {
            if (_isMovingUp) {
                return;
            }
            _finalPosition = transform.position + new Vector3(verticalTraversalDistance, 0 ,0);
            _midwayPoint = transform.position.x + verticalTraversalDistance / 2;
            
            _isMovingUp = true;
            _movementType = Movement.Forward;
            
            _characterMovedAhead++;
            HopperGameManager.GetInstance()?.IncrementScore();

            RotateCharacter(0);

            if (_characterMovedAhead == 9) {
                HopperGameManager.GetInstance().AddNextScenePrefab();
                _characterMovedAhead = 0;
            }
        }

        private void RotateCharacter(int yAngle) {
            transform.eulerAngles = new Vector3(0, yAngle, 0);
        }

        private void MoveCharacterDown() {
            if (_isMovingDown) {
                return;
            }
            _isMovingDown = true;
            _movementType =  Movement.Backward;
            
            _finalPosition = transform.position + new Vector3(-verticalTraversalDistance, 0 ,0);
            _midwayPoint = transform.position.x - verticalTraversalDistance / 2;
            
            HopperGameManager.GetInstance().DecrementScore();
            _characterMovedAhead--;
            
            RotateCharacter(180);
        }

        private void MoveCharacterLeft() {
            if (_isMovingLeft) {
                return;
            }
            _isMovingLeft = true;
            _movementType = Movement.Left;
            
            _finalPosition = transform.position + new Vector3(0, 0 ,horizontalTraversalDistance);
            _midwayPoint = transform.position.z + horizontalTraversalDistance / 2;

            RotateCharacter(-90);
        }
    
        private void MoveCharacterRight() {
            if (_isMovingRight) {
                return;
            }
            _isMovingRight = true;
            _movementType = Movement.Right;
            
            _midwayPoint = transform.position.z - horizontalTraversalDistance / 2;
            _finalPosition = transform.position - new Vector3(0, 0 ,horizontalTraversalDistance);

            RotateCharacter(90);
        }

        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.CompareTag(TAG_ENEMY)) {
                if (_isDead) {
                    return;
                }
                SetDeadState();
            }
            
            if (other.gameObject.CompareTag(TAG_OBSTACLE)) {

                switch (_movementType) {
                    case Movement.Forward : 
                        _isMovingUp = false;
                        _characterMovedAhead--;
                        HopperGameManager.GetInstance().DecrementScore();
                        transform.position = new Vector3(transform.position.x - 4.0f, _lastYPos, transform.position.z);
                        break;
                    
                    case Movement.Backward :
                        _isMovingDown = false;
                        _characterMovedAhead++;
                        HopperGameManager.GetInstance().IncrementScore();
                        transform.position = new Vector3(transform.position.x + 4.0f, _lastYPos, transform.position.z);
                        break;
                    
                    case Movement.Left :
                        _isMovingLeft = false;
                        transform.position = new Vector3(transform.position.x , _finalPosition.y, transform.position.z - 4.0f);
                        break;
                    
                    case Movement.Right :
                        _isMovingRight = false;
                        transform.position = new Vector3(transform.position.x, _finalPosition.y, transform.position.z + 4.0f);
                        break;
                }
               
            } else if (other.gameObject.CompareTag(TAG_GRASS_PLAIN)) {
                _lastYPos = _finalPosition.y;
                _finalPosition.y = 2.0f;
                Debug.Log("Character is on the grass");
                
            } else if (other.gameObject.CompareTag(TAG_ROAD_PLAIN)) {
                _lastYPos = _finalPosition.y;
                _finalPosition.y = 1.0f;
                Debug.Log("Character is on the road");
                
            } else if (other.gameObject.CompareTag(TAG_BOUNDARY)) {
                Debug.Log("Character is at the boundary. Can't move further");
                switch (_movementType) {
                    case Movement.Left :
                        _isMovingLeft = false;
                        transform.position = new Vector3(transform.position.x , _initialYPos, transform.position.z - 2.0f);
                        break;
                        
                    case Movement.Right :
                        _isMovingRight = false;
                        transform.position = new Vector3(transform.position.x, _initialYPos, transform.position.z + 2.0f);
                        break;
                    }
            }
        }

        public void SetDeadState() {
            _isDead = true;
            cameraMovement.SetCameraMovement(false);
            Debug.Log("Character is dead");
            playerMesh.localScale = new Vector3(playerMesh.localScale.x * 1.5f , 0.1f, playerMesh.localScale.z * 1.5f);
            transform.position = new Vector3(transform.position.x, _finalPosition.y, transform.position.z);
            UiScreenManager.GetInstance().SetGameOverScreen();
        }

        public void StartCharacterMovement() {
            _hasGameStarted = true;
        }
    
    }
}
