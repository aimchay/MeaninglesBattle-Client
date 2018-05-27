﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;

public class RippleAttackState : FSMState
{

    public RippleAttackState()
    {
        stateID = FSMStateType.RippleAttack;
    }

    public override void Act(BaseFSM FSM)
    {
        if (BagManager.Instance.skillAttributesList[0].skillInfo != BagManager.Instance.NullInfo)
            if (BagManager.Instance.skillAttributesList[0].skillInfo.magicProperties.magicType == MagicType.Ripple)
            {
                if (BagManager.Instance.skillAttributesList[0].isOn)
                {
                    BagManager.Instance.UseMagic(0);
                    FSM.PlayAnimation("Magic Shoot Attack");
                    AudioManager.PlaySound2D("Ripple").Play();
                    NetworkManager.SendPlayerMagic("Ripple", GameTool.FindTheChild(FSM.gameObject, "RigLArmPalmGizmo").position,Look(FSM));
                }
            }
        if (BagManager.Instance.skillAttributesList[1].skillInfo != BagManager.Instance.NullInfo)
            if (BagManager.Instance.skillAttributesList[1].skillInfo.magicProperties.magicType == MagicType.Ripple)
            {
                if (BagManager.Instance.skillAttributesList[1].isOn)
                {
                    BagManager.Instance.UseMagic(1);
                    FSM.PlayAnimation("Magic Shoot Attack");
                    AudioManager.PlaySound2D("Ripple").Play();
                    NetworkManager.SendPlayerMagic("Ripple", GameTool.FindTheChild(FSM.gameObject, "RigLArmPalmGizmo").position,Look(FSM));
                }
            }

    }

    public override void Reason(BaseFSM FSM)
    {
        CharacterMessageDispatcher.Instance.DispatchMesssage
            (FSMTransitionType.IsIdle,
            FSM,
            FSM.animationManager.baseStateInfo.IsName("Idle")
            );
    }
    private Quaternion Look(BaseFSM FSM)
    {
        //Ray ray = new Ray(GameTool.FindTheChild(FSM.gameObject, "RigLArmPalmGizmo").position, new Vector3(Screen.width / 2, Screen.height / 2, 0));
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hitInfo;
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hitInfo))
        {
            targetPoint = hitInfo.point;
        }
        else
        {
            targetPoint = Camera.main.transform.forward * 100;
        }
        return Quaternion.LookRotation(targetPoint - GameTool.FindTheChild(FSM.gameObject, "RigLArmPalmGizmo").position);
    }
}
