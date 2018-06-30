using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateActionDispatcher : BaseSMB {

    public event AnimatorStateEvent StateEnterEvent;
    public event AnimatorStateEvent StateExitEvent;
    public event AnimatorStateEvent StateUpdateEvent;

    public delegate void AnimatorStateEvent(StateActionDispatcher owner, AnimatorStateInfo state, int layerIndex);

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (StateEnterEvent != null)
            StateEnterEvent(this,stateInfo, layerIndex);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (StateUpdateEvent != null)
            StateUpdateEvent(this, stateInfo, layerIndex);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (StateExitEvent != null)
            StateExitEvent(this, stateInfo, layerIndex);
    }
}
