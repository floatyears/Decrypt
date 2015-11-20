using Holoville.HOTween;
using Holoville.HOTween.Core;
using System;
using UnityEngine;

public class BillboardParentBtn : MonoBehaviour
{
	private GUIBillboard mBaseScene;

	private UIButton mBtn;

	private UILabel mName;

	private Transform mChildren;

	private BillboardChildBtn firstChild;

	public void Init(GUIBillboard baseScene, string name)
	{
		this.mBaseScene = baseScene;
		this.CreateObjects();
		this.mName.text = name;
	}

	private void CreateObjects()
	{
		this.mBtn = base.GetComponent<UIButton>();
		this.mName = GameUITools.FindUILabel("Name", base.gameObject);
		this.mChildren = GameUITools.FindGameObject("Children", base.gameObject).transform;
		this.mChildren.gameObject.SetActive(false);
		this.mBtn.normalSprite = "btn_dark2";
		EventDelegate.Add(this.mBtn.onClick, new EventDelegate.Callback(this.OnBtnClick));
	}

	public void OnBtnClick()
	{
		if (this.mBaseScene.isWaitingMessageReply)
		{
			return;
		}
		if (this.mBaseScene.mCurParentBtn == this)
		{
			this.SetActiveState(false, true);
			this.mBaseScene.mCurParentBtn = null;
		}
		else
		{
			if (this.mBaseScene.mCurParentBtn != null)
			{
				this.mBaseScene.mCurParentBtn.SetActiveState(false, false);
			}
			this.SetActiveState(true, true);
			this.mBaseScene.mCurParentBtn = this;
		}
		this.firstChild.OnBtnClick();
	}

	public void SetActiveState(bool isChecked, bool update = true)
	{
		if (isChecked)
		{
			this.mBtn.normalSprite = "btn_check2";
			this.mChildren.gameObject.SetActive(true);
			this.mChildren.transform.localScale = new Vector3(1f, 0f, 1f);
			HOTween.To(this.mChildren.transform, (this.mBaseScene.ParentBtnDuration <= 0f) ? 0.4f : this.mBaseScene.ParentBtnDuration, new TweenParms().Prop("localScale", Vector3.one).Ease(this.mBaseScene.ParentBtnCurve).OnUpdate(new TweenDelegate.TweenCallbackWParms(this.RepositionChildren), new object[]
			{
				true
			}).OnComplete(new TweenDelegate.TweenCallbackWParms(this.RepositionChildren), new object[]
			{
				true
			}));
		}
		else
		{
			this.mBtn.normalSprite = "btn_dark2";
			if (update)
			{
				HOTween.To(this.mChildren.transform, (this.mBaseScene.ParentBtnDuration <= 0f) ? 0.4f : this.mBaseScene.ParentBtnDuration, new TweenParms().Prop("localScale", new Vector3(1f, 0f, 1f)).Ease(this.mBaseScene.ParentBtnCurve).OnUpdate(new TweenDelegate.TweenCallbackWParms(this.RepositionChildren), new object[]
				{
					true
				}).OnComplete(new TweenDelegate.TweenCallbackWParms(this.RepositionChildren), new object[]
				{
					false
				}));
			}
			else
			{
				HOTween.To(this.mChildren.transform, (this.mBaseScene.ParentBtnDuration <= 0f) ? 0.4f : this.mBaseScene.ParentBtnDuration, new TweenParms().Prop("localScale", new Vector3(1f, 0f, 1f)).Ease(this.mBaseScene.ParentBtnCurve).OnComplete(new TweenDelegate.TweenCallbackWParms(this.RepositionChildren), new object[]
				{
					false
				}));
			}
		}
	}

	private void RepositionChildren(TweenEvent e)
	{
		if (e.parms != null)
		{
			this.mChildren.gameObject.SetActive((bool)e.parms[0]);
		}
		this.mBaseScene.mBtnContents.repositionNow = true;
	}

	public void AddChildBtn(BillboardChildBtn child)
	{
		if (this.firstChild == null)
		{
			this.firstChild = child;
		}
		GameUITools.AddChild(this.mChildren.gameObject, child.gameObject);
		child.gameObject.transform.localPosition = new Vector3(0f, -32.5f - (float)(65 * (this.mChildren.childCount - 1)), 0f);
	}
}
