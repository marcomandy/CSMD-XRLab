using UnityEngine;
public class PlayerMove : MonoBehaviour
{
    private Animator animator;
    void Awake()
    {
    animator = GetComponent<Animator>();
    }
    void Update()
    {
    float vertical = Input.GetAxis("Vertical");
    bool walking = Mathf.Abs(vertical) > 0.1f;
    animator.SetBool("IsWalking", walking);
    }
}