
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityInput
{
    public class IsGetInputAction : Conditional
    {
        public SharedString actionName;
        public SharedGameObject targetGameObject;
        private GameObject prevGameObject;
        private NetworkPlayer player;

        public override void OnStart()
        {
            var currentGameObject = GetDefaultGameObject(targetGameObject.Value);
            if (currentGameObject != prevGameObject)
            {
                player = currentGameObject.GetComponent<NetworkPlayer>();
                prevGameObject = currentGameObject;
            }
        }

        public override TaskStatus OnUpdate()
        {
           if(player.changeAction)
            {
                if(player.currentAction==actionName.Value)
                {
                    Debug.Log(player.currentAction + " Q");
                    player.changeAction = false;
                    return TaskStatus.Success;
                }
                else
                    return TaskStatus.Failure;
            }
           else
                return TaskStatus.Failure;
        }
    }
}
