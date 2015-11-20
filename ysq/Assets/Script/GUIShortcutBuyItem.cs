using Att;
using Proto;
using System;
using UnityEngine;

public class GUIShortcutBuyItem : GameUIBasePopup
{
	public enum BuyType
	{
		Energy,
		Stamina,
		WarFree1,
		WarFree8,
		MAX
	}

	public static int[] ShortcutBuy = new int[]
	{
		10301,
		10302,
		10306,
		10307
	};

	public static string[] ShortcutBuyKey = new string[]
	{
		"energy",
		"stamina",
		string.Empty,
		string.Empty
	};

	public static string[] ShortcutBuyItemKey = new string[]
	{
		"energyItem",
		"staminaItem",
		string.Empty,
		string.Empty
	};

	public static ShopInfo[] ShopInfos = new ShopInfo[4];

	public static ItemInfo[] ItemInfos = new ItemInfo[4];

	private UILabel tip;

	private UISprite StaminaIcon;

	private UISprite StaminaQuality;

	private UILabel itemAtt;

	private UILabel buyTimes;

	private UILabel curCount;

	private UIButton btnBuy;

	private UILabel price;

	private GameObject btnUse;

	private GameObject btnDisableUse;

	private GUIShortcutBuyItem.BuyType currentType = GUIShortcutBuyItem.BuyType.MAX;

	private bool RefrehFlag;

	private GUIAttributeTip mGUIAttributeTip;

	public static void Show(GUIShortcutBuyItem.BuyType type)
	{
		GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GUIShortcutBuyItem, false, null, null);
		GameUIPopupManager.GetInstance().GetCurrentPopup().InitPopUp((int)type);
	}

	public static void BuildShopInfoData()
	{
		for (int i = 0; i < 4; i++)
		{
			if (GUIShortcutBuyItem.ShopInfos[i] == null)
			{
				GUIShortcutBuyItem.ShopInfos[i] = Globals.Instance.AttDB.ShopDict.GetInfo(GUIShortcutBuyItem.ShortcutBuy[i]);
				if (GUIShortcutBuyItem.ShopInfos[i] != null)
				{
					GUIShortcutBuyItem.ItemInfos[i] = Globals.Instance.AttDB.ItemDict.GetInfo(GUIShortcutBuyItem.ShopInfos[i].InfoID);
				}
				else
				{
					GUIShortcutBuyItem.ItemInfos[i] = null;
				}
			}
		}
	}

	public static void RequestShopBuyRTItem(RTShopGridData data, EShopType shopType)
	{
		if (data == null || data.shopData == null)
		{
			return;
		}
		if (data.shopData.BuyCount != 0u)
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("ShopT3"), 0f, 0f);
			return;
		}
		int num = data.GetPrice();
		if (Tools.GetCurrencyMoney((ECurrencyType)data.shopData.Type, 0) < num)
		{
			Tools.MoneyNotEnough((ECurrencyType)data.shopData.Type, num, 0);
			return;
		}
		MC2S_ShopBuyItem mC2S_ShopBuyItem = new MC2S_ShopBuyItem();
		mC2S_ShopBuyItem.ID = data.shopData.ID;
		mC2S_ShopBuyItem.Price = (uint)num;
		mC2S_ShopBuyItem.ShopType = (int)shopType;
		mC2S_ShopBuyItem.Count = 1;
		Globals.Instance.CliSession.Send(514, mC2S_ShopBuyItem);
	}

	public static void RequestShopBuyItem(ShopInfo shopInfo, EShopType shopType, int count)
	{
		if (shopInfo == null)
		{
			return;
		}
		LocalPlayer player = Globals.Instance.Player;
		if (shopInfo.Type == 1 && (player.GuildSystem.Guild == null || player.GuildSystem.Guild.Level < shopInfo.Value))
		{
			GameUIManager.mInstance.ShowMessageTip(string.Format(Singleton<StringManager>.Instance.GetString("ShopT4"), shopInfo.Value), 0f, 0f);
			return;
		}
		if (shopInfo.Type == 2 && player.Data.TrialMaxWave < shopInfo.Value)
		{
			GameUIManager.mInstance.ShowMessageTip(string.Format(Singleton<StringManager>.Instance.GetString("ShopT41"), shopInfo.Value), 0f, 0f);
			return;
		}
		if (shopInfo.Type == 3 && (player.Data.ArenaHighestRank == 0 || player.Data.ArenaHighestRank > shopInfo.Value))
		{
			GameUIManager.mInstance.ShowMessageTip(string.Format(Singleton<StringManager>.Instance.GetString("ShopT42"), shopInfo.Value), 0f, 0f);
			return;
		}
		int buyCount = player.GetBuyCount(shopInfo);
		if (shopInfo.Times != 0)
		{
			int num = Tools.GetShopBuyTimes(shopInfo) - buyCount;
			if (num <= 0)
			{
				GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("ShopT3"), 0f, 0f);
				return;
			}
		}
		int num2 = 0;
		int num3 = 0;
		if (shopInfo.IsFashion != 0)
		{
			if (Tools.GetCurrencyMoney((ECurrencyType)shopInfo.CurrencyType2, 0) < shopInfo.Price2)
			{
				Tools.MoneyNotEnough((ECurrencyType)shopInfo.CurrencyType2, shopInfo.Price2, 0);
				return;
			}
			num2 = count * shopInfo.Price;
			if (Tools.GetCurrencyMoney((ECurrencyType)shopInfo.CurrencyType, 0) < num2)
			{
				Tools.MoneyNotEnough((ECurrencyType)shopInfo.CurrencyType, num2, 0);
				return;
			}
			MC2S_ShopBuyFashion mC2S_ShopBuyFashion = new MC2S_ShopBuyFashion();
			mC2S_ShopBuyFashion.ID = shopInfo.ID;
			Globals.Instance.CliSession.Send(534, mC2S_ShopBuyFashion);
		}
		else
		{
			ItemInfo info = Globals.Instance.AttDB.ItemDict.GetInfo(shopInfo.InfoID);
			while (count - num3 > 0)
			{
				num2 += Tools.GetItemBuyConst(info, buyCount + ++num3, shopInfo);
			}
			if (Tools.GetCurrencyMoney((ECurrencyType)shopInfo.CurrencyType, 0) < num2)
			{
				Tools.MoneyNotEnough((ECurrencyType)shopInfo.CurrencyType, num2, 0);
				return;
			}
			if (shopInfo.Price2 > 0 && Tools.MoneyNotEnough((ECurrencyType)shopInfo.CurrencyType2, num3 * shopInfo.Price2, 0))
			{
				return;
			}
			MC2S_ShopBuyItem mC2S_ShopBuyItem = new MC2S_ShopBuyItem();
			mC2S_ShopBuyItem.ID = shopInfo.ID;
			mC2S_ShopBuyItem.Price = (uint)shopInfo.Price;
			mC2S_ShopBuyItem.ShopType = (int)shopType;
			mC2S_ShopBuyItem.Count = count;
			Globals.Instance.CliSession.Send(514, mC2S_ShopBuyItem);
		}
	}

	private void Awake()
	{
		this.CreateObjects();
	}

	public override void InitPopUp(int type)
	{
		if (type >= 4 || GUIShortcutBuyItem.ShortcutBuy[type] == 0)
		{
			global::Debug.LogErrorFormat("param error {0}", new object[]
			{
				type
			});
			GameUIPopupManager.GetInstance().PopState(false, null);
			return;
		}
		GUIShortcutBuyItem.BuildShopInfoData();
		if (GUIShortcutBuyItem.ShopInfos[type] == null)
		{
			global::Debug.LogErrorFormat("Can not find Shop ID {0}", new object[]
			{
				GUIShortcutBuyItem.ShortcutBuy[type]
			});
			GameUIPopupManager.GetInstance().PopState(false, null);
			return;
		}
		if (GUIShortcutBuyItem.ItemInfos[type] == null)
		{
			global::Debug.LogErrorFormat("Can not find Shop item ID {0}", new object[]
			{
				GUIShortcutBuyItem.ShopInfos[type].InfoID
			});
			GameUIPopupManager.GetInstance().PopState(false, null);
			return;
		}
		this.currentType = (GUIShortcutBuyItem.BuyType)type;
		this.StaminaIcon.spriteName = GUIShortcutBuyItem.ItemInfos[type].Icon;
		this.StaminaQuality.spriteName = Tools.GetItemQualityIcon(GUIShortcutBuyItem.ItemInfos[type].Quality);
		if (this.currentType == GUIShortcutBuyItem.BuyType.WarFree1 || this.currentType == GUIShortcutBuyItem.BuyType.WarFree8)
		{
			this.tip.text = Singleton<StringManager>.Instance.GetString("Pillage13");
			this.itemAtt.text = GUIShortcutBuyItem.ItemInfos[type].Desc;
		}
		else
		{
			string @string = Singleton<StringManager>.Instance.GetString(GUIShortcutBuyItem.ShortcutBuyKey[type]);
			string string2 = Singleton<StringManager>.Instance.GetString(GUIShortcutBuyItem.ShortcutBuyItemKey[type]);
			this.tip.text = string.Format(Singleton<StringManager>.Instance.GetString("Pillage12"), string2, @string);
			this.itemAtt.text = string.Format("{0} [96e600]+{1}[-]", @string, GUIShortcutBuyItem.ItemInfos[type].Value1);
		}
		this.RefreshPrice();
		this._RefrehCurrentCount();
	}

	public override void OnPopUpClosing()
	{
		base.OnPopUpClosing();
		if (Globals.Instance == null)
		{
			return;
		}
		if (this.mGUIAttributeTip != null)
		{
			this.mGUIAttributeTip.DestroySelf();
			this.mGUIAttributeTip = null;
		}
		ItemSubSystem expr_49 = Globals.Instance.Player.ItemSystem;
		expr_49.AddItemEvent = (ItemSubSystem.AddItemCallback)Delegate.Remove(expr_49.AddItemEvent, new ItemSubSystem.AddItemCallback(this.OnAddItemEvent));
		ItemSubSystem expr_79 = Globals.Instance.Player.ItemSystem;
		expr_79.RemoveItemEvent = (ItemSubSystem.RemoveItemCallback)Delegate.Remove(expr_79.RemoveItemEvent, new ItemSubSystem.RemoveItemCallback(this.OnRemoveItemEvent));
		ItemSubSystem expr_A9 = Globals.Instance.Player.ItemSystem;
		expr_A9.UpdateItemEvent = (ItemSubSystem.UpdateItemCallback)Delegate.Remove(expr_A9.UpdateItemEvent, new ItemSubSystem.UpdateItemCallback(this.OnUpdateItemEvent));
		ItemSubSystem expr_D9 = Globals.Instance.Player.ItemSystem;
		expr_D9.AddItemEvent = (ItemSubSystem.AddItemCallback)Delegate.Remove(expr_D9.AddItemEvent, new ItemSubSystem.AddItemCallback(this.OnAddItemEvent));
		LocalPlayer expr_104 = Globals.Instance.Player;
		expr_104.PlayerUpdateEvent = (LocalPlayer.VoidCallback)Delegate.Remove(expr_104.PlayerUpdateEvent, new LocalPlayer.VoidCallback(this.OnPlayerUpdateEvent));
		LocalPlayer expr_12F = Globals.Instance.Player;
		expr_12F.UseItemEvent = (LocalPlayer.UseItemCallback)Delegate.Remove(expr_12F.UseItemEvent, new LocalPlayer.UseItemCallback(this.OnUseItemEvent));
	}

	private void RefreshPrice()
	{
		if (GUIShortcutBuyItem.ShopInfos[(int)this.currentType] == null || GUIShortcutBuyItem.ItemInfos[(int)this.currentType] == null)
		{
			return;
		}
		LocalPlayer player = Globals.Instance.Player;
		int buyCount = player.GetBuyCount(GUIShortcutBuyItem.ShopInfos[(int)this.currentType]);
		int itemBuyConst = Tools.GetItemBuyConst(GUIShortcutBuyItem.ItemInfos[(int)this.currentType], buyCount + 1, GUIShortcutBuyItem.ShopInfos[(int)this.currentType]);
		this.price.text = itemBuyConst.ToString();
		if (player.Data.Diamond >= itemBuyConst)
		{
			this.price.color = Color.white;
		}
		else
		{
			this.price.color = Color.red;
		}
	}

	private void _RefrehCurrentCount()
	{
		this.RefrehFlag = false;
		LocalPlayer player = Globals.Instance.Player;
		if (GUIShortcutBuyItem.ShopInfos[(int)this.currentType].Times != 0)
		{
			int num = Tools.GetShopBuyTimes(GUIShortcutBuyItem.ShopInfos[(int)this.currentType]) - player.GetBuyCount(GUIShortcutBuyItem.ShopInfos[(int)this.currentType]);
			if (num < 0)
			{
				num = 0;
			}
			this.buyTimes.text = string.Format(Singleton<StringManager>.Instance.GetString("Pillage6"), (num <= 0) ? "[FF3232]" : "[96e600]", num);
			this.buyTimes.gameObject.SetActive(true);
		}
		else
		{
			this.buyTimes.gameObject.SetActive(false);
		}
		int itemCount = player.ItemSystem.GetItemCount(GUIShortcutBuyItem.ItemInfos[(int)this.currentType].ID);
		this.curCount.text = string.Format(Singleton<StringManager>.Instance.GetString("Pillage5"), (itemCount <= 0) ? "[FF3232]" : "[96e600]", itemCount);
		if (itemCount > 0)
		{
			this.btnDisableUse.SetActive(false);
			this.btnUse.SetActive(true);
		}
		else
		{
			this.btnDisableUse.SetActive(true);
			this.btnUse.SetActive(false);
		}
	}

	private void Update()
	{
		if (this.RefrehFlag)
		{
			this._RefrehCurrentCount();
			this.RefreshPrice();
		}
	}

	private void OnRemoveItemEvent(ulong id)
	{
		this.RefrehFlag = true;
	}

	private void OnAddItemEvent(ItemDataEx data)
	{
		if (this.currentType == GUIShortcutBuyItem.BuyType.MAX || GUIShortcutBuyItem.ItemInfos[(int)this.currentType] == null || data.Info != GUIShortcutBuyItem.ItemInfos[(int)this.currentType])
		{
			return;
		}
		this.RefrehFlag = true;
	}

	private void OnUpdateItemEvent(ItemDataEx data)
	{
		if (this.currentType == GUIShortcutBuyItem.BuyType.MAX || GUIShortcutBuyItem.ItemInfos[(int)this.currentType] == null || data.Info != GUIShortcutBuyItem.ItemInfos[(int)this.currentType])
		{
			return;
		}
		this.RefrehFlag = true;
	}

	private void OnUseItemEvent(int infoID, int value)
	{
		this.mGUIAttributeTip = GUIPropsBagScene.ShowItemUseTips(infoID, value, 1);
	}

	private void OnPlayerUpdateEvent()
	{
		this.RefrehFlag = true;
	}

	private void CreateObjects()
	{
		Transform transform = base.transform.Find("BG");
		GameObject gameObject = transform.Find("closeBtn").gameObject;
		UIEventListener expr_28 = UIEventListener.Get(gameObject);
		expr_28.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_28.onClick, new UIEventListener.VoidDelegate(this.OnCloseClick));
		UIEventListener expr_54 = UIEventListener.Get(base.gameObject);
		expr_54.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_54.onClick, new UIEventListener.VoidDelegate(this.OnCloseClick));
		this.tip = transform.Find("tip").GetComponent<UILabel>();
		this.StaminaIcon = transform.FindChild("titleBg/StaminaIcon").GetComponent<UISprite>();
		this.StaminaQuality = this.StaminaIcon.transform.Find("Quality").GetComponent<UISprite>();
		this.itemAtt = this.StaminaIcon.transform.Find("itemAtt").GetComponent<UILabel>();
		this.buyTimes = transform.Find("buyTimes").GetComponent<UILabel>();
		this.curCount = transform.Find("curCount").GetComponent<UILabel>();
		this.btnBuy = transform.Find("btnBuy").GetComponent<UIButton>();
		UIEventListener expr_133 = UIEventListener.Get(this.btnBuy.gameObject);
		expr_133.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_133.onClick, new UIEventListener.VoidDelegate(this.OnBuyClick));
		this.price = this.btnBuy.transform.Find("price").GetComponent<UILabel>();
		this.btnUse = transform.Find("btnUse").gameObject;
		UIEventListener expr_19A = UIEventListener.Get(this.btnUse.gameObject);
		expr_19A.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_19A.onClick, new UIEventListener.VoidDelegate(this.OnUseClick));
		this.btnDisableUse = transform.Find("btnDisableUse").gameObject;
		ItemSubSystem expr_1E0 = Globals.Instance.Player.ItemSystem;
		expr_1E0.AddItemEvent = (ItemSubSystem.AddItemCallback)Delegate.Combine(expr_1E0.AddItemEvent, new ItemSubSystem.AddItemCallback(this.OnAddItemEvent));
		ItemSubSystem expr_210 = Globals.Instance.Player.ItemSystem;
		expr_210.RemoveItemEvent = (ItemSubSystem.RemoveItemCallback)Delegate.Combine(expr_210.RemoveItemEvent, new ItemSubSystem.RemoveItemCallback(this.OnRemoveItemEvent));
		ItemSubSystem expr_240 = Globals.Instance.Player.ItemSystem;
		expr_240.UpdateItemEvent = (ItemSubSystem.UpdateItemCallback)Delegate.Combine(expr_240.UpdateItemEvent, new ItemSubSystem.UpdateItemCallback(this.OnUpdateItemEvent));
		LocalPlayer expr_26B = Globals.Instance.Player;
		expr_26B.PlayerUpdateEvent = (LocalPlayer.VoidCallback)Delegate.Combine(expr_26B.PlayerUpdateEvent, new LocalPlayer.VoidCallback(this.OnPlayerUpdateEvent));
		LocalPlayer expr_296 = Globals.Instance.Player;
		expr_296.UseItemEvent = (LocalPlayer.UseItemCallback)Delegate.Combine(expr_296.UseItemEvent, new LocalPlayer.UseItemCallback(this.OnUseItemEvent));
	}

	private void OnCloseClick(GameObject go)
	{
		GameUIPopupManager.GetInstance().PopState(false, null);
	}

	private static void OnRechargeMessageBoxOK(object obj)
	{
		GameUIPopupManager.GetInstance().PopState(false, null);
	}

	private void OnBuyClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.currentType == GUIShortcutBuyItem.BuyType.MAX || GUIShortcutBuyItem.ShopInfos[(int)this.currentType] == null)
		{
			return;
		}
		GUIShortcutBuyItem.RequestShopBuyItem(GUIShortcutBuyItem.ShopInfos[(int)this.currentType], EShopType.EShop_Common, 1);
	}

	private void OnUseClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.currentType == GUIShortcutBuyItem.BuyType.MAX || GUIShortcutBuyItem.ItemInfos[(int)this.currentType] == null)
		{
			return;
		}
		LocalPlayer player = Globals.Instance.Player;
		ItemDataEx itemByInfoID = player.ItemSystem.GetItemByInfoID(GUIShortcutBuyItem.ItemInfos[(int)this.currentType].ID);
		if (itemByInfoID == null)
		{
			return;
		}
		if ((this.currentType == GUIShortcutBuyItem.BuyType.WarFree1 || this.currentType == GUIShortcutBuyItem.BuyType.WarFree8) && Globals.Instance.Player.Data.WarFreeTime > 0)
		{
			int num = Globals.Instance.Player.Data.WarFreeTime - Globals.Instance.Player.GetTimeStamp();
			if (num > 0)
			{
				GameMessageBox gameMessageBox = GameMessageBox.ShowMessageBox(Singleton<StringManager>.Instance.GetString("Pillage19"), MessageBox.Type.OKCancel, itemByInfoID.Data.ID);
				if (gameMessageBox != null)
				{
					GameMessageBox expr_F3 = gameMessageBox;
					expr_F3.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_F3.OkClick, new MessageBox.MessageDelegate(delegate(object obj)
					{
						MC2S_UseItem mC2S_UseItem2 = new MC2S_UseItem();
						mC2S_UseItem2.ItemID = (ulong)obj;
						Globals.Instance.CliSession.Send(516, mC2S_UseItem2);
					}));
				}
				return;
			}
		}
		MC2S_UseItem mC2S_UseItem = new MC2S_UseItem();
		mC2S_UseItem.ItemID = itemByInfoID.Data.ID;
		Globals.Instance.CliSession.Send(516, mC2S_UseItem);
	}
}
