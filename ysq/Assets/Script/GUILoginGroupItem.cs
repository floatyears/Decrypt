using System;
using UnityEngine;

public class GUILoginGroupItem : UICustomGridItem
{
	public static GUILoginGroupItem selectedGroup;

	public static GroupItemInfoData selectedGroupData;

	private GUIGameLoginScene mBaseScene;

	private GroupItemInfoData mData;

	private UILabel mName;

	private UISprite mBG;

	private UISprite mHead;

	public void Init(GUIGameLoginScene basescene)
	{
		this.mBaseScene = basescene;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mName = GameUITools.FindUILabel("Name", base.gameObject);
		this.mBG = GameUITools.FindUISprite("BG", base.gameObject);
		this.mHead = GameUITools.FindUISprite("Head", base.gameObject);
		this.mHead.enabled = false;
		UIEventListener expr_59 = UIEventListener.Get(base.gameObject);
		expr_59.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(expr_59.onPress, new UIEventListener.BoolDelegate(this.OnItemPress));
		UIEventListener expr_85 = UIEventListener.Get(base.gameObject);
		expr_85.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_85.onClick, new UIEventListener.VoidDelegate(this.OnItemClick));
		UIEventListener expr_B1 = UIEventListener.Get(base.gameObject);
		expr_B1.onDragStart = (UIEventListener.VoidDelegate)Delegate.Combine(expr_B1.onDragStart, new UIEventListener.VoidDelegate(this.OnItemDragStart));
	}

	private void OnItemClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (GUILoginGroupItem.selectedGroup)
		{
			GUILoginGroupItem.selectedGroup.mBG.enabled = false;
		}
		GUILoginGroupItem.selectedGroup = this;
		GUILoginGroupItem.selectedGroupData = this.mData;
		this.mBG.enabled = true;
		this.mBaseScene.RefreshZones(this.mData.StartNum, this.mData.EndNum);
	}

	private void OnItemDragStart(GameObject go)
	{
		this.mBG.enabled = false;
	}

	private void OnItemPress(GameObject obj, bool isPressed)
	{
		if (this.mData == null)
		{
			return;
		}
		if (isPressed)
		{
			this.mBG.enabled = true;
		}
		else
		{
			this.mBG.enabled = false;
		}
	}

	public override void Refresh(object data)
	{
		if (this.mData == data)
		{
			return;
		}
		this.mData = (GroupItemInfoData)data;
		this.Refresh();
	}

	private void Refresh()
	{
		if (this.mData != null)
		{
			this.mName.text = Singleton<StringManager>.Instance.GetString("loginZoneGroup", new object[]
			{
				this.mData.StartNum,
				this.mData.EndNum
			});
			if (GUILoginGroupItem.selectedGroupData != null && GUILoginGroupItem.selectedGroupData == this.mData)
			{
				this.mBG.enabled = true;
			}
			else
			{
				this.mBG.enabled = false;
			}
		}
	}
}
