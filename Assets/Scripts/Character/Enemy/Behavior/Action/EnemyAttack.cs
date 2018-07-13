using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Enemy
{

    public class EnemyAttack : Action
    {
        public SharedGameObject targetGameObject;

        public SharedFloat distance;
        public SharedFloat angle;

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
            
            enemy.CheckCanAttack(distance.Value, angle.Value);
            return TaskStatus.Success;
        }
    }
}