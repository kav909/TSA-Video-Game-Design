using UnityEngine;

public class mob_controller : MonoBehaviour
{
    [SerializeField] mob_patrol patrol;
    [SerializeField] mob_player_track track;
    [SerializeField] mob_wander wander;
    [SerializeField] GameObject mob;

    public enum MobState
    {
        Default,
        Idle,
        Patrol,
        Wander,
        Track,

    }

    public MobState currentState = MobState.Patrol;
    private MobState defaultState;

    bool isColliding = false;
    void Start()
    {
        defaultState = currentState;
        SwitchState(currentState);
    }

   
    void Update()
    {
        if(canTrack()&& !isColliding)
        {
            SwitchState(MobState.Track);
        }
        else if(currentState == MobState.Track)
        {
            SwitchState(defaultState);
        }
    }

    public void SwitchState(MobState newState)
    {
        currentState = newState;
        patrol.enabled = newState == MobState.Patrol;
        track.enabled = newState == MobState.Track;
        wander.enabled = newState == MobState.Wander;


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isColliding = true;
        if (collision.CompareTag("Player"))
        {
            SwitchState(MobState.Patrol);
                
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            SwitchState(defaultState);
                
        }
    }
    private bool canTrack() 
    {
        return mob.GetComponent<mob_player_track>().canSeePlayer();
    }
}
