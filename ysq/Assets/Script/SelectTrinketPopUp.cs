using Holoville.HOTween.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SelectTrinketPopUp : MonoBehaviour
{
	private class TrinketTable : CommonBagUITable
	{
		protected override int Sort(BaseData a, BaseData b)
		{
			return 0;
		}
	}

	private GUIPillageCastingPopUp mBaseScene;

	private GameObject mBG;

	private SelectTrinketPopUp.TrinketTable mContent;

	private List<ItemDataEx> mCurSelectItems = new List<ItemDataEx>();

	public void Init(GUIPillageCastingPopUp basescene)
	{
		this.mBaseScene = basescene;
		GameUITools.RegisterClickEvent("FadeBG", new UIEventListener.VoidDelegate(this.OnCloseClick), base.gameObject);
		this.mBG = GameUITools.FindGameObject("BG", base.gameObject);
		GameUITools.RegisterClickEvent("CloseBtn", new UIEventListener.VoidDelegate(this.OnCloseClick), this.mBG);
		GameUITools.RegisterClickEvent("OK", new UIEventListener.VoidDelegate(this.OnOKClick), this.mBG);
		this.mContent = GameUITools.FindGameObject("Panel/Content", this.mBG).AddComponent<SelectTrinketPopUp.TrinketTable>();
		this.mContent.InitWithBaseScene(this, "GUICastTrinketSelectBagItem");
		this.mContent.maxPerLine = 1;
		this.mContent.arrangement = UICustomGrid.Arrangement.Vertical;
		this.mContent.cellWidth = 440f;
		this.mContent.cellHeight = 116f;
		this.mContent.gapHeight = 6f;
		this.mContent.gapWidth = 0f;
	}

	private void OnCloseClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		GameUITools.PlayCloseWindowAnim(this.mBG.transform, new TweenDelegate.TweenCallback(this.Hide), true);
	}

	public void Open(int infoID, List<ulong> ids)
	{
		if (ids != null && ids.Count > this.mBaseScene.CastItemCount)
		{
			ids.RemoveRange(this.mBaseScene.CastItemCount, ids.Count - this.mBaseScene.CastItemCount);
		}
		this.mContent.SetDragAmount(0f, 0f);
		this.mContent.ClearGridItem();
		this.mContent.ClearData();
		this.mCurSelectItems.Clear();
		foreach (ItemDataEx current in Globals.Instance.Player.ItemSystem.Values)
		{
			if (current.Info.ID == infoID && current.IsTrinketAndCastItem())
			{
				current.IsSelected = false;
				if (ids != null && ids.Contains(current.GetID()))
				{
					current.IsSelected = true;
					this.mCurSelectItems.Add(current);
				}
				this.mContent.AddData(current);
			}
		}
		base.gameObject.SetActive(true);
		GameUITools.PlayOpenWindowAnim(this.mBG.transform, null, true);
	}

	public void Hide()
	{
		base.gameObject.SetActive(false);
	}

	private void OnOKClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.mBaseScene.AddCastTrinketItems(this.mCurSelectItems);
		GameUITools.PlayCloseWindowAnim(this.mBG.transform, new TweenDelegate.TweenCallback(this.Hide), true);
	}

	public bool CanAddItem(ItemDataEx data)
	{
		if (this.mCurSelectItems.Contains(data))
		{
			return false;
		}
		if (this.mCurSelectItems.Count >= this.mBaseScene.CastItemCount)
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("equipImprove35", new object[]
			{
				this.mBaseScene.CastItemCount
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
		}
	}

	public void DeleteItem(ItemDataEx data)
	{
		if (this.mCurSelectItems.Contains(data))
		{
			this.mCurSelectItems.Remove(data);
		}
	}
}
