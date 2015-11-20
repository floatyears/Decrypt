using System;
using UnityEngine;

public class GUISelectItemBagScene : GameUISession
{
	public enum ESelectItemSceneType
	{
		ESIST_TrinketEnhance,
		ESIST_EquipBreak,
		ESIST_TrinketReborn,
		ESIST_LopetBreak,
		ESIST_LopetReborn
	}

	public ItemDataEx selfData;

	public SelectItemLayer mSelectItemLayer;

	public GUISelectItemBagScene.ESelectItemSceneType curType;

	public static void ChangeFromTrinketEnhance(ItemDataEx self)
	{
		if (self == null)
		{
			global::Debug.LogError(new object[]
			{
				"self ItemDataEx is null"
			});
			return;
		}
		GameUIManager.mInstance.ChangeSession<GUISelectItemBagScene>(delegate(GUISelectItemBagScene sen)
		{
			sen.curType = GUISelectItemBagScene.ESelectItemSceneType.ESIST_TrinketEnhance;
			sen.mSelectItemLayer.InitDatas(self);
		}, false, false);
	}

	public static void ChangeFromEquipBreak()
	{
		GameUIManager.mInstance.ChangeSession<GUISelectItemBagScene>(delegate(GUISelectItemBagScene sen)
		{
			sen.curType = GUISelectItemBagScene.ESelectItemSceneType.ESIST_EquipBreak;
			sen.mSelectItemLayer.InitDatas(null);
		}, false, false);
	}

	public static void ChangeFromTrinketReborn()
	{
		GameUIManager.mInstance.ChangeSession<GUISelectItemBagScene>(delegate(GUISelectItemBagScene sen)
		{
			sen.curType = GUISelectItemBagScene.ESelectItemSceneType.ESIST_TrinketReborn;
			sen.mSelectItemLayer.InitDatas(null);
		}, false, false);
	}

	public static void ChangeFromLopetBreak()
	{
		GameUIManager.mInstance.ChangeSession<GUISelectItemBagScene>(delegate(GUISelectItemBagScene sen)
		{
			sen.curType = GUISelectItemBagScene.ESelectItemSceneType.ESIST_LopetBreak;
			sen.mSelectItemLayer.InitDatas(null);
		}, false, false);
	}

	public static void ChangeFromLopetReborn()
	{
		GameUIManager.mInstance.ChangeSession<GUISelectItemBagScene>(delegate(GUISelectItemBagScene sen)
		{
			sen.curType = GUISelectItemBagScene.ESelectItemSceneType.ESIST_LopetReborn;
			sen.mSelectItemLayer.InitDatas(null);
		}, false, false);
	}

	protected override void OnPostLoadGUI()
	{
		TopGoods topGoods = GameUIManager.mInstance.GetTopGoods();
		topGoods.Show("equipImprove34");
		topGoods.BackClickListener = new UIEventListener.VoidDelegate(this.OnBackClick);
		this.CreateObjects();
	}

	protected override void OnPreDestroyGUI()
	{
		GameUITools.CompleteAllHotween();
		GameUIManager.mInstance.GetTopGoods().Hide();
	}

	public void OnBackClick(GameObject go)
	{
		switch (this.curType)
		{
		case GUISelectItemBagScene.ESelectItemSceneType.ESIST_TrinketEnhance:
			GUITrinketUpgradeScene.Change2This(this.selfData, GUITrinketUpgradeScene.EUpgradeType.EUT_Enhance, -2);
			break;
		case GUISelectItemBagScene.ESelectItemSceneType.ESIST_EquipBreak:
			GUIRecycleScene.Change2This(GUIRecycleScene.ERecycleT.ERecycleT_EquipBreak);
			break;
		case GUISelectItemBagScene.ESelectItemSceneType.ESIST_TrinketReborn:
			GUIRecycleScene.Change2This(GUIRecycleScene.ERecycleT.ERecycleT_TrinketReborn);
			break;
		case GUISelectItemBagScene.ESelectItemSceneType.ESIST_LopetBreak:
			GUIRecycleScene.Change2This(GUIRecycleScene.ERecycleT.ERecycleT_LopetBreak);
			break;
		case GUISelectItemBagScene.ESelectItemSceneType.ESIST_LopetReborn:
			GUIRecycleScene.Change2This(GUIRecycleScene.ERecycleT.ERecycleT_LopetReborn);
			break;
		}
		foreach (ItemDataEx current in this.mSelectItemLayer.mCurSelectItems)
		{
			current.ClearUIData();
		}
	}

	private void CreateObjects()
	{
		GameObject parent = GameUITools.FindGameObject("WindowBg", base.gameObject);
		this.mSelectItemLayer = GameUITools.FindGameObject("ItemLayer", parent).AddComponent<SelectItemLayer>();
		this.mSelectItemLayer.InitWithBaseScene(this);
	}
}
