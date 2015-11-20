using Att;
using System;
using UnityEngine;

public class RewardLopetItem : MonoBehaviour
{
	private ItemInfo itemInfo;

	private LopetInfo lopetInfo;

	private int count;

	public void Init(ItemInfo info, int num, bool showValue, bool showTips)
	{
		this.itemInfo = info;
		this.count = num;
		this.lopetInfo = Globals.Instance.AttDB.LopetDict.GetInfo(this.itemInfo.Value2);
		if (this.lopetInfo == null)
		{
			global::Debug.LogError(new object[]
			{
				string.Format("LopetDict.GetInfo, ID = {0}", this.itemInfo.Value2)
			});
			base.gameObject.SetActive(false);
			return;
		}
		UISprite component = base.GetComponent<UISprite>();
		component.spriteName = Tools.GetItemQualityIcon(this.lopetInfo.Quality);
		UISprite uISprite = GameUITools.FindUISprite("icon", base.gameObject);
		uISprite.spriteName = this.lopetInfo.Icon;
		UILabel uILabel = GameUITools.FindUILabel("num", base.gameObject);
		if (showValue)
		{
			uILabel.text = this.count.ToString();
		}
		else
		{
			uILabel.gameObject.SetActive(false);
		}
		if (showTips)
		{
			UIEventListener expr_FD = UIEventListener.Get(base.gameObject);
			expr_FD.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_FD.onClick, new UIEventListener.VoidDelegate(this.OnRewardClick));
		}
	}

	private void OnRewardClick(GameObject go)
	{
		GameUIManager.mInstance.ShowItemInfo(this.itemInfo);
	}
}
