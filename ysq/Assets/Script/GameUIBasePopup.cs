using Att;
using Proto;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameUIBasePopup : MonoBehaviour
{
	public virtual void InitPopUp()
	{
	}

	public virtual void InitPopUp(PetDataEx petData)
	{
	}

	public virtual void InitPopUp(PetInfo petInfo)
	{
	}

	public virtual void InitPopUp(FashionInfo fashionInfo)
	{
	}

	public virtual void InitPopUp(PetDataEx petData, PetInfo petInfo)
	{
	}

	public virtual void InitPopUp(ItemInfo itemInfo)
	{
	}

	public virtual void InitPopUp(ItemInfo itemInfo, GUIAwakeItemInfoPopUp.EOpenType type, UIEventListener.VoidDelegate cb)
	{
	}

	public virtual void InitPopUp(MS2C_PvpArenaResult resultData)
	{
	}

	public virtual void InitPopUp(ItemDataEx item)
	{
	}

	public virtual void InitPopUp(ItemDataEx data, GUIEquipInfoPopUp.EIPT type, int index)
	{
	}

	public virtual void InitPopUp(SocketDataEx item, int index)
	{
	}

	public virtual void InitPopUp(GuildMember guildMember)
	{
	}

	public virtual void InitPopUp(OpenLootData lootData, int lootNum)
	{
	}

	public virtual void InitPopUp(int VipLevel)
	{
	}

	public virtual void InitPopUp(uint level)
	{
	}

	public virtual void InitPopUp(GUIBillboardPopUp.BillboardContent contents)
	{
	}

	public virtual void InitPopUp(EInviteType type)
	{
	}

	public virtual void InitPopUp(string rules, List<RewardData> datas)
	{
	}

	public virtual void InitPopUp(GUIRollingSceneV2.ERollType type)
	{
	}

	public virtual void InitPopUp(GUIRecycleScene.ERecycleT type, GUIRecycleGetItemsPopUp.VoidCallBack cb)
	{
	}

	public virtual void InitPopUp(int value1, int value2)
	{
	}

	public virtual void InitPopUp(int value1, int value2, bool value3)
	{
	}

	public virtual void InitPopUp(bool value)
	{
	}

	public virtual void InitPopUp(ItemDataEx data, GUIMultiUsePopUp.UseItemCallBack cb)
	{
	}

	public virtual void OnPopUpClosing()
	{
	}

	public virtual void OnButtonBlockerClick()
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		GameUIPopupManager.GetInstance().PopState(false, null);
	}

	public virtual void SetState(int state)
	{
	}
}
