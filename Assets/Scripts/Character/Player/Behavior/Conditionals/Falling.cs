using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityTransform
{
    public class Falling : Action
    {
        public SharedGameObject targetGameObject;
        private GameObject prevGameObject;
        private Transform targetTransform;
        private PlayerAvatar avatar;

        public override void OnStart()
        {
            var currentGameObject = GetDefaultGameObject(targetGameObject.Value);
            if (currentGameObject != prevGameObject)
            {
                targetTransform = currentGameObject.transform;
                avatar = currentGameObject.GetComponent<PlayerAvatar>();
                prevGameObject = currentGameObject;
            }
        }

        public override TaskStatus OnUpdate()
        {
            if(targetTransform.position.y>10)
            {
                avatar.animatorMgr.animator.SetBool("Sky",true);
                return TaskStatus.Success;
            }
            else
            {
                avatar.animatorMgr.animator.SetBool("Sky", false);
                return TaskStatus.Failure;
            }
        }

    }

}
