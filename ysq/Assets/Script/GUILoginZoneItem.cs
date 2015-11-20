using System;
using UnityEngine;

public class GUILoginZoneItem : UICustomGridItem
{
	public static GUILoginZoneItem selectedZone;

	public static ZoneItemInfoData selectedZoneData;

	protected GUIGameLoginScene mBaseScene;

	private ZoneItemInfoData mZoneData;

	private GameObject mZoneButton;

	private UISprite mBG;

	public UILabel mNum;

	public UISprite mState;

	public UISprite mNew;

	public UILabel mName;

	public UISprite mHead;

	public void InitWithBaseScene(GUIGameLoginScene baseScene)
	{
		this.mBaseScene = baseScene;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mZoneButton = GameUITools.FindGameObject("ZoneButton", base.gameObject);
		this.mNum = GameUITools.FindUILabel("Num", this.mZoneButton);
		this.mName = GameUITools.FindUILabel("Name", this.mZoneButton);
		this.mNew = GameUITools.FindUISprite("New", this.mZoneButton);
		this.mState = GameUITools.FindUISprite("State", this.mZoneButton);
		this.mHead = GameUITools.FindUISprite("Head", this.mZoneButton);
		this.mBG = GameUITools.FindUISprite("BG", this.mZoneButton);
		this.mState.enabled = false;
		UIEventListener expr_B1 = UIEventListener.Get(this.mZoneButton);
		expr_B1.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(expr_B1.onPress, new UIEventListener.BoolDelegate(this.OnZoneButtonPress));
		UIEventListener expr_DD = UIEventListener.Get(this.mZoneButton);
		expr_DD.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_DD.onClick, new UIEventListener.VoidDelegate(this.OnItemClick));
		UIEventListener expr_109 = UIEventListener.Get(this.mZoneButton);
		expr_109.onDragStart = (UIEventListener.VoidDelegate)Delegate.Combine(expr_109.onDragStart, new UIEventListener.VoidDelegate(this.OnDragStart));
		GameUITools.UpdateUIBoxCollider(this.mZoneButton.transform, 20f, true);
	}

	private void OnDragStart(GameObject go)
	{
		this.mBG.enabled = false;
	}

	public override void Refresh(object data)
	{
		if (this.mZoneData == data)
		{
			return;
		}
		this.mZoneData = (ZoneItemInfoData)data;
		this.Refresh();
	}

	private void Refresh()
	{
		if (this.mZoneData != null)
		{
			this.mState.enabled = true;
			this.mNum.text = Singleton<StringManager>.Instance.GetString("loginZoneNum", new object[]
			{
				this.mZoneData.mShowNum
			});
			this.mName.text = this.mZoneData.mName;
			if (this.mZoneData.mGender == 0)
			{
				this.mHead.spriteName = "man";
				this.mHead.enabled = true;
			}
			else if (this.mZoneData.mGender == 1)
			{
				this.mHead.spriteName = "woman";
				this.mHead.enabled = true;
			}
			else
			{
				this.mHead.enabled = false;
			}
			if (this.mZoneData.mState == 0)
			{
				global::Debug.LogError(new object[]
				{
					string.Format("zoneitem mState error , zoneName : {0}, ", this.mZoneData.mName)
				});
				return;
			}
			this.mName.color = Color.white;
			this.mNum.color = Color.white;
			switch (this.mZoneData.mState)
			{
			case 1:
				this.mState.spriteName = "green";
				break;
			case 2:
				this.mState.spriteName = "green";
				break;
			case 3:
				this.mState.spriteName = "yellow";
				break;
			case 4:
				this.mState.spriteName = "red";
				break;
			case 5:
				this.mState.spriteName = "gray";
				this.mName.color = Color.gray;
				this.mNum.color = Color.gray;
				break;
			}
			int num = this.mZoneData.mNew;
			if (num != 0)
			{
				if (num == 1)
				{
					this.mNew.enabled = true;
				}
			}
			else
			{
				this.mNew.enabled = false;
			}
			if (GUILoginZoneItem.selectedZoneData != null && GUILoginZoneItem.selectedZoneData == this.mZoneData)
			{
				this.mBG.enabled = true;
			}
			else
			{
				this.mBG.enabled = false;
			}
		}
		else
		{
			this.mState.enabled = false;
		}
		this.mHead.enabled = false;
	}

	public void OnItemClick(GameObject go)
	{
		if (!GameSetting.Data.LastGMLogin && (this.mZoneData == null || this.mZoneData.mState == 5))
		{
			return;
		}
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (GUILoginZoneItem.selectedZone)
		{
			GUILoginZoneItem.selectedZone.mBG.enabled = false;
		}
		GUILoginZoneItem.selectedZone = this;
		GUILoginZoneItem.selectedZoneData = this.mZoneData;
		this.mBG.enabled = true;
		this.mBaseScene.selectedZoneData = this.mZoneData;
		this.mBaseScene.mNum.text = this.mNum.text;
		this.mBaseScene.mNum.color = this.mNum.color;
		this.mBaseScene.mName.text = this.mName.text;
		this.mBaseScene.mName.color = this.mName.color;
		this.mBaseScene.mNew.enabled = this.mNew.enabled;
		this.mBaseScene.mState.spriteName = this.mState.spriteName;
		this.mBaseScene.CloseZonesWindow();
	}

	private void OnZoneButtonPress(GameObject obj, bool isPressed)
	{
		if (this.mZoneData == null || this.mZoneData.mState == 5)
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
}
