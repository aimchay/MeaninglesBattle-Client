using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;

public class DoubleHandsAttackState : FSMState
{

    private float attackDistance = 0;

    public DoubleHandsAttackState()
    {
        stateID = FSMStateType.DoubleHandsAttack;
    }

    public override void Act(BaseFSM FSM)
    {


        if (FSM.controller.GetCurSelectedWeaponInfo() != null)
        {
            attackDistance = FSM.controller.GetCurSelectedWeaponInfo().weaponProperties.weaponLength;
        }
        if (FSM.Attacked)
        {
            foreach (KeyValuePair<string, NetworkPlayer> enemy in FSM.controller.ScenePlayers)
            {
                if (FSM.controller.CheckCanAttack(FSM.gameObject, enemy.Value.gameObject, attackDistance, 45))
                {
                    NetworkManager.SendPlayerHitSomeone(enemy.Value.name, FSM.characterStatus.Attack_Physics * (1 - enemy.Value.characterStatus.Defend_Physics / 100));
                    //单机测试
                    //enemy.playerFSM.characterStatus.HP -= FSM.characterStatus.Attack_Physics * (1 - enemy.playerFSM.characterStatus.Defend_Physics / 100);
                    if (FSM.characterStatus.weaponType == WeaponType.DoubleHands)
                    {
                        AudioManager.PlaySound2D("Axe").Play();
                    }
                }
            }

            FSM.animationManager.PlayAnimation("AttackID", FSM.animationManager.combo + 4);

           
            FSM.Attacked = false;
        }
    }

    public override void Reason(BaseFSM FSM)
    {

        if (!FSM.animationManager.baseStateInfo.IsName("Idle") && FSM.animationManager.attackStateInfo.normalizedTime > 1f)
        {
            FSM.animationManager.combo = 0;
            FSM.animationManager.PlayAnimation("AttackID", 0);
            FSM.PerformTransition(FSMTransitionType.IsIdle);
        }

        if ( Input.GetButtonDown("Fire1") && FSM.characterStatus.weaponType == WeaponType.DoubleHands)
        {
            FSM.Attacked = true;
        }

        CharacterMessageDispatcher.Instance.DispatchMesssage
     (FSMTransitionType.CanBeMove,
     FSM,
     (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.5 || Mathf.Abs(Input.GetAxis("Vertical")) > 0.5) && FSM.controller.CC.isGrounded
     );

    }
}
