using Holoville.HOTween;
using Proto;
using System;
using UnityEngine;

public class CombatMainWavePanel : MonoBehaviour
{
	private UILabel mWaveNum;

	private UILabel mScoreNum;

	private UILabel mWaveBoxNumLb;

	private int mWaveBoxNum;

	private Transform mWaveBoxSp;

	public void InitWithBaseScene()
	{
		this.CreateObjects();
		TrialScene trialScene = Globals.Instance.ActorMgr.TrialScene;
		if (trialScene != null)
		{
			this.mWaveNum.text = Singleton<StringManager>.Instance.GetString("trailTower9", new object[]
			{
				Mathf.Min(GameConst.GetInt32(187), trialScene.CurWave)
			});
			this.mScoreNum.text = "0";
		}
	}

	private void CreateObjects()
	{
		this.mWaveNum = base.transform.Find("waveBg/waveNum").GetComponent<UILabel>();
		this.mScoreNum = base.transform.Find("scoreNum").GetComponent<UILabel>();
		this.mWaveBoxNumLb = base.transform.Find("waveBoxNum").GetComponent<UILabel>();
		this.mWaveBoxNumLb.text = "x0";
		this.mWaveBoxNum = 0;
		this.mWaveBoxSp = base.transform.Find("waveBox");
	}

	public void OnTrialScoreEvent(int wave, int score)
	{
		this.mWaveNum.text = Singleton<StringManager>.Instance.GetString("trailTower9", new object[]
		{
			Mathf.Min(GameConst.GetInt32(187), wave)
		});
		ActivityValueData valueMod = Globals.Instance.Player.ActivitySystem.GetValueMod(4);
		if (valueMod != null)
		{
			score *= valueMod.Value1 / 100;
		}
		this.mScoreNum.text = score.ToString();
	}

	public void RefreshBoxNum()
	{
		this.mWaveBoxNum++;
		this.mWaveBoxNumLb.text = string.Format("x{0}", this.mWaveBoxNum);
		HOTween.Shake(this.mWaveBoxSp, 3f, "localRotation", Quaternion.Euler(0f, 0f, 30f), 0.1f, 0.12f);
	}
}
