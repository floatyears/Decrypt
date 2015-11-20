using System;
using UnityEngine;

public class SelectLopetLayer : MonoBehaviour
{
	private GUISelectLopetBagScene mBaseScene;

	public SelectLopetBagUITable mContentsTable;

	private bool IsInit = true;

	private bool ShowEquiped;

	public void InitWithBaseScene(GUISelectLopetBagScene baseScene)
	{
		this.mBaseScene = baseScene;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mContentsTable = GameUITools.FindGameObject("Contents", base.gameObject).AddComponent<SelectLopetBagUITable>();
		this.mContentsTable.maxPerLine = 2;
		this.mContentsTable.arrangement = UICustomGrid.Arrangement.Vertical;
		this.mContentsTable.cellWidth = 442f;
		this.mContentsTable.cellHeight = 130f;
		this.mContentsTable.gapHeight = 8f;
		this.mContentsTable.gapWidth = 8f;
		this.mContentsTable.InitWithBaseScene(this.mBaseScene, "GUISelectLopetBagItem");
	}

	public void Refresh()
	{
		if (this.IsInit)
		{
			this.IsInit = false;
			this.InitBagItems();
		}
	}

	public void ReInit(bool filter)
	{
		this.ShowEquiped = !filter;
		this.InitBagItems();
	}

	private void InitBagItems()
	{
		this.mContentsTable.SetDragAmount(0f, 0f);
		this.mContentsTable.ClearData();
		LopetDataEx curLopet = Globals.Instance.Player.LopetSystem.GetCurLopet(true);
		foreach (LopetDataEx current in Globals.Instance.Player.LopetSystem.Values)
		{
			if (curLopet == null || this.ShowEquiped || curLopet.GetID() != current.GetID())
			{
				current.ClearUIData();
				this.mContentsTable.AddData(current);
			}
		}
		this.mContentsTable.ConstraintSort();
	}
}
