using Att;
using System;
using UnityEngine;

public sealed class UIRechargeItem : MonoBehaviour
{
	private UISprite icon;

	private UILabel itemName;

	private UILabel price;

	private UILabel give;

	private UISprite recommend;

	private UILabel recommendText;

	private PayInfo payInfo;

	private UILabel mCurrency;

	public static UIRechargeItem CreateItem(Transform parent, UIScrollView scrollView)
	{
		UnityEngine.Object @object = Res.LoadGUI("GUI/VIPRechargeItem");
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(@object);
		gameObject.name = @object.name;
		gameObject.transform.parent = parent;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		gameObject.AddComponent<UIDragScrollView>().scrollView = scrollView;
		return gameObject.AddComponent<UIRechargeItem>();
	}

	private void Awake()
	{
		this.icon = base.transform.FindChild("icon").GetComponent<UISprite>();
		this.itemName = base.transform.FindChild("name").GetComponent<UILabel>();
		this.price = base.transform.FindChild("Price/Label").GetComponent<UILabel>();
		this.mCurrency = this.price.transform.parent.Find("Rmb").GetComponent<UILabel>();
		this.give = base.transform.FindChild("give/Label").GetComponent<UILabel>();
		this.recommend = base.transform.FindChild("Tag").GetComponent<UISprite>();
		this.recommendText = this.recommend.transform.FindChild("Label").GetComponent<UILabel>();
		UIEventListener expr_D7 = UIEventListener.Get(base.gameObject);
		expr_D7.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_D7.onClick, new UIEventListener.VoidDelegate(this.OnItemClicked));
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
		int payCount = Globals.Instance.Player.GetPayCount(this.payInfo.ID);
		if (payCount == 0)
		{
			if (this.payInfo.Give == 0)
			{
				NGUITools.SetActive(this.give.transform.parent.gameObject, false);
			}
			else
			{
				this.give.text = string.Format(Singleton<StringManager>.Instance.GetString("VIPGive1"), this.payInfo.Give);
			}
		}
		else
		{
			NGUITools.SetActive(this.give.transform.parent.gameObject, false);
		}
		this.recommend.spriteName = ((payCount != 0) ? string.Empty : "Tag");
		this.recommendText.text = Singleton<StringManager>.Instance.GetString("recommendText2");
		NGUITools.SetActive(this.recommendText.gameObject, payCount == 0);
	}

	private void OnItemClicked(GameObject go)
	{
		GameMessageBox.ShowPayMessageBox(this.payInfo);
	}
}
