using Att;
using Proto;
using System;
using System.Text;
using UnityEngine;

public class GUIHowGetPetItemPopUp : GameUIBasePopup
{
	private static int lowLopetInfoID = 350001;

	public static EItemSource[] sortArray = new EItemSource[]
	{
		EItemSource.EISource_SceneLoot,
		EItemSource.EISource_AllSceneLoot,
		EItemSource.EISource_AllAwakeSceneLoot,
		EItemSource.EISource_PetShop,
		EItemSource.EISource_TrialShop,
		EItemSource.EISource_SoulReliquary,
		EItemSource.EISource_HonorShop,
		EItemSource.EISource_KRShop,
		EItemSource.EISource_Common,
		EItemSource.EISource_GuildShop,
		EItemSource.EISource_AwakeShop,
		EItemSource.EISource_LopetShop,
		EItemSource.EISource_Trial,
		EItemSource.EISource_KingReward,
		EItemSource.EISource_LuckyRoll,
		EItemSource.EISource_CostumeParty,
		EItemSource.EISource_WorldBoss,
		EItemSource.EISource_Pillage,
		EItemSource.EISource_GuildWarMVP
	};

	private UILabel mName;

	private UILabel mNumLabel;

	private UILabel mNum;

	private UILabel mTipTxt;

	private GameObject mLootPanel;

	private SourceItemUITable mContentsTable;

	private CommonIconItem mIconItem;

	private StringBuilder mStringBuilder = new StringBuilder();

	public PetSceneInfo mPetSceneInfo
	{
		get;
		private set;
	}

	public PetDataEx mPetData
	{
		get;
		private set;
	}

	public PetInfo mPetInfo
	{
		get;
		private set;
	}

	public ItemDataEx mItemData
	{
		get;
		private set;
	}

	public ItemInfo mItemInfo
	{
		get;
		private set;
	}

	public FashionInfo mFashionInfo
	{
		get;
		private set;
	}

	public static void ShowThis(ItemInfo info)
	{
		if (info == null)
		{
			return;
		}
		GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GUIHowGetPetItemPopUp, false, null, null);
		GameUIPopupManager.GetInstance().GetCurrentPopup().InitPopUp(info);
	}

	public static void ShowThis(FashionInfo info)
	{
		if (info == null)
		{
			return;
		}
		GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GUIHowGetPetItemPopUp, false, null, null);
		GameUIPopupManager.GetInstance().GetCurrentPopup().InitPopUp(info);
	}

	public static void ShowLowQualityEquip(int index)
	{
		if (index < 0 && index >= GameConst.LOW_QUALITY_EQUIP_ID.Length)
		{
			global::Debug.LogErrorFormat("index error : {0} ", new object[]
			{
				index
			});
			return;
		}
		ItemInfo info = Globals.Instance.AttDB.ItemDict.GetInfo(GameConst.LOW_QUALITY_EQUIP_ID[index]);
		if (info == null)
		{
			global::Debug.LogErrorFormat("ItemDict.GetInfo error, id = {0}", new object[]
			{
				Globals.Instance.AttDB.ItemDict.GetInfo(GameConst.LOW_QUALITY_EQUIP_ID[index])
			});
			return;
		}
		GUIHowGetPetItemPopUp.ShowThis(info);
	}

	public static void ShowNullLopet()
	{
		ItemInfo info = Globals.Instance.AttDB.ItemDict.GetInfo(GUIHowGetPetItemPopUp.lowLopetInfoID);
		if (info == null)
		{
			global::Debug.LogErrorFormat("ItemDict.GetInfo error, id = {0}", new object[]
			{
				Globals.Instance.AttDB.ItemDict.GetInfo(GUIHowGetPetItemPopUp.lowLopetInfoID)
			});
			return;
		}
		GUIHowGetPetItemPopUp.ShowThis(info);
	}

	public static void ShowThis(PetInfo info)
	{
		if (info == null)
		{
			return;
		}
		ItemInfo fragmentInfo = PetFragment.GetFragmentInfo(info.ID);
		if (fragmentInfo == null)
		{
			global::Debug.LogErrorFormat("PetFragment.GetFragmentInfo error, PetInfo id = {0}", new object[]
			{
				info.ID
			});
			return;
		}
		GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GUIHowGetPetItemPopUp, false, null, null);
		GameUIPopupManager.GetInstance().GetCurrentPopup().InitPopUp(fragmentInfo);
	}

	public static void ShowThis(LopetInfo info)
	{
		if (info == null)
		{
			return;
		}
		ItemInfo fragmentInfo = LopetFragment.GetFragmentInfo(info.ID);
		if (fragmentInfo == null)
		{
			global::Debug.LogErrorFormat("LopetFragment.GetFragmentInfo error, LopetInfo id = {0}", new object[]
			{
				info.ID
			});
			return;
		}
		GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GUIHowGetPetItemPopUp, false, null, null);
		GameUIPopupManager.GetInstance().GetCurrentPopup().InitPopUp(fragmentInfo);
	}

	public static void InitSourceItems(ItemInfo mItemInfo, UICustomGrid mContentsTable)
	{
		if (mItemInfo == null)
		{
			return;
		}
		mContentsTable.ClearData();
		ulong num = 0uL;
		EItemSource[] array = GUIHowGetPetItemPopUp.sortArray;
		for (int i = 0; i < array.Length; i++)
		{
			EItemSource eItemSource = array[i];
			if ((mItemInfo.Source & (int)eItemSource) != 0)
			{
				if (eItemSource == EItemSource.EISource_SceneLoot)
				{
					PetSceneInfo info = Globals.Instance.AttDB.PetSceneDict.GetInfo(mItemInfo.ID);
					if (info != null)
					{
						for (int j = 0; j < info.SceneIDs.Count; j++)
						{
							mContentsTable.AddData(new CommonSourceItemData(info.SceneIDs[j], mItemInfo, num));
							num += 1uL;
						}
					}
				}
				else
				{
					mContentsTable.AddData(new CommonSourceItemData(eItemSource, mItemInfo, num));
					num += 1uL;
				}
			}
		}
	}

	private void Awake()
	{
		this.CreateObjects();
		LocalPlayer expr_10 = Globals.Instance.Player;
		expr_10.ShopDataEvent = (LocalPlayer.ShopDataCallback)Delegate.Combine(expr_10.ShopDataEvent, new LocalPlayer.ShopDataCallback(this.OnPlayerShopDataEvent));
	}

	private void OnDestroy()
	{
		if (Globals.Instance == null)
		{
			return;
		}
		LocalPlayer expr_1B = Globals.Instance.Player;
		expr_1B.ShopDataEvent = (LocalPlayer.ShopDataCallback)Delegate.Remove(expr_1B.ShopDataEvent, new LocalPlayer.ShopDataCallback(this.OnPlayerShopDataEvent));
	}

	private void CreateObjects()
	{
		GameObject gameObject = base.transform.Find("Bg").gameObject;
		this.mName = gameObject.transform.Find("Name").GetComponent<UILabel>();
		this.mNum = gameObject.transform.Find("Num").GetComponent<UILabel>();
		this.mTipTxt = gameObject.transform.Find("txt0").GetComponent<UILabel>();
		this.mLootPanel = gameObject.transform.Find("lootPanel").gameObject;
		this.mContentsTable = GameUITools.FindGameObject("lootContents", this.mLootPanel.gameObject).AddComponent<SourceItemUITable>();
		this.mContentsTable.maxPerLine = 1;
		this.mContentsTable.arrangement = UICustomGrid.Arrangement.Vertical;
		this.mContentsTable.cellWidth = 362f;
		this.mContentsTable.cellHeight = 76f;
		this.mContentsTable.gapHeight = 6f;
		this.mContentsTable.gapWidth = 0f;
		this.mContentsTable.bgScrollView = GameUITools.FindGameObject("PanelBG", gameObject).GetComponent<UIDragScrollView>();
		this.mIconItem = CommonIconItem.Create(base.gameObject, new Vector3(-166f, 217f, 0f), null, false, 0.9f, null);
		GameObject gameObject2 = gameObject.transform.Find("closebtn").gameObject;
		UIEventListener expr_163 = UIEventListener.Get(gameObject2.gameObject);
		expr_163.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_163.onClick, new UIEventListener.VoidDelegate(this.OnCloseBtnClicked));
	}

	private void OnCloseBtnClicked(GameObject go)
	{
		this.OnButtonBlockerClick();
	}

	public override void InitPopUp(ItemInfo itemInfo)
	{
		if (itemInfo == null)
		{
			return;
		}
		this.mItemInfo = itemInfo;
		this.InitItemItems();
	}

	public override void InitPopUp(ItemDataEx itemData)
	{
		if (itemData == null)
		{
			return;
		}
		this.mItemData = itemData;
		this.mItemInfo = itemData.Info;
		this.InitItemItems();
	}

	public override void InitPopUp(FashionInfo fashionInfo)
	{
		if (fashionInfo == null)
		{
			return;
		}
		this.mFashionInfo = fashionInfo;
		this.InitFashion();
	}

	public override void OnButtonBlockerClick()
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		GameUIPopupManager.eSTATE stateByIndex = GameUIPopupManager.GetInstance().GetStateByIndex(1);
		GameUIPopupManager.eSTATE eSTATE = stateByIndex;
		if (eSTATE != GameUIPopupManager.eSTATE.GUIEquipInfoPopUp)
		{
			GameUIPopupManager.GetInstance().PopState(false, null);
		}
		else
		{
			GameUIPopupManager.GetInstance().PopState(true, null);
			GameUIPopupManager.GetInstance().GetCurrentPopup().InitPopUp();
		}
	}

	private void InitItemItems()
	{
		if (this.mItemInfo == null)
		{
			return;
		}
		GUIHowGetPetItemPopUp.InitSourceItems(this.mItemInfo, this.mContentsTable);
		if (this.mContentsTable.mDatas.Count > 0)
		{
			this.mTipTxt.gameObject.SetActive(false);
		}
		else
		{
			this.mTipTxt.gameObject.SetActive(true);
		}
		this.RefreshItem();
	}

	private void RefreshItem()
	{
		if (this.mItemInfo == null)
		{
			return;
		}
		this.mIconItem.Refresh(this.mItemInfo, false, false, false);
		this.mName.text = this.mItemInfo.Name;
		this.mName.color = Tools.GetItemQualityColor(this.mItemInfo.Quality);
		int itemCount = Globals.Instance.Player.ItemSystem.GetItemCount(this.mItemInfo.ID);
		if (this.mItemInfo.Type == 3 && (this.mItemInfo.SubType == 1 || this.mItemInfo.SubType == 0 || this.mItemInfo.SubType == 3))
		{
			if (itemCount < this.mItemInfo.Value1)
			{
				this.mStringBuilder.Remove(0, this.mStringBuilder.Length).Append("[ff0000]");
			}
			else
			{
				this.mStringBuilder.Remove(0, this.mStringBuilder.Length).Append("[00ff00]");
			}
			this.mStringBuilder.Append(itemCount).Append("[-]").Append("/").Append(this.mItemInfo.Value1);
			this.mNum.text = this.mStringBuilder.ToString();
		}
		else if (this.mItemInfo.Type == 0 || this.mItemInfo.Type == 1)
		{
			this.mNum.text = Globals.Instance.Player.ItemSystem.GetEquipCount(this.mItemInfo.ID).ToString();
		}
		else
		{
			this.mNum.text = itemCount.ToString();
		}
	}

	private void InitFashion()
	{
		this.mContentsTable.ClearData();
		if (this.mFashionInfo == null)
		{
			return;
		}
		ulong num = 0uL;
		for (int i = 1; i <= 262144; i *= 2)
		{
			if ((this.mFashionInfo.Source & i) != 0)
			{
				this.mContentsTable.AddData(new CommonSourceItemData((EItemSource)i, num, this.mFashionInfo));
				num += 1uL;
			}
		}
		if (this.mContentsTable.mDatas.Count > 0)
		{
			this.mTipTxt.gameObject.SetActive(false);
		}
		else
		{
			this.mTipTxt.gameObject.SetActive(true);
		}
		this.RefreshFashion();
	}

	private void RefreshFashion()
	{
		if (this.mFashionInfo == null)
		{
			return;
		}
		this.mIconItem.Refresh(this.mFashionInfo, false, false, false);
		this.mName.text = this.mFashionInfo.Name;
		this.mName.color = Tools.GetItemQualityColor(this.mFashionInfo.Quality);
		int num = (!Globals.Instance.Player.ItemSystem.HasFashion(this.mFashionInfo.ID)) ? 0 : 1;
		this.mNum.text = num.ToString();
	}

	private void Refresh()
	{
		if (this.mPetData != null)
		{
			this.mIconItem.Refresh(PetFragment.GetFragmentInfo(this.mPetData.Info.ID), false, false, false);
			this.mName.text = Tools.GetPetName(this.mPetData.Info);
			this.mName.color = Tools.GetItemQualityColor(this.mPetData.Info.Quality);
		}
		else if (this.mPetInfo != null)
		{
			this.mIconItem.Refresh(PetFragment.GetFragmentInfo(this.mPetInfo.ID), false, false, false);
			this.mName.text = Tools.GetPetName(this.mPetInfo);
			this.mName.color = Tools.GetItemQualityColor(this.mPetInfo.Quality);
		}
		else
		{
			this.mIconItem.IsVisible = false;
		}
	}

	public void OnPlayerShopDataEvent(int shopType)
	{
		if (shopType >= 10)
		{
			return;
		}
		GUIShopScene.TryOpen((EShopType)shopType);
	}
}
