using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;

public class SingleWieldAttackState : FSMState
{
    private float attackDistance = 0;
    private AudioSource audio=null;

    public SingleWieldAttackState()
    {
        stateID = FSMStateType.SingleWieldAttack;
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
                    NetworkManager.SendPlayerHitSomeone(enemy.Value.name, FSM.characterStatus.Attack_Physics * (1 - enemy.Value.status.Defend_Physics / 100));
                    if (FSM.characterStatus.weaponType == WeaponType.Sword)
                    {
                        AudioManager.PlaySound2D("Sword").Play();
                    }
                    if (FSM.characterStatus.weaponType == WeaponType.Club)
                    {
                        AudioManager.PlaySound2D("Club").Play();
                    }
                }
            }

            FSM.animationManager.PlayAnimation("AttackID", FSM.animationManager.combo+1);

           
          
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


        if (Input.GetButtonDown("Fire1"))
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
