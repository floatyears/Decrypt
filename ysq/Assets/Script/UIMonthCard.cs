using Att;
using System;
using UnityEngine;

public sealed class UIMonthCard : MonoBehaviour
{
	private UILabel itemName;

	private UILabel desc;

	private UISprite recommend;

	private UILabel recommendText;

	private UILabel price;

	private GameObject rmb;

	private PayInfo payInfo;

	private UISprite icon;

	private UISprite halo;

	private UILabel mCurrency;

	public static UIMonthCard CreateItem(Transform parent, UIScrollView scrollView)
	{
		UnityEngine.Object @object = Res.LoadGUI("GUI/VIPMonthCard");
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(@object);
		gameObject.name = @object.name;
		gameObject.transform.parent = parent;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		gameObject.AddComponent<UIDragScrollView>().scrollView = scrollView;
		return gameObject.AddComponent<UIMonthCard>();
	}

	private void Awake()
	{
		this.itemName = base.transform.FindChild("name").GetComponent<UILabel>();
		this.icon = base.transform.FindChild("icon").GetComponent<UISprite>();
		this.halo = base.transform.FindChild("halo").GetComponent<UISprite>();
		this.desc = base.transform.FindChild("Sprite/Label").GetComponent<UILabel>();
		this.recommend = base.transform.FindChild("Tag").GetComponent<UISprite>();
		this.recommendText = this.recommend.transform.FindChild("Label").GetComponent<UILabel>();
		this.price = base.transform.FindChild("Price/Label").GetComponent<UILabel>();
		this.rmb = base.transform.FindChild("Price/Rmb").gameObject;
		this.mCurrency = this.rmb.GetComponent<UILabel>();
		UIEventListener expr_F9 = UIEventListener.Get(base.gameObject);
		expr_F9.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_F9.onClick, new UIEventListener.VoidDelegate(this.OnItemClicked));
	}

	public void Init(PayInfo info)
	{
		if (info == null)
		{
			return;
		}
		this.payInfo = info;
		this.icon.spriteName = this.payInfo.Icon;
		this.itemName.text = this.payInfo.Name;
		this.mCurrency.enabled = true;
		this.price.text = this.payInfo.Price.ToString();
		this.Refresh();
	}

	public void Refresh()
	{
		if (this.payInfo == null)
		{
			return;
		}
		bool flag = true;
		if (this.payInfo.Type == 1)
		{
			if (GameUIManager.mInstance.uiState.ShowMonthCardHalo == 1)
			{
				this.halo.enabled = true;
				GameUIManager.mInstance.uiState.ShowMonthCardHalo = 0;
			}
			int cardRemainDays = Globals.Instance.Player.GetCardRemainDays();
			if (cardRemainDays > 0)
			{
				flag = false;
				this.desc.text = string.Format(Singleton<StringManager>.Instance.GetString("VIPMouthCard1"), cardRemainDays);
				NGUITools.SetActive(this.price.transform.parent.gameObject, false);
			}
			else
			{
				this.desc.text = Singleton<StringManager>.Instance.GetString("VIPMouthCard2");
				this.price.text = this.payInfo.Price.ToString();
			}
		}
		else
		{
			if (GameUIManager.mInstance.uiState.ShowMonthCardHalo == 2)
			{
				this.halo.enabled = true;
				GameUIManager.mInstance.uiState.ShowMonthCardHalo = 0;
			}
			this.desc.text = Singleton<StringManager>.Instance.GetString("VIPSuperCard2");
			if (Globals.Instance.Player.IsBuySuperCard())
			{
				this.price.text = Singleton<StringManager>.Instance.GetString("VIPSuperCard1");
				NGUITools.SetActive(this.rmb, false);
				flag = false;
			}
			else
			{
				this.price.text = this.payInfo.Price.ToString();
			}
		}
		this.recommend.spriteName = ((!flag) ? string.Empty : "Tag");
		this.recommendText.text = Singleton<StringManager>.Instance.GetString("recommendText");
		NGUITools.SetActive(this.recommendText.gameObject, flag);
	}

	private void OnItemClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.payInfo.Type == 1)
		{
			int cardRemainDays = Globals.Instance.Player.GetCardRemainDays();
			if (cardRemainDays > 0)
			{
				return;
			}
		}
		else if (Globals.Instance.Player.IsBuySuperCard())
		{
			return;
		}
		GameMessageBox.ShowPayMessageBox(this.payInfo);
	}
}
