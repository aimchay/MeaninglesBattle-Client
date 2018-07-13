using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic
{
    public class UseSkill : Action
    {
        public SharedGameObject targetGameObject;
        public SharedInt magicType;
        private GameObject prevGameObject;

        private PlayerAvatar player;

        public override void OnStart()
        {
            var currentGameObject = GetDefaultGameObject(targetGameObject.Value);
            if (currentGameObject != prevGameObject)
            {
                player = currentGameObject.GetComponent<PlayerAvatar>();
                prevGameObject = currentGameObject;
            }


        }

        public override TaskStatus OnUpdate()
        {
            if (player != null)
            {
                player.playerController.UseSkill((MagicType)magicType.Value);
                return TaskStatus.Success;
            }
            else
                return TaskStatus.Failure;
        }
    }

}

