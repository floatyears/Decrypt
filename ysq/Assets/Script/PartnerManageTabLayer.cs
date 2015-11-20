using System;
using UnityEngine;

public class PartnerManageTabLayer : MonoBehaviour
{
	public GUIPartnerManageScene mBaseScene;

	public PartnerManageInventoryTable mInventoryTable;

	public void InitWithBaseScene(GUIPartnerManageScene PartnerManagementScene)
	{
		this.mBaseScene = PartnerManagementScene;
		this.CreateObjects();
		this.InitInventoryItems();
	}

	private void CreateObjects()
	{
		this.mInventoryTable = base.transform.FindChild("bagPanel/bagContents").gameObject.AddComponent<PartnerManageInventoryTable>();
		this.mInventoryTable.maxPerLine = 2;
		this.mInventoryTable.arrangement = UICustomGrid.Arrangement.Vertical;
		this.mInventoryTable.cellWidth = 450f;
		this.mInventoryTable.cellHeight = 135f;
		this.mInventoryTable.gapHeight = 2f;
		this.mInventoryTable.gapWidth = 2f;
		this.mInventoryTable.InitWithBaseScene(this.mBaseScene);
		this.mInventoryTable.focusID = GameUIManager.mInstance.uiState.SelectPetID;
		GameUIManager.mInstance.uiState.SelectPetID = 0uL;
	}

	public void InitInventoryItems()
	{
		foreach (PetDataEx current in Globals.Instance.Player.PetSystem.Values)
		{
			if (!GameUIManager.mInstance.uiState.CachePetBag)
			{
				current.State = 0;
			}
			if ((GameUIManager.mInstance.uiState.PropsBagSceneToTrainIndex != 1 && GameUIManager.mInstance.uiState.PropsBagSceneToTrainIndex != 3) || current.Data.ID != 100uL)
			{
				this.mInventoryTable.AddData(current);
			}
		}
		this.mInventoryTable.repositionNow = true;
		GameUIManager.mInstance.uiState.CachePetBag = true;
	}

	public void AddPetItem(PetDataEx data)
	{
		data.State = 0;
		this.mInventoryTable.AddData(data);
	}

	public void RemovePetItem(ulong id)
	{
		this.mInventoryTable.RemoveData(id);
	}
}
