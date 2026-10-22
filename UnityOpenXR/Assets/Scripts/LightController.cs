 using UnityEngine;
 public class LightController : MonoBehaviour
 {
     [SerializeField] private Light targetLight;
     
     public void ToggleLight()
     {
         targetLight.enabled = !targetLight.enabled;
     }
     
     public void SetIntensity(float value)
     {
         targetLight.intensity = value;
     }
 }