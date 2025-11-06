using System.Collections;
using UnityEngine;

public class mob_controller_v2 : MonoBehaviour
{
    [SerializeField] mob_patrol patrol;
    [SerializeField] mob_player_track_v2 track;
    [SerializeField] mob_wander wander;
    [SerializeField] GameObject mob;
    
    float time =0f;

    [SerializeField] float hitPauseDuration = 3f;
    Coroutine GoIdle;
    public enum MobState
    {
        Default,
        Idle,
        Patrol,
        Wander,
        TrackPlayer,

    }

    public MobState currentState = MobState.Wander;
    private MobState defaultState;


    void Start()
    {
        defaultState = currentState;
        SwitchState(currentState);
        track.SetControllerRequirements(false);
    }


    void Update()
    {
        
        
        if (currentState == MobState.Patrol && patrol.IsCycleComplete())
        {
            
            track.SetControllerRequirements(false);
            SwitchState(MobState.Idle);
            return;
        }

       
        if (currentState != MobState.Patrol && track.GetCanTrack())
        {
            track.SetControllerRequirements(true);
            SwitchState(MobState.TrackPlayer);
            return;
        }

       
        if (currentState == MobState.TrackPlayer && !track.GetCanTrack())
        {
            track.SetControllerRequirements(false);
            SwitchState(MobState.Idle);
            
        }
        if (currentState == MobState.Idle)
        {
            time += Time.deltaTime;
            if (time >= 3f)
            {
                time = 0f;
                SwitchState(MobState.Wander);
            }
        }
        

    }

    public void SwitchState(MobState newState)
    {
        mob.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        currentState = newState;
        if (newState == MobState.Patrol)
        {
            patrol.ResetPatrol();
        }

        patrol.enabled = newState == MobState.Patrol;
        wander.enabled = newState == MobState.Wander;
        if(newState == MobState.Idle)
        {
           time = 0f;
        }

    }





    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            StartCoroutine(GoIldle());
        }
        
    }

    private IEnumerator GoIldle()
    {
        Debug.Log("Hit");
        SwitchState(MobState.Idle);
        track.SetControllerRequirements(false);

        yield return new WaitForSeconds(hitPauseDuration);
        SwitchState(MobState.Patrol);
    }




}
