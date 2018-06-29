using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityCharacterController
{

    public class PlayerMove : Action
    {

        public SharedGameObject targetGameObject;
        public SharedFloat Horizontal;
        public SharedFloat Vertical;
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
            Horizontal.Value = CrossPlatformInputManager.GetAxis("Horizontal");
            Vertical.Value = CrossPlatformInputManager.GetAxis("Vertical");

            if (avatar != null)
            {
                avatar.animatorMgr.animator.SetFloat("Horizontal", Horizontal.Value);
                avatar.animatorMgr.animator.SetFloat("Vertical", Vertical.Value);
                avatar.playerController.Move(avatar.characterStatus.moveSpeed);
                return TaskStatus.Running;

            }
            else
                return TaskStatus.Failure;

        }
    }
}