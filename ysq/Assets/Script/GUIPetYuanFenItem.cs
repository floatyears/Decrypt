using Att;
using System;
using UnityEngine;

public class GUIPetYuanFenItem : MonoBehaviour
{
	private GUITeamManageSceneV2 mBaseScene;

	private SocketDataEx mPetDataEx;

	private UILabel mPetName;

	private GUIYuanFenItem[] mYuanFenPets = new GUIYuanFenItem[3];

	public void InitWithBaseScene(GUITeamManageSceneV2 baseScene, SocketDataEx pdEx)
	{
		this.mBaseScene = baseScene;
		this.mPetDataEx = pdEx;
		this.CreateObjects();
		this.Refresh();
	}

	private void CreateObjects()
	{
		this.mPetName = base.transform.Find("petName").GetComponent<UILabel>();
		for (int i = 0; i < 3; i++)
		{
			this.mYuanFenPets[i] = base.transform.Find(string.Format("item{0}", i)).gameObject.AddComponent<GUIYuanFenItem>();
			this.mYuanFenPets[i].InitWithBaseScene(this.mBaseScene);
		}
	}

	private void Refresh()
	{
		if (this.mPetDataEx != null)
		{
			PetDataEx pet = this.mPetDataEx.GetPet();
			if (pet != null)
			{
				this.mPetName.text = Tools.GetPetName(pet.Info);
				int i = 0;
				for (int j = 0; j < 3; j++)
				{
					RelationInfo info = Globals.Instance.AttDB.RelationDict.GetInfo(pet.Info.RelationID[j]);
					if (info != null)
					{
						this.mYuanFenPets[i].IsVisible = true;
						this.mYuanFenPets[i].Refresh(info, this.mPetDataEx.IsRelationActive(info));
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
}
