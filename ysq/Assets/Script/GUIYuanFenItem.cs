using Att;
using System;
using UnityEngine;

public class GUIYuanFenItem : MonoBehaviour
{
	private GUITeamManageSceneV2 mBaseScene;

	private RelationInfo mRelationInfoData;

	private UILabel mName;

	private UILabel mDesc;

	private GUIYuanFenItemIcon[] mYuanFenPets = new GUIYuanFenItemIcon[3];

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

	public void InitWithBaseScene(GUITeamManageSceneV2 baseScene)
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
			this.mYuanFenPets[i] = base.transform.Find(string.Format("pet{0}", i)).gameObject.AddComponent<GUIYuanFenItemIcon>();
			this.mYuanFenPets[i].InitWithBaseScene(this.mBaseScene);
		}
	}

	public void Refresh(RelationInfo ri, bool isActive)
	{
		this.mRelationInfoData = ri;
		if (this.mRelationInfoData != null)
		{
			this.mName.text = string.Format("{0}{1}[-]", (!isActive) ? "[b2b2b2]" : "[00ff00]", this.mRelationInfoData.Name);
			this.mDesc.text = string.Format("{0}{1}[-]", (!isActive) ? "[b2b2b2]" : "[00ff00]", this.mRelationInfoData.Desc);
			int i = 0;
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
			while (i < 3)
			{
				this.mYuanFenPets[i].IsVisible = false;
				i++;
			}
		}
	}
}
