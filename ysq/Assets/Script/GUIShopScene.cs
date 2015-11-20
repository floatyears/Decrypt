using Att;
using Proto;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GUIShopScene : GameUISession
{
	private ShopCommonGrid mTargetTable;

	private UILabel Title;

	private UIButton refreshBtn;

	private UILabel refreshBtnText;

	private Transform refreshPrice;

	private UISprite refreshPriceIcon;

	private UILabel refreshPriceTex;

	private UILabel refreshTime;

	private Transform Currency;

	private UISprite CurrencyIcon;

	private UILabel CurrencyText;

	private Transform CurrencyPlusBtn;

	private Transform Currency2;

	private UISprite CurrencyIcon2;

	private UILabel CurrencyText2;

	private Transform CurrencyPlusBtn2;

	private UISprite mBG;

	private UIToggle[] tabEquip = new UIToggle[4];

	private int curEquipTab;

	private ShopBuyCheck BuyCheckWnd;

	private ShopBuyMutilCheck BuyMutilCheckWnd;

	private bool RefreshPlayerDataFlag;

	private static EShopType PendingOpenShopType = EShopType.EShop_Max;

	private float RefreshShopStatus = 3.40282347E+38f;

	public EShopType ShopType
	{
		get;
		private set;
	}

	public static void TryOpen(EShopType type)
	{
		if (type == EShopType.EShop_Max)
		{
			return;
		}
		if (!GUIShopScene.IsShopOpen(type))
		{
			switch (type)
			{
			case EShopType.EShop_Common2:
				GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("ShopD0", new object[]
				{
					GameConst.GetInt32(27)
				}), 0f, 0f);
				break;
			case EShopType.EShop_Awaken:
				GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("ShopD1", new object[]
				{
					GameConst.GetInt32(24)
				}), 0f, 0f);
				break;
			case EShopType.EShop_Guild:
				GameUIManager.mInstance.ShowMessageTipByKey("ShopD2", 0f, 0f);
				break;
			case EShopType.EShop_Trial:
				GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("ShopD4", new object[]
				{
					GameConst.GetInt32(5)
				}), 0f, 0f);
				break;
			case EShopType.EShop_KR:
				GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("ShopD5", new object[]
				{
					GameConst.GetInt32(2)
				}), 0f, 0f);
				break;
			case EShopType.EShop_Pvp:
				GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("ShopD8", new object[]
				{
					GameConst.GetInt32(6)
				}), 0f, 0f);
				break;
			case EShopType.EShop_Lopet:
				GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("ShopD9", new object[]
				{
					GameConst.GetInt32(201)
				}), 0f, 0f);
				break;
			}
			return;
		}
		GUIShopScene.PendingOpenShopType = type;
		GUIShopScene gUIShopScene = GameUIManager.mInstance.CurUISession as GUIShopScene;
		if (gUIShopScene == null)
		{
			GameUIManager.mInstance.ChangeSession<GUIShopScene>(null, false, true);
		}
		else if (gUIShopScene.ShopType != type)
		{
			gUIShopScene.OpenPendingShop();
		}
	}

	public static bool IsShopOpen(EShopType type)
	{
		LocalPlayer player = Globals.Instance.Player;
		switch (type)
		{
		case EShopType.EShop_Common2:
			return (ulong)player.Data.Level >= (ulong)((long)GameConst.GetInt32(27));
		case EShopType.EShop_Awaken:
			return (ulong)player.Data.Level >= (ulong)((long)GameConst.GetInt32(24));
		case EShopType.EShop_Guild:
			return player.Data.HasGuild == 1;
		case EShopType.EShop_Trial:
			return (ulong)player.Data.Level >= (ulong)((long)GameConst.GetInt32(5));
		case EShopType.EShop_KR:
			return (ulong)player.Data.Level >= (ulong)((long)GameConst.GetInt32(2));
		case EShopType.EShop_Pvp:
			return (ulong)player.Data.Level >= (ulong)((long)GameConst.GetInt32(6));
		case EShopType.EShop_Lopet:
			return (ulong)player.Data.Level >= (ulong)((long)GameConst.GetInt32(201));
		}
		return true;
	}

	private void OpenPendingShop()
	{
		if (GUIShopScene.PendingOpenShopType == EShopType.EShop_Max)
		{
			global::Debug.LogError(new object[]
			{
				"data error"
			});
			GUIShopScene.PendingOpenShopType = EShopType.EShop_Common;
		}
		if (!GUIShopScene.IsShopOpen(GUIShopScene.PendingOpenShopType))
		{
			global::Debug.LogError(new object[]
			{
				"data error"
			});
			GameUIManager.mInstance.GobackSession();
			return;
		}
		this.ShopType = GUIShopScene.PendingOpenShopType;
		this.Title.text = Singleton<StringManager>.Instance.GetString(string.Format("ShopN{0}", (int)this.ShopType));
		TopGoods topGoods = GameUIManager.mInstance.GetTopGoods();
		topGoods.BackLabelText = this.Title.text;
		this._RefreshShopStatus();
		for (int i = 0; i < 4; i++)
		{
			this.tabEquip[i].gameObject.SetActive(this.ShopType == EShopType.EShop_Trial);
		}
		if (GUIShopScene.PendingOpenShopType == EShopType.EShop_Common2)
		{
			MC2S_GetShopData mC2S_GetShopData = new MC2S_GetShopData();
			mC2S_GetShopData.ShopVersion = Globals.Instance.Player.PetShopVersion;
			mC2S_GetShopData.Refresh = false;
			mC2S_GetShopData.ShopType = 0;
			mC2S_GetShopData.DiamondRefresh = false;
			Globals.Instance.CliSession.Send(512, mC2S_GetShopData);
		}
		else if (GUIShopScene.PendingOpenShopType == EShopType.EShop_Awaken)
		{
			MC2S_GetShopData mC2S_GetShopData2 = new MC2S_GetShopData();
			mC2S_GetShopData2.ShopVersion = Globals.Instance.Player.AwakeShopVersion;
			mC2S_GetShopData2.Refresh = false;
			mC2S_GetShopData2.ShopType = 1;
			mC2S_GetShopData2.DiamondRefresh = false;
			Globals.Instance.CliSession.Send(512, mC2S_GetShopData2);
		}
		else if (GUIShopScene.PendingOpenShopType == EShopType.EShop_Lopet)
		{
			MC2S_GetShopData mC2S_GetShopData3 = new MC2S_GetShopData();
			mC2S_GetShopData3.ShopVersion = Globals.Instance.Player.AwakeShopVersion;
			mC2S_GetShopData3.Refresh = false;
			mC2S_GetShopData3.ShopType = 9;
			mC2S_GetShopData3.DiamondRefresh = false;
			Globals.Instance.CliSession.Send(512, mC2S_GetShopData3);
		}
		else
		{
			this.OpenShopData();
		}
	}

	private void OpenShopData()
	{
		this.mTargetTable.ClearData();
		this.mTargetTable.ShopType = this.ShopType;
		this.mTargetTable.cellWidth = 448f;
		this.mTargetTable.cellHeight = 114f;
		this.mTargetTable.ClearGridItem();
		this.mTargetTable.SetDragAmount(0f, 0f);
		this.mBG.height = 512;
		this.Currency2.gameObject.SetActive(false);
		if (this.ShopType == EShopType.EShop_Common2)
		{
			List<ShopItemData> petShopData = Globals.Instance.Player.PetShopData;
			for (int i = 0; i < petShopData.Count; i++)
			{
				this.mTargetTable.AddData(new RTShopGridData(this.ShopType, petShopData[i], new RTShopGridData.ShopItemCallback(this.OnOpenRTShopBuyCheckDialog), new RTShopGridData.ShopItemCallback(this.OnOpenShowRTShopItemDialog)));
			}
			if (petShopData.Count > 6)
			{
				this.mBG.height = 576;
			}
		}
		else if (this.ShopType == EShopType.EShop_Awaken)
		{
			List<ShopItemData> awakeShopData = Globals.Instance.Player.AwakeShopData;
			for (int j = 0; j < awakeShopData.Count; j++)
			{
				this.mTargetTable.AddData(new RTShopGridData(this.ShopType, awakeShopData[j], new RTShopGridData.ShopItemCallback(this.OnOpenRTShopBuyCheckDialog), new RTShopGridData.ShopItemCallback(this.OnOpenShowRTShopItemDialog)));
			}
		}
		else if (this.ShopType == EShopType.EShop_Lopet)
		{
			List<ShopItemData> lopetShopData = Globals.Instance.Player.LopetShopData;
			for (int k = 0; k < lopetShopData.Count; k++)
			{
				this.mTargetTable.AddData(new RTShopGridData(this.ShopType, lopetShopData[k], new RTShopGridData.ShopItemCallback(this.OnOpenRTShopBuyCheckDialog), new RTShopGridData.ShopItemCallback(this.OnOpenShowRTShopItemDialog)));
			}
		}
		else
		{
			this.RefreshCurrency2();
			LocalPlayer player = Globals.Instance.Player;
			foreach (ShopInfo current in Globals.Instance.AttDB.ShopDict.Values)
			{
				if ((ulong)player.Data.Level >= (ulong)((long)current.Level))
				{
					EShopType shopType = (EShopType)current.ShopType;
					if (shopType == this.ShopType)
					{
						bool flag = true;
						if (current.ResetType == 1 && player.GetBuyCount(current) >= current.Times)
						{
							flag = false;
						}
						else if (current.IsFashion != 0)
						{
							FashionInfo info = Globals.Instance.AttDB.FashionDict.GetInfo(current.InfoID);
							if (info == null || info.Gender != player.Data.Gender + 1 || player.ItemSystem.HasFashion(info.ID))
							{
								flag = false;
							}
						}
						else if (this.ShopType == EShopType.EShop_Trial)
						{
							switch (this.curEquipTab)
							{
							case 1:
							{
								ItemInfo info2 = Globals.Instance.AttDB.ItemDict.GetInfo(current.InfoID);
								flag = (info2 != null && (info2.Type == 0 || info2.Type == 3) && info2.Quality == 2);
								break;
							}
							case 2:
							{
								ItemInfo info3 = Globals.Instance.AttDB.ItemDict.GetInfo(current.InfoID);
								flag = (info3 != null && (info3.Type == 0 || info3.Type == 3) && info3.Quality == 3);
								break;
							}
							case 3:
							{
								ItemInfo info4 = Globals.Instance.AttDB.ItemDict.GetInfo(current.InfoID);
								flag = (info4 != null && (info4.Type == 0 || info4.Type == 3) && info4.Quality == 4);
								break;
							}
							default:
							{
								ItemInfo info5 = Globals.Instance.AttDB.ItemDict.GetInfo(current.InfoID);
								if (info5 == null)
								{
									flag = false;
								}
								else if (info5.Type == 0 || info5.Type == 3)
								{
									flag = (info5.Quality != 4 && info5.Quality != 3 && info5.Quality != 2);
								}
								break;
							}
							}
						}
						if (flag)
						{
							this.mTargetTable.AddData(new ShopGridData(this.ShopType, current, new ShopGridData.ShopItemCallback(this.OnOpenShopBuyCheckDialog), new ShopGridData.ShopItemCallback(this.OnOpenShowShopItemDialog)));
						}
					}
				}
			}
			if (this.ShopType == EShopType.EShop_Common)
			{
				this.mTargetTable.cellHeight = 143f;
			}
		}
		this.mTargetTable.repositionNow = true;
	}

	protected override void OnPostLoadGUI()
	{
		Globals.Instance.BackgroundMusicMgr.PlayLobbyMusic();
		TopGoods topGoods = GameUIManager.mInstance.GetTopGoods();
		topGoods.Show("shopLb");
		Transform transform = base.transform.Find("WindowBg");
		this.mBG = transform.Find("BG").GetComponent<UISprite>();
		this.mTargetTable = transform.FindChild("bagPanel/bagContents").gameObject.AddComponent<ShopCommonGrid>();
		this.mTargetTable.maxPerLine = 2;
		this.mTargetTable.arrangement = UICustomGrid.Arrangement.Vertical;
		this.mTargetTable.cellWidth = 442f;
		this.mTargetTable.cellHeight = 114f;
		this.Title = transform.transform.Find("Title").GetComponent<UILabel>();
		Transform transform2 = base.transform.Find("BuyMutilCheck");
		this.BuyMutilCheckWnd = transform2.gameObject.AddComponent<ShopBuyMutilCheck>();
		this.BuyMutilCheckWnd.Init();
		this.BuyMutilCheckWnd.gameObject.SetActive(false);
		Transform transform3 = base.transform.Find("BuyCheck");
		this.BuyCheckWnd = transform3.gameObject.AddComponent<ShopBuyCheck>();
		this.BuyCheckWnd.Init();
		this.BuyCheckWnd.gameObject.SetActive(false);
		this.refreshBtn = transform.transform.FindChild("BtnRefresh").GetComponent<UIButton>();
		UIEventListener expr_161 = UIEventListener.Get(this.refreshBtn.gameObject);
		expr_161.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_161.onClick, new UIEventListener.VoidDelegate(this.OnRefreshClick));
		this.refreshBtnText = this.refreshBtn.transform.Find("Label").GetComponent<UILabel>();
		this.refreshBtn.gameObject.SetActive(false);
		this.refreshPrice = transform.transform.FindChild("RefreshPrice");
		this.refreshPriceIcon = this.refreshPrice.Find("icon").GetComponent<UISprite>();
		this.refreshPriceTex = this.refreshPrice.Find("Label").GetComponent<UILabel>();
		this.refreshPrice.gameObject.SetActive(false);
		this.refreshTime = transform.Find("refreshTime").GetComponent<UILabel>();
		this.Currency = transform.Find("Currency");
		this.CurrencyIcon = this.Currency.Find("icon").GetComponent<UISprite>();
		this.CurrencyText = this.Currency.Find("Label").GetComponent<UILabel>();
		this.CurrencyPlusBtn = this.Currency.Find("plusBtn");
		UIEventListener expr_293 = UIEventListener.Get(this.CurrencyPlusBtn.gameObject);
		expr_293.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_293.onClick, new UIEventListener.VoidDelegate(this.OnCurrencyPlusBtnClicked));
		this.Currency.gameObject.SetActive(false);
		this.Currency2 = transform.Find("Currency2");
		this.CurrencyIcon2 = this.Currency2.Find("icon").GetComponent<UISprite>();
		this.CurrencyText2 = this.Currency2.Find("Label").GetComponent<UILabel>();
		this.CurrencyPlusBtn2 = this.Currency2.Find("plusBtn");
		UIEventListener expr_332 = UIEventListener.Get(this.CurrencyPlusBtn2.gameObject);
		expr_332.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_332.onClick, new UIEventListener.VoidDelegate(this.OnCurrency2PlusBtnClicked));
		this.Currency2.gameObject.SetActive(false);
		for (int i = 0; i < 4; i++)
		{
			this.tabEquip[i] = base.transform.Find(string.Format("tab{0}", i)).GetComponent<UIToggle>();
			EventDelegate.Add(this.tabEquip[i].onChange, new EventDelegate.Callback(this.OnTabCheckChanged));
			this.tabEquip[i].gameObject.SetActive(false);
		}
		LocalPlayer expr_3E2 = Globals.Instance.Player;
		expr_3E2.ShopBuyItemEvent = (LocalPlayer.ShopBuyItemCallback)Delegate.Combine(expr_3E2.ShopBuyItemEvent, new LocalPlayer.ShopBuyItemCallback(this.OnShopBuyItemEvent));
		LocalPlayer expr_40D = Globals.Instance.Player;
		expr_40D.PlayerUpdateEvent = (LocalPlayer.VoidCallback)Delegate.Combine(expr_40D.PlayerUpdateEvent, new LocalPlayer.VoidCallback(this.OnPlayerUpdateEvent));
		LocalPlayer expr_438 = Globals.Instance.Player;
		expr_438.ShopDataEvent = (LocalPlayer.ShopDataCallback)Delegate.Combine(expr_438.ShopDataEvent, new LocalPlayer.ShopDataCallback(this.OnPlayerShopDataEvent));
		Globals.Instance.CliSession.Register(535, new ClientSession.MsgHandler(this.OnMsgShopBuyFashion));
		Globals.Instance.TutorialMgr.InitializationCompleted(this, null);
	}

	protected override void OnLoadedFinished()
	{
		this.OpenPendingShop();
	}

	protected override void OnPreDestroyGUI()
	{
		TopGoods topGoods = GameUIManager.mInstance.GetTopGoods();
		topGoods.Hide();
		LocalPlayer expr_1B = Globals.Instance.Player;
		expr_1B.ShopBuyItemEvent = (LocalPlayer.ShopBuyItemCallback)Delegate.Remove(expr_1B.ShopBuyItemEvent, new LocalPlayer.ShopBuyItemCallback(this.OnShopBuyItemEvent));
		LocalPlayer expr_46 = Globals.Instance.Player;
		expr_46.PlayerUpdateEvent = (LocalPlayer.VoidCallback)Delegate.Remove(expr_46.PlayerUpdateEvent, new LocalPlayer.VoidCallback(this.OnPlayerUpdateEvent));
		LocalPlayer expr_71 = Globals.Instance.Player;
		expr_71.ShopDataEvent = (LocalPlayer.ShopDataCallback)Delegate.Remove(expr_71.ShopDataEvent, new LocalPlayer.ShopDataCallback(this.OnPlayerShopDataEvent));
		Globals.Instance.CliSession.Unregister(535, new ClientSession.MsgHandler(this.OnMsgShopBuyFashion));
	}

	private void Update()
	{
		if (this.RefreshPlayerDataFlag)
		{
			this._RefreshPlayerData();
			this._RefreshShopStatus();
		}
		this.RefreshShopStatus -= Time.deltaTime;
		if (this.RefreshShopStatus < 0f)
		{
			this._RefreshShopStatus();
		}
	}

	private void _RefreshPlayerData()
	{
		this.RefreshPlayerDataFlag = false;
		this.mTargetTable.repositionNow = true;
	}

	private void _RefreshShopStatus()
	{
		this.RefreshShopStatus = 1f;
		LocalPlayer player = Globals.Instance.Player;
		this.RefreshCurrency2();
		EShopType shopType = this.ShopType;
		if (shopType != EShopType.EShop_Common2)
		{
			if (shopType != EShopType.EShop_Awaken)
			{
				if (shopType != EShopType.EShop_Lopet)
				{
					this.RefreshShopStatus = 3.40282347E+38f;
					this.refreshBtn.gameObject.SetActive(false);
					this.refreshPrice.gameObject.SetActive(false);
					this.refreshTime.gameObject.SetActive(false);
					this.refreshBtn.gameObject.SetActive(false);
					if (this.ShopType == EShopType.EShop_Common)
					{
						this.Currency.gameObject.SetActive(false);
						this.CurrencyPlusBtn.gameObject.SetActive(false);
					}
					else
					{
						this.Currency.gameObject.SetActive(true);
						this.CurrencyIcon.spriteName = Tools.GetShopCurrencyIcon(this.ShopType);
						this.CurrencyText.text = Tools.FormatCurrency(Tools.GetShopCurrencyValue(this.ShopType));
						this.CurrencyPlusBtn.gameObject.SetActive(true);
					}
				}
				else
				{
					this.refreshBtn.isEnabled = true;
					this.refreshBtn.gameObject.SetActive(true);
					this.refreshPrice.gameObject.SetActive(true);
					int num = Tools.GetShopRefreshTimes(this.ShopType) - player.Data.LopetShopRefresh;
					if (num > 0)
					{
						this.refreshBtnText.text = string.Format(Singleton<StringManager>.Instance.GetString("ShopCommon4"), num.ToString());
					}
					else
					{
						this.refreshBtnText.text = Singleton<StringManager>.Instance.GetString("ShopCommon3");
					}
					int itemCount = player.ItemSystem.GetItemCount(GameConst.GetInt32(82));
					if (itemCount > 0)
					{
						this.refreshPriceIcon.spriteName = "refresh";
						this.refreshPriceTex.text = itemCount.ToString();
						this.refreshPriceTex.color = Color.white;
					}
					else
					{
						this.refreshPriceIcon.spriteName = "redGem_1";
						this.refreshPriceTex.text = GameConst.GetInt32(51).ToString();
						if (player.Data.Diamond >= GameConst.GetInt32(51))
						{
							this.refreshPriceTex.color = Color.white;
						}
						else
						{
							this.refreshPriceTex.color = Color.red;
						}
					}
					int num2 = (Globals.Instance.Player.Data.ShopCommon2TimeStamp <= 0) ? 0 : (Globals.Instance.Player.Data.ShopCommon2TimeStamp - Globals.Instance.Player.GetTimeStamp());
					if (num2 > 0)
					{
						this.refreshTime.gameObject.SetActive(true);
						this.refreshTime.text = string.Format(Singleton<StringManager>.Instance.GetString("ShopCommon0"), Tools.FormatTime(num2));
					}
					else
					{
						this.refreshTime.gameObject.SetActive(false);
						this.RefreshShopStatus = 3.40282347E+38f;
					}
					this.Currency.gameObject.SetActive(true);
					this.CurrencyIcon.spriteName = Tools.GetCurrencyIcon(ECurrencyType.ECurrencyT_LopetSoul);
					this.CurrencyText.text = Tools.FormatCurrency(player.Data.LopetSoul);
					this.CurrencyPlusBtn.gameObject.SetActive(true);
				}
			}
			else
			{
				this.refreshBtn.isEnabled = true;
				this.refreshBtn.gameObject.SetActive(true);
				this.refreshPrice.gameObject.SetActive(true);
				int num3 = Tools.GetShopRefreshTimes(this.ShopType) - (int)player.Data.AwakenShopRefresh;
				if (num3 > 0)
				{
					this.refreshBtnText.text = string.Format(Singleton<StringManager>.Instance.GetString("ShopCommon4"), num3.ToString());
				}
				else
				{
					this.refreshBtnText.text = Singleton<StringManager>.Instance.GetString("ShopCommon3");
				}
				int itemCount2 = player.ItemSystem.GetItemCount(GameConst.GetInt32(82));
				if (itemCount2 > 0)
				{
					this.refreshPriceIcon.spriteName = "refresh";
					this.refreshPriceTex.text = itemCount2.ToString();
					this.refreshPriceTex.color = Color.white;
				}
				else
				{
					this.refreshPriceIcon.spriteName = "redGem_1";
					this.refreshPriceTex.text = GameConst.GetInt32(51).ToString();
					if (player.Data.Diamond >= GameConst.GetInt32(51))
					{
						this.refreshPriceTex.color = Color.white;
					}
					else
					{
						this.refreshPriceTex.color = Color.red;
					}
				}
				int num4 = (Globals.Instance.Player.Data.ShopCommon2TimeStamp <= 0) ? 0 : (Globals.Instance.Player.Data.ShopCommon2TimeStamp - Globals.Instance.Player.GetTimeStamp());
				if (num4 > 0)
				{
					this.refreshTime.gameObject.SetActive(true);
					this.refreshTime.text = string.Format(Singleton<StringManager>.Instance.GetString("ShopCommon0"), Tools.FormatTime(num4));
				}
				else
				{
					this.refreshTime.gameObject.SetActive(false);
					this.RefreshShopStatus = 3.40282347E+38f;
				}
				this.Currency.gameObject.SetActive(true);
				this.CurrencyIcon.spriteName = Tools.GetCurrencyIcon(ECurrencyType.ECurrencyT_StarSoul);
				this.CurrencyText.text = Tools.FormatCurrency(player.Data.StarSoul);
				this.CurrencyPlusBtn.gameObject.SetActive(true);
			}
		}
		else
		{
			this.refreshBtn.isEnabled = true;
			this.refreshBtn.gameObject.SetActive(true);
			this.refreshPrice.gameObject.SetActive(true);
			int num5 = Tools.GetShopRefreshTimes(this.ShopType) - (int)player.Data.Common2ShopRefresh;
			if (num5 > 0)
			{
				this.refreshBtnText.text = string.Format(Singleton<StringManager>.Instance.GetString("ShopCommon4"), num5.ToString());
			}
			else
			{
				this.refreshBtnText.text = Singleton<StringManager>.Instance.GetString("ShopCommon3");
			}
			int itemCount3 = player.ItemSystem.GetItemCount(GameConst.GetInt32(82));
			if (itemCount3 > 0)
			{
				this.refreshPriceIcon.spriteName = "refresh";
				this.refreshPriceTex.text = itemCount3.ToString();
				this.refreshPriceTex.color = Color.white;
			}
			else
			{
				this.refreshPriceIcon.spriteName = "redGem_1";
				this.refreshPriceTex.text = GameConst.GetInt32(51).ToString();
				if (player.Data.Diamond >= GameConst.GetInt32(51))
				{
					this.refreshPriceTex.color = Color.white;
				}
				else
				{
					this.refreshPriceTex.color = Color.red;
				}
			}
			int num6 = (Globals.Instance.Player.Data.ShopCommon2TimeStamp <= 0) ? 0 : (Globals.Instance.Player.Data.ShopCommon2TimeStamp - Globals.Instance.Player.GetTimeStamp());
			if (num6 > 0)
			{
				this.refreshTime.gameObject.SetActive(true);
				this.refreshTime.text = string.Format(Singleton<StringManager>.Instance.GetString("ShopCommon0"), Tools.FormatTime(num6));
			}
			else
			{
				this.refreshTime.gameObject.SetActive(false);
				this.RefreshShopStatus = 3.40282347E+38f;
			}
			this.Currency.gameObject.SetActive(true);
			this.CurrencyIcon.spriteName = Tools.GetCurrencyIcon(ECurrencyType.ECurrencyT_MagicSoul);
			this.CurrencyText.text = Tools.FormatCurrency(player.Data.MagicSoul);
			this.CurrencyPlusBtn.gameObject.SetActive(true);
		}
		if (this.CurrencyIcon.gameObject.activeInHierarchy)
		{
			this.CurrencyIcon.MakePixelPerfect();
		}
	}

	private void RefreshCurrency2()
	{
		if (this.ShopType == EShopType.EShop_Trial)
		{
			int num = this.curEquipTab;
			if (num == 3)
			{
				this.Currency2.gameObject.SetActive(true);
				this.CurrencyIcon2.spriteName = Tools.GetCurrencyIcon(ECurrencyType.ECurrencyT_Emblem);
				this.CurrencyText2.text = Tools.FormatCurrency(Tools.GetCurrencyMoney(ECurrencyType.ECurrencyT_Emblem, 0));
				this.CurrencyPlusBtn2.gameObject.SetActive(false);
			}
		}
	}

	private void OnMsgShopBuyFashion(MemoryStream stream)
	{
		MS2C_ShopBuyFashion mS2C_ShopBuyFashion = Serializer.NonGeneric.Deserialize(typeof(MS2C_ShopBuyFashion), stream) as MS2C_ShopBuyFashion;
		if (mS2C_ShopBuyFashion.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("ItemR", mS2C_ShopBuyFashion.Result);
			return;
		}
		GameUIManager.mInstance.ShowMessageTipByKey("ShopT5", 0f, 0f);
		this.mTargetTable.repositionNow = true;
		GameUIManager.mInstance.TryCommend(ECommentType.EComment_Fashion, 0f);
	}

	private void OnPlayerUpdateEvent()
	{
		this.RefreshPlayerDataFlag = true;
	}

	private void OnShopBuyItemEvent(int shopType, int id)
	{
		this.mTargetTable.repositionNow = true;
		if (shopType == 0 || shopType == 1 || shopType == 9)
		{
			ShopItemData shopItem = Globals.Instance.Player.GetShopItem(shopType, id);
			if (shopItem != null && shopItem.Type == 2)
			{
				PetDataEx petByInfoID = Globals.Instance.Player.PetSystem.GetPetByInfoID(shopItem.InfoID);
				if (petByInfoID != null && petByInfoID.Info.Quality > 1)
				{
					GetPetLayer.Show(petByInfoID, null, GetPetLayer.EGPL_ShowNewsType.Null);
					return;
				}
			}
		}
		GameUIManager.mInstance.ShowMessageTipByKey("ShopT5", 0f, 0f);
	}

	private void OnPlayerShopDataEvent(int shopType)
	{
		if (shopType >= 10)
		{
			return;
		}
		if (shopType != (int)this.ShopType)
		{
			global::Debug.LogErrorFormat("shop type {0} != {1} error maybe network delay", new object[]
			{
				(EShopType)shopType,
				this.ShopType
			});
		}
		this.OpenShopData();
	}

	private void OnTabCheckChanged()
	{
		if (UIToggle.current.value)
		{
			for (int i = 0; i < 4; i++)
			{
				if (UIToggle.current == this.tabEquip[i] && this.curEquipTab != i)
				{
					Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
					this.curEquipTab = i;
					this.OpenShopData();
				}
			}
		}
	}

	private void OnRefreshClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.ShopType != EShopType.EShop_Common2 && this.ShopType != EShopType.EShop_Awaken && this.ShopType != EShopType.EShop_Lopet)
		{
			global::Debug.LogErrorFormat("shop can not refresh {0}", new object[]
			{
				this.ShopType
			});
			return;
		}
		LocalPlayer player = Globals.Instance.Player;
		if (this.ShopType == EShopType.EShop_Common2)
		{
			int num = Tools.GetShopRefreshTimes(this.ShopType) - (int)player.Data.Common2ShopRefresh;
			if (num <= 0)
			{
				if (player.Data.VipLevel >= 15u)
				{
					GameUIManager.mInstance.ShowMessageTipByKey("ShopT6_1", 0f, 0f);
				}
				else
				{
					VipLevelInfo vipLevelInfo = null;
					for (VipLevelInfo info = Globals.Instance.AttDB.VipLevelDict.GetInfo((int)(player.Data.VipLevel + 1u)); info != null; info = Globals.Instance.AttDB.VipLevelDict.GetInfo(info.ID + 1))
					{
						if (info.ID > 15)
						{
							break;
						}
						if ((long)info.ShopCommon2Count > (long)((ulong)player.Data.Common2ShopRefresh))
						{
							vipLevelInfo = info;
							break;
						}
					}
					if (vipLevelInfo != null)
					{
						GameMessageBox.ShowPrivilegeMessageBox(string.Format(Singleton<StringManager>.Instance.GetString("ShopT6"), vipLevelInfo.ID, Singleton<StringManager>.Instance.GetString("ShopN0"), vipLevelInfo.ShopCommon2Count));
					}
					else
					{
						GameUIManager.mInstance.ShowMessageTipByKey("ShopT6_1", 0f, 0f);
					}
				}
			}
			else
			{
				int itemCount = player.ItemSystem.GetItemCount(GameConst.GetInt32(82));
				if (itemCount > 0)
				{
					MC2S_GetShopData mC2S_GetShopData = new MC2S_GetShopData();
					mC2S_GetShopData.ShopVersion = Globals.Instance.Player.PetShopVersion;
					mC2S_GetShopData.Refresh = true;
					mC2S_GetShopData.ShopType = (int)this.ShopType;
					mC2S_GetShopData.DiamondRefresh = false;
					Globals.Instance.CliSession.Send(512, mC2S_GetShopData);
					this.refreshBtn.isEnabled = false;
					return;
				}
				if (player.Data.Diamond < GameConst.GetInt32(51))
				{
					GameMessageBox.ShowRechargeMessageBox();
				}
				else
				{
					MC2S_GetShopData mC2S_GetShopData2 = new MC2S_GetShopData();
					mC2S_GetShopData2.ShopVersion = Globals.Instance.Player.PetShopVersion;
					mC2S_GetShopData2.Refresh = true;
					mC2S_GetShopData2.ShopType = (int)this.ShopType;
					mC2S_GetShopData2.DiamondRefresh = true;
					Globals.Instance.CliSession.Send(512, mC2S_GetShopData2);
					this.refreshBtn.isEnabled = false;
				}
			}
		}
		else if (this.ShopType == EShopType.EShop_Awaken)
		{
			int num2 = Tools.GetShopRefreshTimes(this.ShopType) - (int)player.Data.AwakenShopRefresh;
			if (num2 <= 0)
			{
				if (player.Data.VipLevel >= 15u)
				{
					GameUIManager.mInstance.ShowMessageTipByKey("ShopT6_1", 0f, 0f);
				}
				else
				{
					VipLevelInfo vipLevelInfo2 = null;
					for (VipLevelInfo info2 = Globals.Instance.AttDB.VipLevelDict.GetInfo((int)(player.Data.VipLevel + 1u)); info2 != null; info2 = Globals.Instance.AttDB.VipLevelDict.GetInfo(info2.ID + 1))
					{
						if (info2.ID > 15)
						{
							break;
						}
						if ((long)info2.ShopAwakenCount > (long)((ulong)player.Data.AwakenShopRefresh))
						{
							vipLevelInfo2 = info2;
							break;
						}
					}
					if (vipLevelInfo2 != null)
					{
						GameMessageBox.ShowPrivilegeMessageBox(string.Format(Singleton<StringManager>.Instance.GetString("ShopT6"), vipLevelInfo2.ID, Singleton<StringManager>.Instance.GetString("ShopN1"), vipLevelInfo2.ShopAwakenCount));
					}
					else
					{
						GameUIManager.mInstance.ShowMessageTipByKey("ShopT6_1", 0f, 0f);
					}
				}
			}
			else
			{
				int itemCount2 = player.ItemSystem.GetItemCount(GameConst.GetInt32(82));
				if (itemCount2 > 0)
				{
					MC2S_GetShopData mC2S_GetShopData3 = new MC2S_GetShopData();
					mC2S_GetShopData3.ShopVersion = Globals.Instance.Player.PetShopVersion;
					mC2S_GetShopData3.Refresh = true;
					mC2S_GetShopData3.ShopType = (int)this.ShopType;
					mC2S_GetShopData3.DiamondRefresh = false;
					Globals.Instance.CliSession.Send(512, mC2S_GetShopData3);
					this.refreshBtn.isEnabled = false;
					return;
				}
				if (player.Data.Diamond < GameConst.GetInt32(51))
				{
					GameMessageBox.ShowRechargeMessageBox();
				}
				else
				{
					MC2S_GetShopData mC2S_GetShopData4 = new MC2S_GetShopData();
					mC2S_GetShopData4.ShopVersion = Globals.Instance.Player.AwakeShopVersion;
					mC2S_GetShopData4.Refresh = true;
					mC2S_GetShopData4.ShopType = (int)this.ShopType;
					mC2S_GetShopData4.DiamondRefresh = true;
					Globals.Instance.CliSession.Send(512, mC2S_GetShopData4);
					this.refreshBtn.isEnabled = false;
				}
			}
		}
		else if (this.ShopType == EShopType.EShop_Lopet)
		{
			int num3 = Tools.GetShopRefreshTimes(this.ShopType) - player.Data.LopetShopRefresh;
			if (num3 <= 0)
			{
				if (player.Data.VipLevel >= 15u)
				{
					GameUIManager.mInstance.ShowMessageTipByKey("ShopT6_1", 0f, 0f);
				}
				else
				{
					VipLevelInfo vipLevelInfo3 = null;
					for (VipLevelInfo info3 = Globals.Instance.AttDB.VipLevelDict.GetInfo((int)(player.Data.VipLevel + 1u)); info3 != null; info3 = Globals.Instance.AttDB.VipLevelDict.GetInfo(info3.ID + 1))
					{
						if (info3.ID > 15)
						{
							break;
						}
						if (info3.ShopLopetCount > player.Data.LopetShopRefresh)
						{
							vipLevelInfo3 = info3;
							break;
						}
					}
					if (vipLevelInfo3 != null)
					{
						GameMessageBox.ShowPrivilegeMessageBox(string.Format(Singleton<StringManager>.Instance.GetString("ShopT6"), vipLevelInfo3.ID, Singleton<StringManager>.Instance.GetString("ShopN9"), vipLevelInfo3.ShopLopetCount));
					}
					else
					{
						GameUIManager.mInstance.ShowMessageTipByKey("ShopT6_1", 0f, 0f);
					}
				}
			}
			else
			{
				int itemCount3 = player.ItemSystem.GetItemCount(GameConst.GetInt32(82));
				if (itemCount3 > 0)
				{
					MC2S_GetShopData mC2S_GetShopData5 = new MC2S_GetShopData();
					mC2S_GetShopData5.ShopVersion = Globals.Instance.Player.PetShopVersion;
					mC2S_GetShopData5.Refresh = true;
					mC2S_GetShopData5.ShopType = (int)this.ShopType;
					mC2S_GetShopData5.DiamondRefresh = false;
					Globals.Instance.CliSession.Send(512, mC2S_GetShopData5);
					this.refreshBtn.isEnabled = false;
					return;
				}
				if (player.Data.Diamond < GameConst.GetInt32(51))
				{
					GameMessageBox.ShowRechargeMessageBox();
				}
				else
				{
					MC2S_GetShopData mC2S_GetShopData6 = new MC2S_GetShopData();
					mC2S_GetShopData6.ShopVersion = Globals.Instance.Player.AwakeShopVersion;
					mC2S_GetShopData6.Refresh = true;
					mC2S_GetShopData6.ShopType = (int)this.ShopType;
					mC2S_GetShopData6.DiamondRefresh = true;
					Globals.Instance.CliSession.Send(512, mC2S_GetShopData6);
					this.refreshBtn.isEnabled = false;
				}
			}
		}
	}

	private void OnCurrencyPlusBtnClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		switch (this.ShopType)
		{
		case EShopType.EShop_Common2:
			GUIRecycleScene.Change2This(GUIRecycleScene.ERecycleT.ERecycleT_PetBreak);
			break;
		case EShopType.EShop_Awaken:
			GUIPropsBagScene.TryOpenAwakeItemsLayer();
			break;
		case EShopType.EShop_Guild:
			GUIGuildManageScene.TryOpen();
			break;
		case EShopType.EShop_Trial:
			GUIRecycleScene.Change2This(GUIRecycleScene.ERecycleT.ERecycleT_EquipBreak);
			break;
		case EShopType.EShop_KR:
			GUIKingRewardScene.TryOpen();
			break;
		case EShopType.EShop_Pvp:
			GUIPVP4ReadyScene.TryOpen();
			break;
		case EShopType.EShop_Lopet:
			GUIKingRewardScene.TryOpen();
			break;
		}
	}

	private void OnCurrency2PlusBtnClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
	}

	private void OnOpenShowRTShopItemDialog(RTShopGridData data)
	{
		if (data == null)
		{
			return;
		}
		if (data.shopData.InfoType == 1 && data.itemInfo != null)
		{
			GameUIManager.mInstance.ShowItemInfo(data.itemInfo);
		}
		else if (data.shopData.InfoType == 2 && data.petInfo != null)
		{
			GameUIManager.mInstance.ShowPetInfo(data.petInfo);
		}
	}

	private void OnOpenRTShopBuyCheckDialog(RTShopGridData data)
	{
		if (data.shopData.Type == 1)
		{
			Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
			if (this.BuyCheckWnd != null)
			{
				this.BuyCheckWnd.Show(data);
			}
		}
		else
		{
			Globals.Instance.EffectSoundMgr.Play("ui/ui_026");
			GUIShortcutBuyItem.RequestShopBuyRTItem(data, data.ShopType);
		}
	}

	private void OnOpenShowShopItemDialog(ShopGridData data)
	{
		if (data == null)
		{
			return;
		}
		if (data.shopInfo.IsFashion != 0)
		{
			GUIHowGetPetItemPopUp.ShowThis(data.fashionInfo);
		}
		else
		{
			GameUIManager.mInstance.ShowItemInfo(data.itemInfo);
		}
	}

	private void OnOpenShopBuyCheckDialog(ShopGridData data)
	{
		LocalPlayer player = Globals.Instance.Player;
		if (data.shopInfo.IsFashion != 0)
		{
			if (Tools.GetCurrencyMoney((ECurrencyType)data.shopInfo.CurrencyType2, 0) < data.shopInfo.Price2)
			{
				Tools.MoneyNotEnough((ECurrencyType)data.shopInfo.CurrencyType2, data.shopInfo.Price2, 0);
				return;
			}
			if (Tools.GetCurrencyMoney((ECurrencyType)data.shopInfo.CurrencyType, 0) < data.shopInfo.Price)
			{
				Tools.MoneyNotEnough((ECurrencyType)data.shopInfo.CurrencyType, data.shopInfo.Price, 0);
				return;
			}
			if (this.BuyCheckWnd != null)
			{
				this.BuyCheckWnd.Show(data);
			}
		}
		else
		{
			int buyCount = player.GetBuyCount(data.shopInfo);
			int currencyMoney = Tools.GetCurrencyMoney((ECurrencyType)data.shopInfo.CurrencyType, 0);
			int itemBuyConst = Tools.GetItemBuyConst(data.itemInfo, buyCount + 1, data.shopInfo);
			if (currencyMoney < itemBuyConst)
			{
				Tools.MoneyNotEnough((ECurrencyType)data.shopInfo.CurrencyType, itemBuyConst, 0);
				return;
			}
			if (data.shopInfo.Price2 > 0 && Tools.MoneyNotEnough((ECurrencyType)data.shopInfo.CurrencyType2, data.shopInfo.Price2, 0))
			{
				return;
			}
			if (this.BuyMutilCheckWnd != null)
			{
				this.BuyMutilCheckWnd.Show(data);
			}
		}
	}
}
