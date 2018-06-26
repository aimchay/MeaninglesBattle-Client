using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace BehaviorDesigner.Runtime.Tasks
{

    public class GetMovement : Action
    {
        private BehaviorTree tree;
        public SharedGameObject targetGameObject;
        private Vector3 moveDirection;
        private CharacterController CC;

        private GameObject prevGameObject;

        public override void OnStart()
        {
            var currentGameObject = GetDefaultGameObject(targetGameObject.Value);
            if (currentGameObject != prevGameObject)
            {
                tree = currentGameObject.GetComponent<BehaviorTree>();
                CC = currentGameObject.GetComponent<CharacterController>();
                prevGameObject = currentGameObject;
            }
        }


        public override TaskStatus OnUpdate()
        {
    
            tree.GetVariable("Horizontal").SetValue(CrossPlatformInputManager.GetAxis("Horizontal"));
            tree.GetVariable("Vertical").SetValue(CrossPlatformInputManager.GetAxis("Vertical"));
           
            return TaskStatus.Success;
        }

  

        public override void OnReset()
        {
        }
    }
}