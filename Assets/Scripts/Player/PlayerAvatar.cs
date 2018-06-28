using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Meaningless;

public class PlayerAvatar : Entity
{
    public string playerName;
    public CharacterStatus characterStatus;
    public PlayerController playerController;


    private float lastTime;

    private void Start()
    {
        animatorMgr = new AnimatorManager(this);
        playerController = GetComponent<PlayerController>();

        MessageCenter.AddListener(EMessageType.EquipItem,
            (object[] obj) =>
            {
                switch((EquippedItem)obj[0])
                {
                    case EquippedItem.Head:
                        playerController.UnEquip(EquippedItem.Head);
                        playerController.EquipHelmet((int)obj[1]);
                        NetworkManager.SendPlayerEquipHelmet((int)obj[1]); //发送戴头盔消息
                        break;
                    case EquippedItem.Body:
                        playerController.EquipClothes((int)obj[1]);
                        NetworkManager.SendPlayerEquipClothe((int)obj[1]); //发送着衫消息
                        break;
                    case EquippedItem.Weapon1:
                        playerController.UnEquip(EquippedItem.Weapon1);
                        playerController.EquipWeapon((int)obj[1]);
                        NetworkManager.SendPlayerEquipWeapon((int)obj[1]); //发送换武器消息
                        break;
                }

                MessageCenter.AddListener(EMessageType.UnEquipItem, 
                   (object[] ob) =>
                 {
                     playerController.UnEquip((EquippedItem)ob[0]);
                 }
                );
            }
            );

      
    }

    private void Update()
    {
        if (playerController.List_CanPickUp.Count > 0)
            MessageCenter.Send(EMessageType.FoundItem, true);
        else
            MessageCenter.Send(EMessageType.FoundItem, false);
        characterStatus = PlayerStatusManager.Instance.GetCharacterStatus();
        animatorMgr.animator.SetInteger("WeaponType", (int)characterStatus.weaponType);
    }

    private void FixedUpdate()
    {
        playerController.UseGravity(9.8f);
        if (Time.time - lastTime > 0.2f)
        {
            NetworkManager.SendUpdatePlayerTranform(transform.position, transform.rotation.eulerAngles);
            lastTime = Time.time;
        }
    }

}
