using Proto;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class GUICommonRewardInfo : MonoBehaviour
{
	public GUIReward mBaseScene;

	public GUIRewardCheckBtn mCheckBtn;

	private UIPanel mPanel;

	private UISprite mGo;

	private UILabel mTitle;

	private UILabel mContent;

	private UISprite mLine;

	private UIScrollBar mScrollBar;

	public void InitWithBaseScene(GUIReward basescene)
	{
		this.mBaseScene = basescene;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mPanel = GameUITools.FindGameObject("rewardPanel", base.gameObject).GetComponent<UIPanel>();
		this.mGo = GameUITools.RegisterClickEvent("Go", new UIEventListener.VoidDelegate(this.OnGoBtnClick), base.gameObject).GetComponent<UISprite>();
		GameObject parent = GameUITools.FindGameObject("rewardPanel/Contents", base.gameObject);
		this.mTitle = GameUITools.FindUILabel("Title", parent);
		this.mContent = GameUITools.FindUILabel("Content", parent);
		this.mLine = GameUITools.FindUISprite("line", base.gameObject);
		this.mScrollBar = GameUITools.FindGameObject("scrollBar", base.gameObject).GetComponent<UIScrollBar>();
		this.mGo.gameObject.SetActive(false);
		base.StartCoroutine(this.ShowBar());
	}

	public void Refresh(GUIRewardCheckBtn btn)
	{
		this.mCheckBtn = btn;
		if (this.mCheckBtn.ActivateObj == null)
		{
			this.mCheckBtn.ActivateObj = base.gameObject;
		}
		this.GetActivityDesc(this.mCheckBtn.ActivityType);
	}

	public void GetActivityDesc(EActivityType type)
	{
		MC2S_GetActivityDesc mC2S_GetActivityDesc = new MC2S_GetActivityDesc();
		mC2S_GetActivityDesc.Type = (int)type;
		Globals.Instance.CliSession.Send(743, mC2S_GetActivityDesc);
	}

	public void OnGetActivityDescEvent()
	{
		MS2C_GetActivityDesc activityDescData = Globals.Instance.Player.ActivitySystem.GetActivityDescData(this.mCheckBtn.ActivityType);
		if (activityDescData == null)
		{
			this.mTitle.text = string.Empty;
			this.mContent.text = string.Empty;
			this.mGo.gameObject.SetActive(false);
			this.mLine.gameObject.SetActive(false);
			return;
		}
		if (this.mCheckBtn.ActivityType == EActivityType.EAT_LevelRank && this.mCheckBtn.ActivityType == EActivityType.EAT_VipLevel)
		{
			this.mGo.gameObject.SetActive(true);
			this.mLine.gameObject.SetActive(true);
			this.mPanel.bottomAnchor.absolute = 79;
		}
		else
		{
			this.mGo.gameObject.SetActive(false);
			this.mLine.gameObject.SetActive(false);
			this.mPanel.bottomAnchor.absolute = 14;
		}
		this.mTitle.text = this.mCheckBtn.Text;
		this.mContent.text = activityDescData.Content;
		this.mScrollBar.value = 0f;
	}

	[DebuggerHidden]
	private IEnumerator ShowBar()
	{
        return null;
        //GUICommonRewardInfo.<ShowBar>c__Iterator2D <ShowBar>c__Iterator2D = new GUICommonRewardInfo.<ShowBar>c__Iterator2D();
        //<ShowBar>c__Iterator2D.<>f__this = this;
        //return <ShowBar>c__Iterator2D;
	}

	private void OnGoBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.mCheckBtn.AAData != null)
		{
			return;
		}
		EActivityType activityType = this.mCheckBtn.ActivityType;
		if (activityType != EActivityType.EAT_LevelRank)
		{
			if (activityType == EActivityType.EAT_VipLevel)
			{
				GameUIVip.OpenRecharge();
			}
		}
		else
		{
			GameUIManager.mInstance.ChangeSession<GUIWorldMap>(null, false, true);
		}
	}
}
