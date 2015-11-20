using Att;
using System;
using UnityEngine;

public class RewardItem : MonoBehaviour
{
	private ItemInfo itemInfo;

	private int count;

	public void Init(ItemInfo info, int num, bool showValue, bool showTips)
	{
		this.itemInfo = info;
		this.count = num;
		UISprite component = base.GetComponent<UISprite>();
		component.spriteName = info.Icon;
		UISprite uISprite = GameUITools.FindUISprite("QualityMark", base.gameObject);
		uISprite.spriteName = Tools.GetItemQualityIcon(this.itemInfo.Quality);
		UISprite uISprite2 = GameUITools.FindUISprite("Flag", base.gameObject);
		if (info.Type == 3)
		{
			uISprite2.gameObject.SetActive(true);
			if (info.SubType == 0)
			{
				uISprite2.spriteName = "frag";
			}
			else
			{
				uISprite2.spriteName = "frag2";
			}
		}
		else
		{
			uISprite2.gameObject.SetActive(false);
		}
		UILabel uILabel = GameUITools.FindUILabel("num", base.gameObject);
		if (!showValue)
		{
			uILabel.gameObject.SetActive(false);
		}
		else
		{
			uILabel.text = this.count.ToString();
		}
		if (showTips)
		{
			UIEventListener expr_F3 = UIEventListener.Get(base.gameObject);
			expr_F3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_F3.onClick, new UIEventListener.VoidDelegate(this.OnRewardClick));
		}
	}

	private void OnRewardClick(GameObject go)
	{
		GameUIManager.mInstance.ShowItemInfo(this.itemInfo);
	}
}
