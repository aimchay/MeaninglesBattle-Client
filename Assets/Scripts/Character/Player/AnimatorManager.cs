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

    #region 动作事件
    private void StateEnter(StateActionDispatcher owner, AnimatorStateInfo state, int layerIndex)
    {
        if (state.IsName("StygianDesolator"))
            player.playerController.UseSkill(MagicType.StygianDesolator);
    }

    private void StateExit(StateActionDispatcher owner, AnimatorStateInfo state, int layerIndex)
    {
        if (layerIndex == 5)
            if (!state.IsName("Empty"))
            {
                player.CheckCanAttack(1.5f + PlayerStatusManager.Instance.Weapon1.weaponProperties.weaponLength, 60);
            }
        if(layerIndex==4)
        {
            if (state.IsName("Ripple"))
                player.playerController.UseSkill(MagicType.Ripple);
            if (state.IsName("HeartAttack"))
                player.playerController.UseSkill(MagicType.HeartAttack);
            if (state.IsName("Thunderbolt"))
                player.playerController.UseSkill(MagicType.Thunderbolt);
            if (state.IsName("IceArrow"))
                player.playerController.UseSkill(MagicType.IceArrow);
            if (state.IsName("ChoshimArrow"))
                player.playerController.UseSkill(MagicType.ChoshimArrow);
        }
    }

    private void StateUpdate(StateActionDispatcher owner, AnimatorStateInfo state, int layerIndex)
    {
      
    }
    #endregion

  
}