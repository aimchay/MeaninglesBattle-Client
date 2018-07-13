﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using Meaningless;
using MeaninglessNetwork;

public class NetworkPlayer : MonoBehaviour {

    public string playerName;
    public PlayerController playerController;
    private Animator animator;
    //public AnimationManager animationManager;

    //public string PlayerName="";
    public float hp = 100f;
    public int headItemID;
    public int bodyItemID;
    public int weaponID;
    public string currentAction;
    private string preAction;
    public bool changeAction;
    public CharacterStatus characterStatus;
    

    #region 记录上一次刷新的变量
    public float LastUpdateTime=0;
    private Vector3 lastPos=Vector3.zero;
    private Vector3 lastRot = Vector3.zero;
    //预测坐标
    private Vector3 forseePos= Vector3.zero;
    //预测旋转度
    private Vector3 forseeRot = Vector3.zero;
    private float DeltaTime;
    #endregion

    public void Start()
    {
        playerController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
    }


    /// <summary>
    /// 设置玩家变换数据与更新时间
    /// </summary>
    public void SetPlayerTransform(float posX,float posY,float posZ,float rotX,float rotY,float rotZ,float HP)
    {
        Vector3 recvPos = new Vector3(posX,posY,posZ);
        Vector3 recvRot = new Vector3(rotX, rotY, rotZ);
        forseePos = lastPos + (recvPos - lastPos) * 2;
        forseeRot = lastRot + (recvRot - lastRot) * 2;
        if (Time.time -LastUpdateTime>0.3f)
        {
            forseePos = recvPos;
            forseeRot = recvRot;
        }
         DeltaTime= Time.time - LastUpdateTime;
        lastPos = recvPos;
        lastRot = recvRot;

        hp = HP;
        //刷新更新时间
        LastUpdateTime = Time.time;
    }

    /// <summary>
    /// 设置当前输入动作
    /// </summary>
    public void SetPlayerAction(string CurrentAction)
    {
        preAction = currentAction;
        currentAction = CurrentAction;
        changeAction = true;
        //animator.Play(currentAction);
        //animationManager.NetPlayClip(CurrentAction);
    }

    /// <summary>
    /// 设置玩家头盔
    /// </summary>
    /// <param name="ItemID"></param>
    public void SetPlayerHelmet(int ItemID)
    {
        headItemID = ItemID;
        playerController.EquipHelmet(ItemID);
    }
    /// <summary>
    /// 设置玩家衣服
    /// </summary>
    /// <param name="ItemID"></param>
    public void SetPlayeClothe(int ItemID)
    {
        bodyItemID = ItemID;
        playerController.EquipClothes(ItemID);
    }
    /// <summary>
    /// 设置玩家武器
    /// </summary>
    /// <param name="ItemID"></param>
    public void SetPlayerWeapon(int ItemID)
    {
        weaponID = ItemID;
        playerController.EquipWeapon(ItemID);
        animator.SetInteger("WeaponType", (int)ItemInfoManager.Instance.GetItemInfo(ItemID).weaponProperties.weaponType);
    }
    /// <summary>
    /// 设置玩家名字
    /// </summary>
    public void SetPlayerName(string playerName)
    {
        this.playerName = playerName;
    }

    /// <summary>
    /// 获取自己的数据,作为协议
    /// </summary>
    public BytesProtocol GetPlayerInfo()
    {
        BytesProtocol protocol = new BytesProtocol();

        protocol.SpliceString("UpdatePlayerInfo");
        protocol.SpliceFloat(hp);
        protocol.SpliceFloat(transform.position.x);
        protocol.SpliceFloat(transform.position.y);
        protocol.SpliceFloat(transform.position.z);
        protocol.SpliceFloat(transform.rotation.x);
        protocol.SpliceFloat(transform.rotation.y);
        protocol.SpliceFloat(transform.rotation.z);

        if (playerName == NetworkManager.PlayerName)
        {
            if (PlayerStatusManager.Instance.Head == null)
            {
                protocol.SpliceInt(0);
            }
            else
            {
                protocol.SpliceInt(PlayerStatusManager.Instance.Head.ItemID);
            }

            if (PlayerStatusManager.Instance.Body == null)
            {
                protocol.SpliceInt(0);
            }
            else
            {
                protocol.SpliceInt(PlayerStatusManager.Instance.Body.ItemID);
            }
            if (PlayerStatusManager.Instance.Weapon1 == null && PlayerStatusManager.Instance.Weapon2 == null)
            {
                protocol.SpliceInt(0);
            }
            else
            {
                protocol.SpliceInt(PlayerStatusManager.Instance.Body.ItemID);
            }

            // protocol.SpliceInt(WeaponID);
            // protocol.SpliceString(CurrentAction);

            //playerFSM.characterStatus.HP;

            characterStatus = PlayerStatusManager.Instance.characterStatus;
        }
        

        return protocol;
    }

    /// <summary>
    /// 预测行走
    /// </summary>
    private void ForseeMove()
    {
        if (DeltaTime > 0)
        {
            //位置插值
            //Vector3 curPos = transform.position;
            transform.position = Vector3.Lerp(transform.position, forseePos, 0.1f);
            animator.SetFloat("Horizontal", Mathf.Clamp(forseePos.x-transform.position.x ,-1,1));
            animator.SetFloat("Vertical", Mathf.Clamp(forseePos.z-transform.position.z ,-1,1));
            //角度插值
            Vector3 curRot = transform.eulerAngles;
            transform.rotation = Quaternion.Lerp(Quaternion.Euler(curRot), Quaternion.Euler(forseeRot), DeltaTime);
        }
    }
    private void FixedUpdate()
    {
        //transform.position = forseePos;
        //transform.rotation = Quaternion.Euler(forseeRot);
        ForseeMove();

    }
    private void Update()
    {
       
    }
}
