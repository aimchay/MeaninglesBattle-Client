using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AnimatorManager
{

    public PlayerAvatar player;

    public Animator animator;

    private BaseSMB[] SMBList;

    private float lastTime = 0;

    public AnimatorManager(PlayerAvatar player)
    {
        this.player = player;
        animator = player.GetComponent<Animator>();
        SMBList = animator.GetBehaviours<BaseSMB>();
        //RegisterSMBs
        foreach (BaseSMB smb in SMBList)
        {
            if(smb is StateActionDispatcher)
            {
                smb.Init(player);
                ((StateActionDispatcher)smb).StateEnterEvent += StateEnter;
                ((StateActionDispatcher)smb).StateExitEvent += StateExit;
                ((StateActionDispatcher)smb).StateUpdateEvent += StateUpdate;
            }
             else
                smb.Init(player);

        }
    }    
    
    private void StateEnter(StateActionDispatcher owner, AnimatorStateInfo state, int layerIndex)
    {
        NetworkManager.SendUpdatePlayerAction(state.fullPathHash);
    }

    private void StateExit(StateActionDispatcher owner, AnimatorStateInfo state, int layerIndex)
    {
        
    }

    private void StateUpdate(StateActionDispatcher owner, AnimatorStateInfo state, int layerIndex)
    {

    }
}