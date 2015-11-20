using Att;
using Proto;
using System;
using UnityEngine;

public class CommonSourceItem : UICustomGridItem
{
	private int AllAwakeSceneLootSceneID = 601001;

	public CommonSourceItemData mData;

	private UISprite mBG;

	private UISprite mIcon;

	private UILabel mTitle;

	private UILabel mName;

	private UIButton mGoBtn;

	private UIButton[] mGoUIBtns;

	private UILabel mBtnTxt;

	private bool isOpen = true;

	public SceneInfo sceneInfo
	{
		get;
		private set;
	}

	public MapInfo mapInfo
	{
		get;
		private set;
	}

	public SceneData sceneData
	{
		get;
		private set;
	}

	private bool IsOpen
	{
		get
		{
			return this.isOpen;
		}
		set
		{
			if (value != this.isOpen)
			{
				this.isOpen = value;
				this.mGoBtn.isEnabled = this.isOpen;
				for (int i = 0; i < this.mGoUIBtns.Length; i++)
				{
					this.mGoUIBtns[i].SetState((!this.isOpen) ? UIButtonColor.State.Disabled : UIButtonColor.State.Normal, true);
				}
				this.mBtnTxt.text = Singleton<StringManager>.Instance.GetString((!this.isOpen) ? "notopen" : "go");
			}
		}
	}

	public void InitWithBaseScene(int width = 362)
	{
		this.CreateObjects();
		this.mBG.width = width;
	}

	private void CreateObjects()
	{
		this.mBG = base.gameObject.GetComponent<UISprite>();
		this.mIcon = GameUITools.FindUISprite("Icon", base.gameObject);
		this.mTitle = GameUITools.FindUILabel("Title", base.gameObject);
		this.mName = GameUITools.FindUILabel("Name", base.gameObject);
		this.mGoBtn = GameUITools.FindGameObject("Go", base.gameObject).GetComponent<UIButton>();
		this.mGoUIBtns = this.mGoBtn.gameObject.GetComponents<UIButton>();
		this.mBtnTxt = GameUITools.FindUILabel("Txt", this.mGoBtn.gameObject);
		UIEventListener expr_AF = UIEventListener.Get(this.mGoBtn.gameObject);
		expr_AF.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_AF.onClick, new UIEventListener.VoidDelegate(this.OnGoClick));
	}

	public override void Refresh(object data)
	{
		if (this.mData == data)
		{
			return;
		}
		this.mData = (CommonSourceItemData)data;
		this.Refresh();
	}

	private void Refresh()
	{
		if (this.mData != null)
		{
			EItemSource mSourceType = this.mData.mSourceType;
			switch (mSourceType)
			{
			case EItemSource.EISource_HonorShop:
				this.mTitle.enabled = true;
				this.mName.enabled = true;
				this.mIcon.spriteName = "honorShop";
				this.mTitle.text = Singleton<StringManager>.Instance.GetString("ShopN8");
				this.mName.text = Singleton<StringManager>.Instance.GetString("itemSource1");
				this.IsOpen = Tools.CanPlay(GameConst.GetInt32(6), true);
				return;
			case EItemSource.EISource_GuildShop:
				this.mTitle.enabled = true;
				this.mName.enabled = true;
				this.mIcon.spriteName = "guildShop";
				this.mTitle.text = Singleton<StringManager>.Instance.GetString("ShopN2");
				if (this.mData.mItemInfo != null && this.mData.mItemInfo.Type == 3 && this.mData.mItemInfo.SubType == 0)
				{
					this.mName.text = Singleton<StringManager>.Instance.GetString("itemSource32");
				}
				else
				{
					this.mName.text = Singleton<StringManager>.Instance.GetString("itemSource2");
				}
				this.IsOpen = (Tools.CanPlay(GameConst.GetInt32(4), true) && Globals.Instance.Player.Data.HasGuild == 1);
				return;
			case (EItemSource)3:
			case (EItemSource)5:
			case (EItemSource)6:
			case (EItemSource)7:
				IL_3F:
				if (mSourceType == EItemSource.EISource_PetShop)
				{
					this.mTitle.enabled = true;
					this.mName.enabled = true;
					this.mIcon.spriteName = "petShop";
					this.mTitle.text = Singleton<StringManager>.Instance.GetString("ShopN0");
					this.mName.text = Singleton<StringManager>.Instance.GetString("itemSource5");
					this.IsOpen = true;
					return;
				}
				if (mSourceType == EItemSource.EISource_Common)
				{
					this.mTitle.enabled = true;
					this.mName.enabled = true;
					this.mIcon.spriteName = "commonShop";
					this.mTitle.text = Singleton<StringManager>.Instance.GetString("ShopN3");
					if (this.mData.mFashionInfo != null)
					{
						this.mName.text = Singleton<StringManager>.Instance.GetString("itemSource13");
					}
					else
					{
						this.mName.text = Singleton<StringManager>.Instance.GetString("itemSource10");
					}
					this.IsOpen = true;
					return;
				}
				if (mSourceType == EItemSource.EISource_KRShop)
				{
					this.mTitle.enabled = true;
					this.mName.enabled = true;
					this.mIcon.spriteName = "kingRewardShop";
					this.mTitle.text = Singleton<StringManager>.Instance.GetString("ShopN5");
					this.mName.text = Singleton<StringManager>.Instance.GetString("itemSource6");
					this.IsOpen = Tools.CanPlay(GameConst.GetInt32(2), true);
					return;
				}
				if (mSourceType == EItemSource.EISource_TrialShop)
				{
					this.mTitle.enabled = true;
					this.mName.enabled = true;
					this.mIcon.spriteName = "trialShop";
					this.mTitle.text = Singleton<StringManager>.Instance.GetString("ShopN4");
					this.mName.text = Singleton<StringManager>.Instance.GetString("itemSource20");
					this.IsOpen = Tools.CanPlay(GameConst.GetInt32(5), true);
					return;
				}
				if (mSourceType == EItemSource.EISource_Trial)
				{
					this.mTitle.enabled = true;
					this.mName.enabled = true;
					this.mIcon.spriteName = "trial";
					this.mTitle.text = Singleton<StringManager>.Instance.GetString("itemSource16");
					this.mName.text = Singleton<StringManager>.Instance.GetString("itemSource11");
					this.IsOpen = Tools.CanPlay(GameConst.GetInt32(5), true);
					return;
				}
				if (mSourceType == EItemSource.EISource_WorldBoss)
				{
					this.mTitle.enabled = true;
					this.mName.enabled = true;
					this.mIcon.spriteName = "worldBoss";
					this.mTitle.text = Singleton<StringManager>.Instance.GetString("worldBossTxt5");
					this.mName.text = Singleton<StringManager>.Instance.GetString("itemSource12");
					this.IsOpen = Tools.CanPlay(GameConst.GetInt32(1), true);
					return;
				}
				if (mSourceType == EItemSource.EISource_CostumeParty)
				{
					this.mTitle.enabled = true;
					this.mName.enabled = true;
					this.mIcon.spriteName = "costumeParty";
					this.mTitle.text = Singleton<StringManager>.Instance.GetString("costumeParty");
					this.mName.text = Singleton<StringManager>.Instance.GetString("itemSource8");
					this.IsOpen = Tools.CanPlay(GameConst.GetInt32(10), true);
					return;
				}
				if (mSourceType == EItemSource.EISource_SoulReliquary)
				{
					this.mTitle.enabled = true;
					this.mName.enabled = true;
					this.mIcon.spriteName = "soulReliquary";
					this.mTitle.text = Singleton<StringManager>.Instance.GetString("activitySoulReliquaryTitle");
					this.mName.text = Singleton<StringManager>.Instance.GetString("itemSource5");
					this.IsOpen = GUISoulReliquaryInfo.IsVisible;
					return;
				}
				if (mSourceType == EItemSource.EISource_SceneLoot)
				{
					this.sceneInfo = Globals.Instance.AttDB.SceneDict.GetInfo(this.mData.mSceneID);
					if (this.sceneInfo != null)
					{
						this.mapInfo = Globals.Instance.AttDB.MapDict.GetInfo(this.sceneInfo.MapID);
						this.sceneData = Globals.Instance.Player.GetSceneData(this.sceneInfo.ID);
					}
					else
					{
						this.mapInfo = null;
						this.sceneData = null;
					}
					this.RefreshSceneItem();
					return;
				}
				if (mSourceType == EItemSource.EISource_Pillage)
				{
					this.mTitle.enabled = true;
					this.mName.enabled = true;
					this.mIcon.spriteName = "pillage";
					this.mTitle.text = Singleton<StringManager>.Instance.GetString("itemSource18");
					this.mName.text = Singleton<StringManager>.Instance.GetString("itemSource17");
					this.IsOpen = Tools.CanPlay(GameConst.GetInt32(8), true);
					return;
				}
				if (mSourceType == EItemSource.EISource_AwakeShop)
				{
					this.mTitle.enabled = true;
					this.mName.enabled = true;
					this.mIcon.spriteName = "awakeShop";
					this.mTitle.text = Singleton<StringManager>.Instance.GetString("ShopN9");
					this.mName.text = Singleton<StringManager>.Instance.GetString("itemSource25");
					this.IsOpen = Tools.CanPlay(GameConst.GetInt32(24), true);
					return;
				}
				if (mSourceType == EItemSource.EISource_AllSceneLoot)
				{
					this.mTitle.enabled = true;
					this.mName.enabled = true;
					this.mIcon.spriteName = "scene";
					this.mTitle.text = Singleton<StringManager>.Instance.GetString("itemSource26");
					this.mName.text = Singleton<StringManager>.Instance.GetString("itemSource28");
					this.isOpen = true;
					return;
				}
				if (mSourceType == EItemSource.EISource_AllAwakeSceneLoot)
				{
					this.mTitle.enabled = true;
					this.mName.enabled = true;
					this.mIcon.spriteName = "awakeRoad";
					this.mTitle.text = Singleton<StringManager>.Instance.GetString("itemSource27");
					this.mName.text = Singleton<StringManager>.Instance.GetString("itemSource29");
					this.IsOpen = Tools.CanPlay(GameConst.GetInt32(24), true);
					return;
				}
				if (mSourceType == EItemSource.EISource_LopetShop)
				{
					this.mTitle.enabled = true;
					this.mName.enabled = true;
					this.mIcon.spriteName = "lopetShop";
					this.mTitle.text = Singleton<StringManager>.Instance.GetString("ShopN6");
					this.mName.text = Singleton<StringManager>.Instance.GetString("itemSource31");
					this.IsOpen = Tools.CanPlay(GameConst.GetInt32(201), true);
					return;
				}
				if (mSourceType != EItemSource.EISource_GuildWarMVP)
				{
					return;
				}
				this.mTitle.enabled = true;
				this.mName.enabled = true;
				this.mIcon.spriteName = "guildShop";
				this.mTitle.text = Singleton<StringManager>.Instance.GetString("itemSource34");
				this.mName.text = Singleton<StringManager>.Instance.GetString("itemSource33");
				this.IsOpen = Tools.CanPlay(GameConst.GetInt32(4), true);
				return;
			case EItemSource.EISource_LuckyRoll:
				this.mTitle.enabled = true;
				this.mName.enabled = true;
				this.mIcon.spriteName = "roll";
				this.mTitle.text = Singleton<StringManager>.Instance.GetString("rollLb");
				this.mName.text = Singleton<StringManager>.Instance.GetString("itemSource3");
				this.IsOpen = true;
				return;
			case EItemSource.EISource_KingReward:
				this.mTitle.enabled = true;
				this.mName.enabled = true;
				this.mIcon.spriteName = "kingReward";
				this.mTitle.text = Singleton<StringManager>.Instance.GetString("activityKingRewardTitle");
				this.mName.text = Singleton<StringManager>.Instance.GetString("itemSource4");
				this.IsOpen = Tools.CanPlay(GameConst.GetInt32(2), true);
				return;
			}
			goto IL_3F;
		}
	}

	private void RefreshSceneItem()
	{
		if (this.sceneInfo == null)
		{
			return;
		}
		this.mTitle.enabled = true;
		this.mName.enabled = true;
		if (this.sceneInfo.Difficulty == 2)
		{
			this.mIcon.spriteName = "awakeRoad";
			this.mTitle.text = Singleton<StringManager>.Instance.GetString("awakeRoad0");
			this.mName.text = Singleton<StringManager>.Instance.GetString("itemSource24", new object[]
			{
				this.mTitle.text,
				this.mapInfo.ID % 100,
				this.sceneInfo.ID % 100
			});
		}
		else if (this.sceneInfo.Difficulty == 1)
		{
			this.mIcon.spriteName = "hardScene";
			this.mTitle.text = Singleton<StringManager>.Instance.GetString("itemSource23");
			this.mName.text = Singleton<StringManager>.Instance.GetString("itemSource24", new object[]
			{
				this.mTitle.text,
				this.mapInfo.ID % 100,
				this.sceneInfo.ID % 100
			});
		}
		else if (this.sceneInfo.Difficulty == 0)
		{
			this.mIcon.spriteName = "easyScene";
			this.mTitle.text = Singleton<StringManager>.Instance.GetString("itemSource22");
			this.mName.text = Singleton<StringManager>.Instance.GetString("itemSource24", new object[]
			{
				this.mTitle.text,
				this.mapInfo.ID % 100,
				this.sceneInfo.ID % 100
			});
		}
		else
		{
			this.mIcon.spriteName = "dreamlandScene";
			this.mTitle.text = Singleton<StringManager>.Instance.GetString("itemSource30");
			this.mName.text = Singleton<StringManager>.Instance.GetString("itemSource24", new object[]
			{
				this.mTitle.text,
				this.mapInfo.ID % 100,
				this.sceneInfo.ID % 100
			});
		}
		this.IsOpen = !this.IsNotOpen();
	}

	private bool IsNotOpen()
	{
		return this.sceneData == null && Globals.Instance.Player.GetSceneData(this.sceneInfo.PreID) == null;
	}

	private void OnGoClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (!this.IsOpen)
		{
			return;
		}
		EItemSource mSourceType = this.mData.mSourceType;
		switch (mSourceType)
		{
		case EItemSource.EISource_HonorShop:
			this.BeforeClose();
			this.Change2Shop(EShopType.EShop_Pvp);
			return;
		case EItemSource.EISource_GuildShop:
			this.BeforeClose();
			this.Change2Shop(EShopType.EShop_Guild);
			return;
		case (EItemSource)3:
		case (EItemSource)5:
		case (EItemSource)6:
		case (EItemSource)7:
			IL_55:
			if (mSourceType == EItemSource.EISource_PetShop)
			{
				this.BeforeClose();
				this.Change2Shop(EShopType.EShop_Common2);
				return;
			}
			if (mSourceType == EItemSource.EISource_Common)
			{
				this.BeforeClose();
				this.Change2Shop(EShopType.EShop_Common);
				return;
			}
			if (mSourceType == EItemSource.EISource_KRShop)
			{
				this.BeforeClose();
				this.Change2Shop(EShopType.EShop_KR);
				return;
			}
			if (mSourceType == EItemSource.EISource_TrialShop)
			{
				this.BeforeClose();
				this.Change2Shop(EShopType.EShop_Trial);
				return;
			}
			if (mSourceType == EItemSource.EISource_Trial)
			{
				this.BeforeClose();
				GUITrailTowerSceneV2.TryOpen();
				return;
			}
			if (mSourceType == EItemSource.EISource_WorldBoss)
			{
				this.BeforeClose();
				GUIBossReadyScene.TryOpen();
				return;
			}
			if (mSourceType == EItemSource.EISource_CostumeParty)
			{
				this.BeforeClose();
				GUICostumePartyScene.TryOpen();
				return;
			}
			if (mSourceType == EItemSource.EISource_SoulReliquary)
			{
				this.BeforeClose();
				GUIReward.Change2Reward(GUIReward.ERewardActivityType.ERAT_SoulReliquary);
				return;
			}
			if (mSourceType == EItemSource.EISource_SceneLoot)
			{
				this.BeforeClose();
				this.OnSceneItemClick();
				return;
			}
			if (mSourceType == EItemSource.EISource_Pillage)
			{
				this.BeforeClose();
				if (this.mData.mItemInfo != null)
				{
					GUIPillageScene.LastSelectRecipe = this.mData.mItemInfo.ID;
				}
				GUIPillageScene.TryOpen(false);
				return;
			}
			if (mSourceType == EItemSource.EISource_AwakeShop)
			{
				this.BeforeClose();
				this.Change2Shop(EShopType.EShop_Awaken);
				return;
			}
			if (mSourceType == EItemSource.EISource_AllSceneLoot)
			{
				this.BeforeClose();
				GameUIManager.mInstance.ChangeSession<GUIWorldMap>(null, false, true);
				return;
			}
			if (mSourceType != EItemSource.EISource_AllAwakeSceneLoot)
			{
				if (mSourceType == EItemSource.EISource_LopetShop)
				{
					this.BeforeClose();
					this.Change2Shop(EShopType.EShop_Lopet);
					return;
				}
				if (mSourceType != EItemSource.EISource_GuildWarMVP)
				{
					return;
				}
				this.BeforeClose();
				GUIGuildManageScene.TryOpen();
				return;
			}
			else
			{
				this.BeforeClose();
				this.sceneInfo = Globals.Instance.AttDB.SceneDict.GetInfo(this.AllAwakeSceneLootSceneID);
				if (this.sceneInfo == null)
				{
					global::Debug.LogErrorFormat("SceneDict get info error , ID : {0} ", new object[]
					{
						this.AllAwakeSceneLootSceneID
					});
					return;
				}
				GameUIManager.mInstance.uiState.AdventureSceneInfo = this.sceneInfo;
				GameUIManager.mInstance.ChangeSession<GUIAwakeRoadSceneV2>(null, false, true);
				return;
			}
			break;
		case EItemSource.EISource_LuckyRoll:
			this.BeforeClose();
			GameUIManager.mInstance.ChangeSession<GUIRollSceneV2>(null, false, true);
			return;
		case EItemSource.EISource_KingReward:
			this.BeforeClose();
			GUIKingRewardScene.TryOpen();
			return;
		}
		goto IL_55;
	}

	private void Change2Shop(EShopType type)
	{
		GUIShopScene.TryOpen(type);
	}

	private void OnSceneItemClick()
	{
		if (this.IsNotOpen())
		{
			GameUIManager.mInstance.ShowMessageTipByKey("notopened", 0f, 0f);
			return;
		}
		if (this.sceneInfo.Difficulty == 2)
		{
			GUIAwakeRoadSceneV2.TryOpen(this.sceneInfo);
		}
		else
		{
			GameUIManager.mInstance.uiState.PetSceneInfo = this.sceneInfo;
			GameUIManager.mInstance.ChangeSession<GUIWorldMap>(null, false, true);
		}
	}

	private void BeforeClose()
	{
		if (GameUIManager.mInstance.GetPetInfoSceneV2() != null)
		{
			GameUIManager.mInstance.DestroyPetInfoSceneV2();
		}
		GUISignIn session = GameUIManager.mInstance.GetSession<GUISignIn>();
		if (session != null)
		{
			session.Close();
		}
		GUITeamManageSceneV2 session2 = GameUIManager.mInstance.GetSession<GUITeamManageSceneV2>();
		if (session2 != null)
		{
			GameUIManager.mInstance.uiState.CombatPetSlot = session2.GetCurSelectIndex();
		}
		GUIPetTrainSceneV2 session3 = GameUIManager.mInstance.GetSession<GUIPetTrainSceneV2>();
		if (session3 != null)
		{
			if (session3.CurPetDataEx != null)
			{
				GameUIManager.mInstance.uiState.mPetTrainCurPetDataEx = session3.CurPetDataEx;
			}
			else if (session3.CurLopetDataEx != null)
			{
				GameUIManager.mInstance.uiState.mLopetTrainCurLopetDataEx = session3.CurLopetDataEx;
			}
			GameUIManager.mInstance.uiState.mPetTrainCurPageIndex = session3.GetCurPageIndex();
			GameUIManager.mInstance.uiState.mPetTrainLvlPageIndex = session3.GetCurLvlPageIndex();
		}
		GUILopetInfoScene.TryClose();
		if ((GameUIManager.mInstance.CurUISession is GUIEquipBagScene || GameUIManager.mInstance.CurUISession is GUITrinketBagScene || GameUIManager.mInstance.CurUISession is GUIPartnerManageScene || GameUIManager.mInstance.CurUISession is GUILopetBagScene) && this.mData.mItemInfo != null)
		{
			ItemDataEx itemByInfoID = Globals.Instance.Player.ItemSystem.GetItemByInfoID(this.mData.mItemInfo.ID);
			if (itemByInfoID != null)
			{
				GameUIManager.mInstance.uiState.SelectItemID = itemByInfoID.GetID();
			}
		}
		ItemsBox.TryClose();
		this.CloseAllPopUp();
	}

	private void CloseAllPopUp()
	{
		if (GameUIPopupManager.GetInstance().GetStackSize() > 0)
		{
			GameUIPopupManager.GetInstance().PopState(true, null);
			this.CloseAllPopUp();
		}
	}
}
