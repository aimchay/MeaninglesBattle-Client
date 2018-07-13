using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BehaviorDesigner.Runtime.Tasks.Enemy
{

    public class CanAttack : Conditional
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
            return enemy.CanAttack() ? TaskStatus.Success : TaskStatus.Failure;
        }

    }
}
