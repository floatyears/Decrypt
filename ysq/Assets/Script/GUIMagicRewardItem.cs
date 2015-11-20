using Att;
using System;
using System.Text;
using UnityEngine;

public class GUIMagicRewardItem : MonoBehaviour
{
	private UISprite mIcon;

	private UISprite mQualityMask;

	private UILabel mItemNum;

	private UILabel mItemName;

	private int mRewardType;

	private int mRewardValue1;

	private int mRewardValue2;

	private ItemInfo mItemInfo;

	private GameUIToolTip mToolTips;

	private StringBuilder mSb = new StringBuilder(42);

	public void InitWithBaseScene()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		Transform transform = base.transform.Find("magicRewardItem");
		this.mIcon = transform.Find("icon").GetComponent<UISprite>();
		this.mQualityMask = transform.Find("qualityMask").GetComponent<UISprite>();
		this.mItemNum = transform.Find("num").GetComponent<UILabel>();
		this.mItemName = transform.Find("name").GetComponent<UILabel>();
		UIEventListener expr_74 = UIEventListener.Get(transform.gameObject);
		expr_74.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(expr_74.onPress, new UIEventListener.BoolDelegate(this.OnRewardPress));
	}

	public void Refresh(int rewardType, int value1, int value2)
	{
		this.mRewardType = rewardType;
		this.mRewardValue1 = value1;
		this.mRewardValue2 = value2;
		if (this.mRewardType == 1)
		{
			this.mItemInfo = null;
			this.mIcon.spriteName = "M101";
			this.mQualityMask.gameObject.SetActive(false);
			this.mItemName.text = Singleton<StringManager>.Instance.GetString("money");
			this.mItemName.color = Color.white;
			this.mItemNum.text = this.mRewardValue1.ToString();
		}
		else if (this.mRewardType == 2)
		{
			this.mItemInfo = null;
			this.mIcon.spriteName = "M102";
			this.mQualityMask.gameObject.SetActive(false);
			this.mItemName.text = Singleton<StringManager>.Instance.GetString("diamond");
			this.mItemName.color = Color.white;
			this.mItemNum.text = this.mRewardValue1.ToString();
		}
		else if (this.mRewardType == 7)
		{
			this.mItemInfo = null;
			this.mIcon.spriteName = "M103";
			this.mQualityMask.gameObject.SetActive(false);
			this.mItemName.text = Singleton<StringManager>.Instance.GetString("guildValue");
			this.mItemName.color = Color.white;
			this.mItemNum.text = this.mRewardValue1.ToString();
		}
		else if (this.mRewardType == 3)
		{
			this.mItemInfo = Globals.Instance.AttDB.ItemDict.GetInfo(this.mRewardValue1);
			if (this.mItemInfo != null)
			{
				this.mIcon.spriteName = this.mItemInfo.Icon;
				this.mQualityMask.gameObject.SetActive(true);
				this.mQualityMask.spriteName = Tools.GetItemQualityIcon(this.mItemInfo.Quality);
				this.mItemNum.text = this.mRewardValue2.ToString();
				this.mItemName.text = this.mItemInfo.Name;
				this.mItemName.color = Tools.GetItemQualityColor(this.mItemInfo.Quality);
			}
		}
	}

	private void OnRewardPress(GameObject go, bool isPressed)
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
			else if (this.mRewardType == 1)
			{
				this.mSb.Remove(0, this.mSb.Length);
				this.mSb.Append(this.mRewardValue1).Append(Singleton<StringManager>.Instance.GetString("money"));
				string title = this.mSb.ToString();
				string description = string.Format(Singleton<StringManager>.Instance.GetString("takeMoney"), this.mRewardValue1);
				this.mToolTips.Create(go.transform, title, description);
			}
			else if (this.mRewardType == 2)
			{
				this.mSb.Remove(0, this.mSb.Length);
				this.mSb.Append(this.mRewardValue1).Append(Singleton<StringManager>.Instance.GetString("diamond"));
				string title2 = this.mSb.ToString();
				string description2 = string.Format(Singleton<StringManager>.Instance.GetString("takeDiamond"), this.mRewardValue1);
				this.mToolTips.Create(go.transform, title2, description2);
			}
			else if (this.mRewardType == 7)
			{
				this.mSb.Remove(0, this.mSb.Length);
				this.mSb.Append(this.mRewardValue1).Append(Singleton<StringManager>.Instance.GetString("guildValue"));
				string title3 = this.mSb.ToString();
				string description3 = string.Format(Singleton<StringManager>.Instance.GetString("takeGuildReputation"), this.mRewardValue1);
				this.mToolTips.Create(go.transform, title3, description3);
			}
			this.mToolTips.transform.localPosition = new Vector3(0f, 100f, -7000f);
			this.mToolTips.EnableToolTip();
		}
		else if (this.mToolTips != null)
		{
			this.mToolTips.HideTipAnim();
		}
	}
}
