using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ScorpionFiniteStateMachine : MonoBehaviour
{
    #region variables
    public float sightRange;
    public float meleeRange;
    public Transform player;
    public NavMeshAgent agent;
    public Color sightColor;

    private Animator anim;
    public EnemySoldier damageTaken;
    #endregion

    #region States
    //Declare states. If you add a new sate to your character, remember to add a new States enum for it.
    public enum States {CHASING, ATTACKING, DEATH }
    public States currentState;
    #endregion

    #region Initialization
    //set default state
    private void Awake()
    {
        currentState = States.CHASING;
    }
    #endregion

    void Start()
    {
        anim = GetComponent<Animator>();
        damageTaken = GetComponent<EnemySoldier>();
        agent = GetComponent<NavMeshAgent>();
        //start the fsm
        StartCoroutine(EnemyFSM());
    }

    #region Finite StateMachine
    IEnumerator EnemyFSM()
    {
        while (true)
        {
            yield return StartCoroutine(currentState.ToString());
        }

    }
    #endregion

    #region Behavior Coroutines
    IEnumerator CHASING()
    {
        //Update IDLE STATE > put any code here to repeat during the state being active
        while (currentState == States.CHASING)
        {
            anim.Play("WalkStart");
            agent.SetDestination(player.position);

            if(agent.remainingDistance <= agent.stoppingDistance)
            {
                currentState = States.ATTACKING;
            }

            yield return new WaitForEndOfFrame();
        }

        //EXIT IDLE STATE > write any code here you want to run when the state is left

    }
    IEnumerator ATTACK()
    {
        while(currentState == States.ATTACKING)
        {
            anim.Play("Attack");
            yield return new WaitForSeconds(0.5f);
            damageTaken.Attack();

            yield return new WaitForSeconds(1.5f);
        }
    }
    #endregion

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = sightColor;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeRange);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
