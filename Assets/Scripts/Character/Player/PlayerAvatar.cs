using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;
using BehaviorDesigner.Runtime;

public class PlayerAvatar : Entity
{
    public string playerName;
    public CharacterStatus characterStatus;
    public PlayerController playerController;

    public BehaviorTree behaviorTree;

    public bool IsDefend;

    private float lastTime;

    private void Start()
    {
        if (!ResourcesManager.Instance.IsStandalone)
            playerName = NetworkManager.PlayerName;
        animatorMgr = new AnimatorManager(this);
        playerController = GetComponent<PlayerController>();
        behaviorTree = GetComponent<BehaviorTree>();

        MessageCenter.AddListener(EMessageType.EquipItem,
            (object[] obj) =>
            {
                switch ((EquippedItem)obj[0])
                {
                    case EquippedItem.Head:
                        playerController.UnEquip(EquippedItem.Head);
                        playerController.EquipHelmet((int)obj[1]);
                        if (!ResourcesManager.Instance.IsStandalone)
                            NetworkManager.SendPlayerEquipHelmet((int)obj[1]); //发送戴头盔消息
                        break;
                    case EquippedItem.Body:
                        playerController.EquipClothes((int)obj[1]);
                        if (!ResourcesManager.Instance.IsStandalone)
                            NetworkManager.SendPlayerEquipClothe((int)obj[1]); //发送着衫消息
                        break;
                    case EquippedItem.Weapon1:
                        playerController.UnEquip(EquippedItem.Weapon1);
                        playerController.EquipWeapon((int)obj[1]);
                        if (!ResourcesManager.Instance.IsStandalone)
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
        behaviorTree.SendEvent<int>("MagicType", (int)characterStatus.magicType);

    }

    private void FixedUpdate()
    {
        playerController.UseGravity(9.8f);
        if (Time.time - lastTime > 0.2f && !ResourcesManager.Instance.IsStandalone)
        {
            NetworkManager.SendUpdatePlayerTranform(transform.position, transform.rotation.eulerAngles);
            lastTime = Time.time;
        }
    }

    public void CheckCanAttack(float distance, float angle)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, distance, LayerMask.GetMask("Enemy"));
        foreach (Collider collider in colliders)
        {
            Vector3 relativeVector = collider.transform.position - transform.position;
            float ang = Vector3.Angle(relativeVector, transform.forward);
            if (ang < angle)
            {
                float randomDamage = Random.Range(0.8f, 1.2f);
                if (collider.GetComponent<NetworkPlayer>())
                {
                    NetworkPlayer networkPlayer = collider.GetComponent<NetworkPlayer>();
                    NetworkManager.SendPlayerHitSomeone(networkPlayer.playerName, (characterStatus.Attack_Physics - networkPlayer.characterStatus.Defend_Physics) * randomDamage);
                }
                else if(collider.GetComponent<EnemyAvatar>())
                {
                    EnemyAvatar enemy = collider.GetComponent<EnemyAvatar>();
                    enemy.ReceiveDamage(characterStatus.Attack_Physics * randomDamage);
                }
            }
        }
    }

    public void ReceiveDamage(float Damage)
    {
        if(IsDefend)
        {
            Damage /= 9; //临时数值
        }
        if(!ResourcesManager.Instance.IsStandalone)
        {
            NetworkManager.SendPlayerHitSomeone(playerName, Damage*(1-characterStatus.Defend_Physics/100));
        }
        else
        {
            PlayerStatusManager.Instance.characterStatus.HP -= Damage * (1 - characterStatus.Defend_Physics / 100);
            if (PlayerStatusManager.Instance.characterStatus.HP < 0)
                Dead();
        }
    }

    public void Dead()
    {
        UIManager.Instance.ShowUI(UIid.FinishUI);
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(0);
    }
}
