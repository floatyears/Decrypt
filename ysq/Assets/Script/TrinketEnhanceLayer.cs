using Proto;
using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class TrinketEnhanceLayer : MonoBehaviour
{
	public GUITrinketUpgradeScene mBaseScene;

	private CommonEquipInfoLayer mCommonEquipInfoLayer;

	private GameObject mEffect;

	private UILabel mLevel;

	private UILabel mAddLevel;

	private UISlider mExpProgressBar;

	private UISprite mBarFG;

	private UISprite mBarBG;

	private UILabel mBarValue;

	private UILabel mPoint0;

	private UILabel mPoint0StartingValue;

	private UILabel mPoint0AddedValue;

	private UILabel mPoint1;

	private UILabel mPoint1StartingValue;

	private UILabel mPoint1AddedValue;

	private List<TrinketEnhanceExpItem> mItems = new List<TrinketEnhanceExpItem>();

	private UILabel mGoldValue;

	private GameObject mAutoAddBtn;

	private GameObject mEnhanceBtn;

	private int tempExp;

	private int tempCost;

	private int oldEnhanceLevel;

	private int oldMasterLevel;

	public void InitWithBaseScene(GUITrinketUpgradeScene baseScene)
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
		gameObject = GameUITools.FindGameObject("EnhanceInfo/Info", base.gameObject);
		this.mLevel = GameUITools.FindUILabel("Level", gameObject);
		this.mAddLevel = GameUITools.FindUILabel("AddLevel", this.mLevel.gameObject);
		this.mExpProgressBar = GameUITools.FindGameObject("ExpProgressBar", this.mLevel.gameObject).GetComponent<UISlider>();
		this.mBarFG = GameUITools.FindUISprite("FG", this.mExpProgressBar.gameObject);
		this.mBarBG = GameUITools.FindUISprite("BG", this.mExpProgressBar.gameObject);
		this.mBarValue = GameUITools.FindUILabel("Value", this.mExpProgressBar.gameObject);
		this.mPoint0 = GameUITools.FindUILabel("Point0", gameObject);
		this.mPoint0StartingValue = GameUITools.FindUILabel("StartingValue", this.mPoint0.gameObject);
		this.mPoint0AddedValue = GameUITools.FindUILabel("AddedValue", this.mPoint0.gameObject);
		this.mPoint1 = GameUITools.FindUILabel("Point1", gameObject);
		this.mPoint1StartingValue = GameUITools.FindUILabel("StartingValue", this.mPoint1.gameObject);
		this.mPoint1AddedValue = GameUITools.FindUILabel("AddedValue", this.mPoint1.gameObject);
		gameObject = GameUITools.FindGameObject("Items", gameObject.transform.parent.gameObject);
		for (int i = 0; i < gameObject.transform.childCount; i++)
		{
			this.mItems.Add(gameObject.transform.GetChild(i).gameObject.AddComponent<TrinketEnhanceExpItem>());
			this.mItems[i].InitWithBaseScene(this);
		}
		gameObject = gameObject.transform.parent.gameObject;
		this.mGoldValue = GameUITools.FindUILabel("GoldTxt/Value", gameObject);
		this.mAutoAddBtn = GameUITools.RegisterClickEvent("AutoAddBtn", new UIEventListener.VoidDelegate(this.OnAutoAddBtnClick), gameObject);
		this.mEnhanceBtn = GameUITools.RegisterClickEvent("EnhanceBtn", new UIEventListener.VoidDelegate(this.OnEnhanceBtnClick), gameObject);
	}

	public void Refresh(List<ItemDataEx> datas = null)
	{
		if (datas == null)
		{
			NGUITools.SetActive(this.mEffect, false);
		}
		this.mCommonEquipInfoLayer.Refresh(this.mBaseScene.mEquipData, true, true);
		this.mLevel.text = Singleton<StringManager>.Instance.GetString("equipImprove36", new object[]
		{
			this.mBaseScene.mEquipData.GetTrinketEnhanceLevel()
		});
		if (this.mBaseScene.mEquipData.IsEnhanceMax())
		{
			this.mExpProgressBar.GetComponent<UISprite>().enabled = false;
			this.mBarFG.enabled = false;
			this.mBarBG.enabled = false;
			this.mBarValue.text = Singleton<StringManager>.Instance.GetString("equipImprove26");
			this.mBarValue.color = Color.red;
			this.mAddLevel.enabled = false;
			this.mPoint0AddedValue.enabled = false;
			this.mPoint1AddedValue.enabled = false;
			foreach (TrinketEnhanceExpItem current in this.mItems)
			{
				current.gameObject.SetActive(false);
			}
			this.mGoldValue.transform.parent.gameObject.SetActive(false);
			this.mAutoAddBtn.gameObject.SetActive(false);
			this.mEnhanceBtn.gameObject.SetActive(false);
		}
		else
		{
			int trinketEnhanceMaxExp = (int)this.mBaseScene.mEquipData.GetTrinketEnhanceMaxExp();
			this.mBarValue.text = Singleton<StringManager>.Instance.GetString("equipImprove6", new object[]
			{
				this.mBaseScene.mEquipData.GetTrinketEnhanceExp(),
				trinketEnhanceMaxExp
			});
			this.mExpProgressBar.value = ((trinketEnhanceMaxExp != 0) ? ((float)this.mBaseScene.mEquipData.GetTrinketEnhanceExp() / (float)trinketEnhanceMaxExp) : 1f);
			this.tempExp = 0;
			this.tempCost = 0;
			if (datas != null)
			{
				int i = 0;
				while (i < datas.Count && i < this.mItems.Count)
				{
					this.mItems[i].Refresh(datas[i]);
					if (this.mItems[i].mData != null)
					{
						this.tempExp += this.mItems[i].mData.GetTrinketOrItem2EnhanceExp();
						this.tempCost += this.mItems[i].mData.GetTrinketOrItem2EnhanceCost();
					}
					i++;
				}
				while (i < this.mItems.Count)
				{
					this.mItems[i].Refresh(null);
					i++;
				}
			}
			else
			{
				foreach (TrinketEnhanceExpItem current2 in this.mItems)
				{
					current2.Refresh(true);
					if (current2.mData != null)
					{
						this.tempExp += current2.mData.GetTrinketOrItem2EnhanceExp();
						this.tempCost += current2.mData.GetTrinketOrItem2EnhanceCost();
					}
				}
			}
			this.mBarBG.enabled = (this.tempExp > 0);
			int num = this.mBaseScene.mEquipData.GetTrinketEnhanceLevelWithExp(this.tempExp) - this.mBaseScene.mEquipData.GetTrinketEnhanceLevel();
			if (num > 0 && !this.mBaseScene.mEquipData.IsEnhanceMax())
			{
				this.mAddLevel.enabled = true;
				this.mAddLevel.text = Singleton<StringManager>.Instance.GetString("equipImprove23", new object[]
				{
					num
				});
				this.mBarBG.fillAmount = 1f;
				this.mBarBG.enabled = true;
				this.mPoint0AddedValue.enabled = true;
				this.mPoint1AddedValue.enabled = true;
				this.mPoint0AddedValue.text = Singleton<StringManager>.Instance.GetString("equipImprove23", new object[]
				{
					this.mBaseScene.mEquipData.GetTrinketEnhanceAttDelta0() * num
				});
				this.mPoint1AddedValue.text = Singleton<StringManager>.Instance.GetString("equipImprove23", new object[]
				{
					Singleton<StringManager>.Instance.GetString("equipImprove18", new object[]
					{
						(this.mBaseScene.mEquipData.GetTrinketEnhanceAttDelta1() * (float)num).ToString("0.0")
					})
				});
			}
			else
			{
				this.mAddLevel.enabled = false;
				this.mBarBG.fillAmount = ((trinketEnhanceMaxExp != 0) ? ((float)(this.tempExp + this.mBaseScene.mEquipData.GetTrinketEnhanceExp()) / (float)trinketEnhanceMaxExp) : 1f);
				this.mPoint0AddedValue.enabled = false;
				this.mPoint1AddedValue.enabled = false;
			}
			this.RefreshMoney();
		}
		this.mPoint0.text = Tools.GetTrinketAEStr(this.mBaseScene.mEquipData, 0);
		this.mPoint1.text = Tools.GetTrinketAEStr(this.mBaseScene.mEquipData, 1);
		this.mPoint0StartingValue.text = this.mBaseScene.mEquipData.GetTrinketEnhanceAttValue0().ToString();
		this.mPoint1StartingValue.text = Singleton<StringManager>.Instance.GetString("equipImprove18", new object[]
		{
			this.mBaseScene.mEquipData.GetTrinketEnhanceAttValue1().ToString("0.0")
		});
	}

	public void RefreshMoney()
	{
		if (this.mGoldValue.gameObject.activeInHierarchy)
		{
			this.mGoldValue.text = this.tempCost.ToString();
			if (Tools.CanBuy(ECurrencyType.ECurrencyT_Money, this.tempCost))
			{
				this.mGoldValue.color = Tools.GetDefaultTextColor();
			}
			else
			{
				this.mGoldValue.color = Color.red;
			}
		}
	}

	private void OnAutoAddBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		List<ItemDataEx> list = new List<ItemDataEx>();
		foreach (ItemDataEx current in Globals.Instance.Player.ItemSystem.Values)
		{
			if ((current.Info.Type == 4 && current.Info.SubType == 9) || (current.Info.Type == 1 && !current.IsEquiped() && current.GetTrinketRefineLevel() <= 0 && current.Info.Quality <= 1 && current.GetID() != this.mBaseScene.mEquipData.GetID()))
			{
				list.Add(current);
			}
		}
		if (list.Count == 0)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("equipImprove38", 0f, 0f);
		}
		else
		{
			list.Sort(new Comparison<ItemDataEx>(TrinketEnhanceSelectItemBagUITable.Sort));
			if (list.Count > 5)
			{
				list.RemoveRange(5, list.Count - 5);
			}
			MC2S_TrinketEnhance trinketEnhanceData = GameUIManager.mInstance.uiState.TrinketEnhanceData;
			trinketEnhanceData.ItemID.Clear();
			foreach (ItemDataEx current2 in list)
			{
				trinketEnhanceData.ItemID.Add(current2.GetID());
			}
			this.Refresh(list);
		}
	}

	private void OnEnhanceBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (!this.mBaseScene.mEquipData.CanEnhance())
		{
			GameUIManager.mInstance.ShowMessageTipByKey("equipImprove28", 0f, 0f);
			return;
		}
		if (this.tempExp <= 0)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("equipImprove37", 0f, 0f);
		}
		else if (!Tools.MoneyNotEnough(ECurrencyType.ECurrencyT_Money, this.tempCost, 0))
		{
			this.oldEnhanceLevel = this.mBaseScene.mEquipData.GetTrinketEnhanceLevel();
			if (this.mBaseScene.mSocketData != null)
			{
				this.oldMasterLevel = this.mBaseScene.mSocketData.TrinketMasterEnhanceLevel;
			}
			MC2S_TrinketEnhance trinketEnhanceData = GameUIManager.mInstance.uiState.TrinketEnhanceData;
			trinketEnhanceData.ItemID.Sort();
			for (int i = 0; i < trinketEnhanceData.ItemID.Count - 1; i++)
			{
				if (trinketEnhanceData.ItemID[i] == trinketEnhanceData.ItemID[i + 1])
				{
					global::Debug.LogErrorFormat("same id : {0} ", new object[]
					{
						trinketEnhanceData.ItemID[i]
					});
					GameUIManager.mInstance.ShowMessageTipByKey("equipImprove79", 0f, 0f);
					return;
				}
			}
			Globals.Instance.CliSession.Send(524, GameUIManager.mInstance.uiState.TrinketEnhanceData);
		}
	}

	public void OnMsgTrinketEnhance(MemoryStream stream)
	{
		MS2C_TrinketEnhance mS2C_TrinketEnhance = Serializer.NonGeneric.Deserialize(typeof(MS2C_TrinketEnhance), stream) as MS2C_TrinketEnhance;
		if (mS2C_TrinketEnhance.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("ItemR", mS2C_TrinketEnhance.Result);
			return;
		}
		GameUIManager.mInstance.uiState.TrinketEnhanceData.ItemID.Clear();
		this.tempExp = 0;
		this.tempCost = 0;
		this.PlayTweenAnim();
	}

	private void PlayTweenAnim()
	{
		foreach (TrinketEnhanceExpItem current in this.mItems)
		{
			if (current.mData != null)
			{
				current.Refresh(null);
				current.PlayAnim();
			}
		}
		base.StopCoroutine("PlayAnim");
		base.StartCoroutine("PlayAnim");
	}

	[DebuggerHidden]
	private IEnumerator PlayAnim()
	{
        return null;
        //TrinketEnhanceLayer.<PlayAnim>c__Iterator4B <PlayAnim>c__Iterator4B = new TrinketEnhanceLayer.<PlayAnim>c__Iterator4B();
        //<PlayAnim>c__Iterator4B.<>f__this = this;
        //return <PlayAnim>c__Iterator4B;
	}

	private void OnDisable()
	{
		NGUITools.SetActive(this.mEffect, false);
	}

	public List<ItemDataEx> GetAllExpItems()
	{
		List<ItemDataEx> list = new List<ItemDataEx>();
		foreach (TrinketEnhanceExpItem current in this.mItems)
		{
			if (current.mData != null)
			{
				list.Add(current.mData);
			}
		}
		return list;
	}
}
