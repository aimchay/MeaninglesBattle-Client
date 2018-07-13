using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Meaningless;

public class EnemyAvatar : Entity
{
    public string enemyName;

    [HideInInspector]
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
    protected GameObject targetGameObj;

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

        return dis> attackDistance && dis < sightDistance && ang < sightAngle ? true : false;

    }
    #endregion




    public bool isAttack;
    public bool IsDefend;
    public float attackDistance;

    public bool CanAttack()
    {
        return Vector3.Distance(transform.position, targetGameObj.transform.position) <= attackDistance ? true : false;
    }

    public CharacterStatus characterStatus;

    protected BehaviorDesigner.Runtime.BehaviorTree behaviorTree; 
    [HideInInspector]
    public Animator animator;


    void Start()
    {
        characterStatus=MeaninglessJson.LoadJsonFromFile<CharacterStatus>(MeaninglessJson.Path_StreamingAssets + enemyName + ".json");

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

    public void CheckCanAttack(float distance, float angle)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, distance, LayerMask.GetMask("Player"));
        foreach (Collider collider in colliders)
        {
            Vector3 relativeVector = collider.transform.position - transform.position;
            float ang = Vector3.Angle(relativeVector, transform.forward);

            if (ang < angle)
            {
                float randomDamage = Random.Range(0.8f, 1.2f);
                if (collider.GetComponent<NetworkPlayer>())
                {
                    NetworkPlayer networkPlayer = collider.GetComponent<NetworkPlayer>();
                    NetworkManager.SendPlayerHitSomeone(networkPlayer.playerName, characterStatus.Attack_Physics * randomDamage);
                }
                else if (collider.GetComponent<PlayerAvatar>())
                {
                    PlayerAvatar player = collider.GetComponent<PlayerAvatar>();
                    player.ReceiveDamage(characterStatus.Attack_Physics* randomDamage);
                }
            }
        }
    }

    public void ReceiveDamage(float Damage)
    {
        if (IsDefend)
        {
            Damage /= 9; //临时数值
        }

        characterStatus.HP -= Damage;

        if (characterStatus.HP < 0)
            Dead();
    }

    public void Dead()
    {
        Destroy(gameObject);
    }

}
