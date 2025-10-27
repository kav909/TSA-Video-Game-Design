using System.Linq;
using UnityEngine;

public class mob_player_track : MonoBehaviour
{
    public GameObject player;
    [SerializeField] float speed = 3f;
    [SerializeField] float distanceThreshold = 5f;
    private float distance = 2f;

    private bool hasLineOfSight = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = (player.transform.position - transform.position);
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        

        if(distance < distanceThreshold && hasLineOfSight)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(Vector3.forward * angle);
        }
    }

    private void FixedUpdate()
    {
        hasLineOfSight = false;
        RaycastHit2D[] ray = Physics2D.RaycastAll(transform.position, player.transform.position - transform.position);
        if(ray.Length>1 && ray[1].collider != null)
        {
           hasLineOfSight = ray[1].collider.gameObject == player;

            if (hasLineOfSight) 
            {
                Debug.DrawLine(transform.position, player.transform.position , Color.green);
            }
            else
            {
                
                Debug.DrawLine(transform.position, player.transform.position , Color.red);
            }
        }
        
    }
}
