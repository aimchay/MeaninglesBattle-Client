using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{

    /// <summary>
    ///  Key:GroundItemID Value:ItemID
    /// </summary>

    public Transform LHand;
    public Transform RHand;
    public Transform Head;
    private CharacterController CC;
    private Dictionary<int, int> Dict_PickUp_Tran = new Dictionary<int, int>();
    public List<GroundItem> List_CanPickUp = new List<GroundItem>();

    public void Start()
    {
        CC = GetComponent<CharacterController>();
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

    public void UseSkill(MagicType magicType)
    {
        switch (magicType)
        {
            case MagicType.Ripple:
                if (!ResourcesManager.Instance.IsStandalone)
                    NetworkManager.SendPlayerMagic(magicType.ToString(), transform.position + new Vector3(0, 1, 0), transform.rotation);
                else
                    NetPoolManager.Instantiate(magicType.ToString(), transform.position + new Vector3(0, 1, 0), transform.rotation);
                break;
            case MagicType.HeartAttack:
                if (!ResourcesManager.Instance.IsStandalone)
                    NetworkManager.SendPlayerMagic("Heart Attack", transform.position + new Vector3(0, 1, 0), transform.rotation);
                else
                    NetPoolManager.Instantiate("Heart Attack", transform.position + new Vector3(0, 1, 0), transform.rotation);
                break;
            case MagicType.StygianDesolator:
                if (!ResourcesManager.Instance.IsStandalone)
                    NetworkManager.SendPlayerMagic("Stygian Desolator", transform.position, transform.rotation);
                else
                    NetPoolManager.Instantiate("Stygian Desolator", transform.position, transform.rotation);
                break;
            case MagicType.IceArrow:
                if (!ResourcesManager.Instance.IsStandalone)
                    NetworkManager.SendPlayerMagic("Ice Arrow", RHand.position, transform.rotation);
                else
                    NetPoolManager.Instantiate("Stygian Desolator", transform.position, transform.rotation);
                break;
            case MagicType.ChoshimArrow:
                if (!ResourcesManager.Instance.IsStandalone)
                    NetworkManager.SendPlayerMagic("Choshim Arrow", RHand.position, transform.rotation);
                else
                    NetPoolManager.Instantiate("Choshim Arrow", RHand.position, transform.rotation);
                break;
            case MagicType.Thunderbolt:
                if (!ResourcesManager.Instance.IsStandalone)
                    NetworkManager.SendPlayerMagic(magicType.ToString(), transform.position + transform.forward * 5, transform.rotation);
                else
                    NetPoolManager.Instantiate(magicType.ToString(), transform.position + transform.forward * 5, transform.rotation);
                break;

        }
    }

    /*
        public override void ChangeWeapon(int currentSelected)
        {
            switch (currentSelected)
            {
                case 1:
                    if (PlayerStatusManager.Instance.Weapon1 != null)
                    {
                        if (PlayerStatusManager.Instance.Weapon2 != null)
                        {
                            UnEquip(EquippedItem.Weapon2);
                        }
                        EquipWeapon(PlayerStatusManager.Instance.Weapon1.ItemID);
                    }
                    break;
                case 2:
                    if (PlayerStatusManager.Instance.Weapon2 != null)
                    {
                        if (PlayerStatusManager.Instance.Weapon1 != null)
                        {
                            UnEquip(EquippedItem.Weapon1);
                        }
                        EquipWeapon(PlayerStatusManager.Instance.Weapon2.ItemID);
                    }
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
            }

        }
        */

    #region 拾取相关
    public void PickItem(Transform Item)
    {
        GroundItem GItem = Item.GetComponent<GroundItem>();
        Dict_PickUp_Tran.Add(GItem.GroundItemID, GItem.ItemID);
        Item.SetParent(transform);
        if (!ResourcesManager.Instance.IsStandalone)
            NetworkManager.SendPickItem(GItem.GroundItemID);
        Item.gameObject.SetActive(false);

    }

    public void PickItem()
    {
        GroundItem GItem = List_CanPickUp[0];
        List_CanPickUp.Remove(GItem);
        Dict_PickUp_Tran.Add(GItem.GroundItemID, GItem.ItemID);
        GItem.transform.SetParent(transform);
        if (!ResourcesManager.Instance.IsStandalone)
            NetworkManager.SendPickItem(GItem.GroundItemID);
        PlayerStatusManager.Instance.PickItem(GItem.ItemID);
        GItem.gameObject.SetActive(false);

    }

    public void DiscardItem(int itemID)
    {
        foreach (int GID in Dict_PickUp_Tran.Keys)
        {
            if (Dict_PickUp_Tran[GID] == itemID)
            {
                GroundItem GItem = MapManager.Instance.Items[GID];
                //发送
                if (!ResourcesManager.Instance.IsStandalone)
                    NetworkManager.SendDropItem(GID, GItem.gameObject.transform);
                GItem.gameObject.transform.SetParent(null);
                GItem.gameObject.SetActive(true);
                Dict_PickUp_Tran.Remove(GID);
                break;
            }
        }
    }



    /*
    public override SingleItemInfo GetCurSelectedWeaponInfo()
    {
        SingleItemInfo itemInfo = null;
        switch (CurrentSelected)
        {
            case 0:
                itemInfo = null;
                break;
            case 1:
                itemInfo = PlayerStatusManager.Instance.Weapon1;
                break;
            case 2:
                itemInfo = PlayerStatusManager.Instance.Weapon2;
                break;
            case 3:
                itemInfo = PlayerStatusManager.Instance.Magic1;
                break;
            case 4:
                itemInfo = PlayerStatusManager.Instance.Magic2;
                break;
        }
        return itemInfo;
    }



        */

    public bool CheckCanPickUp(GameObject center, GameObject Item, float distance, float angle)
    {
        float dis = (Item.transform.position - center.transform.position).magnitude;
        Vector3 relativeVector = Item.transform.position - center.transform.position;
        float ang = Vector3.Angle(relativeVector, center.transform.forward);
        if (Item.GetComponent<GroundItem>())
        {
            if (dis < distance && ang < angle)
            {
                if (!List_CanPickUp.Contains(Item.GetComponent<GroundItem>()))
                    List_CanPickUp.Add(Item.GetComponent<GroundItem>());
                return true;
            }
            else
            {
                if (List_CanPickUp.Contains(Item.GetComponent<GroundItem>()))
                    List_CanPickUp.Remove(Item.GetComponent<GroundItem>());
                return false;
            }
        }
        else
            return false;

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<GroundItem>())
        {
            if (!List_CanPickUp.Contains(other.GetComponent<GroundItem>()))
                List_CanPickUp.Add(other.GetComponent<GroundItem>());
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<GroundItem>())
        {
            if (List_CanPickUp.Contains(other.GetComponent<GroundItem>()))
                List_CanPickUp.Remove(other.GetComponent<GroundItem>());
        }
    }


    #endregion

    /*
    /// <summary>
    /// 检测敌人是否处于攻击范围
    /// </summary>
    /// <param name="center"></param>
    /// <param name="enemy"></param>
    /// <param name="distance"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
    public  bool CheckCanAttack(GameObject center, GameObject enemy, float distance, float angle)
    {
        float dis = (enemy.transform.position - center.transform.position).magnitude;
        Vector3 relativeVector = enemy.transform.position - center.transform.position;
        float ang = Vector3.Angle(relativeVector, center.transform.forward);

        if (dis < distance && ang < angle)
        {
            if (!List_CanAttack.Contains(enemy.GetComponent<NetworkPlayer>()))
                List_CanAttack.Add(enemy.GetComponent<NetworkPlayer>());
            return true;
        }
        else
        {
            if (List_CanAttack.Contains(enemy.GetComponent<NetworkPlayer>()))
                List_CanAttack.Remove(enemy.GetComponent<NetworkPlayer>());
            return false;
        }
    }*/




    #region Buff相关
    public IEnumerator GetBuff(BuffType buffType, float time, CharacterStatus status)
    {
        GetBuff(buffType, status);

        yield return new WaitForSeconds(time);

        Losebuff(buffType, status);
    }

    private void GetBuff(BuffType buff, CharacterStatus status)
    {
        ;
        Material buffMat = Resources.Load("Debuff/" + buff.ToString()) as Material;
        switch (buff)
        {
            case BuffType.SlowDown:
                if (buffMat != null)
                    GameTool.FindTheChild(gameObject, "Base").GetComponent<SkinnedMeshRenderer>().material = buffMat;
                status.moveSpeed *= 0.7f;
                break;
            case BuffType.Freeze:
                if (buffMat != null)
                    GameTool.FindTheChild(gameObject, "Base").GetComponent<SkinnedMeshRenderer>().material = buffMat;
                status.moveSpeed *= 0.001f;
                break;
            case BuffType.Blind:
                Camera.main.GetComponent<BlindEffect>().enabled = true;
                break;
        }

    }

    private void Losebuff(BuffType buff, CharacterStatus status)
    {
        Material noBuffMat = Resources.Load("Debuff/NoBuff") as Material;
        switch (buff)
        {
            case BuffType.SlowDown:
                status.moveSpeed /= 0.7f;
                if (PlayerStatusManager.Instance.Body == null)
                    UnEquip(EquippedItem.Body);
                else
                    EquipClothes(PlayerStatusManager.Instance.Body.ItemID);
                break;
            case BuffType.Freeze:
                if (PlayerStatusManager.Instance.Body == null)
                    UnEquip(EquippedItem.Body);
                else
                    EquipClothes(PlayerStatusManager.Instance.Body.ItemID);
                status.moveSpeed /= 0.001f;
                break;
            case BuffType.Blind:
                Camera.main.GetComponent<BlindEffect>().enabled = false;
                break;
        }

    }
    #endregion

    #region 穿戴相关

    public void EquipClothes(int itemID)
    {
        string itemName = ItemInfoManager.Instance.GetResname(itemID);

        GameObject itemObj = ResourcesManager.Instance.GetItem(itemName);
        Material clothesMat = itemObj.GetComponent<MeshRenderer>().sharedMaterial;
        GameTool.FindTheChild(gameObject, "Base").GetComponent<SkinnedMeshRenderer>().material = clothesMat;

    }

    public void EquipHelmet(int itemID)
    {
        string itemName = ItemInfoManager.Instance.GetResname(itemID);
        GameObject itemObj = ResourcesManager.Instance.GetItem(itemName);
        GameObject RWeapon = Instantiate(itemObj, Head);
    }


    public void EquipWeapon(int itemID)
    {

        string itemName = ItemInfoManager.Instance.GetResname(itemID);
        GameObject itemObj = ResourcesManager.Instance.GetItem(itemName);
        Debug.Log(itemObj);
        GameObject RWeapon = Instantiate(itemObj, RHand);
        RWeapon.transform.parent = RHand;

        if (ItemInfoManager.Instance.GetWeaponWeaponType(itemID) == WeaponType.DoubleHands)
        {
            GameObject LWeapon = Instantiate(itemObj, LHand);
            LWeapon.transform.parent = LHand;
        }

    }

    public void EquipShield(int itemID)
    {
        string itemName = ItemInfoManager.Instance.GetResname(itemID);
        GameObject itemObj = ResourcesManager.Instance.GetItem(itemName);
        GameObject Shield = Instantiate(itemObj, LHand);
        Shield.transform.parent = LHand;
    }



    public void UnEquip(EquippedItem equippedItem)
    {
        switch (equippedItem)
        {
            case EquippedItem.Head:
                if (Head.childCount != 0)
                {
                    GameObject preHead = Head.GetChild(0).gameObject;
                    Destroy(preHead);
                }
                break;
            case EquippedItem.Body:
                GameObject itemObj = ResourcesManager.Instance.GetItem("Armor_Casual");
                Material clothesMat = itemObj.GetComponent<MeshRenderer>().sharedMaterial;
                GameTool.FindTheChild(gameObject, "Base").GetComponent<SkinnedMeshRenderer>().material = clothesMat;
                break;
            case EquippedItem.Weapon1:
                if ((RHand.childCount != 0))
                {
                    GameObject preWeapon1 = RHand.GetChild(0).gameObject;
                    string preWeaponName = preWeapon1.name;
                    //销毁
                    Destroy(preWeapon1);
                    if (LHand.childCount != 0)
                        if (LHand.GetChild(0).name == preWeaponName)
                        {
                            //销毁
                            Destroy(LHand.GetChild(0).gameObject);
                        }
                }
                break;
            case EquippedItem.Weapon2:
                if (RHand.childCount != 0)
                {
                    GameObject preWeapon2 = RHand.GetChild(0).gameObject;
                    string preWeaponName = preWeapon2.name;
                    //销毁
                    Destroy(preWeapon2);
                    if (LHand.childCount != 0)
                        if (LHand.GetChild(0).name == preWeaponName)
                        {
                            //销毁
                            Destroy(LHand.GetChild(0).gameObject);
                        }
                }
                break;
            case EquippedItem.Shield:
                if (LHand.childCount != 0)
                {
                    GameObject preShield = LHand.GetChild(0).gameObject;
                    //销毁
                    Destroy(preShield);
                }
                break;
        }
    }



    public void Move(float walkSpeed)
    {
        Vector3 moveDirection = Vector3.zero;


        moveDirection = CameraBase.Instance.transform.right * CrossPlatformInputManager.GetAxis("Horizontal") + Vector3.Scale(CameraBase.Instance.transform.forward, new Vector3(1, 0, 1)).normalized * CrossPlatformInputManager.GetAxis("Vertical");
        // moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= walkSpeed;
        CC.Move(moveDirection * Time.fixedDeltaTime);

    }

    #endregion

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



}
