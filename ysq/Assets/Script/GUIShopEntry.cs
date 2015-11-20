using Proto;
using System;
using UnityEngine;

public class GUIShopEntry : GameUISession
{
	public class ShopItem
	{
		public UISprite shopIcon;

		public UISprite shopIcon2;

		public UILabel shopTile;

		public Transform newTip;

		public Transform disableMask;

		public UILabel openTip;
	}

	private static EShopType[] initType = new EShopType[]
	{
		EShopType.EShop_Common,
		EShopType.EShop_Common2,
		EShopType.EShop_Trial,
		EShopType.EShop_Pvp,
		EShopType.EShop_KR,
		EShopType.EShop_Guild,
		EShopType.EShop_Awaken,
		EShopType.EShop_Lopet
	};

	private UIGrid mEntryTable;

	private GUIShopEntry.ShopItem[] ShopItems = new GUIShopEntry.ShopItem[10];

	protected override void OnPostLoadGUI()
	{
		Globals.Instance.BackgroundMusicMgr.PlayLobbyMusic();
		TopGoods topGoods = GameUIManager.mInstance.GetTopGoods();
		topGoods.Show("shopLb");
		this.mEntryTable = base.transform.Find("ShopPanel/ShopContents").GetComponent<UIGrid>();
		for (int i = 0; i < GUIShopEntry.initType.Length; i++)
		{
			Transform shop = this.mEntryTable.transform.Find(string.Format("Shop{0}", i));
			this.InitShopEntryItem(GUIShopEntry.initType[i], shop);
		}
	}

	protected override void OnPreDestroyGUI()
	{
		GameUIManager.mInstance.GetTopGoods().Hide();
	}

	private void InitShopEntryItem(EShopType type, Transform Shop)
	{
		if (Shop == null)
		{
			return;
		}
		GUIShopEntry.ShopItem shopItem = new GUIShopEntry.ShopItem();
		this.ShopItems[(int)type] = shopItem;
		shopItem.shopIcon = Shop.GetComponent<UISprite>();
		shopItem.shopTile = Shop.Find("title").GetComponent<UILabel>();
		shopItem.newTip = Shop.Find("new");
		shopItem.disableMask = Shop.Find("disable");
		shopItem.openTip = Shop.Find("tips").GetComponent<UILabel>();
		shopItem.shopIcon.spriteName = string.Format("shop{0}", (int)type);
		shopItem.shopTile.text = Singleton<StringManager>.Instance.GetString(string.Format("ShopN{0}", (int)type));
		this.RefreshShopOpenState(type);
		if (type == EShopType.EShop_Common2)
		{
			shopItem.newTip.gameObject.SetActive(Globals.Instance.Player.HasRedFlag(1024));
		}
		else if (type == EShopType.EShop_Awaken)
		{
			shopItem.newTip.gameObject.SetActive(Globals.Instance.Player.HasRedFlag(2048));
		}
		else if (type == EShopType.EShop_Lopet)
		{
			shopItem.newTip.gameObject.SetActive(Globals.Instance.Player.HasRedFlag(131072));
			shopItem.shopIcon2 = Shop.Find("Icon2").GetComponent<UISprite>();
			shopItem.shopIcon2.enabled = true;
			shopItem.shopIcon2.spriteName = string.Format("shop{0}", (int)type);
		}
		else
		{
			shopItem.newTip.gameObject.SetActive(false);
		}
		UIEventListener expr_1A6 = UIEventListener.Get(Shop.gameObject);
		expr_1A6.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_1A6.onClick, new UIEventListener.VoidDelegate(this.OnShopItemClick));
		int num = (int)type;
		Shop.name = num.ToString();
	}

	private void RefreshShopOpenState(EShopType type)
	{
		GUIShopEntry.ShopItem shopItem = this.ShopItems[(int)type];
		if (shopItem == null)
		{
			return;
		}
		shopItem.shopIcon.gameObject.SetActive(true);
		if (GUIShopScene.IsShopOpen(type))
		{
			shopItem.disableMask.gameObject.SetActive(false);
			shopItem.openTip.gameObject.SetActive(false);
		}
		else if (type == EShopType.EShop_Guild)
		{
			shopItem.openTip.text = Singleton<StringManager>.Instance.GetString("ShopD2");
			shopItem.openTip.gameObject.SetActive(true);
			shopItem.disableMask.gameObject.SetActive(true);
		}
		else if (type == EShopType.EShop_KR)
		{
			shopItem.openTip.text = string.Format(Singleton<StringManager>.Instance.GetString(string.Format("ShopD{0}", (int)type)), GameConst.GetInt32(2));
			shopItem.openTip.gameObject.SetActive(true);
			shopItem.disableMask.gameObject.SetActive(true);
		}
		else if (type == EShopType.EShop_Trial)
		{
			shopItem.openTip.text = string.Format(Singleton<StringManager>.Instance.GetString(string.Format("ShopD{0}", (int)type)), GameConst.GetInt32(5));
			shopItem.openTip.gameObject.SetActive(true);
			shopItem.disableMask.gameObject.SetActive(true);
		}
		else if (type == EShopType.EShop_Pvp)
		{
			shopItem.openTip.text = string.Format(Singleton<StringManager>.Instance.GetString(string.Format("ShopD{0}", (int)type)), GameConst.GetInt32(6));
			shopItem.openTip.gameObject.SetActive(true);
			shopItem.disableMask.gameObject.SetActive(true);
		}
		else if (type == EShopType.EShop_Common2)
		{
			shopItem.openTip.text = string.Format(Singleton<StringManager>.Instance.GetString(string.Format("ShopD{0}", (int)type)), GameConst.GetInt32(27));
			shopItem.openTip.gameObject.SetActive(true);
			shopItem.disableMask.gameObject.SetActive(true);
		}
		else if (type == EShopType.EShop_Awaken)
		{
			if ((ulong)Globals.Instance.Player.Data.Level < (ulong)((long)GameConst.GetInt32(26)))
			{
				shopItem.shopIcon.gameObject.SetActive(false);
			}
			else
			{
				shopItem.openTip.text = string.Format(Singleton<StringManager>.Instance.GetString(string.Format("ShopD{0}", (int)type)), GameConst.GetInt32(24));
				shopItem.openTip.gameObject.SetActive(true);
				shopItem.disableMask.gameObject.SetActive(true);
			}
		}
		else if (type == EShopType.EShop_Lopet)
		{
			if ((ulong)Globals.Instance.Player.Data.Level < (ulong)((long)GameConst.GetInt32(209)))
			{
				shopItem.shopIcon.gameObject.SetActive(false);
			}
			else
			{
				shopItem.openTip.text = string.Format(Singleton<StringManager>.Instance.GetString(string.Format("ShopD{0}", (int)type)), GameConst.GetInt32(201));
				shopItem.openTip.gameObject.SetActive(true);
				shopItem.disableMask.gameObject.SetActive(true);
			}
		}
		else
		{
			shopItem.openTip.gameObject.SetActive(false);
			shopItem.disableMask.gameObject.SetActive(false);
		}
	}

	private void OnShopItemClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		EShopType type = (EShopType)int.Parse(go.name);
		GUIShopScene.TryOpen(type);
	}
}
