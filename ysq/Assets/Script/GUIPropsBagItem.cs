using System;
using UnityEngine;

public class GUIPropsBagItem : GUICommonBagItem
{
	private GUIPropsBagScene mBaseScene;

	protected override void InitObjects()
	{
		this.mBaseScene = (this.mOriginal as GUIPropsBagScene);
	}

	protected override bool IsShowPanel()
	{
		return false;
	}

	protected override string GetName()
	{
		return this.mData.Info.Name;
	}

	protected override string GetLevel()
	{
		return Singleton<StringManager>.Instance.GetString("equipImprove69", new object[]
		{
			this.mData.GetCount()
		});
	}

	protected override string GetDesc()
	{
		return this.mData.Info.Desc;
	}

	protected override void OnIconClick(GameObject go)
	{
		GUIPropsInfoPopUp.Show(this.mData);
	}

	protected override string GetClickableBtnTxt()
	{
		if (this.mData.Info.Type == 2)
		{
			return Singleton<StringManager>.Instance.GetString("useitem");
		}
		if (this.mData.Info.Type != 4)
		{
			global::Debug.LogErrorFormat("PropsBagItem Type Error, InfoID : {0} , Type : {1} , SubType : {2}", new object[]
			{
				this.mData.Info.ID,
				this.mData.Info.Type,
				this.mData.Info.SubType
			});
			return string.Empty;
		}
		if (this.mData.Info.ID == GameConst.GetInt32(188))
		{
			return string.Empty;
		}
		return Singleton<StringManager>.Instance.GetString("equipImprove41");
	}

	public override void OnClickableBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.mData.Info.Type == 2)
		{
			switch (this.mData.Info.SubType)
			{
			case 0:
			case 1:
			case 2:
			case 3:
			case 4:
			case 5:
			case 6:
			case 7:
			case 8:
				this.mBaseScene.UseItem(this.mData);
				break;
			case 9:
				if (Tools.IsTrinketBagFull())
				{
					return;
				}
				this.mBaseScene.UseItem(this.mData);
				break;
			}
		}
		else if (this.mData.Info.Type == 4)
		{
			switch (this.mData.Info.SubType)
			{
			case 0:
				if (this.mData.Info.ID == GameConst.GetInt32(82))
				{
					GameUIManager.mInstance.ChangeSession<GUIShopEntry>(null, false, true);
				}
				else if (this.mData.Info.ID == GameConst.GetInt32(221) && Globals.Instance.Player.ActivitySystem.GBData != null && Tools.GetRemainAARewardTime(Globals.Instance.Player.ActivitySystem.GBData.Base.CloseTimeStamp) > 0)
				{
					GUIReward.Change2Reward(GUIReward.ERewardActivityType.ERAT_GBReward);
				}
				break;
			case 1:
				GameUIManager.mInstance.uiState.PropsBagSceneToTrainIndex = 1;
				GameUIManager.mInstance.ChangeSession<GUIPartnerManageScene>(null, false, true);
				break;
			case 2:
				GameUIManager.mInstance.ChangeSession<GUIEquipBagScene>(null, false, true);
				break;
			case 3:
				GameUIManager.mInstance.uiState.PropsBagSceneToTrainIndex = 2;
				GameUIManager.mInstance.ChangeSession<GUIPartnerManageScene>(null, false, true);
				break;
			case 4:
				GameUIManager.mInstance.ChangeSession<GUITrinketBagScene>(null, false, true);
				break;
			case 5:
				GameUIManager.mInstance.uiState.PropsBagSceneToTrainIndex = 3;
				GameUIManager.mInstance.ChangeSession<GUIPartnerManageScene>(null, false, true);
				break;
			case 6:
				GameUIManager.mInstance.ChangeSession<GUIRollSceneV2>(null, false, true);
				break;
			case 7:
				if (!Tools.CanPlay(GameConst.GetInt32(7), true))
				{
					GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("XingZuo27", new object[]
					{
						GameConst.GetInt32(7)
					}), 0f, 0f);
					return;
				}
				GameUIManager.mInstance.ChangeSession<GUIConstellationScene>(null, false, true);
				break;
			case 8:
				GameUIManager.mInstance.uiState.PropsBagSceneToTrainIndex = 5;
				GameUIManager.mInstance.ChangeSession<GUIPartnerManageScene>(null, false, true);
				break;
			case 9:
				GameUIManager.mInstance.ChangeSession<GUITrinketBagScene>(null, false, true);
				break;
			case 10:
				GameUIManager.mInstance.uiState.PropsBagSceneToTrainIndex = 4;
				GameUIManager.mInstance.ChangeSession<GUIPartnerManageScene>(null, false, true);
				break;
			}
		}
		else
		{
			global::Debug.LogErrorFormat("PropsBagItem Type Error, InfoID : {0} , Type : {1} , SubType : {2}", new object[]
			{
				this.mData.Info.ID,
				this.mData.Info.Type,
				this.mData.Info.SubType
			});
		}
	}
}
