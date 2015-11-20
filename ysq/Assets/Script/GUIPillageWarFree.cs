using Att;
using Proto;
using System;
using UnityEngine;

public class GUIPillageWarFree : MonoBehaviour
{
	private GameObject mCloseBtn;

	private UISprite[] ItemIcon = new UISprite[2];

	private UISprite[] ItemQuality = new UISprite[2];

	private UILabel[] ItemNum = new UILabel[2];

	private UILabel[] ItemAtt = new UILabel[2];

	public void Init()
	{
		base.transform.localPosition = new Vector3(0f, 0f, -580f);
		Transform transform = base.transform.Find("BG");
		this.mCloseBtn = transform.Find("closeBtn").gameObject;
		UIEventListener expr_51 = UIEventListener.Get(this.mCloseBtn);
		expr_51.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_51.onClick, new UIEventListener.VoidDelegate(this.OnCloseBtnClicked));
		UIEventListener expr_7D = UIEventListener.Get(base.gameObject);
		expr_7D.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_7D.onClick, new UIEventListener.VoidDelegate(this.OnCloseBtnClicked));
		GUIShortcutBuyItem.BuildShopInfoData();
		Transform transform2 = transform.transform.Find("itemBg");
		for (int i = 0; i < 2; i++)
		{
			this.ItemIcon[i] = transform2.transform.Find(string.Format("Item{0}", i + 1)).GetComponent<UISprite>();
			this.ItemQuality[i] = this.ItemIcon[i].transform.Find("Quality").GetComponent<UISprite>();
			this.ItemNum[i] = this.ItemIcon[i].transform.Find("itemNum").GetComponent<UILabel>();
			this.ItemAtt[i] = this.ItemIcon[i].transform.Find("itemAtt").GetComponent<UILabel>();
			ItemInfo itemInfo = GUIShortcutBuyItem.ItemInfos[2 + i];
			if (itemInfo != null)
			{
				this.ItemIcon[i].spriteName = itemInfo.Icon;
				this.ItemQuality[i].spriteName = Tools.GetItemQualityIcon(itemInfo.Quality);
				this.ItemAtt[i].color = Tools.GetItemQualityColor(itemInfo.Quality);
			}
		}
		this.ItemAtt[0].text = Singleton<StringManager>.Instance.GetString("Pillage16");
		this.ItemAtt[1].text = Singleton<StringManager>.Instance.GetString("Pillage15");
		UIEventListener expr_1F9 = UIEventListener.Get(this.ItemIcon[0].gameObject);
		expr_1F9.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_1F9.onClick, new UIEventListener.VoidDelegate(this.OnItem1Clicked));
		UIEventListener expr_22C = UIEventListener.Get(this.ItemIcon[1].gameObject);
		expr_22C.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_22C.onClick, new UIEventListener.VoidDelegate(this.OnItem2Clicked));
		base.gameObject.SetActive(false);
	}

	public void Show()
	{
		base.gameObject.SetActive(true);
		this.RefreshItemCount();
	}

	public void RefreshItemCount()
	{
		LocalPlayer player = Globals.Instance.Player;
		for (int i = 0; i < 2; i++)
		{
			ItemInfo itemInfo = GUIShortcutBuyItem.ItemInfos[2 + i];
			if (itemInfo != null)
			{
				int itemCount = player.ItemSystem.GetItemCount(itemInfo.ID);
				this.ItemNum[i].text = itemCount.ToString();
				this.ItemNum[i].color = ((itemCount <= 0) ? Color.red : Color.white);
			}
		}
	}

	private void OnItem1Clicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		ItemInfo itemInfo = GUIShortcutBuyItem.ItemInfos[2];
		LocalPlayer player = Globals.Instance.Player;
		ItemDataEx itemByInfoID = player.ItemSystem.GetItemByInfoID(itemInfo.ID);
		if (itemByInfoID == null)
		{
			GUIShortcutBuyItem.Show(GUIShortcutBuyItem.BuyType.WarFree1);
		}
		else
		{
			MC2S_UseItem mC2S_UseItem = new MC2S_UseItem();
			mC2S_UseItem.ItemID = itemByInfoID.Data.ID;
			Globals.Instance.CliSession.Send(516, mC2S_UseItem);
		}
	}

	private void OnItem2Clicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		ItemInfo itemInfo = GUIShortcutBuyItem.ItemInfos[3];
		LocalPlayer player = Globals.Instance.Player;
		ItemDataEx itemByInfoID = player.ItemSystem.GetItemByInfoID(itemInfo.ID);
		if (itemByInfoID == null)
		{
			GUIShortcutBuyItem.Show(GUIShortcutBuyItem.BuyType.WarFree8);
		}
		else
		{
			MC2S_UseItem mC2S_UseItem = new MC2S_UseItem();
			mC2S_UseItem.ItemID = itemByInfoID.Data.ID;
			Globals.Instance.CliSession.Send(516, mC2S_UseItem);
		}
	}

	private void OnCloseBtnClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		base.gameObject.SetActive(false);
	}
}
