using Att;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class GUIXingZuoPage : MonoBehaviour
{
	public GameObject mLeftBtn;

	public GameObject mRightBtn;

	public static int mXingZuo;

	private GameObject objItem;

	public UIXingZuoItem mUIXingZuoItem;

	private GUIConstellationScene mBaseScene;

	private ConstellationInfo mConInfo;

	private GUIRightInfo mRightInfo;

	public Vector3 mDragPostion;

	public void InitWithBaseScene(GUIConstellationScene baseScene)
	{
		this.mBaseScene = baseScene;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mLeftBtn = base.transform.Find("leftBtn").gameObject;
		UIEventListener expr_26 = UIEventListener.Get(this.mLeftBtn);
		expr_26.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_26.onClick, new UIEventListener.VoidDelegate(this.OnLeftBtnClick));
		this.mRightBtn = base.transform.Find("rightBtn").gameObject;
		UIEventListener expr_6D = UIEventListener.Get(this.mRightBtn);
		expr_6D.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_6D.onClick, new UIEventListener.VoidDelegate(this.OnRightBtnClick));
		UnityEngine.Object @object = Res.LoadGUI("GUI/xingzuo1");
		this.objItem = (GameObject)UnityEngine.Object.Instantiate(@object);
		this.objItem.name = @object.name;
		this.objItem.transform.parent = base.transform;
		this.objItem.transform.localPosition = Vector3.zero;
		this.objItem.transform.localScale = Vector3.one;
		GameUITools.AddChild(base.gameObject, this.objItem);
		this.mUIXingZuoItem = this.objItem.AddComponent<UIXingZuoItem>();
		this.mUIXingZuoItem.InitItemData(this.mBaseScene);
		this.mUIXingZuoItem.InitItemData(this);
	}

	public void ChooseCurPage(int conLv)
	{
		int num = (conLv - 1) % 5 + 1;
		if (num == 5)
		{
			GUIXingZuoPage.mXingZuo = Mathf.Clamp((conLv + 4) / 5 + 1, 1, 12);
		}
		else
		{
			GUIXingZuoPage.mXingZuo = Mathf.Clamp((conLv + 4) / 5, 1, 12);
		}
	}

	public void Refresh(int conLv)
	{
		if (GUIXingZuoPage.mXingZuo < 1 || GUIXingZuoPage.mXingZuo > 12)
		{
			conLv = Globals.Instance.Player.Data.ConstellationLevel;
		}
		this.ChooseCurPage(conLv);
		this.Refresh();
	}

	private void Refresh()
	{
		this.mConInfo = Globals.Instance.AttDB.ConstellationDict.GetInfo(GUIXingZuoPage.mXingZuo);
		this.mBaseScene.mGUIXingZuoPage.mUIXingZuoItem.mWaitTimeToHide = 0f;
		this.mUIXingZuoItem.Refresh(this.mConInfo);
		NGUITools.SetActive(this.mLeftBtn.gameObject, GUIXingZuoPage.mXingZuo > 1);
		NGUITools.SetActive(this.mRightBtn.gameObject, GUIXingZuoPage.mXingZuo < 12);
	}

	public void OnRightBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.mRightBtn.GetComponent<BoxCollider>().enabled = false;
		this.mDragPostion = Vector3.zero;
		this.BeginRightAnimation();
		base.StartCoroutine(this.WaitChangePageTimeRight());
		int constellationLevel = Globals.Instance.Player.Data.ConstellationLevel;
		GUIRightInfo.RefreshLightBtn(constellationLevel);
	}

	public void BeginRightAnimation()
	{
		if (GUIXingZuoPage.mXingZuo == 12)
		{
			return;
		}
		GUIXingZuoPage.mXingZuo++;
		float num = 0.2f;
		TweenPosition.Begin(this.objItem, 0f, this.mDragPostion);
		TweenPosition.Begin(this.objItem.gameObject, num, new Vector3(-700f, 0f, 0f)).method = UITweener.Method.EaseOut;
		base.Invoke("EndRightAnimation", num);
	}

	private void EndRightAnimation()
	{
		this.Refresh();
		float duration = 0.2f;
		TweenPosition.Begin(this.objItem.gameObject, 0f, new Vector3(700f, 0f, 0f));
		TweenPosition.Begin(this.objItem.gameObject, duration, Vector3.zero).method = UITweener.Method.EaseIn;
	}

	public void OnLeftBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.mLeftBtn.GetComponent<BoxCollider>().enabled = false;
		this.mDragPostion = Vector3.zero;
		this.BeginLeftAnimaton();
		base.StartCoroutine(this.WaitChangePageTimeLeft());
		int constellationLevel = Globals.Instance.Player.Data.ConstellationLevel;
		GUIRightInfo.RefreshLightBtn(constellationLevel);
	}

	public void BeginLeftAnimaton()
	{
		if (GUIXingZuoPage.mXingZuo == 1)
		{
			return;
		}
		GUIXingZuoPage.mXingZuo--;
		float num = 0.2f;
		TweenPosition.Begin(this.objItem.gameObject, 0f, this.mDragPostion);
		TweenPosition.Begin(this.objItem.gameObject, num, new Vector3(700f, 0f, 0f)).method = UITweener.Method.EaseOut;
		base.Invoke("EndLeftAnimation", num);
	}

	private void EndLeftAnimation()
	{
		this.Refresh();
		float duration = 0.2f;
		TweenPosition.Begin(this.objItem.gameObject, 0f, new Vector3(-700f, 0f, 0f));
		TweenPosition.Begin(this.objItem.gameObject, duration, Vector3.zero).method = UITweener.Method.EaseIn;
	}

	[DebuggerHidden]
	public IEnumerator WaitChangePageTimeLeft()
	{
        return null;
        //GUIXingZuoPage.<WaitChangePageTimeLeft>c__Iterator3C <WaitChangePageTimeLeft>c__Iterator3C = new GUIXingZuoPage.<WaitChangePageTimeLeft>c__Iterator3C();
        //<WaitChangePageTimeLeft>c__Iterator3C.<>f__this = this;
        //return <WaitChangePageTimeLeft>c__Iterator3C;
	}

	[DebuggerHidden]
	public IEnumerator WaitChangePageTimeRight()
	{
        return null;
        //GUIXingZuoPage.<WaitChangePageTimeRight>c__Iterator3D <WaitChangePageTimeRight>c__Iterator3D = new GUIXingZuoPage.<WaitChangePageTimeRight>c__Iterator3D();
        //<WaitChangePageTimeRight>c__Iterator3D.<>f__this = this;
        //return <WaitChangePageTimeRight>c__Iterator3D;
	}
}
