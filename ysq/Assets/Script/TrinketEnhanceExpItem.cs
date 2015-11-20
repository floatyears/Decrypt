using System;
using UnityEngine;

public class TrinketEnhanceExpItem : MonoBehaviour
{
	private TrinketEnhanceLayer mBaseLayer;

	public ItemDataEx mData;

	public GameObject mItem;

	private UISprite mIcon;

	private UISprite mQualityMark;

	private UISprite mAdd;

	private UISprite mMinus;

	private GameObject ui56_2;

	public void InitWithBaseScene(TrinketEnhanceLayer baseLayer)
	{
		this.mBaseLayer = baseLayer;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mAdd = GameUITools.FindUISprite("Add", base.gameObject);
		this.mItem = GameUITools.FindGameObject("Item", base.gameObject);
		this.mIcon = GameUITools.RegisterClickEvent("Icon", new UIEventListener.VoidDelegate(this.OnIconClick), this.mItem).GetComponent<UISprite>();
		this.mQualityMark = GameUITools.FindUISprite("QualityMark", this.mItem);
		this.mMinus = GameUITools.RegisterClickEvent("Minus", new UIEventListener.VoidDelegate(this.OnMinusClick), this.mItem).GetComponent<UISprite>();
		this.ui56_2 = GameUITools.FindGameObject("ui56_2", base.gameObject);
		Tools.SetParticleRQWithUIScale(this.ui56_2, 4500);
		NGUITools.SetActive(this.ui56_2, false);
	}

	public void Refresh(ItemDataEx data)
	{
		this.mData = data;
		this.Refresh(true);
	}

	public void Refresh(bool disableEffect = true)
	{
		if (disableEffect)
		{
			NGUITools.SetActive(this.ui56_2, false);
		}
		if (this.mData == null)
		{
			this.mIcon.enabled = false;
			this.mAdd.enabled = true;
			this.mMinus.enabled = false;
			this.mQualityMark.enabled = false;
		}
		else
		{
			this.mIcon.enabled = true;
			this.mIcon.spriteName = this.mData.Info.Icon;
			this.mQualityMark.spriteName = Tools.GetItemQualityIcon(this.mData.Info.Quality);
			this.mAdd.enabled = false;
			this.mQualityMark.enabled = true;
			this.mMinus.enabled = true;
		}
	}

	private void OnIconClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_003");
		if (this.mBaseLayer.mBaseScene.mEquipData.CanEnhance())
		{
			GUISelectItemBagScene.ChangeFromTrinketEnhance(this.mBaseLayer.mBaseScene.mEquipData);
		}
		else
		{
			GameUIManager.mInstance.ShowMessageTipByKey("equipImprove28", 0f, 0f);
		}
	}

	private void OnClick()
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_003");
		if (this.mBaseLayer.mBaseScene.mEquipData.CanEnhance())
		{
			GUISelectItemBagScene.ChangeFromTrinketEnhance(this.mBaseLayer.mBaseScene.mEquipData);
		}
		else
		{
			GameUIManager.mInstance.ShowMessageTipByKey("equipImprove28", 0f, 0f);
		}
	}

	private void OnMinusClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_003");
		GameUIManager.mInstance.uiState.TrinketEnhanceData.ItemID.Remove(this.mData.GetID());
		this.mData = null;
		this.mBaseLayer.Refresh(null);
	}

	public void PlayAnim()
	{
		NGUITools.SetActive(this.ui56_2, false);
		NGUITools.SetActive(this.ui56_2, true);
	}
}
