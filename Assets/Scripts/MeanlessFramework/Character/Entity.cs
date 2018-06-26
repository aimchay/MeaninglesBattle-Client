using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {

     public AnimatorManager animatorMgr;

    public virtual int GetId()
    {
        return 0;
    }

    public virtual void InitEntity()
    {

    }
}
