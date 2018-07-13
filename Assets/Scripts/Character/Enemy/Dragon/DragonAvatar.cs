using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonAvatar : EnemyAvatar
{

    public bool isMad;
    private BaseSMB[] SMBList;


    
    public void Awake()
    {
        animator = GetComponent<Animator>();
        SMBList = animator.GetBehaviours<BaseSMB>();
        //RegisterSMBs
        foreach (BaseSMB smb in SMBList)
        {
            if (smb is StateActionDispatcher)
            {
                smb.Init(this);
                ((StateActionDispatcher)smb).StateEnterEvent += StateEnter;
                ((StateActionDispatcher)smb).StateExitEvent += StateExit;
                ((StateActionDispatcher)smb).StateUpdateEvent += StateUpdate;
            }

        }
    }
    
    private void StateEnter(StateActionDispatcher owner, AnimatorStateInfo state, int layerIndex)
    {

    }

    private void StateExit(StateActionDispatcher owner, AnimatorStateInfo state, int layerIndex)
    {
       if(state.IsName("BiteAttack"))
        {
            CheckCanAttack(5, 60);
        }
        
    }

    private void StateUpdate(StateActionDispatcher owner, AnimatorStateInfo state, int layerIndex)
    {

    }

}
