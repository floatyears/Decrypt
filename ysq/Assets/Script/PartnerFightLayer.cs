using System;
using UnityEngine;

public class PartnerFightLayer : MonoBehaviour
{
	private GUIPartnerFightScene mBaseScene;

	public PartnerFightTable mContentsTable;

	private PartnerFightBagItem mPartnerFightBagItem;

	private bool ShowPet;

	public void InitWithBaseScene(GUIPartnerFightScene baseScene)
	{
		this.mBaseScene = baseScene;
		this.CreateObjects();
		this.InitInventoryItems();
	}

	private void CreateObjects()
	{
		this.mContentsTable = GameUITools.FindGameObject("bagContents", base.gameObject).AddComponent<PartnerFightTable>();
		this.mContentsTable.maxPerLine = 2;
		this.mContentsTable.arrangement = UICustomGrid.Arrangement.Vertical;
		this.mContentsTable.cellWidth = 450f;
		this.mContentsTable.cellHeight = 135f;
		this.mContentsTable.gapHeight = 2f;
		this.mContentsTable.gapWidth = 2f;
		this.mContentsTable.InitWithBaseScene(this.mBaseScene);
	}

	public void IsShowFightOrHide(bool showPet)
	{
		if (this.ShowPet == showPet || this.mBaseScene.mPetData == null)
		{
			return;
		}
		this.ShowPet = showPet;
		if (this.ShowPet)
		{
			this.mContentsTable.AddData(this.mBaseScene.mPetData);
		}
		else
		{
			this.mContentsTable.RemoveData(this.mBaseScene.mPetData.GetID());
		}
		this.mContentsTable.repositionNow = true;
	}

	public void InitInventoryItems()
	{
		TeamSubSystem teamSystem = Globals.Instance.Player.TeamSystem;
		if (this.mBaseScene.mSlot == -1)
		{
			foreach (PetDataEx current in Globals.Instance.Player.PetSystem.Values)
			{
				if (!current.IsBattling() && !current.IsPetAssisting())
				{
					if (current.IsOld())
					{
						this.mContentsTable.AddData(current);
					}
					else
					{
						int num = 0;
						while (num < 4 && num < current.Info.SkillID.Count)
						{
							if (current.Data.SkillLevel != 0u)
							{
								this.mContentsTable.AddData(current);
							}
							num++;
						}
					}
				}
			}
		}
		else
		{
			teamSystem.UpdateToActiveRelation(this.mBaseScene.mSlot);
			foreach (PetDataEx current2 in Globals.Instance.Player.PetSystem.Values)
			{
				if (current2.GetSocketSlot() >= 0)
				{
					if (!this.ShowPet || current2 != this.mBaseScene.mPetData)
					{
						continue;
					}
				}
				else if (this.mBaseScene.mPetData != null)
				{
					if (current2.Info.ID != this.mBaseScene.mPetData.Info.ID && teamSystem.HasPetInfoID(current2.Info.ID))
					{
						continue;
					}
				}
				else if (teamSystem.HasPetInfoID(current2.Info.ID))
				{
					continue;
				}
				this.mContentsTable.AddData(current2);
			}
		}
		this.mContentsTable.repositionNow = true;
	}
}
