using UnityEngine;

public class mob_controller : MonoBehaviour
{
    [SerializeField] mob_patrol patrol;
    [SerializeField] mob_player_track track;
    [SerializeField] mob_wander wander;

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
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        defaultState = currentState;
        SwitchState(currentState);
    }

    // Update is called once per frame
    void Update()
    {
        
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
        if(collision.CompareTag("Player"))
        {
            SwitchState(MobState.Wander);
                
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            SwitchState(defaultState);
                
        }
    }
}
