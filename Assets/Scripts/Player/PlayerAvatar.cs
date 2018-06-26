using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerAvatar : Entity
{
    public CharacterController CC;

    public CharacterStatus characterStatus;

    private void Start()
    {
        animatorMgr = new AnimatorManager(this);
        CC = GetComponent<CharacterController>();
        characterStatus = PlayerStatusManager.Instance.GetCharacterStatus();
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        UseGravity(9.8f);
    }

    

    public void UseGravity(float Gravity)
    {
        Vector3 moveDirection = Vector3.zero;
        if (!CC.isGrounded)
        {
            moveDirection.y -= Gravity * Time.fixedDeltaTime;
        }
        else
            moveDirection = Vector3.zero;
        CC.Move(moveDirection);
    }

    public void Move(float walkSpeed)
    {
        Vector3 moveDirection = Vector3.zero;  
     

            moveDirection = CameraBase.Instance.transform.right * CrossPlatformInputManager.GetAxis("Horizontal") + Vector3.Scale(CameraBase.Instance.transform.forward, new Vector3(1, 0, 1)).normalized * CrossPlatformInputManager.GetAxis("Vertical");
           // moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= walkSpeed;
            CC.Move(moveDirection*Time.fixedDeltaTime);
        
    }

    public void Jump(float jumpSpeed)
    {
        Vector3 moveDirection = Vector3.zero;
         moveDirection.y += jumpSpeed;
        CC.Move(moveDirection * Time.fixedDeltaTime);
    }

    public void Roll(float rollSpeed)
    {
        

    }
}
