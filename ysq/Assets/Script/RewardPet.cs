using Att;
using System;
using UnityEngine;

public class RewardPet : MonoBehaviour
{
	private PetInfo petInfo;

	public void Init(int petInfoID, bool showTips)
	{
		this.petInfo = Globals.Instance.AttDB.PetDict.GetInfo(petInfoID);
		if (this.petInfo == null)
		{
			global::Debug.LogError(new object[]
			{
				string.Format("PetDict.GetInfo, ID = {0}", petInfoID)
			});
			return;
		}
		UISprite component = base.GetComponent<UISprite>();
		component.spriteName = Tools.GetItemQualityIcon(this.petInfo.Quality);
		UISprite uISprite = GameUITools.FindUISprite("itemIcon", base.gameObject);
		uISprite.spriteName = this.petInfo.Icon;
		if (showTips)
		{
			UIEventListener expr_95 = UIEventListener.Get(base.gameObject);
			expr_95.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_95.onClick, new UIEventListener.VoidDelegate(this.OnRewardClick));
		}
	}

	private void OnRewardClick(GameObject go)
	{
		GameUIManager.mInstance.ShowPetInfo(this.petInfo);
	}
}
