using Att;
using Proto;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class TrinketRefineLayer : MonoBehaviour
{
	public GUITrinketUpgradeScene mBaseScene;

	private CommonEquipInfoLayer mCommonEquipInfoLayer;

	private GameObject ui53;

	private UILabel mLevelStartingValue;

	private UILabel mLevelNextValue;

	private GameObject mLevelEffect;

	private UILabel mPoint0;

	private UILabel mPoint0StartingValue;

	private UILabel mPoint0AddedValue;

	private UILabel mPoint1;

	private UILabel mPoint1StartingValue;

	private UILabel mPoint1AddedValue;

	private UILabel mLegendPoint;

	private UILabel mGoldValue;

	private int needMoney;

	private List<TrinketRefineExpItem> mItems = new List<TrinketRefineExpItem>();

	private GameObject mRefineBtn;

	private int oldMasterLevel;

	public void InitWithBaseScene(GUITrinketUpgradeScene baseScene)
	{
		this.mBaseScene = baseScene;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mCommonEquipInfoLayer = GameUITools.FindGameObject("CommonEquipInfoLayer", base.gameObject).AddComponent<CommonEquipInfoLayer>();
		this.ui53 = GameUITools.FindGameObject("ui53", base.gameObject);
		Tools.SetParticleRQWithUIScale(this.ui53, 4500);
		this.ui53.gameObject.SetActive(false);
		GameObject gameObject = GameUITools.FindGameObject("RefineInfo/Info/Level", base.gameObject);
		this.mLevelStartingValue = GameUITools.FindUILabel("StartingValue", gameObject);
		this.mLevelEffect = GameUITools.FindGameObject("Effect", gameObject);
		this.mLevelNextValue = GameUITools.FindUILabel("NextValue", gameObject);
		gameObject = gameObject.transform.parent.gameObject;
		this.mPoint0 = GameUITools.FindUILabel("Point0", gameObject);
		this.mPoint0StartingValue = GameUITools.FindUILabel("StartingValue", this.mPoint0.gameObject);
		this.mPoint0AddedValue = GameUITools.FindUILabel("AddedValue", this.mPoint0.gameObject);
		this.mPoint1 = GameUITools.FindUILabel("Point1", gameObject);
		this.mPoint1StartingValue = GameUITools.FindUILabel("StartingValue", this.mPoint1.gameObject);
		this.mPoint1AddedValue = GameUITools.FindUILabel("AddedValue", this.mPoint1.gameObject);
		this.mLegendPoint = GameUITools.FindUILabel("LegendPoint", gameObject);
		gameObject = gameObject.transform.parent.gameObject;
		this.mRefineBtn = GameUITools.RegisterClickEvent("RefineBtn", new UIEventListener.VoidDelegate(this.OnRefineBtnClick), gameObject);
		this.mGoldValue = GameUITools.FindUILabel("GoldTxt/Value", gameObject);
		gameObject = GameUITools.FindGameObject("Items", gameObject);
		int num = 0;
		while (num < 2 && num < gameObject.transform.childCount)
		{
			this.mItems.Add(gameObject.transform.GetChild(num).gameObject.AddComponent<TrinketRefineExpItem>());
			this.mItems[num].InitWithBaseScene();
			num++;
		}
	}

	private void OnRefineBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (!this.mBaseScene.mEquipData.CanRefine())
		{
			GameUIManager.mInstance.ShowMessageTipByKey("equipImprove27", 0f, 0f);
			return;
		}
		if (!Tools.MoneyNotEnough(ECurrencyType.ECurrencyT_Money, this.needMoney, 0))
		{
			if (this.mBaseScene.mEquipData.IsTrinketEnoughItem2Refine())
			{
				if (this.mBaseScene.mSocketData != null)
				{
					this.oldMasterLevel = this.mBaseScene.mSocketData.TrinketMasterRefineLevel;
				}
				MC2S_TrinketRefine mC2S_TrinketRefine = new MC2S_TrinketRefine();
				mC2S_TrinketRefine.TrinketID = this.mBaseScene.mEquipData.GetID();
				Globals.Instance.CliSession.Send(526, mC2S_TrinketRefine);
			}
			else
			{
				GameUIManager.mInstance.ShowMessageTipByKey("equipImprove39", 0f, 0f);
			}
		}
	}

	public void OnMsgTrinketRefine(MemoryStream stream)
	{
		MS2C_TrinketRefine mS2C_TrinketRefine = Serializer.NonGeneric.Deserialize(typeof(MS2C_TrinketRefine), stream) as MS2C_TrinketRefine;
		if (mS2C_TrinketRefine.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("ItemR", mS2C_TrinketRefine.Result);
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine(Singleton<StringManager>.Instance.GetString("equipImprove14", new object[]
		{
			Tools.GetTrinketARStr(this.mBaseScene.mEquipData, 0),
			Singleton<StringManager>.Instance.GetString("equipImprove18", new object[]
			{
				this.mBaseScene.mEquipData.GetTrinketRefineAttDelta0().ToString("0.0")
			})
		}));
		stringBuilder.AppendLine(Singleton<StringManager>.Instance.GetString("equipImprove14", new object[]
		{
			Tools.GetTrinketARStr(this.mBaseScene.mEquipData, 1),
			Singleton<StringManager>.Instance.GetString("equipImprove18", new object[]
			{
				this.mBaseScene.mEquipData.GetTrinketRefineAttDelta1().ToString("0.0")
			})
		}));
		if (this.mBaseScene.mSocketData != null && this.mBaseScene.mSocketData.TrinketMasterRefineLevel > this.oldMasterLevel)
		{
			GUIUpgradeTipPopUp.ShowThis(Singleton<StringManager>.Instance.GetString("equipImprove65", new object[]
			{
				this.mBaseScene.mEquipData.GetTrinketRefineLevel()
			}), stringBuilder.ToString().TrimEnd(new char[0]), Singleton<StringManager>.Instance.GetString("equipImprove58", new object[]
			{
				this.mBaseScene.mSocketData.TrinketMasterRefineLevel
			}), Master.GetMasterDiffValueStr(this.oldMasterLevel, this.mBaseScene.mSocketData.TrinketMasterRefineLevel, Master.EMT.EMT_TrinletRefine), 5f, 1f);
		}
		else
		{
			GUIUpgradeTipPopUp.ShowThis(Singleton<StringManager>.Instance.GetString("equipImprove65", new object[]
			{
				this.mBaseScene.mEquipData.GetTrinketRefineLevel()
			}), stringBuilder.ToString().TrimEnd(new char[0]), string.Empty, string.Empty, 5f, 1f);
		}
		Globals.Instance.EffectSoundMgr.Play("ui/ui_020b");
		foreach (TrinketRefineExpItem current in this.mItems)
		{
			if (current.gameObject.activeInHierarchy)
			{
				current.PlayAnim();
			}
		}
		this.Refresh(true);
		NGUITools.SetActive(this.ui53, true);
		GameUIManager.mInstance.TryCommend(ECommentType.EComment_GoldRefine, 0f);
	}

	public void Refresh(bool showEffect = false)
	{
		NGUITools.SetActive(this.ui53, false);
		if (!showEffect)
		{
			foreach (TrinketRefineExpItem current in this.mItems)
			{
				NGUITools.SetActive(current.ui56_2, false);
			}
		}
		this.mCommonEquipInfoLayer.Refresh(this.mBaseScene.mEquipData, false, true);
		this.mLevelStartingValue.text = this.mBaseScene.mEquipData.GetTrinketRefineLevel().ToString();
		this.mPoint0.text = Tools.GetTrinketARStr(this.mBaseScene.mEquipData, 0);
		this.mPoint1.text = Tools.GetTrinketARStr(this.mBaseScene.mEquipData, 1);
		this.mPoint0StartingValue.text = Singleton<StringManager>.Instance.GetString("equipImprove23", new object[]
		{
			Singleton<StringManager>.Instance.GetString("equipImprove18", new object[]
			{
				this.mBaseScene.mEquipData.GetTrinketRefineAttValue0().ToString("0.0")
			})
		});
		this.mPoint1StartingValue.text = Singleton<StringManager>.Instance.GetString("equipImprove23", new object[]
		{
			Singleton<StringManager>.Instance.GetString("equipImprove18", new object[]
			{
				this.mBaseScene.mEquipData.GetTrinketRefineAttValue1().ToString("0.0")
			})
		});
		LegendInfo legendInfo = Tools.GetLegendInfo(this.mBaseScene.mEquipData.Info);
		if (legendInfo != null)
		{
			this.mLegendPoint.enabled = true;
			this.mLegendPoint.text = Tools.GetNextLegendSkillStr(legendInfo, this.mBaseScene.mEquipData.GetTrinketRefineLevel());
		}
		else
		{
			this.mLegendPoint.enabled = false;
		}
		int num;
		int needCount;
		int needCount2;
		this.mBaseScene.mEquipData.GetTrinketRefineCost(out num, out needCount, out this.needMoney, out needCount2);
		ItemDataEx itemDataEx = Globals.Instance.Player.ItemSystem.GetItemByInfoID(GameConst.GetInt32(102));
		if (itemDataEx == null)
		{
			itemDataEx = new ItemDataEx(new ItemData(), Globals.Instance.AttDB.ItemDict.GetInfo(GameConst.GetInt32(102)));
		}
		this.mItems[0].Refresh(itemDataEx, needCount);
		this.mItems[1].Refresh(this.mBaseScene.mEquipData, needCount2);
		if (this.mBaseScene.mEquipData.IsRefineMax())
		{
			this.mLevelNextValue.text = Singleton<StringManager>.Instance.GetString("equipImprove26");
			this.mLevelNextValue.color = Color.red;
			this.mLevelEffect.gameObject.SetActive(false);
			this.mPoint0AddedValue.enabled = false;
			this.mPoint1AddedValue.enabled = false;
			this.mGoldValue.transform.parent.gameObject.SetActive(false);
			this.mRefineBtn.gameObject.SetActive(false);
		}
		else
		{
			this.mLevelNextValue.text = (this.mBaseScene.mEquipData.GetTrinketRefineLevel() + 1).ToString();
			this.mPoint0AddedValue.text = Singleton<StringManager>.Instance.GetString("equipImprove23", new object[]
			{
				Singleton<StringManager>.Instance.GetString("equipImprove18", new object[]
				{
					this.mBaseScene.mEquipData.GetTrinketRefineAttDelta0().ToString("0.0")
				})
			});
			this.mPoint1AddedValue.text = Singleton<StringManager>.Instance.GetString("equipImprove23", new object[]
			{
				Singleton<StringManager>.Instance.GetString("equipImprove18", new object[]
				{
					this.mBaseScene.mEquipData.GetTrinketRefineAttDelta1().ToString("0.0")
				})
			});
			this.RefreshMoney();
		}
	}

	public void RefreshMoney()
	{
		if (this.mGoldValue.gameObject.activeInHierarchy)
		{
			this.mGoldValue.text = this.needMoney.ToString();
			if (Tools.CanBuy(ECurrencyType.ECurrencyT_Money, this.needMoney))
			{
				this.mGoldValue.color = Tools.GetDefaultTextColor();
			}
			else
			{
				this.mGoldValue.color = Color.red;
			}
		}
	}
}
