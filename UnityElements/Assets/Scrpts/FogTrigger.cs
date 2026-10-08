using UnityEngine;
public class FogTrigger : MonoBehaviour
{
    public GameObject fogEffect;
    void Start()
    {
        fogEffect.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            fogEffect.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            fogEffect.SetActive(false);
        }
    }
}