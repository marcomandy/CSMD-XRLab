using UnityEngine;

public class ChangeColorOnTrigger : MonoBehaviour
{
    private Renderer renderer;

    private void OnTriggerEnter(Collider collision)
    {
        Color randomColor = getRandomColor();
        if(!renderer)
        {
            renderer = GetComponent<Renderer>();
        }
        
        renderer.material.color = randomColor;
    }

    private Color getRandomColor()
    {
        return new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
    }
}
