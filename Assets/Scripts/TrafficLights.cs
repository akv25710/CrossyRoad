using UnityEngine;

namespace Hopper {
    public class TrafficLights : MonoBehaviour {
        [SerializeField] private GameObject redLight;
        [SerializeField] private GameObject greenLight;

        public void TurnRedLightOn() => redLight.SetActive(true);
        
        public void TurnGreenLightOn() => greenLight.SetActive(true);
        
        public void TurnRedLightOff() => redLight.SetActive(false);
        
        public void TurnGreenLightOff() => greenLight.SetActive(false);
    }
}
