﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;

public class ChoshimArrowState : FSMState
{

    public ChoshimArrowState()
    {
        stateID = FSMStateType.ChoshimArrow;
    }

    public override void Act(BaseFSM FSM)
    {
        if (PlayerStatusManager.Instance.skillAttributesList[0].skillInfo != PlayerStatusManager.Instance.NullInfo)
            if (PlayerStatusManager.Instance.skillAttributesList[0].skillInfo.magicProperties.magicType == MagicType.ChoshimArrow)
            {
                if (PlayerStatusManager.Instance.skillAttributesList[0].isOn)
                {
                    PlayerStatusManager.Instance.UseMagic(0);
                    FSM.PlayAnimation("Magic Shoot Attack");
                    AudioManager.PlaySound2D("Arrow").Play();
                    NetworkManager.SendPlayerMagic("Choshim Arrow", GameTool.FindTheChild(FSM.gameObject, "RigLArmPalmGizmo").position, Look(FSM));
                }

            }
    
        if (PlayerStatusManager.Instance.skillAttributesList[1].skillInfo != PlayerStatusManager.Instance.NullInfo)
            if (PlayerStatusManager.Instance.skillAttributesList[1].skillInfo.magicProperties.magicType == MagicType.ChoshimArrow)
            {
                if (PlayerStatusManager.Instance.skillAttributesList[1].isOn)
                {
                    PlayerStatusManager.Instance.UseMagic(1);
                    FSM.PlayAnimation("Magic Shoot Attack");
                    AudioManager.PlaySound2D("Arrow").Play();
                    NetworkManager.SendPlayerMagic("Choshim Arrow", GameTool.FindTheChild(FSM.gameObject, "RigLArmPalmGizmo").position, Look(FSM));
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
