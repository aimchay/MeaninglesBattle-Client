using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;

public class ThunderBoltState : FSMState
{


    public ThunderBoltState()
    {
        stateID = FSMStateType.ThunderBolt;
    }

    public override void Act(BaseFSM FSM)
    {
        //MessageCenter.AddListener(EMessageType.GetHitPoint, (object obj) => { hitpoint = (Vector3)obj; });
        if (PlayerStatusManager.Instance.skillAttributesList[0].skillInfo != PlayerStatusManager.Instance.NullInfo)
            if (PlayerStatusManager.Instance.skillAttributesList[0].skillInfo.magicProperties.magicType == MagicType.Thunderbolt)
            {
                if (PlayerStatusManager.Instance.skillAttributesList[0].isOn)
                {
                    PlayerStatusManager.Instance.UseMagic(0);
                   // GameObject go = NetPoolManager.Instantiate("ThunderBolt", FSM.transform.position + FSM.transform.forward * 5, FSM.transform.rotation);
                    AudioManager.PlaySound2D("ThunderBolt").Play();
                   // go.GetComponent<MagicBehaviour>().isHit = true;
                    NetworkManager.SendPlayerMagic("ThunderBolt", FSM.transform.position + FSM.transform.forward * 5, FSM.transform.rotation);
                }
            }
        if (PlayerStatusManager.Instance.skillAttributesList[1].skillInfo != PlayerStatusManager.Instance.NullInfo)
            if (PlayerStatusManager.Instance.skillAttributesList[1].skillInfo.magicProperties.magicType == MagicType.Thunderbolt)
            {

                if (PlayerStatusManager.Instance.skillAttributesList[1].isOn)
                {
                    PlayerStatusManager.Instance.UseMagic(1);
                  //  GameObject go = NetPoolManager.Instantiate("ThunderBolt", FSM.transform.position + FSM.transform.forward * 5, FSM.transform.rotation);
                    AudioManager.PlaySound2D("ThunderBolt").Play();
                   // go.GetComponent<MagicBehaviour>().isHit = true;
                    NetworkManager.SendPlayerMagic("ThunderBolt", FSM.transform.position + FSM.transform.forward * 5, FSM.transform.rotation);
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
}
