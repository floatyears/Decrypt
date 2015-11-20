using Att;
using Proto;
using System;
using UnityEngine;

public class GUIPetTrainJueXingNeedItem : MonoBehaviour
{
	private GUIPetTrainSceneV2 mBaseScene;

	private UISprite mIcon;

	private UISprite mQualityMask;

	private UILabel mDesc;

	private GameObject mMaskGo;

	private int mIndex;

	private ItemInfo mItemInfo;

	private GameObject mEquipEffect;

	public bool IsItemEquiped
	{
		get;
		set;
	}

	public bool IsItemCanEquip
	{
		get;
		set;
	}

	public void InitWithBaseScene(GUIPetTrainSceneV2 baseScene, int index)
	{
		this.mBaseScene = baseScene;
		this.mIndex = index;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mIcon = base.transform.Find("icon").GetComponent<UISprite>();
		this.mIcon.spriteName = string.Empty;
		UIEventListener expr_3B = UIEventListener.Get(this.mIcon.gameObject);
		expr_3B.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_3B.onClick, new UIEventListener.VoidDelegate(this.OnIconClick));
		this.mQualityMask = base.transform.Find("qualityMask").GetComponent<UISprite>();
		this.mQualityMask.spriteName = string.Empty;
		this.mDesc = base.transform.Find("desc").GetComponent<UILabel>();
		this.mDesc.text = string.Empty;
		this.mMaskGo = base.transform.Find("mask").gameObject;
		this.mEquipEffect = base.transform.Find("ui55").gameObject;
		Tools.SetParticleRenderQueue(this.mEquipEffect, 3500, 1f);
		NGUITools.SetActive(this.mEquipEffect, false);
	}

	public void Refresh()
	{
		PetDataEx curPetDataEx = this.mBaseScene.CurPetDataEx;
		if (curPetDataEx != null)
		{
			int awakeItemID = curPetDataEx.GetAwakeItemID(this.mIndex);
			this.mItemInfo = Globals.Instance.AttDB.ItemDict.GetInfo(awakeItemID);
			if (this.mItemInfo != null)
			{
				this.mIcon.gameObject.SetActive(true);
				this.mQualityMask.gameObject.SetActive(true);
				this.mIcon.spriteName = this.mItemInfo.Icon;
				this.mQualityMask.spriteName = Tools.GetItemQualityIcon(this.mItemInfo.Quality);
				if (curPetDataEx.IsAwakeItemEquip(this.mIndex))
				{
					this.IsItemEquiped = true;
					this.IsItemCanEquip = false;
					this.mMaskGo.SetActive(false);
					this.mDesc.text = string.Empty;
				}
				else
				{
					this.IsItemEquiped = false;
					this.mMaskGo.SetActive(true);
					if (Globals.Instance.Player.ItemSystem.GetItemCount(awakeItemID) > 0)
					{
						this.mDesc.text = Singleton<StringManager>.Instance.GetString("petJueXing4");
						this.IsItemCanEquip = true;
					}
					else if (Globals.Instance.Player.ItemSystem.EnoughItem2CreateAwakeItem(awakeItemID, 1, true))
					{
						this.mDesc.text = Singleton<StringManager>.Instance.GetString("petJueXing5");
						this.IsItemCanEquip = true;
					}
					else
					{
						this.mDesc.text = Singleton<StringManager>.Instance.GetString("petJueXing7");
						this.IsItemCanEquip = false;
					}
				}
			}
			else
			{
				this.IsItemEquiped = true;
				this.IsItemCanEquip = false;
				this.mIcon.gameObject.SetActive(false);
				this.mQualityMask.gameObject.SetActive(false);
				this.mMaskGo.SetActive(false);
			}
		}
	}

	private void OnIconClick(GameObject go)
	{
		PetDataEx curPetDataEx = this.mBaseScene.CurPetDataEx;
		if (curPetDataEx != null && this.mItemInfo != null)
		{
			if (curPetDataEx.IsAwakeItemEquip(this.mIndex))
			{
				GUIAwakeItemInfoPopUp.Show(this.mItemInfo, GUIAwakeItemInfoPopUp.EOpenType.EOT_View, null);
			}
			else if (Globals.Instance.Player.ItemSystem.GetItemCount(this.mItemInfo.ID) > 0)
			{
				GUIAwakeItemInfoPopUp.Show(this.mItemInfo, GUIAwakeItemInfoPopUp.EOpenType.EOT_Equip, new UIEventListener.VoidDelegate(this.OnEquipBtnClick));
			}
			else
			{
				GUIAwakeItemInfoPopUp.Show(this.mItemInfo, GUIAwakeItemInfoPopUp.EOpenType.EOT_Get, new UIEventListener.VoidDelegate(this.OnEquipBtnClick));
			}
		}
	}

	private void OnEquipBtnClick(GameObject go)
	{
		PetDataEx curPetDataEx = this.mBaseScene.CurPetDataEx;
		if (curPetDataEx != null && this.mItemInfo != null && !curPetDataEx.IsAwakeItemEquip(this.mIndex))
		{
			int @int = GameConst.GetInt32(24);
			if ((ulong)curPetDataEx.Data.Level < (ulong)((long)@int))
			{
				GameUIManager.mInstance.ShowMessageTip("petJueXing9", @int);
				return;
			}
			ItemDataEx itemByInfoID = Globals.Instance.Player.ItemSystem.GetItemByInfoID(this.mItemInfo.ID);
			if (itemByInfoID != null)
			{
				MC2S_EquipAwakeItem mC2S_EquipAwakeItem = new MC2S_EquipAwakeItem();
				mC2S_EquipAwakeItem.PetID = ((curPetDataEx.GetSocketSlot() != 0) ? curPetDataEx.Data.ID : 100uL);
				mC2S_EquipAwakeItem.Index = this.mIndex;
				mC2S_EquipAwakeItem.ItemID = itemByInfoID.Data.ID;
				Globals.Instance.CliSession.Send(417, mC2S_EquipAwakeItem);
			}
		}
	}

	public void HideEffects()
	{
		NGUITools.SetActive(this.mEquipEffect, false);
	}

	public void PlayEquipEffect()
	{
		NGUITools.SetActive(this.mEquipEffect, false);
		NGUITools.SetActive(this.mEquipEffect, true);
	}
}
