using Att;
using System;
using System.Text;
using UnityEngine;

public class GUIFirstRechargeItem : MonoBehaviour
{
	private UISprite mIcon;

	private UISprite mQualityMask;

	private UILabel mNum;

	private UILabel mName;

	private ItemInfo mItemInfo;

	private int mGoldNum;

	private GameUIToolTip mToolTips;

	private StringBuilder mSb = new StringBuilder();

	public void InitWithBaseScene()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mIcon = base.transform.Find("icon").GetComponent<UISprite>();
		this.mQualityMask = base.transform.Find("qualityMark").GetComponent<UISprite>();
		this.mNum = base.transform.Find("num").GetComponent<UILabel>();
		this.mName = base.transform.Find("name").GetComponent<UILabel>();
	}

	public void Refresh(ItemInfo iInfo, int num)
	{
		this.mItemInfo = iInfo;
		if (this.mItemInfo != null)
		{
			this.mIcon.spriteName = this.mItemInfo.Icon;
			this.mQualityMask.gameObject.SetActive(true);
			this.mQualityMask.spriteName = Tools.GetItemQualityIcon(this.mItemInfo.Quality);
			this.mNum.text = num.ToString();
			this.mName.text = this.mItemInfo.Name;
			this.mName.color = Tools.GetItemQualityColor(this.mItemInfo.Quality);
			UIEventListener expr_A2 = UIEventListener.Get(base.gameObject);
			expr_A2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_A2.onClick, new UIEventListener.VoidDelegate(this.OnItemClick));
		}
	}

	public void Refresh(int moneyNum)
	{
		this.mGoldNum = moneyNum;
		this.mIcon.spriteName = "M101";
		this.mQualityMask.gameObject.SetActive(false);
		this.mName.text = Singleton<StringManager>.Instance.GetString("money");
		this.mName.color = Color.white;
		this.mNum.text = moneyNum.ToString();
		UIEventListener expr_6F = UIEventListener.Get(base.gameObject);
		expr_6F.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(expr_6F.onPress, new UIEventListener.BoolDelegate(this.OnItemPress));
	}

	private void OnItemClick(GameObject go)
	{
		GameUIManager.mInstance.ShowItemInfo(this.mItemInfo);
	}

	private void OnItemPress(GameObject go, bool isPressed)
	{
		if (isPressed)
		{
			if (this.mToolTips == null)
			{
				this.mToolTips = GameUIToolTipManager.GetInstance().CreateBasicTooltip(go.transform, string.Empty, string.Empty);
			}
			if (this.mItemInfo != null)
			{
				this.mToolTips.Create(go.transform, this.mItemInfo.Name, this.mItemInfo.Desc, this.mItemInfo.Quality);
			}
			else if (this.mGoldNum != 0)
			{
				this.mSb.Remove(0, this.mSb.Length);
				this.mSb.Append(this.mGoldNum).Append(Singleton<StringManager>.Instance.GetString("money"));
				string title = this.mSb.ToString();
				string description = string.Format(Singleton<StringManager>.Instance.GetString("takeMoney"), this.mGoldNum);
				this.mToolTips.Create(go.transform, title, description);
			}
			this.mToolTips.transform.localPosition = new Vector3(50f, 50f, -2000f);
			this.mToolTips.EnableToolTip();
		}
		else if (this.mToolTips != null)
		{
			this.mToolTips.HideTipAnim();
		}
	}
}
