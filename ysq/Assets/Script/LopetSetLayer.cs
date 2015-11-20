using System;
using UnityEngine;

public class LopetSetLayer : MonoBehaviour
{
	private LopetSetBagUITable mContent;

	private bool IsInit = true;

	public void Init(GUILopetBagScene basescene)
	{
	}

	private void CreateObjects()
	{
		this.mContent = GameUITools.FindGameObject("Contents", base.gameObject).AddComponent<LopetSetBagUITable>();
		this.mContent.maxPerLine = 2;
		this.mContent.arrangement = UICustomGrid.Arrangement.Vertical;
		this.mContent.cellWidth = 442f;
		this.mContent.cellHeight = 130f;
		this.mContent.gapHeight = 8f;
		this.mContent.gapWidth = 8f;
		this.mContent.focusID = GameUIManager.mInstance.uiState.SelectItemID;
		GameUIManager.mInstance.uiState.SelectItemID = 0uL;
	}

	public void Refresh()
	{
		if (this.IsInit)
		{
			this.CreateObjects();
			this.IsInit = false;
			this.InitBagItems();
		}
	}

	private void InitBagItems()
	{
		this.mContent.ClearData();
	}
}
