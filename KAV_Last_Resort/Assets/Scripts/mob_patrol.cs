using System.Collections;
using System.Xml.Serialization;
using UnityEngine;

public class mob_patrol : MonoBehaviour
{
    [SerializeField] Vector2[] patrolPoints;
    [SerializeField] float speed = 2f;
    private Vector2 currentTarget;
    private Rigidbody2D rb;
    [SerializeField] float pauseDuration = 2f;
    private bool isPaused = false;
    private int currentIndex;
    private Animator animator;

    bool cycleComplete = true;

    void Start()
    {
        currentIndex = 0;
        rb = GetComponent<Rigidbody2D>();
        currentTarget = patrolPoints[currentIndex];
        //animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPaused) 
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
        Vector2 direction  = (Vector3)currentTarget - transform.position;
        if (direction.x < 0 && transform.localScale.x > 0 || direction.x > 0 && transform.localScale.x < 0) 
        {
            transform.localScale = new Vector3(transform.localScale.x *-1, transform.localScale.y, transform.localScale.z);
        }
        rb.linearVelocity = direction.normalized * speed;

        if(Vector2.Distance(transform.position, currentTarget) < 0.1f)
        {
            StartCoroutine(SetPatrolPoint());   
        }
    }
    //IEnumerator +yield return new WaitForSeconds allows us to pause the function withoutpausing the entire game
    IEnumerator SetPatrolPoint()
    {
        //animator.Play("Idle");
        isPaused = true;
        yield return new WaitForSeconds(pauseDuration);
        currentIndex = (currentIndex + 1) % patrolPoints.Length;
        currentTarget = patrolPoints[currentIndex];
        isPaused = false;
        //animator.Play("Walk");
        if (currentIndex == patrolPoints.Length - 1)
        {
            cycleComplete = true;
        }
        else
        {
            cycleComplete = false;
        }
    }

    public bool IsCycleComplete()
    {
        return cycleComplete;
    }
}
