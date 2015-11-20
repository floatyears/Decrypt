using Proto;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RecycleItem : MonoBehaviour
{
	private GUIRecycleScene mBaseScene;

	private CommonIconItem mIconItem;

	private UISprite mAdd;

	public ItemDataEx itemData;

	public PetDataEx petData;

	public LopetDataEx lopetData;

	public void InitWithBaseScene(GUIRecycleScene basescene)
	{
		this.mBaseScene = basescene;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mAdd = GameUITools.RegisterClickEvent("Add", new UIEventListener.VoidDelegate(this.OnAddClick), base.gameObject).GetComponent<UISprite>();
		this.mIconItem = CommonIconItem.Create(base.gameObject, new Vector3(-43f, 43f, 0f), new CommonIconItem.VoidCallBack(this.OnAddClick), true, 0.9f, new CommonIconItem.VoidCallBack(this.OnMinusClick));
		this.mIconItem.IsVisible = false;
	}

	public void OnAddClick(GameObject go)
	{
		switch (this.mBaseScene.mCurType)
		{
		case GUIRecycleScene.ERecycleT.ERecycleT_PetBreak:
		{
			List<PetDataEx> list;
			bool flag;
			Globals.Instance.Player.PetSystem.GetAllPet2BreakUp(out list, out flag);
			if (list.Count > 0 || flag)
			{
				PetDataEx[] datas = new PetDataEx[5];
				MC2S_PetBreakUp petBreakUpData = GameUIManager.mInstance.uiState.PetBreakUpData;
				if (petBreakUpData.PetID.Count > 0)
				{
					int num = 0;
					while (num < petBreakUpData.PetID.Count && num < 5)
					{
						datas[num] = Globals.Instance.Player.PetSystem.GetPet(petBreakUpData.PetID[num]);
						num++;
					}
				}
				GameUIManager.mInstance.ChangeSession<GUILvlUpSelPetSceneV2>(delegate(GUILvlUpSelPetSceneV2 sen)
				{
					sen.CurPetDataEx = null;
					sen.SetSelectPetDatas(datas);
				}, false, false);
			}
			else
			{
				GameUIManager.mInstance.ShowMessageTipByKey("recycle29", 0f, 0f);
			}
			break;
		}
		case GUIRecycleScene.ERecycleT.ERecycleT_EquipBreak:
		{
			List<ItemDataEx> list2;
			bool flag2;
			Globals.Instance.Player.ItemSystem.GetAllEquip2BreakUp(out list2, out flag2);
			if (list2.Count > 0 || flag2)
			{
				GUISelectItemBagScene.ChangeFromEquipBreak();
			}
			else
			{
				GameUIManager.mInstance.ShowMessageTipByKey("recycle30", 0f, 0f);
			}
			break;
		}
		case GUIRecycleScene.ERecycleT.ERecycleT_PetReborn:
			if (Globals.Instance.Player.PetSystem.HasPet2Reborn())
			{
				GameUIManager.mInstance.uiState.CombatPetSlot = -1;
				GameUIManager.mInstance.ChangeSession<GUIPartnerFightScene>(null, false, false);
			}
			else
			{
				GameUIManager.mInstance.ShowMessageTipByKey("recycle31", 0f, 0f);
			}
			break;
		case GUIRecycleScene.ERecycleT.ERecycleT_TrinketReborn:
			if (Globals.Instance.Player.ItemSystem.HasTrinket2Reborn())
			{
				GUISelectItemBagScene.ChangeFromTrinketReborn();
			}
			else
			{
				GameUIManager.mInstance.ShowMessageTipByKey("recycle32", 0f, 0f);
			}
			break;
		case GUIRecycleScene.ERecycleT.ERecycleT_LopetBreak:
			if (Globals.Instance.Player.LopetSystem.HasLopet2Break())
			{
				GUISelectItemBagScene.ChangeFromLopetBreak();
			}
			else
			{
				GameUIManager.mInstance.ShowMessageTipByKey("recycle39", 0f, 0f);
			}
			break;
		case GUIRecycleScene.ERecycleT.ERecycleT_LopetReborn:
			if (Globals.Instance.Player.LopetSystem.HasLopet2Reborn())
			{
				GUISelectItemBagScene.ChangeFromLopetReborn();
			}
			else
			{
				GameUIManager.mInstance.ShowMessageTipByKey("recycle40", 0f, 0f);
			}
			break;
		}
	}

	private void OnMinusClick(GameObject go)
	{
		switch (this.mBaseScene.mCurType)
		{
		case GUIRecycleScene.ERecycleT.ERecycleT_PetBreak:
			if (this.petData != null)
			{
				GameUIManager.mInstance.uiState.PetBreakUpData.PetID.Remove(this.petData.GetID());
			}
			break;
		case GUIRecycleScene.ERecycleT.ERecycleT_EquipBreak:
			if (this.itemData != null)
			{
				GameUIManager.mInstance.uiState.EquipBreakUpData.EquipID.Remove(this.itemData.GetID());
			}
			break;
		case GUIRecycleScene.ERecycleT.ERecycleT_PetReborn:
			if (this.petData != null)
			{
				GameUIManager.mInstance.uiState.PetRebornData.PetID = 0uL;
			}
			break;
		case GUIRecycleScene.ERecycleT.ERecycleT_TrinketReborn:
			if (this.itemData != null)
			{
				GameUIManager.mInstance.uiState.TrinketRebornData.TrinketID = 0uL;
			}
			break;
		case GUIRecycleScene.ERecycleT.ERecycleT_LopetBreak:
			if (this.lopetData != null)
			{
				GameUIManager.mInstance.uiState.LopetBreakData.LopetID.Remove(this.lopetData.GetID());
			}
			break;
		case GUIRecycleScene.ERecycleT.ERecycleT_LopetReborn:
			if (this.lopetData != null)
			{
				GameUIManager.mInstance.uiState.LopetRebornData.LopetID = 0uL;
			}
			break;
		}
		this.Clear();
	}

	public void Clear()
	{
		this.itemData = null;
		this.petData = null;
		this.lopetData = null;
		this.mAdd.enabled = true;
		this.mIconItem.IsVisible = false;
	}

	public void Refresh(ItemDataEx data)
	{
		if (data == null)
		{
			this.OnMinusClick(null);
			return;
		}
		this.itemData = data;
		this.petData = null;
		this.lopetData = null;
		this.mAdd.enabled = false;
		this.mIconItem.IsVisible = true;
		if (this.mBaseScene.mCurType == GUIRecycleScene.ERecycleT.ERecycleT_PetBreak || this.mBaseScene.mCurType == GUIRecycleScene.ERecycleT.ERecycleT_EquipBreak)
		{
			this.mIconItem.Refresh(this.itemData, false, false, true);
		}
		else
		{
			this.mIconItem.Refresh(this.itemData, true, false, true);
		}
	}

	public void Refresh(PetDataEx data)
	{
		if (data == null)
		{
			this.OnMinusClick(null);
			return;
		}
		this.petData = data;
		this.itemData = null;
		this.lopetData = null;
		this.mAdd.enabled = false;
		this.mIconItem.IsVisible = true;
		if (this.mBaseScene.mCurType == GUIRecycleScene.ERecycleT.ERecycleT_PetBreak || this.mBaseScene.mCurType == GUIRecycleScene.ERecycleT.ERecycleT_EquipBreak)
		{
			this.mIconItem.Refresh(this.petData, false, false, true);
		}
		else
		{
			this.mIconItem.Refresh(this.petData, true, false, true);
		}
	}

	public void Refresh(LopetDataEx data)
	{
		if (data == null)
		{
			this.OnMinusClick(null);
			return;
		}
		this.itemData = null;
		this.petData = null;
		this.lopetData = data;
		this.mAdd.enabled = false;
		this.mIconItem.IsVisible = true;
		if (this.mBaseScene.mCurType == GUIRecycleScene.ERecycleT.ERecycleT_LopetBreak || this.mBaseScene.mCurType == GUIRecycleScene.ERecycleT.ERecycleT_LopetReborn)
		{
			this.mIconItem.Refresh(this.lopetData, true, false, false);
		}
	}

	public bool IsNotNull()
	{
		return this.itemData != null || this.petData != null || this.lopetData != null;
	}
}
