using Att;
using System;
using UnityEngine;

public class GUIPetInfoSceneYFItem : MonoBehaviour
{
	private GUIPetInfoSceneV2 mBaseScene;

	private RelationInfo mRelationInfoData;

	private UILabel mName;

	private UILabel mDesc;

	private GUIPetInfoYuanFenItemIcon[] mYuanFenPets = new GUIPetInfoYuanFenItemIcon[3];

	private bool mIsVisible;

	public bool IsVisible
	{
		get
		{
			return this.mIsVisible;
		}
		set
		{
			this.mIsVisible = value;
			base.gameObject.SetActive(this.mIsVisible);
		}
	}

	public void InitWithBaseScene(GUIPetInfoSceneV2 baseScene)
	{
		this.mBaseScene = baseScene;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mName = base.transform.Find("name").GetComponent<UILabel>();
		this.mDesc = base.transform.Find("desc").GetComponent<UILabel>();
		for (int i = 0; i < 3; i++)
		{
			this.mYuanFenPets[i] = base.transform.Find(string.Format("pet{0}", i)).gameObject.AddComponent<GUIPetInfoYuanFenItemIcon>();
			this.mYuanFenPets[i].InitWithBaseScene(this.mBaseScene);
		}
		base.gameObject.AddComponent<UIDragScrollView>();
	}

	public void Refresh(RelationInfo ri, bool isActive)
	{
		this.mRelationInfoData = ri;
		if (this.mRelationInfoData != null)
		{
			if (isActive)
			{
				this.mName.color = Color.green;
				this.mDesc.color = Color.green;
			}
			else
			{
				this.mName.color = new Color32(178, 178, 178, 255);
				this.mDesc.color = this.mName.color;
			}
			this.mName.text = this.mRelationInfoData.Name;
			this.mDesc.text = this.mRelationInfoData.Desc;
			int i = 0;
			if (this.mRelationInfoData.Type == 0)
			{
				int num = 0;
				while (num < this.mRelationInfoData.PetID.Count && num < 3)
				{
					int id = this.mRelationInfoData.PetID[num];
					PetInfo info = Globals.Instance.AttDB.PetDict.GetInfo(id);
					if (info != null)
					{
						this.mYuanFenPets[i].IsVisible = true;
						this.mYuanFenPets[i].Refresh(info, isActive);
						i++;
					}
					num++;
				}
			}
			else if (this.mRelationInfoData.Type == 1)
			{
				int itemID = this.mRelationInfoData.ItemID;
				ItemInfo info2 = Globals.Instance.AttDB.ItemDict.GetInfo(itemID);
				if (info2 != null)
				{
					this.mYuanFenPets[i].IsVisible = true;
					this.mYuanFenPets[i].Refresh(info2, isActive);
					i++;
				}
			}
			while (i < 3)
			{
				this.mYuanFenPets[i].IsVisible = false;
				i++;
			}
		}
	}
}
