using Att;
using System;
using System.Text;

public sealed class CollectionContent : CollectionContentBase
{
	private GUISummonCollectionScene mBaseScene;

	private CollectionSummonItem[] mCollectionContent = new CollectionSummonItem[8];

	private PetInfo[] mCollectionPetInfos = new PetInfo[8];

	private StringBuilder mStringBuilder = new StringBuilder();

	public void InitWithBaseScene(GUISummonCollectionScene baseScene, EElementType et, PetInfo[] petInfo)
	{
		this.mBaseScene = baseScene;
		this.mElementType = et;
		for (int i = 0; i < 8; i++)
		{
			if (petInfo.Length > i)
			{
				this.mCollectionPetInfos[i] = petInfo[i];
			}
			else
			{
				this.mCollectionPetInfos[i] = null;
			}
		}
		this.mIsTitle = false;
		this.CreateObjects();
		this.Refresh();
	}

	private void CreateObjects()
	{
		for (int i = 0; i < 8; i++)
		{
			this.mStringBuilder.Remove(0, this.mStringBuilder.Length);
			this.mStringBuilder.Append("PetItem").Append(i);
			this.mCollectionContent[i] = base.transform.Find(this.mStringBuilder.ToString()).gameObject.AddComponent<CollectionSummonItem>();
			this.mCollectionContent[i].InitItem(this.mBaseScene, this.mCollectionPetInfos[i], false);
		}
	}

	private void SetItemsVisible(bool isShow)
	{
		for (int i = 0; i < 8; i++)
		{
			this.mCollectionContent[i].SetItemVisible(isShow);
		}
	}

	private void Refresh()
	{
	}

	public void Refresh(PetDataEx petData)
	{
		if (petData != null)
		{
			for (int i = 0; i < 8; i++)
			{
				if (this.mCollectionPetInfos[i] != null && this.mCollectionPetInfos[i].ID == petData.Info.ID)
				{
					this.mCollectionContent[i].Refresh(petData.Info);
				}
			}
		}
	}
}
