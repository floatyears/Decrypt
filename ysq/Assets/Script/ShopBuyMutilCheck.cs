using Proto;
using System;
using UnityEngine;

public class ShopBuyMutilCheck : MonoBehaviour
{
	private ShopGridData shopData;

	protected ShopItemIcon ItemIcon;

	protected UILabel itemNum;

	protected UILabel buyTimes;

	protected UIButton add1;

	protected UIButton add10;

	protected UIButton sub1;

	protected UIButton sub10;

	protected UIIncrementNumberInput numInput;

	private UISprite PriceIcon0;

	private UILabel PriceCount0;

	private UISprite PriceIcon1;

	private UILabel PriceCount1;

	private UIButton ok;

	private UIButton cancel;

	public void Init()
	{
		this.ItemIcon = base.transform.Find("itemQuality").gameObject.AddComponent<ShopItemIcon>();
		this.ItemIcon.Init();
		this.itemNum = this.ItemIcon.transform.Find("num").GetComponent<UILabel>();
		this.buyTimes = base.transform.Find("buyTimes").GetComponent<UILabel>();
		Transform transform = base.transform.Find("constTip");
		this.PriceIcon0 = transform.transform.Find("PriceIcon0").GetComponent<UISprite>();
		this.PriceCount0 = transform.transform.Find("Price0").GetComponent<UILabel>();
		this.PriceIcon1 = transform.transform.Find("PriceIcon1").GetComponent<UISprite>();
		this.PriceCount1 = transform.transform.Find("Price1").GetComponent<UILabel>();
		this.ok = base.transform.Find("BtnBuy").GetComponent<UIButton>();
		UIEventListener expr_10E = UIEventListener.Get(this.ok.gameObject);
		expr_10E.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_10E.onClick, new UIEventListener.VoidDelegate(this.OnBuyClicked));
		this.cancel = base.transform.Find("BtnCancel").GetComponent<UIButton>();
		UIEventListener expr_15A = UIEventListener.Get(this.cancel.gameObject);
		expr_15A.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_15A.onClick, new UIEventListener.VoidDelegate(this.OnCancelClicked));
		this.numInput = base.transform.Find("numInput").GetComponent<UIIncrementNumberInput>();
		this.numInput.Init();
		UIIncrementNumberInput expr_1A7 = this.numInput;
		expr_1A7.NumberChangedEvent = (UIEventListener.VoidDelegate)Delegate.Combine(expr_1A7.NumberChangedEvent, new UIEventListener.VoidDelegate(this.OnNumberInputChanged));
	}

	public void Show(ShopGridData data)
	{
		this.shopData = data;
		if (this.shopData == null || this.shopData.shopInfo == null || this.shopData.itemInfo == null)
		{
			base.gameObject.SetActive(false);
			return;
		}
		base.gameObject.SetActive(true);
		this.ItemIcon.Refresh(this.shopData);
		LocalPlayer player = Globals.Instance.Player;
		this.itemNum.text = player.ItemSystem.GetItemCount(this.shopData.itemInfo.ID).ToString();
		this.numInput.Number = 1;
		this.numInput.MaxStep = 10;
		if (this.shopData.shopInfo.Times == 0)
		{
			this.buyTimes.gameObject.SetActive(false);
			this.ok.isEnabled = true;
			int currencyMoney = Tools.GetCurrencyMoney((ECurrencyType)this.shopData.shopInfo.CurrencyType, 0);
			int currencyMoney2 = Tools.GetCurrencyMoney((ECurrencyType)this.shopData.shopInfo.CurrencyType2, 0);
			if (this.shopData.shopInfo.ID > 1000)
			{
				this.numInput.Maximum = Mathf.Min(currencyMoney / Tools.GetItemBuyConst(this.shopData.itemInfo, 1, this.shopData.shopInfo), (this.shopData.shopInfo.Price2 <= 0) ? 2147483647 : (currencyMoney2 / this.shopData.shopInfo.Price2));
			}
			else
			{
				int buyCount = player.GetBuyCount(this.shopData.shopInfo);
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				while (true)
				{
					num += Tools.GetItemBuyConst(this.shopData.itemInfo, buyCount + num3 + 1, this.shopData.shopInfo);
					num2 += this.shopData.shopInfo.Price2;
					if (num > currencyMoney)
					{
						break;
					}
					if (num2 > currencyMoney2)
					{
						break;
					}
					num3++;
				}
				this.numInput.Maximum = num3;
			}
		}
		else
		{
			this.buyTimes.gameObject.SetActive(true);
			int buyCount2 = player.GetBuyCount(this.shopData.shopInfo);
			int num4 = Tools.GetShopBuyTimes(this.shopData.shopInfo) - buyCount2;
			if (num4 >= 0)
			{
				this.buyTimes.text = string.Format(Singleton<StringManager>.Instance.GetString("ShopT1"), num4);
				this.buyTimes.color = new Color32(244, 199, 159, 255);
				this.ok.isEnabled = true;
				int currencyMoney3 = Tools.GetCurrencyMoney((ECurrencyType)this.shopData.shopInfo.CurrencyType, 0);
				int currencyMoney4 = Tools.GetCurrencyMoney((ECurrencyType)this.shopData.shopInfo.CurrencyType2, 0);
				int num5 = 0;
				int num6 = 0;
				int num7 = 0;
				do
				{
					num5 += Tools.GetItemBuyConst(this.shopData.itemInfo, buyCount2 + num7 + 1, this.shopData.shopInfo);
					num6 += this.shopData.shopInfo.Price2;
					if (num5 > currencyMoney3)
					{
						break;
					}
					if (num6 > currencyMoney4)
					{
						break;
					}
					num7++;
				}
				while (num4 - num7 > 0);
				this.numInput.Maximum = num7;
			}
			else
			{
				this.buyTimes.text = Singleton<StringManager>.Instance.GetString("ShopT3");
				this.buyTimes.color = Color.red;
				this.ok.isEnabled = false;
				this.numInput.Maximum = 0;
			}
		}
		this.PriceIcon0.spriteName = Tools.GetCurrencyIcon((ECurrencyType)this.shopData.shopInfo.CurrencyType);
		if (this.shopData.shopInfo.Price2 > 0)
		{
			this.PriceIcon1.enabled = true;
			this.PriceCount1.enabled = true;
			this.PriceIcon1.spriteName = Tools.GetCurrencyIcon((ECurrencyType)this.shopData.shopInfo.CurrencyType2);
		}
		else
		{
			this.PriceIcon1.enabled = false;
			this.PriceCount1.enabled = false;
		}
		this.RefreshPrice();
	}

	private void RefreshPrice()
	{
		int i = this.numInput.Number;
		if (i < 0)
		{
			i = 1;
		}
		LocalPlayer player = Globals.Instance.Player;
		int buyCount = player.GetBuyCount(this.shopData.shopInfo);
		int num = 0;
		int num2 = 0;
		while (i > 0)
		{
			num += Tools.GetItemBuyConst(this.shopData.itemInfo, buyCount + ++num2, this.shopData.shopInfo);
			i--;
		}
		this.PriceCount0.text = num.ToString();
		if (Tools.GetCurrencyMoney((ECurrencyType)this.shopData.shopInfo.CurrencyType, 0) < num)
		{
			this.PriceCount0.color = Color.red;
		}
		else
		{
			this.PriceCount0.color = Color.white;
		}
		if (this.shopData.shopInfo.Price2 > 0)
		{
			num = this.shopData.shopInfo.Price2 * num2;
			this.PriceCount1.text = num.ToString();
			if (Tools.GetCurrencyMoney((ECurrencyType)this.shopData.shopInfo.CurrencyType2, 0) < num)
			{
				this.PriceCount1.color = Color.red;
			}
			else
			{
				this.PriceCount1.color = Color.white;
			}
		}
	}

	private void OnBuyClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_026");
		if (this.shopData == null || this.shopData.shopInfo == null)
		{
			return;
		}
		int number = this.numInput.Number;
		if (number <= 0)
		{
			return;
		}
		GUIShortcutBuyItem.RequestShopBuyItem(this.shopData.shopInfo, this.shopData.ShopType, number);
		this.shopData = null;
		base.gameObject.SetActive(false);
	}

	private void OnCancelClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		this.shopData = null;
		base.gameObject.SetActive(false);
	}

	private void OnNumberInputChanged(GameObject go)
	{
		if (this.shopData == null || this.shopData.shopInfo == null)
		{
			return;
		}
		this.RefreshPrice();
	}
}
