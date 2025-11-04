using System.Collections;
using UnityEngine;

public class mob_controller_v2 : MonoBehaviour
{
    [SerializeField] mob_patrol patrol;
    [SerializeField] mob_player_track_v2 track;
    [SerializeField] mob_wander wander;
    [SerializeField] GameObject mob;
    bool isIdling = false;
    float time =0f;

    [SerializeField] float hitPauseDuration = 0.5f;
    Coroutine hitPauseCoroutine;
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
        patrol.enabled = newState == MobState.Patrol;
        wander.enabled = newState == MobState.Wander;
        if(newState == MobState.Idle)
        {
           time = 0f;
        }

    }





    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        Debug.Log("TriggerEnter with Player");
        if (hitPauseCoroutine != null) StopCoroutine(hitPauseCoroutine);
        // enter idle using the centralized method so component toggles run
        SwitchState(MobState.Idle);
        hitPauseCoroutine = StartCoroutine(HitPauseRoutine());
    }

    IEnumerator HitPauseRoutine()
    {
        Debug.Log("HitPause start");
        var rb = mob.GetComponent<Rigidbody2D>();
        mob.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;

        // explicitly disable controllers so nothing moves the mob while paused
        patrol.enabled = false;
        wander.enabled = false;
        track.SetControllerRequirements(false);

        // wait
        yield return new WaitForSeconds(hitPauseDuration);

        Debug.Log("HitPause end, resuming Patrol");
        // resume desired behavior (use SwitchState so toggles happen)
        SwitchState(MobState.Patrol);
        hitPauseCoroutine = null;
    }




}
