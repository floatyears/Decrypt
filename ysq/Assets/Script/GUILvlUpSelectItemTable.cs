using System;
using System.Collections.Generic;
using UnityEngine;

public class GUILvlUpSelectItemTable : UICustomGrid
{
	private GUILvlUpSelPetSceneV2 mBaseScene;

	public void InitWithBaseScene(GUILvlUpSelPetSceneV2 baseScene)
	{
		this.mBaseScene = baseScene;
	}

	protected override UICustomGridItem CreateGridItem()
	{
		UnityEngine.Object @object = Res.LoadGUI("GUI/lvlUpSelectItem");
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(@object);
		gameObject.name = @object.name;
		gameObject.transform.parent = base.gameObject.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		GUILvlUpSelectItem gUILvlUpSelectItem = gameObject.AddComponent<GUILvlUpSelectItem>();
		gUILvlUpSelectItem.InitWithBaseScene(this.mBaseScene);
		return gUILvlUpSelectItem;
	}

	private int SortByPetDatas(BaseData a, BaseData b)
	{
		PetDataEx mPetDataEx = ((GUILvlUpSelectItemData)a).mPetDataEx;
		PetDataEx mPetDataEx2 = ((GUILvlUpSelectItemData)b).mPetDataEx;
		if (mPetDataEx == null || mPetDataEx2 == null)
		{
			return 0;
		}
		if (mPetDataEx.Info.Quality > mPetDataEx2.Info.Quality)
		{
			return 1;
		}
		if (mPetDataEx.Info.Quality < mPetDataEx2.Info.Quality)
		{
			return -1;
		}
		if (mPetDataEx.Info.SubQuality > mPetDataEx2.Info.SubQuality)
		{
			return 1;
		}
		if (mPetDataEx.Info.SubQuality < mPetDataEx2.Info.SubQuality)
		{
			return -1;
		}
		if (mPetDataEx.Data.Level > mPetDataEx2.Data.Level)
		{
			return 1;
		}
		if (mPetDataEx.Data.Level < mPetDataEx2.Data.Level)
		{
			return -1;
		}
		return mPetDataEx.Info.ID - mPetDataEx2.Info.ID;
	}

	public static int SortByPetDatas2(PetDataEx aP, PetDataEx bP)
	{
		if (aP == null || bP == null)
		{
			return 0;
		}
		if (aP.Info.Quality > bP.Info.Quality)
		{
			return 1;
		}
		if (aP.Info.Quality < bP.Info.Quality)
		{
			return -1;
		}
		if (aP.Info.SubQuality > bP.Info.SubQuality)
		{
			return 1;
		}
		if (aP.Info.SubQuality < bP.Info.SubQuality)
		{
			return -1;
		}
		if (aP.Data.Level > bP.Data.Level)
		{
			return 1;
		}
		if (aP.Data.Level < bP.Data.Level)
		{
			return -1;
		}
		return aP.Info.ID - bP.Info.ID;
	}

	protected override void Sort(List<BaseData> list)
	{
		list.Sort(new Comparison<BaseData>(this.SortByPetDatas));
	}

	public int GetCanGetExpNum()
	{
		uint num = 0u;
		for (int i = 0; i < this.mDatas.Count; i++)
		{
			GUILvlUpSelectItemData gUILvlUpSelectItemData = (GUILvlUpSelectItemData)this.mDatas[i];
			if (gUILvlUpSelectItemData != null && gUILvlUpSelectItemData.mIsSelected)
			{
				num += gUILvlUpSelectItemData.CanGetExpNum();
			}
		}
		return (int)num;
	}

	public bool IsSelectPetsEnough()
	{
		int num = 0;
		for (int i = 0; i < this.mDatas.Count; i++)
		{
			GUILvlUpSelectItemData gUILvlUpSelectItemData = (GUILvlUpSelectItemData)this.mDatas[i];
			if (gUILvlUpSelectItemData != null && gUILvlUpSelectItemData.mIsSelected)
			{
				num++;
			}
		}
		return num >= 5;
	}

	public List<PetDataEx> GetSelectPets()
	{
		List<PetDataEx> list = new List<PetDataEx>();
		for (int i = 0; i < this.mDatas.Count; i++)
		{
			GUILvlUpSelectItemData gUILvlUpSelectItemData = (GUILvlUpSelectItemData)this.mDatas[i];
			if (gUILvlUpSelectItemData != null && gUILvlUpSelectItemData.mIsSelected)
			{
				list.Add(gUILvlUpSelectItemData.mPetDataEx);
			}
		}
		return list;
	}
}
