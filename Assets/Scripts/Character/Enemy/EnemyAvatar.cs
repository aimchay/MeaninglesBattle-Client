using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAvatar : Entity
{

    public NavMeshAgent navMeshAgent;

    [Header("Patrol")]
    #region 巡逻
    public float Min;
    public float Max;
    public float nextDistance;
    public float patrolSpeed = 3.5f;
    private Vector3 currentDestination;
    public Vector3[] targetVectors;
    private int PI = 1;//计数
    public bool Patrol()
    {
        if (Vector3.Distance(transform.position, currentDestination) <= nextDistance)
        {
            PI++;
            if (PI > targetVectors.Length - 1)
                PI = 0;
            currentDestination = targetVectors[PI];

        }

        if (navMeshAgent.isStopped)
            animator.SetBool("Walk", false);
        else
            animator.SetBool("Walk", true);

        navMeshAgent.speed = patrolSpeed;

        return navMeshAgent.SetDestination(currentDestination);
    }
    private Vector3 GetVector()
    {
        return new Vector3(targetVectors[0].x + Random.Range(Min, Max), transform.position.y, targetVectors[0].z + Random.Range(Min, Max));
    }
    #endregion

    [Header("Chase")]
    #region 追击
    [Tooltip("可视距离")]
    public float sightDistance;
    [Tooltip("可视角度")]
    public float sightAngle;
    public float chaseSpeed = 5f;
    private GameObject targetGameObj;

    public bool Chase()
    {
        animator.SetBool("Walk", false);
        if (navMeshAgent.isStopped)
            animator.SetBool("Run", false);
        else
            animator.SetBool("Run", true);

        navMeshAgent.speed = chaseSpeed;

        return navMeshAgent.SetDestination(targetGameObj.transform.position);
    }

    public bool FindTarget()
    {
        float dis = (targetGameObj.transform.position - transform.position).magnitude;
        Vector3 relativeVector = targetGameObj.transform.position - transform.position;
        float ang = Vector3.Angle(relativeVector, transform.forward);

        return dis < sightDistance && ang < sightAngle ? true : false;

    }
    #endregion

    public CharacterStatus characterStatus;

    protected BehaviorDesigner.Runtime.BehaviorTree behaviorTree; 
    private Animator animator;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        behaviorTree = GetComponent<BehaviorDesigner.Runtime.BehaviorTree>();
        animator = GetComponent<Animator>();
        targetVectors[0] = transform.position;
        for (int i = 1; i < targetVectors.Length; i++)
        {
            targetVectors[i] = GetVector();
        }
        currentDestination = targetVectors[PI];

        targetGameObj = GameObject.FindWithTag("Player");
    }


}
