﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Hopper {
    public class HopperGameManager : MonoBehaviour {
        
        [SerializeField] private List<GameObject> scenePrefabs;
        [SerializeField] private Vector3 initialPosition;

        private List<Transform> _prefabCollection = new List<Transform>();
        private Vector3 _lastPosition;

        private int _currentScore;
        private int _gameScore;
        private int _currentLevel;

        private static HopperGameManager _instance;

        public static HopperGameManager GetInstance() {
            return _instance;
        }

        private void Awake() {
            if (_instance == null) {
                _instance = this;
            } else {
                Debug.LogError("Another HopperGameManager instance already present");
            }
            _currentLevel = 1;
        }

        private void Start() {
            UiScreenManager.GetInstance().SetStartScreen();
            _lastPosition = initialPosition;
            for (int i = 0; i < 4; i++) {
                AddNextScenePrefab();
                DecreaseLevel();
            }
        }

        public void AddNextScenePrefab() {
            if (_prefabCollection.Count > 0) {
                var lastItem =
                    _prefabCollection[_prefabCollection.Count - 1];
                _lastPosition = new Vector3(lastItem.transform.position.x + lastItem.GetComponent<SceneDescription>().GetSceneLength(), 0, 0);
            }
            
            int maxSceneRange = 3;
            
            if (_currentLevel > 2) {
                maxSceneRange = scenePrefabs.Count;
            }
            var nextPrefab = Instantiate(scenePrefabs[Random.Range(0, maxSceneRange)], _lastPosition, Quaternion.identity, transform);
            _prefabCollection.Add(nextPrefab.transform);

            // if (_prefabCollection.Count > 10) {
            //     var sceneObj = _prefabCollection[0];
            //     _prefabCollection.RemoveAt(0);
            //     Destroy(sceneObj.gameObject);
            // }
            
            IncreaseLevel();
        }

        public void IncrementScore() {
            _currentScore++;
            _gameScore = Math.Max(_gameScore, _currentScore);
            UiScreenManager.GetInstance().SetScoreText(_gameScore);
        }

        public void DecrementScore() => _currentScore--;

        public int GetScore() => _gameScore;

        private void IncreaseLevel() => _currentLevel++;
        
        private void DecreaseLevel() => _currentLevel--;
        
        public int GetCurrentLevel() => _currentLevel;

        public int GetCurrentSceneChildCount() =>
            _prefabCollection[GetCurrentLevel()].GetComponent<SceneDescription>().GetChildCount();

    }
}