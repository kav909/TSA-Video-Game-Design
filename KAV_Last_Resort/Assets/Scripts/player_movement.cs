using UnityEngine;

public class player_movement : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator animator;
    public int facingDirection = 1; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       float horizontal = Input.GetAxis("Horizontal");
       float vertical = Input.GetAxis("Vertical");

        if (horizontal > 0 && transform.localScale.x < 0 || horizontal < 0 && transform.localScale.x > 0) 
        { 
            Flip();
        }
        animator.SetFloat("horizontal", Mathf.Abs(horizontal));
        animator.SetFloat("vertical", Mathf.Abs(vertical));
        rb.linearVelocity = new Vector2(horizontal, vertical ).normalized * speed;
       
    }
    void Flip()
    {
        facingDirection *= -1;
         transform.localScale = new Vector3(transform.localScale.x *-1, transform.localScale.y,transform.localScale.z);
       
    }

}
