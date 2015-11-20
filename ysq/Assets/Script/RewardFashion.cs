using Att;
using System;
using UnityEngine;

public class RewardFashion : MonoBehaviour
{
	private FashionInfo fashionInfo;

	public void Init(int fashionInfoID, bool showTips)
	{
		this.fashionInfo = Globals.Instance.AttDB.FashionDict.GetInfo(fashionInfoID);
		if (this.fashionInfo == null)
		{
			global::Debug.LogErrorFormat("FashionDict.GetInfo, ID = {0}", new object[]
			{
				fashionInfoID
			});
			return;
		}
		UISprite component = base.GetComponent<UISprite>();
		component.spriteName = Tools.GetItemQualityIcon(this.fashionInfo.Quality);
		UISprite uISprite = GameUITools.FindUISprite("itemIcon", base.gameObject);
		uISprite.spriteName = this.fashionInfo.Icon;
		if (showTips)
		{
			UIEventListener expr_90 = UIEventListener.Get(base.gameObject);
			expr_90.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_90.onClick, new UIEventListener.VoidDelegate(this.OnRewardClick));
		}
	}

	private void OnRewardClick(GameObject go)
	{
		GUIHowGetPetItemPopUp.ShowThis(this.fashionInfo);
	}
}
