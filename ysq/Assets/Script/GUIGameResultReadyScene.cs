using Holoville.HOTween;
using Holoville.HOTween.Core;
using System;
using UnityEngine;

public class GUIGameResultReadyScene : GameUISession
{
	private bool isPvp;

	private UISprite mVictorySp;

	private UISprite mBg;

	private GameObject mEffectGo;

	protected override void OnPostLoadGUI()
	{
		if (GameUIPopupManager.GetInstance().GetCurrentPopup() != null)
		{
			GameUIPopupManager.GetInstance().PopState(true, null);
		}
		GUIChatWindowV2.TryCloseMe();
		GameUIManager.mInstance.DestroyGameUIOptionPopUp();
		this.mEffectGo = base.transform.Find("endBackground/ui04").gameObject;
		NGUITools.SetActive(this.mEffectGo, false);
		this.mBg = base.transform.Find("endBackground/BackGround").GetComponent<UISprite>();
		this.mVictorySp = base.transform.Find("endBackground/Sprite").GetComponent<UISprite>();
		this.mVictorySp.transform.localScale = new Vector3(6f, 6f, 6f);
		this.mVictorySp.color = new Color(this.mVictorySp.color.r, this.mVictorySp.color.g, this.mVictorySp.color.b, 0.3f);
		Sequence sequence = new Sequence(new SequenceParms().UpdateType(UpdateType.TimeScaleIndependentUpdate));
		sequence.Append(HOTween.To(this.mVictorySp.transform, 0.2f, new TweenParms().Prop("localScale", Vector3.one).Ease(EaseType.EaseOutSine)));
		sequence.Insert(0f, HOTween.To(this.mVictorySp, 0.2f, new TweenParms().Prop("color", new Color(this.mVictorySp.color.r, this.mVictorySp.color.g, this.mVictorySp.color.b, 1f))));
		sequence.AppendCallback(new TweenDelegate.TweenCallback(this.ShowEffectGo));
		sequence.AppendInterval(1.5f);
		sequence.Append(HOTween.To(this.mBg, 1f, new TweenParms().Prop("color", new Color(this.mBg.color.r, this.mBg.color.g, this.mBg.color.b, 0f))));
		sequence.Insert(2f, HOTween.To(this.mVictorySp, 1f, new TweenParms().Prop("color", new Color(this.mVictorySp.color.r, this.mVictorySp.color.g, this.mVictorySp.color.b, 0f))));
		sequence.Play();
		this.isPvp = GameUIManager.mInstance.uiState.IsPvp;
		if (!this.isPvp)
		{
			base.Invoke("ReservedReadyProcess", 1.5f);
		}
		Globals.Instance.BackgroundMusicMgr.PlayGameClearSound();
		Globals.Instance.BackgroundMusicMgr.StopWarmingSound();
		Globals.Instance.BackgroundMusicMgr.ClearGameBGM();
	}

	private void ShowEffectGo()
	{
		NGUITools.SetActive(this.mEffectGo, true);
	}

	protected override void OnPreDestroyGUI()
	{
		if (!this.isPvp)
		{
			Globals.Instance.BackgroundMusicMgr.StopGameClearSound();
		}
	}

	private void ReservedReadyProcess()
	{
		Globals.Instance.SenceMgr.CloseScene();
		GameUIState uiState = GameUIManager.mInstance.uiState;
		if (uiState.AdventureSceneInfo != null && uiState.AdventureSceneInfo.Type == 6)
		{
			GUIKingRewardResultScene.ShowKingRewardResult(uiState.PveResult);
		}
		else if (uiState.AdventureSceneInfo != null && uiState.AdventureSceneInfo.Type == 7)
		{
			GUIGuardResultScene.ShowResult(uiState.PveResult);
		}
		else
		{
			GameUIManager.mInstance.ChangeSession<GUIGameResultVictoryScene>(null, false, false);
		}
	}
}
