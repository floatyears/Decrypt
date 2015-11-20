using Att;
using System;
using UnityEngine;

public class QuestRewardPetItem : MonoBehaviour
{
	private ItemInfo itemInfo;

	private PetInfo petInfo;

	public static QuestRewardPetItem CreateReward(ItemInfo info, int num)
	{
		GameObject gameObject = Tools.InstantiateGUIPrefab("GUI/QuestRewardPetItem");
		if (gameObject != null)
		{
			QuestRewardPetItem questRewardPetItem = gameObject.AddComponent<QuestRewardPetItem>();
			if (questRewardPetItem != null)
			{
				questRewardPetItem.Init(info, num);
				return questRewardPetItem;
			}
		}
		return null;
	}

	private void Init(ItemInfo info, int num)
	{
		if (info.Type != 3 || info.SubType != 0)
		{
			global::Debug.LogErrorFormat("Use reward type error {0}", new object[]
			{
				(EItemType)info.Type
			});
			base.gameObject.SetActive(false);
			return;
		}
		this.itemInfo = info;
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
		UISprite uISprite = GameUITools.FindUISprite("Quality", base.gameObject);
		uISprite.spriteName = Tools.GetItemQualityIcon(this.petInfo.Quality);
		UISprite component = base.GetComponent<UISprite>();
		component.spriteName = this.petInfo.Icon;
		UILabel uILabel = GameUITools.FindUILabel("num", base.gameObject);
		uILabel.text = string.Format("{0}{1}[-] [FFFFFF]x{2}", Tools.GetItemQualityColorHex(this.petInfo.Quality), this.itemInfo.Name, num);
	}
}
