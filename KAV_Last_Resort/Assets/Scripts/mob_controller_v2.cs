using System.Collections;
using UnityEngine;

public class mob_controller_v2 : MonoBehaviour
{
    [SerializeField] mob_patrol patrol;
    [SerializeField] mob_player_track_v2 track;
    [SerializeField] mob_wander wander;
    [SerializeField] GameObject mob;
    bool isIdling = false;

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
        
        //SwitchState(MobState.TrackPlayer); is another of inactivating the other states
        if (currentState == MobState.Patrol && patrol.IsCycleComplete())
        {
            
            track.SetControllerRequirements(false);
            SwitchState(MobState.Idle);
            IdleThen(MobState.Wander, 2f);
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
            IdleThen(MobState.Wander, 2f);
        }
    }

    public void SwitchState(MobState newState)
    {
        mob.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        currentState = newState;
        patrol.enabled = newState == MobState.Patrol;
        wander.enabled = newState == MobState.Wander;


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            Debug.Log("HItt");
            patrol.restt();
            SwitchState(MobState.Patrol);
   
            track.SetControllerRequirements(false);

        }
    }
   private void IdleThen(MobState nextState, float delay)
    {
        if (isIdling) return; 
        StartCoroutine(IdleRoutine());

        IEnumerator IdleRoutine()
        {
            isIdling = true;
            yield return new WaitForSeconds(delay);
            isIdling = false;
            SwitchState(nextState);
        }
    }



}
