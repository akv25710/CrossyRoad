using UnityEngine;

namespace Hopper {
    public class DestroyOnInvisible : MonoBehaviour {

        [SerializeField] private float maxDistance;
        private GameObject _character;

        private void Start() {
            _character = GameObject.FindGameObjectWithTag("player");
        }

        private void Update() {
            var diff = _character.transform.position.x - transform.position.x;
            if (diff > maxDistance) {
                gameObject.SetActive(false);
            }
        }
    }
}
