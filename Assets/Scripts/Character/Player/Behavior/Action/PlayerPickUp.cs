using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic
{
    public class PlayerPickUp : Action
    {
        public SharedBool canPick;
        public SharedGameObject targetGameObject;
        private GameObject prevGameObject;
        private PlayerAvatar avatar;

        public override void OnStart()
        {
            MessageCenter.AddListener(Meaningless.EMessageType.FoundItem, (object[] obj) => { canPick.Value = (bool)obj[0];} );
            var currentGameObject = GetDefaultGameObject(targetGameObject.Value);
            if (currentGameObject != prevGameObject)
            {
                avatar = currentGameObject.GetComponent<PlayerAvatar>();
                prevGameObject = currentGameObject;
            }
        }

        public override TaskStatus OnUpdate()
        {
            if(canPick.Value)
            {
                avatar.playerController.PickItem();
                avatar.animatorMgr.animator.SetBool("PickUp", true);
            }
            return TaskStatus.Success;

        }
    }
}