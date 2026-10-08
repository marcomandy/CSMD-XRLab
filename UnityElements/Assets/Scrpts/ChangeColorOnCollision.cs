using System;
using UnityEngine;

public class ChangeColorOnCollision : MonoBehaviour
{
    private Renderer renderer;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Color randomColor = getRandomColor();
            if(!renderer)
            {
                renderer = GetComponent<Renderer>();
            }
            
            renderer.material.color = randomColor;
        }
    }

    private Color getRandomColor()
    {
        return new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
    }
    
}
