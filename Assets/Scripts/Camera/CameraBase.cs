﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;

public class CameraBase : MonoSingleton<CameraBase>
{
    public bool isFollowing=false;
    public bool isDead = false;
    public bool isBagOpen = false;
    public bool isEscape = false;
    public float moveSpeed = 120f;
    public GameObject cameraFollowGO;
    public float clampAngle = 80f;
    public float inputSensitivity = 150f;
    public GameObject mainCamera;
    public GameObject player;
    public float canDistanceXToPlayer;
    public float canDistanceYToPlayer;
    public float canDistanceZToPlayer;
    private float mouseX;
    private float mouseY;
    private float rotY = 0f;
    private float rotX = 0f;
    private float h;
    private float v;

    private void Awake()
    {
        FindPlayer();
    }

    private void Start()
    {
        
        Vector3 rot = transform.localRotation.eulerAngles;
        rotX = rot.x;
        rotY = rot.y;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isFollowing = true;
    }

    private void Update()
    {
        if(isFollowing)
        {
            Follow();
        } 
        else
        {
           Cursor.lockState = CursorLockMode.None;
           Cursor.visible = true;
        }

        OpenBag();
        Esc();
    }

    public void FindPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (!(player.transform.Find("CameraFollow")))
        {
            cameraFollowGO = new GameObject("CameraFollow");
            cameraFollowGO.transform.SetParent(player.transform);
            cameraFollowGO.transform.localPosition = new Vector3(0.75f, 1.25f, 0);
        }
        else
            cameraFollowGO = player.transform.Find("CameraFollow").gameObject;
    }



    private void Follow()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        rotY += mouseX * inputSensitivity * Time.deltaTime;
        rotX -= mouseY * inputSensitivity * Time.deltaTime;
        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);
        Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0);
        transform.rotation = localRotation;
        player.transform.LookAt(player.transform.position + transform.right * h + Vector3.Scale(transform.forward, new Vector3(1, 0, 1)).normalized * v);
    }

    private void LateUpdate()
    {
        float step = moveSpeed * Time.deltaTime;
        if (isFollowing)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
        } 
    }

    public void OpenBag()
    {
        if (Input.GetButtonUp("Bag"))
        {
            if (!CameraBase.Instance.isDead)
            {
                if (!CameraBase.Instance.isBagOpen)
                {
                    UIManager.Instance.ShowUI(UIid.BagUI);
                    isBagOpen = true;
                    isFollowing = false;
                }
                else
                {

                    UIManager.Instance.ReturnUI(UIid.HUDUI);
                    isBagOpen = false;
                    isFollowing = true;
                }
            }
        }
    }
    public void Esc()
    {
        if (Input.GetButtonDown("Esc"))
        {
            UIManager.Instance.ShowUI(UIid.EscapeUI);
           isFollowing = false;
        }
    }
}
