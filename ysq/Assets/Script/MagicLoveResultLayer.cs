using Holoville.HOTween;
using Holoville.HOTween.Core;
using System;
using UnityEngine;

public class MagicLoveResultLayer : MonoBehaviour
{
	private GUIMagicLoveScene mBaseScene;

	private UIPanel mPanel;

	private Transform mDraw;

	private Transform mLose;

	private Transform mWin;

	private UILabel mWinValue;

	private GameObject mUI57_4;

	private MagicLoveSubSystem.ELastResult result;

	private Sequence anim;

	public void Init(GUIMagicLoveScene basescene)
	{
		this.mBaseScene = basescene;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mPanel = base.GetComponent<UIPanel>();
		this.mDraw = GameUITools.FindGameObject("Draw", base.gameObject).transform;
		this.mLose = GameUITools.FindGameObject("Lose", base.gameObject).transform;
		this.mWin = GameUITools.FindGameObject("Win", base.gameObject).transform;
		this.mUI57_4 = GameUITools.FindGameObject("ui57_4", this.mWin.gameObject);
		Tools.SetParticleRenderQueue2(this.mUI57_4, 3050);
		this.mUI57_4.SetActive(false);
		this.mWinValue = GameUITools.FindUILabel("Value", this.mWin.gameObject);
	}

	public void Play(MagicLoveSubSystem.ELastResult result, int value = 0)
	{
		this.result = result;
		this.mUI57_4.SetActive(false);
		this.mDraw.gameObject.SetActive(false);
		this.mLose.gameObject.SetActive(false);
		this.mWin.gameObject.SetActive(false);
		base.gameObject.SetActive(true);
		switch (result)
		{
		case MagicLoveSubSystem.ELastResult.ELR_Win:
			this.mWinValue.text = value.ToString();
			this.PlaySequence(this.mWin);
			Globals.Instance.EffectSoundMgr.Play("ui/ui_009");
			break;
		case MagicLoveSubSystem.ELastResult.ELR_Draw:
			this.PlaySequence(this.mDraw);
			break;
		case MagicLoveSubSystem.ELastResult.ELR_Lose:
			this.PlaySequence(this.mLose);
			break;
		}
	}

	private void PlaySequence(Transform t)
	{
		this.mPanel.alpha = 0f;
		t.localPosition = new Vector3(0f, -113f, t.localPosition.z);
		t.gameObject.SetActive(true);
		this.anim = new Sequence(new SequenceParms().OnComplete(new TweenDelegate.TweenCallback(this.OnCompleteEvent)));
		this.anim.Append(HOTween.To(t, 0.42f, new TweenParms().Prop("localPosition", new Vector3(0f, 0f, t.localPosition.z))));
		this.anim.Insert(0f, HOTween.To(this.mPanel, 0.42f, new TweenParms().Prop("alpha", 1).OnComplete(new TweenDelegate.TweenCallback(this.OnCenter))));
		this.anim.AppendInterval(1.3f);
		this.anim.AppendCallback(new TweenDelegate.TweenCallback(this.OnCenterEnd));
		this.anim.Append(HOTween.To(t, 0.42f, new TweenParms().Prop("localPosition", new Vector3(0f, 150f, t.localPosition.z))));
		this.anim.Insert(1.71999991f, HOTween.To(this.mPanel, 0.42f, new TweenParms().Prop("alpha", 0)));
		this.anim.Play();
	}

	private void OnDestroy()
	{
		if (this.anim != null)
		{
			this.anim.Kill();
			this.anim = null;
		}
	}

	private void OnCompleteEvent()
	{
		base.gameObject.SetActive(false);
		this.mBaseScene.Refresh();
		this.mBaseScene.TryPlayFinishAnim();
	}

	private void OnCenter()
	{
		if (this.result == MagicLoveSubSystem.ELastResult.ELR_Win)
		{
			this.mUI57_4.SetActive(false);
			this.mUI57_4.SetActive(true);
		}
	}

	private void OnCenterEnd()
	{
		if (this.result == MagicLoveSubSystem.ELastResult.ELR_Win)
		{
			this.mUI57_4.SetActive(false);
		}
	}
}
