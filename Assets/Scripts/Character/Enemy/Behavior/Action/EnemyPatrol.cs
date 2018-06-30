using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviorDesigner.Runtime.Tasks.Basic.Enemy
{
    public class EnemyPatrol : Action
    {
        public SharedGameObject targetGameObject;

        private GameObject prevGameObject;
        private EnemyAvatar enemy;

        public override void OnStart()
        {
            var currentGameObject = GetDefaultGameObject(targetGameObject.Value);
            if (currentGameObject != prevGameObject)
            {
                enemy = currentGameObject.GetComponent<EnemyAvatar>();
                prevGameObject = currentGameObject;
            }
          
            
        }

        public override TaskStatus OnUpdate()
        {  
            return enemy.Patrol()?TaskStatus.Success:TaskStatus.Failure;
        }

      
    }
}