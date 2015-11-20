using Att;
using Proto;
using ProtoBuf;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class EquipEnhanceLayer : MonoBehaviour
{
	private GUIEquipUpgradeScene mBaseScene;

	private CommonEquipInfoLayer mCommonEquipInfoLayer;

	private GameObject mEffect;

	private UILabel mLevelStartingValue;

	private UILabel mLevelNextValue;

	private GameObject mLevelEffect;

	private UILabel mPointName;

	private UILabel mPointStartingValue;

	private UILabel mPointAddedValue;

	private UILabel mGoldValue;

	private GameObject mAutoEnhanceBtn;

	private GameObject mEnhanceBtn;

	private CriticalPanel mCriticalLayer;

	private int totalCrit;

	private int oldEnhanceLevel;

	private int oldMasterLevel;

	public void InitWithBaseScene(GUIEquipUpgradeScene baseScene)
	{
		this.mBaseScene = baseScene;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mCommonEquipInfoLayer = GameUITools.FindGameObject("CommonEquipInfoLayer", base.gameObject).AddComponent<CommonEquipInfoLayer>();
		GameObject gameObject = GameUITools.FindGameObject("EnhanceInfo", base.gameObject);
		this.mEffect = GameUITools.FindGameObject("ui51", gameObject);
		Tools.SetParticleRQWithUIScale(this.mEffect, 4500);
		this.mEffect.gameObject.SetActive(false);
		gameObject = GameUITools.FindGameObject("Info/EnhanceLevel", gameObject);
		this.mLevelStartingValue = GameUITools.FindUILabel("StartingValue", gameObject);
		this.mLevelNextValue = GameUITools.FindUILabel("NextValue", gameObject);
		this.mLevelEffect = GameUITools.FindGameObject("Effect", gameObject);
		gameObject = gameObject.transform.parent.gameObject;
		this.mPointName = GameUITools.FindUILabel("Point", gameObject);
		this.mPointStartingValue = GameUITools.FindUILabel("StartingValue", this.mPointName.gameObject);
		this.mPointAddedValue = GameUITools.FindUILabel("AddedValue", this.mPointName.gameObject);
		gameObject = gameObject.transform.parent.gameObject;
		this.mGoldValue = GameUITools.FindUILabel("GoldTxt/Value", gameObject);
		this.mCriticalLayer = GameUITools.FindGameObject("CriticalPanel", base.gameObject).AddComponent<CriticalPanel>();
		this.mCriticalLayer.InitCriticalLayer();
		this.mAutoEnhanceBtn = GameUITools.RegisterClickEvent("AutoEnhanceBtn", new UIEventListener.VoidDelegate(this.OnAutoEnhanceBtnClick), gameObject);
		this.mEnhanceBtn = GameUITools.RegisterClickEvent("EnhanceBtn", new UIEventListener.VoidDelegate(this.OnEnhanceBtnClick), gameObject);
	}

	public void Refresh()
	{
		NGUITools.SetActive(this.mEffect, false);
		this.mCommonEquipInfoLayer.Refresh(this.mBaseScene.mEquipData, true, true);
		this.mLevelStartingValue.text = Singleton<StringManager>.Instance.GetString("equipImprove6", new object[]
		{
			this.mBaseScene.mEquipData.GetEquipEnhanceLevel(),
			Globals.Instance.Player.ItemSystem.GetMaxEquipEnhanceLevel(true)
		});
		this.mPointName.text = Tools.GetEquipAEStr((ESubTypeEquip)this.mBaseScene.mEquipData.Info.SubType);
		this.mPointStartingValue.text = this.mBaseScene.mEquipData.GetEquipEnhanceAttValue().ToString();
		if (this.mBaseScene.mEquipData.IsEnhanceMax())
		{
			this.mLevelNextValue.text = Singleton<StringManager>.Instance.GetString("equipImprove26");
			this.mLevelNextValue.color = Color.red;
			this.mLevelEffect.gameObject.SetActive(false);
			this.mPointAddedValue.enabled = false;
			this.mGoldValue.transform.parent.gameObject.SetActive(false);
			this.mAutoEnhanceBtn.gameObject.SetActive(false);
			this.mEnhanceBtn.gameObject.SetActive(false);
		}
		else
		{
			this.mLevelNextValue.text = Singleton<StringManager>.Instance.GetString("equipImprove6", new object[]
			{
				this.mBaseScene.mEquipData.GetEquipEnhanceLevel() + 1,
				Globals.Instance.Player.ItemSystem.GetMaxEquipEnhanceLevel(true)
			});
			if (this.mBaseScene.mEquipData.GetEquipEnhanceLevel() + 1 > Globals.Instance.Player.ItemSystem.GetMaxEquipEnhanceLevel(true))
			{
				this.mLevelNextValue.color = Color.red;
			}
			this.mPointAddedValue.text = Singleton<StringManager>.Instance.GetString("equipImprove23", new object[]
			{
				this.mBaseScene.mEquipData.GetEquipEnhanceAttDelta()
			});
			this.RefreshMoney();
		}
	}

	public void RefreshMoney()
	{
		if (this.mLevelEffect.gameObject.activeInHierarchy)
		{
			int equipEnhanceCost = (int)this.mBaseScene.mEquipData.GetEquipEnhanceCost();
			this.mGoldValue.text = equipEnhanceCost.ToString();
			if (Tools.CanBuy(ECurrencyType.ECurrencyT_Money, equipEnhanceCost))
			{
				this.mGoldValue.color = Tools.GetDefaultTextColor();
			}
			else
			{
				this.mGoldValue.color = Color.red;
			}
		}
	}

	public void OnAutoEnhanceBtnClick(GameObject go)
	{
		this.SendEnhanceRequest2Server(true);
	}

	public void OnEnhanceBtnClick(GameObject go)
	{
		this.SendEnhanceRequest2Server(false);
	}

	private void SendEnhanceRequest2Server(bool isFiveTimes)
	{
		if (!this.mBaseScene.mEquipData.CanEnhance())
		{
			GameUIManager.mInstance.ShowMessageTipByKey("equipImprove28", 0f, 0f);
			return;
		}
		if (Tools.MoneyNotEnough(ECurrencyType.ECurrencyT_Money, (int)this.mBaseScene.mEquipData.GetEquipEnhanceCost(), 0))
		{
			return;
		}
		this.oldEnhanceLevel = this.mBaseScene.mEquipData.GetEquipEnhanceLevel();
		if (this.mBaseScene.mSocketData != null)
		{
			this.oldMasterLevel = this.mBaseScene.mSocketData.EquipMasterEnhanceLevel;
		}
		MC2S_EquipEnhance mC2S_EquipEnhance = new MC2S_EquipEnhance();
		mC2S_EquipEnhance.Type = ((!isFiveTimes) ? 2 : 1);
		mC2S_EquipEnhance.Value = this.mBaseScene.mEquipData.Data.ID;
		Globals.Instance.CliSession.Send(520, mC2S_EquipEnhance);
	}

	public void OnMsgEquipEnhance(MemoryStream stream)
	{
		MS2C_EquipEnhance mS2C_EquipEnhance = Serializer.NonGeneric.Deserialize(typeof(MS2C_EquipEnhance), stream) as MS2C_EquipEnhance;
		if (mS2C_EquipEnhance.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("ItemR", mS2C_EquipEnhance.Result);
			return;
		}
		this.totalCrit = mS2C_EquipEnhance.TotalCrit;
		base.StopCoroutine("ShowEnhanceEffect");
		base.StartCoroutine("ShowEnhanceEffect");
	}

	private void OnDisable()
	{
		NGUITools.SetActive(this.mEffect, false);
		this.mCriticalLayer.EnableCriticalLayer(false);
	}

	private void PlayAnim()
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_008");
		this.mCriticalLayer.EnableCriticalLayer(true);
		this.mEffect.SetActive(false);
		this.mEffect.SetActive(true);
	}

	[DebuggerHidden]
	private IEnumerator ShowEnhanceEffect()
	{
        return null;
        //EquipEnhanceLayer.<ShowEnhanceEffect>c__Iterator44 <ShowEnhanceEffect>c__Iterator = new EquipEnhanceLayer.<ShowEnhanceEffect>c__Iterator44();
        //<ShowEnhanceEffect>c__Iterator.<>f__this = this;
        //return <ShowEnhanceEffect>c__Iterator;
	}

	private void HideCritical()
	{
		this.mCriticalLayer.EnableCriticalLayer(false);
	}
}
