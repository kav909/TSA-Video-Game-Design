using UnityEngine;

public class mob_player_track_v2 : MonoBehaviour
{
    public GameObject player;
    [SerializeField] float speed = 3f;
    [SerializeField] float distanceThreshold = 5f;
    private float distance = 2f;

    private bool hasLineOfSight = false;

    public bool controllerRequirements = false;
    bool inRange = false;
    bool canTrack = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    //hasLineOfSight- use raycast to see if the mob can see the player
    //inRange- check if the player is within the mob's range
    //canTrack- if both conditions are true, the mob can track the player
    //conrollerRequirements- if the mob's controller allows it to go to the player
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (controllerRequirements)
            MobMove();
        canTrack = inRange && hasLineOfSight;


    }

    private void FixedUpdate()
    {
        hasLineOfSight = false;
        RaycastHit2D[] ray = Physics2D.RaycastAll(transform.position, player.transform.position - transform.position);
        if (ray.Length > 1 && ray[1].collider != null)
        {
            hasLineOfSight = ray[1].collider.gameObject == player;

            if (hasLineOfSight)
            {
                Debug.DrawLine(transform.position, player.transform.position, Color.green);
            }
            else
            {

                Debug.DrawLine(transform.position, player.transform.position, Color.red);

            }
        }
        distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance < distanceThreshold)
        {
            inRange = true;
        }
        else 
        {
            inRange = false;
        }
    }
    private void MobMove() 
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = (player.transform.position - transform.position);
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;



        if (distance < distanceThreshold && hasLineOfSight)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(Vector3.forward * angle);

        }
    }
    public void SetControllerRequirements(bool x) 
    {
        controllerRequirements = x;
    }
    public bool GetCanTrack()
    {
        return canTrack;
    }


}
