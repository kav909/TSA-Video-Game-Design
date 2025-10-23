using UnityEngine;
using static UnityEditor.PlayerSettings;

public class time_puzzle_worldX_fixed : MonoBehaviour
{
    [SerializeField] GameObject bar;
    [SerializeField] GameObject circle;
    [SerializeField] GameObject mark;
    [SerializeField] GameObject test;

    [SerializeField] Transform left;
    [SerializeField] Transform right;
    

    [SerializeField] float speed = 2f;

    private int direction = 1;


    private float waitTime = 200f;
    private float timeWaited = 0f;


    void Start()
    {
        
    }
    //have to use y axis because the bar is rotated 90 degrees causing x and y to be swapped
    void Update()
    {
        timeWaited += Time.deltaTime * 1000f;
        if (Input.GetKey(KeyCode.Space) && timeWaited > waitTime) 
        {
            chcekHit();
            

            timeWaited = 0f;
        }
        Vector2 target = currentTarget();
        circle.transform.position = Vector3.Lerp(circle.transform.position, target,speed* Time.deltaTime);

        float distance = (target - (Vector2)circle.transform.position).magnitude;
        if (distance < 0.1f)
        {
            direction *= -1;
        }

    }

    private Vector2 currentTarget()
    {
        if (direction == 1)
        {
            return right.position;
        }
        else
        {
            return left.position;
        }
        /*if (  circle.transform.position.y >= right.position.y&& movingRight)
        {
            circle.transform.position = Vector3.SmoothDamp(circle.transform.position, left.position, ref velocity, .2f );
            movingRight = false;
            Debug.Log("Right reached");
        }
        else if ( circle.transform.position.y <= left.position.y&& !movingRight )
        {
            movingRight = true;
            Debug.Log("Left reached");
            circle.transform.position = Vector3.SmoothDamp(circle.transform.position, right.position, ref velocity, .2f);
        }*/
    }

    private void markMove()
    {
        float randomNumber = Random.Range(left.position.x, right.position.x);
        
        mark.transform.position = new Vector2(randomNumber, mark.transform.position.y);
        Debug.Log("Mark pos: " + randomNumber);

    }

    private void chcekHit() 
    {
        float markPos= mark.transform.position.x;
        float circlePos= circle.transform.position.x;
        float distance = Mathf.Abs(markPos - circlePos);
        if (distance <= circle.GetComponent<SpriteRenderer>().bounds.size.x )
        {
            Debug.Log("Hit!");
            markMove();
        }
        else
        {
            Debug.Log("Missed!");
        }
    }
}