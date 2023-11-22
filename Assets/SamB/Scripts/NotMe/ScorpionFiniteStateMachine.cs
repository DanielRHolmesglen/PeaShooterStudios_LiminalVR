using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ScorpionFiniteStateMachine : MonoBehaviour
{
    #region variables
    //public float sightRange;
    //public float meleeRange;
    //public Transform player;
    public NavMeshAgent agent;
    //public Color sightColor;

    private Animator anim;
    //public EnemySoldier damageTaken;
    #endregion

    #region States
    //Declare states.
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
        //damageTaken = GetComponent<EnemySoldier>();
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
    public IEnumerator CHASING()
    {
        while (currentState == States.CHASING)
        {
            anim.Play("WalkStart");
            //agent.SetDestination(player.position);

            /*
            if(agent.remainingDistance <= agent.stoppingDistance)
            {
                currentState = States.ATTACKING;
            }
            */

            yield return new WaitForEndOfFrame();
        }


    }

    public IEnumerator ATTACKING()
    {
        while(currentState == States.ATTACKING)
        {
            anim.Play("Attack");
            //damageTaken.Attack();

            yield return new WaitForSeconds(1f);
        }
    }
    #endregion
    

    void Update()
    {

    }
}
