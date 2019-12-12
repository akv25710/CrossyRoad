using UnityEngine;

namespace Hopper {
    
    public  class SceneDescription : MonoBehaviour {
        [SerializeField] private float sceneLength;
        [SerializeField] private int childCount;

        public int GetChildCount() => childCount;

        public float GetSceneLength() => sceneLength;
    }
}

