using System.Collections;
using UnityEngine;

public class mob_wander : MonoBehaviour
{
    [Header("Wandering Area")]
    [SerializeField] float wanderWidth;
    [SerializeField] float wanderHeight;
    [SerializeField] float speed;
    [SerializeField] float puaseDuration;

    public Vector2 wanderOrigin;
    private bool isPaused = false;
    private Rigidbody2D rb;
    private Vector2 target;
    private Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //aimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPaused) 
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
        if (Vector2.Distance(transform.position, target) < 0.1f) 
        {
             StartCoroutine(PauseAndPickNewDestination());
        }
        Move();

    }
    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.deepPink;
        Gizmos.DrawWireCube(wanderOrigin, new Vector2(wanderWidth, wanderHeight));
    }
    private Vector2 GetRandomTarget() 
    {
        float halfWidth = wanderWidth / 2f;
        float halfHeight = wanderHeight / 2f;
        int edge = Random.Range(0, 4);

        return edge switch
        {
            0 => new Vector2(wanderOrigin.x - halfWidth, Random.Range(wanderOrigin.y - halfHeight, wanderOrigin.y + halfHeight)),
            1 => new Vector2(wanderOrigin.x + halfWidth, Random.Range(wanderOrigin.y - halfHeight, wanderOrigin.y + halfHeight)),
            2 => new Vector2(Random.Range(wanderOrigin.x - halfWidth, wanderOrigin.x + halfWidth), wanderOrigin.y - halfHeight),
            _ => new Vector2(Random.Range(wanderOrigin.x - halfWidth, wanderOrigin.x + halfWidth), wanderOrigin.y + halfHeight),

        };
    }
    private void OnEnable()
    {
        target = GetRandomTarget();
    }
    IEnumerator PauseAndPickNewDestination()
    {
        isPaused = true;
        //animator.Play("Idle");
        yield return new WaitForSeconds(puaseDuration);
        target = GetRandomTarget();
        isPaused = false;
        //animator.Play("Walk");
    }

    private void Move() 
    {
        Vector2 direction = target - (Vector2)transform.position;
        if(direction.x < 0 && transform.localScale.x > 0 || direction.x > 0 && transform.localScale.x < 0) 
        {
            transform.localScale = new Vector3(transform.localScale.x *-1, transform.localScale.y, transform.localScale.z);
        }
        rb.linearVelocity = direction.normalized * speed;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine(PauseAndPickNewDestination());
    }
}
