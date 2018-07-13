using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;
using Meaningless;
using UnityStandardAssets.CrossPlatformInput;

public class HUDUI : BaseUI
{

    //AutoStatement
   // private Image Img_Weapon1 = null;
    //private Image Img_Weapon2 = null;
    private Image Img_Skill1 = null;
    private Image Img_Skill2 = null;
    private Image Img_Skill1_Mask = null;
    private Image Img_Skill2_Mask = null;
    private Image Img_Shield = null;
    private Image Img_PickUpTip = null;
    private Image Img_FrontSight = null;
    private Text Text_Skill1_Count = null;
    private Text Text_Skill2_Count = null;
    private Slider Slider_HP = null;
    private Text Text_Remain = null;
    private Text Text_Time = null;

    private EventTrigger Skill1 = null;
    private EventTrigger Skill2 = null;
    private EventTrigger LightAttack = null;
    private EventTrigger HardAttack = null;
    private EventTrigger Roll = null;
    private EventTrigger Bag = null;

    CrossPlatformInputManager.VirtualButton m_LB;
    CrossPlatformInputManager.VirtualButton m_RB;
    CrossPlatformInputManager.VirtualButton m_Fire1;
    CrossPlatformInputManager.VirtualButton m_Fire2;
    CrossPlatformInputManager.VirtualButton m_Roll;
    CrossPlatformInputManager.VirtualButton m_PickUp;
    CrossPlatformInputManager.VirtualButton m_Bag;
    CrossPlatformInputManager.VirtualButton m_Jump;

    private float lastTime = 0;
    protected override void InitUiOnAwake()
    {
        Img_PickUpTip = GameTool.GetTheChildComponent<Image>(this.gameObject, "PickUpTip");
        //Img_Weapon1 = GameTool.GetTheChildComponent<Image>(this.gameObject, "Img_Weapon1");
        //Img_Weapon2 = GameTool.GetTheChildComponent<Image>(this.gameObject, "Img_Weapon2");
        Img_Skill1 = GameTool.GetTheChildComponent<Image>(this.gameObject, "Img_Skill1");
        Img_Skill2 = GameTool.GetTheChildComponent<Image>(this.gameObject, "Img_Skill2");
        Img_Skill1_Mask = GameTool.GetTheChildComponent<Image>(Img_Skill1.gameObject, "Ing_Skill1_Mask");
        Img_Skill2_Mask = GameTool.GetTheChildComponent<Image>(Img_Skill2.gameObject, "Ing_Skill2_Mask");
        Img_Shield = GameTool.GetTheChildComponent<Image>(this.gameObject, "Img_Shield");
        Img_FrontSight = GameTool.GetTheChildComponent<Image>(gameObject, "Img_FrontSight");
        Text_Skill1_Count = GameTool.GetTheChildComponent<Text>(this.gameObject, "Text_Count1");
        Text_Skill2_Count = GameTool.GetTheChildComponent<Text>(this.gameObject, "Text_Count2");
        Slider_HP = GameTool.GetTheChildComponent<Slider>(this.gameObject, "Slider");
        Text_Remain = GameTool.GetTheChildComponent<Text>(this.gameObject, "Text_Remain");
        Text_Time = GameTool.GetTheChildComponent<Text>(gameObject, "TextTime");

        Skill1= GameTool.GetTheChildComponent<EventTrigger>(this.gameObject, "Panel_Skill1");
        Skill2= GameTool.GetTheChildComponent<EventTrigger>(this.gameObject, "Panel_Skill2");
        LightAttack= GameTool.GetTheChildComponent<EventTrigger>(this.gameObject, "LightAttack");
        HardAttack = GameTool.GetTheChildComponent<EventTrigger>(this.gameObject, "HardAttack");
        Roll=GameTool.GetTheChildComponent<EventTrigger>(this.gameObject, "Roll");
        Bag= GameTool.GetTheChildComponent<EventTrigger>(this.gameObject, "Bag");

        CreateVirturlBtn();

        AddTriggersListener(Skill1, EventTriggerType.PointerUp, OnSkill1Click);
        AddTriggersListener(Skill2, EventTriggerType.PointerUp, OnSkill2Click);
        AddTriggersListener(LightAttack, EventTriggerType.PointerUp, OnLightAttackClick);
        AddTriggersListener(HardAttack, EventTriggerType.PointerUp, OnHardAttackClick);
        AddTriggersListener(Roll, EventTriggerType.PointerUp, OnRollClick);
        AddTriggersListener(Bag, EventTriggerType.PointerUp, OnBagClick);


        MessageCenter.AddListener(EMessageType.FoundItem, AwakePickUpTip);
        MessageCenter.AddListener(EMessageType.CurrentHP, UpdateHP);
        MessageCenter.AddListener(EMessageType.Remain, (object[] obj) => { Text_Remain.text = "" + (int)obj[0]; });
        MessageCenter.AddListener(EMessageType.RefreshHUD, SetBarIcon);
        MessageCenter.AddListener(EMessageType.RefreshHUD, UpdateTime);
        MessageCenter.AddListener(EMessageType.RefreshHUD, UpdateSkillCount);
    }

    private void Update()
    {
        
    }

    protected override void InitDataOnAwake()
    {
        this.uiId = UIid.HUDUI;
    }

    #region 按钮事件
    private void CreateVirturlBtn()
    {
        m_LB = new CrossPlatformInputManager.VirtualButton("LB");
        CrossPlatformInputManager.RegisterVirtualButton(m_LB);
        m_RB = new CrossPlatformInputManager.VirtualButton("RB");
        CrossPlatformInputManager.RegisterVirtualButton(m_RB);
        m_Fire1 = new CrossPlatformInputManager.VirtualButton("Fire1");
        CrossPlatformInputManager.RegisterVirtualButton(m_Fire1);
        m_Fire2 = new CrossPlatformInputManager.VirtualButton("Fire2");
        CrossPlatformInputManager.RegisterVirtualButton(m_Fire2);
        m_Roll = new CrossPlatformInputManager.VirtualButton("Roll");
        CrossPlatformInputManager.RegisterVirtualButton(m_Roll);
        m_PickUp = new CrossPlatformInputManager.VirtualButton("PickUp");
        CrossPlatformInputManager.RegisterVirtualButton(m_PickUp);
        m_Bag = new CrossPlatformInputManager.VirtualButton("Bag");
        CrossPlatformInputManager.RegisterVirtualButton(m_Bag);
        m_Jump = new CrossPlatformInputManager.VirtualButton("Jump");
        CrossPlatformInputManager.RegisterVirtualButton(m_Jump);
    }

    private void OnRollClick(BaseEventData eventData)
    {
        m_Roll.Released();
    }

    private void OnSkill1Click(BaseEventData eventData)
    {
        m_LB.Released();
    }

    private void OnSkill2Click(BaseEventData eventData)
    {
        m_RB.Released();
    }

    private void OnLightAttackClick(BaseEventData eventData)
    {
        if (Img_PickUpTip.IsActive())
        {
            m_PickUp.Released();
        }
        else
            m_Fire1.Released();
    }

    private void OnHardAttackClick(BaseEventData eventData)
    {
        m_Fire2.Released();
    }

    private void OnBagClick(BaseEventData eventData)
    {
        m_Bag.Released();
    }
    #endregion

    private void AwakePickUpTip(object[] Active)
    {

        Img_PickUpTip.gameObject.SetActive((bool)Active[0]);
    }

    private void UpdateSkillCount(object[] obj)
    {
        if (PlayerStatusManager.Instance.Magic1 != PlayerStatusManager.Instance.NullInfo)
        {
            Text_Skill1_Count.gameObject.SetActive(true);
            Img_Skill1_Mask.gameObject.SetActive(true);
            Text_Skill1_Count.text = PlayerStatusManager.Instance.skillAttributesList[0].remainCount + "/" + PlayerStatusManager.Instance.skillAttributesList[0].skillInfo.magicProperties.UsableCount;
            Img_Skill1_Mask.fillAmount = PlayerStatusManager.Instance.skillAttributesList[0].Timer / PlayerStatusManager.Instance.skillAttributesList[0].skillInfo.magicProperties.CDTime;
        }
        else
        {
            Text_Skill1_Count.gameObject.SetActive(false);
            Img_Skill1_Mask.gameObject.SetActive(false);
        }

        if (PlayerStatusManager.Instance.Magic2 != PlayerStatusManager.Instance.NullInfo)
        {
            Text_Skill2_Count.gameObject.SetActive(true);
            Img_Skill2_Mask.gameObject.SetActive(true);
            Text_Skill2_Count.text = PlayerStatusManager.Instance.skillAttributesList[1].remainCount + "/" + PlayerStatusManager.Instance.skillAttributesList[1].skillInfo.magicProperties.UsableCount;
            Img_Skill2_Mask.fillAmount = PlayerStatusManager.Instance.skillAttributesList[1].Timer / PlayerStatusManager.Instance.skillAttributesList[1].skillInfo.magicProperties.CDTime;
        }
        else
        {
            Text_Skill2_Count.gameObject.SetActive(false);
            Img_Skill2_Mask.gameObject.SetActive(false);
        }
    }

    private void UpdateHP(object[] HP)
    {
        Slider_HP.value = (float)HP[0] / 100;
    }

    private void SetBarIcon(object obj)
    {
        #region 
        /*
        if (PlayerStatusManager.Instance.Weapon1.weaponProperties != null)
        {
            if (PlayerStatusManager.Instance.Weapon1.ResName != "")
                Img_Weapon1.sprite = ResourcesManager.Instance.GetUITexture(PlayerStatusManager.Instance.Weapon1.ResName);
        }
        else
            Img_Weapon1.sprite = ResourcesManager.Instance.GetUITexture("Null");

        if (PlayerStatusManager.Instance.Weapon2.weaponProperties != null)
        {
            if (PlayerStatusManager.Instance.Weapon2.ResName != "")
                Img_Weapon2.sprite = ResourcesManager.Instance.GetUITexture(PlayerStatusManager.Instance.Weapon2.ResName);
        }
        else
            Img_Weapon2.sprite = ResourcesManager.Instance.GetUITexture("Null");
            */
        #endregion

        if (PlayerStatusManager.Instance.Magic1.magicProperties != null)
        {
            if (PlayerStatusManager.Instance.Magic1.ResName != "")
                Img_Skill1.sprite = ResourcesManager.Instance.GetUITexture(PlayerStatusManager.Instance.Magic1.ResName);
        }
        else
            Img_Skill1.sprite = ResourcesManager.Instance.GetUITexture("Null");

        if (PlayerStatusManager.Instance.Magic2.magicProperties != null)
        {
            if (PlayerStatusManager.Instance.Magic2.ResName != "")
                Img_Skill2.sprite = ResourcesManager.Instance.GetUITexture(PlayerStatusManager.Instance.Magic2.ResName);
        }
        else
            Img_Skill2.sprite = ResourcesManager.Instance.GetUITexture("Null");

    }

    private void UpdateTime(object[] obj)
    {
        if (MapManager.Instance.Moving)
        {
            Text_Time.text = "暗影移动时间: " + MapManager.Instance.countdownTime.ToString() + "秒";
        }
        else
        {
            Text_Time.text = "暗影保持时间: " + MapManager.Instance.countdownTime.ToString() + "秒";
        }
    }

    private void AddTriggersListener(EventTrigger trigger, EventTriggerType eventID, UnityEngine.Events.UnityAction<BaseEventData> action)
    {

        if (trigger.triggers.Count == 0)
        {
            trigger.triggers = new List<EventTrigger.Entry>();
        }

        UnityEngine.Events.UnityAction<BaseEventData> callback = new UnityEngine.Events.UnityAction<BaseEventData>(action);
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = eventID;
        entry.callback.AddListener(callback);
        trigger.triggers.Add(entry);
    }

}
