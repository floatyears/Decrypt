using Att;
using System;
using UnityEngine;

public class RewardPetItem : MonoBehaviour
{
	private ItemInfo itemInfo;

	private PetInfo petInfo;

	private int count;

	public void Init(ItemInfo info, int num, bool showValue, bool showTips)
	{
		this.itemInfo = info;
		this.count = num;
		this.petInfo = Globals.Instance.AttDB.PetDict.GetInfo(this.itemInfo.Value2);
		if (this.petInfo == null)
		{
			global::Debug.LogError(new object[]
			{
				string.Format("PetDict.GetInfo, ID = {0}", this.itemInfo.Value2)
			});
			base.gameObject.SetActive(false);
			return;
		}
		UISprite component = base.GetComponent<UISprite>();
		component.spriteName = Tools.GetItemQualityIcon(this.petInfo.Quality);
		UISprite uISprite = GameUITools.FindUISprite("icon", base.gameObject);
		uISprite.spriteName = this.petInfo.Icon;
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
