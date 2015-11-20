using Att;
using System;
using UnityEngine;

public class EquipRefineExpItem : MonoBehaviour
{
	private EquipRefineLayer mBaseLayer;

	private ItemInfo mInfo;

	private ItemDataEx mData;

	private UISprite mIcon;

	private UILabel mNum;

	private UISprite mGrayMask;

	private UISprite mQualityMark;

	private UILabel mValue;

	public bool mIsPressed;

	private float mPressedTimer;

	private int mCurSelected;

	private int mRemainCount;

	private float pressTime = 0.15f;

	public void InitWithBaseScene(EquipRefineLayer baseLayer, ItemInfo info)
	{
		this.mBaseLayer = baseLayer;
		this.mInfo = info;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mIcon = GameUITools.FindUISprite("Icon", base.gameObject);
		this.mNum = GameUITools.FindUILabel("Num", base.gameObject);
		this.mGrayMask = GameUITools.FindUISprite("GrayMask", base.gameObject);
		this.mQualityMark = GameUITools.FindUISprite("QualityMark", base.gameObject);
		this.mValue = GameUITools.FindUILabel("Value", base.gameObject);
		this.mIcon.spriteName = this.mInfo.Icon;
		this.mQualityMark.spriteName = Tools.GetItemQualityIcon(this.mInfo.Quality);
		this.mValue.text = Singleton<StringManager>.Instance.GetString("equipImprove23", new object[]
		{
			this.mInfo.Value1
		});
		UIEventListener expr_E2 = UIEventListener.Get(this.mIcon.gameObject);
		expr_E2.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(expr_E2.onPress, new UIEventListener.BoolDelegate(this.OnIconPress));
		UIEventListener expr_113 = UIEventListener.Get(this.mGrayMask.gameObject);
		expr_113.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_113.onClick, new UIEventListener.VoidDelegate(this.OnGrayMaskClick));
	}

	public void Refresh()
	{
		this.mData = Globals.Instance.Player.ItemSystem.GetItemByInfoID(this.mInfo.ID);
		if (this.mData != null && this.mData.GetCount() > 0)
		{
			this.mNum.text = this.mData.GetCount().ToString();
			this.mNum.color = Color.white;
			this.mRemainCount = this.mData.GetCount();
			this.mGrayMask.enabled = false;
		}
		else
		{
			this.mNum.text = "0";
			this.mNum.color = Color.red;
			this.mRemainCount = 0;
			this.mGrayMask.enabled = true;
		}
		this.mIsPressed = false;
		this.mPressedTimer = 0f;
		this.mCurSelected = 0;
	}

	private void OnIconClick(GameObject go)
	{
		if (!this.mIcon.collider.enabled)
		{
			return;
		}
		if (this.mBaseLayer.mBaseScene.mEquipData.CanRefine())
		{
			this.mBaseLayer.AddExp(this.mData, true);
		}
		else
		{
			GameUIManager.mInstance.ShowMessageTipByKey("equipImprove27", 0f, 0f);
		}
	}

	private void OnGrayMaskClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		GUIHowGetPetItemPopUp.ShowThis(this.mInfo);
	}

	public void EnableCollider(bool enable)
	{
		this.mIcon.collider.enabled = enable;
	}

	private void OnIconPress(GameObject go, bool isPressed)
	{
		if (isPressed)
		{
			if (this.mData != null && this.mData.GetCount() > 0)
			{
				if (this.mBaseLayer.mBaseScene.mEquipData.CanRefine())
				{
					this.mIsPressed = true;
					this.mPressedTimer = 0f;
				}
				else
				{
					GameUIManager.mInstance.ShowMessageTipByKey("equipImprove27", 0f, 0f);
				}
			}
		}
		else if (this.mPressedTimer != 0f)
		{
			if (this.mPressedTimer <= this.pressTime)
			{
				GameUIManager.mInstance.ShowMessageTipByKey("equipImprove82", 0f, 0f);
			}
			this.OnIconClick(null);
			this.mPressedTimer = 0f;
			this.mIsPressed = false;
		}
	}

	private void LateUpdate()
	{
		if (this.mIsPressed)
		{
			if (this.mData == null || this.mData.GetCount() < 1)
			{
				return;
			}
			this.mPressedTimer += Time.deltaTime;
			if (this.mPressedTimer > this.pressTime && this.mRemainCount > 0)
			{
				this.mCurSelected++;
				this.mRemainCount = this.mData.GetCount() - this.mCurSelected;
				this.mNum.text = this.mRemainCount.ToString();
				if (this.mIcon.collider.enabled)
				{
					this.mBaseLayer.AddExp(this.mData, false);
				}
			}
		}
	}
}
