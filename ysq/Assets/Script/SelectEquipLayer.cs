using System;
using System.Collections.Generic;
using UnityEngine;

public class SelectEquipLayer : MonoBehaviour
{
	private GUISelectEquipBagScene mBaseScene;

	private int mSocketSlot;

	private int mEquipSlot;

	public SelectEquipBagUITable mContentsTable;

	private bool IsInit = true;

	private bool ShowEquiped;

	public void InitWithBaseScene(GUISelectEquipBagScene baseScene, int socketSlot, int equipSlot)
	{
		this.mBaseScene = baseScene;
		this.mSocketSlot = socketSlot;
		this.mEquipSlot = equipSlot;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mContentsTable = GameUITools.FindGameObject("Contents", base.gameObject).AddComponent<SelectEquipBagUITable>();
		this.mContentsTable.maxPerLine = 2;
		this.mContentsTable.arrangement = UICustomGrid.Arrangement.Vertical;
		this.mContentsTable.cellWidth = 442f;
		this.mContentsTable.cellHeight = 130f;
		this.mContentsTable.gapHeight = 8f;
		this.mContentsTable.gapWidth = 8f;
		this.mContentsTable.InitWithBaseScene(this.mBaseScene, "GUISelectEquipBagItem");
	}

	public void Refresh()
	{
		if (this.IsInit)
		{
			this.IsInit = false;
			this.InitSelectEquipBagItems();
		}
	}

	public void ReInit(bool filter)
	{
		this.ShowEquiped = !filter;
		this.InitSelectEquipBagItems();
	}

	private void InitSelectEquipBagItems()
	{
		SocketDataEx socket = Globals.Instance.Player.TeamSystem.GetSocket(this.mSocketSlot);
		if (socket == null)
		{
			return;
		}
		List<int> relationEquip = socket.GetRelationEquip(this.mEquipSlot);
		this.mContentsTable.SetDragAmount(0f, 0f);
		this.mContentsTable.ClearData();
		List<ItemDataEx> allEquipTrinketBySlot = Globals.Instance.Player.ItemSystem.GetAllEquipTrinketBySlot(this.mEquipSlot, this.ShowEquiped);
		for (int i = 0; i < allEquipTrinketBySlot.Count; i++)
		{
			allEquipTrinketBySlot[i].ClearUIData();
			if (allEquipTrinketBySlot[i].CanActiveRelation(relationEquip))
			{
				allEquipTrinketBySlot[i].RelationCount = 1;
			}
			this.mContentsTable.AddData(allEquipTrinketBySlot[i]);
		}
		this.mContentsTable.ConstraintSort();
	}
}
