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

    public MobState currentState = MobState.Wander;
    private MobState defaultState;

    
    void Start()
    {
        defaultState = currentState;
        SwitchState(currentState);
        track.enabled = true;
        patrol.enabled = false;
        wander.enabled = true;
    }

   
    void Update()
    {
        if (canTrack()) 
        {
            SwitchState(MobState.Track);
            track.enabled = true;
        }
        track.cycleComplete = mob.GetComponent<mob_patrol>().IsCycleComplete();

       
        /*if (currentState == MobState.Patrol && patrol.IsCycleComplete())
        {
            SwitchState(MobState.Wander);
            
            track.cycleComplete = false;
        }
        if (currentState != MobState.Patrol && track.canSeePlayer())
        {
            SwitchState(MobState.Track);
        }*/
       
    }

    public void SwitchState(MobState newState)
    {
        currentState = newState;
        patrol.enabled = newState == MobState.Patrol;
        //track.enabled =newState == MobState.Track;
        wander.enabled = newState == MobState.Wander;


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Player"))
        {
            Debug.Log("HItt");
            track.cycleComplete = false;
            SwitchState(MobState.Patrol);
            

        }
    }

    /*private void OnTriggerExit2D(Collider2D collision)
     {
         if(collision.CompareTag("Player"))
         {
             SwitchState(MobState.Wander);
            track.cycleComplete = false;

        }
     }*/
    private bool canTrack() 
    {
        return mob.GetComponent<mob_player_track>().canTrackPlayer();
    }

    private bool cycleCompelete()
    {
        return (mob.GetComponent<mob_patrol>().IsCycleComplete());
        
    }
}
