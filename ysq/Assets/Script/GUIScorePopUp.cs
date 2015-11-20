using Proto;
using System;
using UnityEngine;

public class GUIScorePopUp : GameUISession
{
	private Transform mWaitBtn;

	private Transform mGoScoreBtn;

	private Transform mNoRemindBtn;

	private UILabel mTitle;

	private UILabel mDesc;

	private string url;

	public static void ShowScorePopUp()
	{
		if (GameUIManager.mInstance.uiState.CommentData == null)
		{
			return;
		}
		GameUIManager.mInstance.CreateSession<GUIScorePopUp>(null);
	}

	protected override void OnPostLoadGUI()
	{
		this.CreateObjects();
	}

	protected override void OnPreDestroyGUI()
	{
	}

	private void CreateObjects()
	{
		MS2C_CommentMsg commentData = GameUIManager.mInstance.uiState.CommentData;
		if (commentData == null)
		{
			base.Close();
			return;
		}
		GameUIManager.mInstance.uiState.CommentData = null;
		Transform transform = base.transform.Find("Panel");
		this.mWaitBtn = transform.Find("waitBtn");
		this.mGoScoreBtn = transform.Find("goScoreBtn");
		this.mNoRemindBtn = transform.Find("noRemindBtn");
		this.mTitle = transform.Find("title").GetComponent<UILabel>();
		this.mDesc = transform.Find("desc").GetComponent<UILabel>();
		UIEventListener expr_AD = UIEventListener.Get(this.mWaitBtn.gameObject);
		expr_AD.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_AD.onClick, new UIEventListener.VoidDelegate(this.OnWaitBtnClick));
		UIEventListener expr_DE = UIEventListener.Get(this.mGoScoreBtn.gameObject);
		expr_DE.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_DE.onClick, new UIEventListener.VoidDelegate(this.OnGoScoreBtnClick));
		UIEventListener expr_10F = UIEventListener.Get(this.mNoRemindBtn.gameObject);
		expr_10F.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_10F.onClick, new UIEventListener.VoidDelegate(this.OnNoRemindBtnClick));
		Transform transform2 = transform.Find("close");
		UIEventListener expr_147 = UIEventListener.Get(transform2.gameObject);
		expr_147.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_147.onClick, new UIEventListener.VoidDelegate(this.OnCloseBtnClick));
		this.mTitle.text = commentData.Title;
		this.mDesc.text = commentData.Content;
		this.url = commentData.Url;
		this.mNoRemindBtn.gameObject.SetActive(commentData.CloseComment);
	}

	private void OnCloseBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		base.Close();
	}

	private void OnWaitBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		base.Close();
	}

	private void OnNoRemindBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		MC2S_CloseComment ojb = new MC2S_CloseComment();
		Globals.Instance.CliSession.Send(297, ojb);
		base.Close();
	}

	private void OnGoScoreBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (string.IsNullOrEmpty(this.url))
		{
			return;
		}
		Application.OpenURL(this.url);
		base.Close();
	}
}
