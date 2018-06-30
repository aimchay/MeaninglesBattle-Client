using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityInput
{
    [TaskCategory("Basic/Input")]
    [TaskDescription("Returns success when the specified button is released.")]
    public class IsVirtualButtonUp : Conditional
    {
        [Tooltip("The name of the button")]
        public SharedString buttonName;

        public override TaskStatus OnUpdate()
        {
            return CrossPlatformInputManager.GetButtonUp(buttonName.Value) ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnReset()
        {
            buttonName = "Fire1";
        }
    }
}
