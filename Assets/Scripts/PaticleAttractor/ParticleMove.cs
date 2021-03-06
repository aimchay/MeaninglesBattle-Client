﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;

public class ParticleMove : MonoBehaviour
{
    public int moveSpeed;
    public int destoryTime;
    private Rigidbody RB;
    float time = 0;
    RaycastHit hitInfo;
    Vector3 targetPoint;
   
    

    private void FixedUpdate()
    {
        Move(moveSpeed);
        time += Time.fixedDeltaTime;
        if(time>destoryTime)
        {
            DestroyObject();
            time = 0;
        }
    }

    void DestroyObject()
    {
        NetPoolManager.Destroy(gameObject);
    }

    private void Move(float speed)
    {
        transform.Translate(0, 0, speed * Time.fixedDeltaTime, Space.Self);
      
        
    }
}
