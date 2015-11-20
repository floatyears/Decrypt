using Att;
using System;
using UnityEngine;

public class RewardLopet : MonoBehaviour
{
	private LopetInfo lopetInfo;

	public void Init(int lopetInfoID, bool showTips)
	{
		this.lopetInfo = Globals.Instance.AttDB.LopetDict.GetInfo(lopetInfoID);
		if (this.lopetInfo == null)
		{
			global::Debug.LogError(new object[]
			{
				string.Format("LopetDict.GetInfo, ID = {0}", lopetInfoID)
			});
			return;
		}
		UISprite component = base.GetComponent<UISprite>();
		component.spriteName = Tools.GetItemQualityIcon(this.lopetInfo.Quality);
		UISprite uISprite = GameUITools.FindUISprite("itemIcon", base.gameObject);
		uISprite.spriteName = this.lopetInfo.Icon;
		if (showTips)
		{
			UIEventListener expr_95 = UIEventListener.Get(base.gameObject);
			expr_95.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_95.onClick, new UIEventListener.VoidDelegate(this.OnRewardClick));
		}
	}

	private void OnRewardClick(GameObject go)
	{
		GameUIManager.mInstance.ShowLopetInfo(this.lopetInfo);
	}
}
