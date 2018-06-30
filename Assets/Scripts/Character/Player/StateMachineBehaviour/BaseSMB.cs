using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSMB : StateMachineBehaviour {

    public Entity entity { get; private set; }
    protected bool isInited { get; private set; }
 
    public virtual void Init(Entity entity)
    {
        this.entity = entity;
        isInited = true;
    }
}
