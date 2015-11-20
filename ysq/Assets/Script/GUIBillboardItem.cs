using Holoville.HOTween;
using Holoville.HOTween.Core;
using System;
using UnityEngine;

public class GUIBillboardItem : MonoBehaviour
{
	private GUIBillboardPopUp mBaseScene;

	private bool opened = true;

	private UILabel mName;

	private UISprite mDirection;

	private UILabel mContents;

	public void InitWithBaseScene(GUIBillboardPopUp popUp)
	{
		this.mBaseScene = popUp;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mName = GameUITools.FindUILabel("Name", base.gameObject);
		this.mDirection = GameUITools.FindUISprite("Direction", base.gameObject);
		this.mContents = GameUITools.FindUILabel("Contents", base.gameObject);
		this.mContents.spaceIsNewLine = false;
		UIEventListener expr_5E = UIEventListener.Get(this.mContents.gameObject);
		expr_5E.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_5E.onClick, new UIEventListener.VoidDelegate(this.OnContentsClick));
		UIEventListener expr_8A = UIEventListener.Get(base.gameObject);
		expr_8A.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_8A.onClick, new UIEventListener.VoidDelegate(this.OnBtnClick));
	}

	public void Refresh(string name, string content)
	{
		this.mName.text = name;
		this.mContents.text = content;
	}

	private void OnBtnClick(GameObject go)
	{
		this.opened = !this.opened;
		if (this.opened)
		{
			this.mDirection.transform.localRotation = Quaternion.identity;
			HOTween.To(this.mContents.transform, (this.mBaseScene.ContentsDuration <= 0f) ? 0.4f : this.mBaseScene.ContentsDuration, new TweenParms().Prop("localScale", Vector3.one).Ease(this.mBaseScene.ContentsCurve).OnUpdate(new TweenDelegate.TweenCallbackWParms(this.RepositionItems), new object[0]).OnComplete(new TweenDelegate.TweenCallbackWParms(this.RepositionItems), new object[0]));
		}
		else
		{
			this.mDirection.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
			HOTween.To(this.mContents.transform, (this.mBaseScene.ContentsDuration <= 0f) ? 0.4f : this.mBaseScene.ContentsDuration, new TweenParms().Prop("localScale", new Vector3(1f, 0f, 1f)).Ease(this.mBaseScene.ContentsCurve).OnUpdate(new TweenDelegate.TweenCallbackWParms(this.RepositionItems), new object[0]).OnComplete(new TweenDelegate.TweenCallbackWParms(this.RepositionItems), new object[0]));
		}
	}

	private void OnContentsClick(GameObject go)
	{
		string urlAtPosition = this.mContents.GetUrlAtPosition(UICamera.lastHit.point);
		if (string.IsNullOrEmpty(urlAtPosition))
		{
			return;
		}
		try
		{
			Uri uri = new Uri(urlAtPosition);
			Application.OpenURL(uri.AbsoluteUri);
		}
		catch
		{
			global::Debug.LogErrorFormat("billboardpopup url error , url : {0} ", new object[]
			{
				urlAtPosition
			});
		}
	}

	private void RepositionItems(TweenEvent e)
	{
		this.mBaseScene.mTable.repositionNow = true;
	}
}
