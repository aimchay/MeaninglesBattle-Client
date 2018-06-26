using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityCharacterController
{

    public class PlayerJump : Action
    {

        public SharedGameObject targetGameObject;
        private GameObject prevGameObject;
        private PlayerAvatar avatar;

        public override void OnStart()
        {
            var currentGameObject = GetDefaultGameObject(targetGameObject.Value);
            if (currentGameObject != prevGameObject)
            {
                avatar = currentGameObject.GetComponent<PlayerAvatar>();
                prevGameObject = currentGameObject;
            }
        }

        public override TaskStatus OnUpdate()
        {

            if (avatar != null)
            {
                avatar.Jump(avatar.characterStatus.jumpSpeed);
                return TaskStatus.Success;
            }
            else
                return TaskStatus.Failure;

        }
    }
}
