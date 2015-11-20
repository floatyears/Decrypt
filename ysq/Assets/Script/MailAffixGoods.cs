using Att;
using System;
using UnityEngine;

public class MailAffixGoods : MailContentElementBase
{
	public Transform[] mMailAffixGoods = new Transform[4];

	public GameObject[] mReward = new GameObject[4];

	public void InitWithBaseScene(GUIMailScene baseScene)
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		for (int i = 0; i < 4; i++)
		{
			this.mMailAffixGoods[i] = base.transform.Find(string.Format("bag{0}", i));
			this.mMailAffixGoods[i].gameObject.SetActive(false);
		}
	}

	public int GetAffixEmptyIndex()
	{
		for (int i = 0; i < 4; i++)
		{
			if (this.mReward[i] == null && !this.mMailAffixGoods[i].gameObject.activeSelf)
			{
				return i;
			}
		}
		return -1;
	}

	public void AddAffixItem(ItemInfo itemInfo, int itemNum)
	{
		int affixEmptyIndex = this.GetAffixEmptyIndex();
		if (affixEmptyIndex != -1)
		{
			this.mReward[affixEmptyIndex] = GameUITools.CreateReward(3, itemInfo.ID, itemNum, this.mMailAffixGoods[affixEmptyIndex], true, true, 36f, -7f, -2000f, 20f, 13f, 7f, 0);
			if (this.mReward[affixEmptyIndex] != null)
			{
				this.mReward[affixEmptyIndex].AddComponent<UIDragScrollView>();
			}
			this.mMailAffixGoods[affixEmptyIndex].gameObject.SetActive(true);
		}
	}

	public void AddAffixPet(PetInfo petInfo)
	{
		int affixEmptyIndex = this.GetAffixEmptyIndex();
		if (affixEmptyIndex != -1)
		{
			this.mReward[affixEmptyIndex] = GameUITools.CreateReward(4, petInfo.ID, 1, this.mMailAffixGoods[affixEmptyIndex], false, true, 36f, -7f, -2000f, 20f, 13f, 7f, 0);
			if (this.mReward[affixEmptyIndex] != null)
			{
				this.mReward[affixEmptyIndex].AddComponent<UIDragScrollView>();
			}
			this.mMailAffixGoods[affixEmptyIndex].gameObject.SetActive(true);
		}
	}

	public void AddAffixLopet(LopetInfo petInfo)
	{
		int affixEmptyIndex = this.GetAffixEmptyIndex();
		if (affixEmptyIndex != -1)
		{
			this.mReward[affixEmptyIndex] = GameUITools.CreateReward(16, petInfo.ID, 1, this.mMailAffixGoods[affixEmptyIndex], false, true, 36f, -7f, -2000f, 20f, 13f, 7f, 0);
			if (this.mReward[affixEmptyIndex] != null)
			{
				this.mReward[affixEmptyIndex].AddComponent<UIDragScrollView>();
			}
			this.mMailAffixGoods[affixEmptyIndex].gameObject.SetActive(true);
		}
	}

	public void AddAffixFashion(FashionInfo fashionInfo)
	{
		int affixEmptyIndex = this.GetAffixEmptyIndex();
		if (affixEmptyIndex != -1)
		{
			this.mReward[affixEmptyIndex] = GameUITools.CreateReward(12, fashionInfo.ID, 1, this.mMailAffixGoods[affixEmptyIndex], false, true, 36f, -7f, -2000f, 20f, 13f, 7f, 0);
			if (this.mReward[affixEmptyIndex] != null)
			{
				this.mReward[affixEmptyIndex].AddComponent<UIDragScrollView>();
			}
			this.mMailAffixGoods[affixEmptyIndex].gameObject.SetActive(true);
		}
	}
}
