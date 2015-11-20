using Proto;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SelectItemLayer : MonoBehaviour
{
	private GUISelectItemBagScene mBaseScene;

	private CommonBagUITable mContentsTable;

	private UILabel mCountName;

	private UILabel mCountValue;

	private UILabel mNeed;

	private int count;

	private int maxCount;

	private GameObject mOKBtn;

	public List<ItemDataEx> mCurSelectItems = new List<ItemDataEx>();

	public void InitWithBaseScene(GUISelectItemBagScene baseScene)
	{
		this.mBaseScene = baseScene;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mCountName = GameUITools.FindUILabel("Count", base.gameObject);
		this.mCountValue = GameUITools.FindUILabel("Value", this.mCountName.gameObject);
		this.mNeed = GameUITools.FindUILabel("Need/Value", base.gameObject);
		this.mOKBtn = GameUITools.RegisterClickEvent("OKBtn", new UIEventListener.VoidDelegate(this.OnOKBtnClick), base.gameObject);
	}

	private void OnOKBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GUISelectItemBagScene.ESelectItemSceneType curType = this.mBaseScene.curType;
		if (curType != GUISelectItemBagScene.ESelectItemSceneType.ESIST_TrinketEnhance)
		{
			if (curType == GUISelectItemBagScene.ESelectItemSceneType.ESIST_EquipBreak)
			{
				MC2S_EquipBreakUp mC2S_EquipBreakUp = new MC2S_EquipBreakUp();
				foreach (ItemDataEx current in this.mCurSelectItems)
				{
					mC2S_EquipBreakUp.EquipID.Add(current.GetID());
					current.ClearUIData();
				}
				GameUIManager.mInstance.uiState.EquipBreakUpData = mC2S_EquipBreakUp;
				mC2S_EquipBreakUp = null;
				GUIRecycleScene.Change2This(GUIRecycleScene.ERecycleT.ERecycleT_EquipBreak);
			}
		}
		else
		{
			MC2S_TrinketEnhance mC2S_TrinketEnhance = new MC2S_TrinketEnhance();
			mC2S_TrinketEnhance.TrinketID = this.mBaseScene.selfData.GetID();
			foreach (ItemDataEx current2 in this.mCurSelectItems)
			{
				mC2S_TrinketEnhance.ItemID.Add(current2.GetID());
				current2.ClearUIData();
			}
			GameUIManager.mInstance.uiState.TrinketEnhanceData = mC2S_TrinketEnhance;
			mC2S_TrinketEnhance = null;
			GUITrinketUpgradeScene.Change2This(this.mBaseScene.selfData, GUITrinketUpgradeScene.EUpgradeType.EUT_Enhance, -2);
		}
	}

	private void InitTEItems(ItemDataEx exculde = null)
	{
		this.mContentsTable.ClearData();
		MC2S_TrinketEnhance trinketEnhanceData = GameUIManager.mInstance.uiState.TrinketEnhanceData;
		foreach (ItemDataEx current in Globals.Instance.Player.ItemSystem.Values)
		{
			if ((current.Info.Type == 4 && current.Info.SubType == 9) || (current.Info.Type == 1 && current.GetTrinketRefineLevel() <= 0 && !current.IsEquiped()))
			{
				if (exculde == null || current.GetID() != exculde.GetID())
				{
					current.IsSelected = false;
					if (trinketEnhanceData != null && trinketEnhanceData.ItemID.Contains(current.GetID()))
					{
						current.IsSelected = true;
						this.mCurSelectItems.Add(current);
					}
					this.mContentsTable.AddData(current);
				}
			}
		}
	}

	private void InitEBItems()
	{
		this.mContentsTable.ClearData();
		MC2S_EquipBreakUp equipBreakUpData = GameUIManager.mInstance.uiState.EquipBreakUpData;
		foreach (ItemDataEx current in Globals.Instance.Player.ItemSystem.Values)
		{
			if (current.Info.Type == 0 && !current.IsEquiped())
			{
				if (equipBreakUpData != null && equipBreakUpData.EquipID.Contains(current.GetID()))
				{
					current.IsSelected = true;
					this.mCurSelectItems.Add(current);
				}
				this.mContentsTable.AddData(current);
			}
		}
	}

	private void InitTrinketRebornItems()
	{
		this.mContentsTable.ClearData();
		foreach (ItemDataEx current in Globals.Instance.Player.ItemSystem.Values)
		{
			if (current.IsTrinketAndCanReborn())
			{
				this.mContentsTable.AddData(current);
			}
		}
	}

	private void InitLopetBreakItems()
	{
		this.mContentsTable.ClearData();
		foreach (LopetDataEx current in Globals.Instance.Player.LopetSystem.Values)
		{
			if (!current.IsBattling())
			{
				this.mContentsTable.AddData(current);
			}
		}
	}

	private void InitLopetRebornItems()
	{
		this.mContentsTable.ClearData();
		foreach (LopetDataEx current in Globals.Instance.Player.LopetSystem.Values)
		{
			if (!current.IsBattling() && current.IsOld())
			{
				this.mContentsTable.AddData(current);
			}
		}
	}

	public bool CanAddItem(ItemDataEx data)
	{
		if (this.mCurSelectItems.Contains(data))
		{
			return false;
		}
		if (this.mCurSelectItems.Count >= this.maxCount)
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("equipImprove35", new object[]
			{
				this.maxCount
			}), 0f, 0f);
			return false;
		}
		return true;
	}

	public void AddItem(ItemDataEx data)
	{
		if (this.CanAddItem(data))
		{
			this.mCurSelectItems.Add(data);
			this.Refresh();
		}
	}

	public void DeleteItem(ItemDataEx data)
	{
		if (this.mCurSelectItems.Contains(data))
		{
			this.mCurSelectItems.Remove(data);
			this.Refresh();
		}
	}

	private void Refresh()
	{
		this.count = 0;
		if (this.mCurSelectItems != null)
		{
			GUISelectItemBagScene.ESelectItemSceneType curType = this.mBaseScene.curType;
			if (curType != GUISelectItemBagScene.ESelectItemSceneType.ESIST_TrinketEnhance)
			{
				if (curType == GUISelectItemBagScene.ESelectItemSceneType.ESIST_EquipBreak)
				{
					this.count = this.mCurSelectItems.Count;
				}
			}
			else
			{
				foreach (ItemDataEx current in this.mCurSelectItems)
				{
					this.count += current.GetTrinketOrItem2EnhanceExp();
				}
			}
		}
		this.mCountValue.text = this.count.ToString();
	}

	public void InitDatas(ItemDataEx self = null)
	{
		switch (this.mBaseScene.curType)
		{
		case GUISelectItemBagScene.ESelectItemSceneType.ESIST_TrinketEnhance:
			this.mContentsTable = GameUITools.FindGameObject("Panel/Contents", base.gameObject).AddComponent<TrinketEnhanceSelectItemBagUITable>();
			this.mContentsTable.InitWithBaseScene(this.mBaseScene, "GUITrinketEnhanceBagItem");
			this.maxCount = 5;
			this.mCountName.gameObject.SetActive(true);
			this.mCountName.text = Singleton<StringManager>.Instance.GetString("recycle20");
			this.mNeed.transform.parent.gameObject.SetActive(true);
			break;
		case GUISelectItemBagScene.ESelectItemSceneType.ESIST_EquipBreak:
			this.mContentsTable = GameUITools.FindGameObject("Panel/Contents", base.gameObject).AddComponent<EquipBreakSelectItemBagUITable>();
			this.mContentsTable.InitWithBaseScene(this.mBaseScene, "GUIEquipBreakBagItem");
			this.maxCount = 5;
			this.mCountName.gameObject.SetActive(true);
			this.mCountName.text = Singleton<StringManager>.Instance.GetString("recycle19");
			this.mNeed.transform.parent.gameObject.SetActive(false);
			break;
		case GUISelectItemBagScene.ESelectItemSceneType.ESIST_TrinketReborn:
			this.mContentsTable = GameUITools.FindGameObject("Panel/Contents", base.gameObject).AddComponent<TrinketRebornSelectItemBagUITable>();
			this.mContentsTable.InitWithBaseScene(this.mBaseScene, "GUITrinketRebornBagItem");
			this.mCountName.gameObject.SetActive(false);
			this.mNeed.transform.parent.gameObject.SetActive(false);
			this.mOKBtn.gameObject.SetActive(false);
			break;
		case GUISelectItemBagScene.ESelectItemSceneType.ESIST_LopetBreak:
			this.mContentsTable = GameUITools.FindGameObject("Panel/Contents", base.gameObject).AddComponent<LopetBreakSelectItemBagUITable>();
			this.mContentsTable.InitWithBaseScene(this.mBaseScene, "GUILopetBreakBagItem");
			this.mCountName.gameObject.SetActive(false);
			this.mNeed.transform.parent.gameObject.SetActive(false);
			this.mOKBtn.gameObject.SetActive(false);
			break;
		case GUISelectItemBagScene.ESelectItemSceneType.ESIST_LopetReborn:
			this.mContentsTable = GameUITools.FindGameObject("Panel/Contents", base.gameObject).AddComponent<LopetRebornSelectItemBagUITable>();
			this.mContentsTable.InitWithBaseScene(this.mBaseScene, "GUILopetRebornBagItem");
			this.mCountName.gameObject.SetActive(false);
			this.mNeed.transform.parent.gameObject.SetActive(false);
			this.mOKBtn.gameObject.SetActive(false);
			break;
		}
		this.mContentsTable.maxPerLine = 2;
		this.mContentsTable.arrangement = UICustomGrid.Arrangement.Vertical;
		this.mContentsTable.cellWidth = 442f;
		this.mContentsTable.cellHeight = 130f;
		this.mContentsTable.gapHeight = 8f;
		this.mContentsTable.gapWidth = 8f;
		switch (this.mBaseScene.curType)
		{
		case GUISelectItemBagScene.ESelectItemSceneType.ESIST_TrinketEnhance:
			this.mBaseScene.selfData = self;
			this.mNeed.text = self.GetTrinketEnhanceExp2Upgrade().ToString();
			this.InitTEItems(self);
			break;
		case GUISelectItemBagScene.ESelectItemSceneType.ESIST_EquipBreak:
			this.InitEBItems();
			break;
		case GUISelectItemBagScene.ESelectItemSceneType.ESIST_TrinketReborn:
			this.InitTrinketRebornItems();
			break;
		case GUISelectItemBagScene.ESelectItemSceneType.ESIST_LopetBreak:
			this.InitLopetBreakItems();
			break;
		case GUISelectItemBagScene.ESelectItemSceneType.ESIST_LopetReborn:
			this.InitLopetRebornItems();
			break;
		}
		this.Refresh();
	}
}
