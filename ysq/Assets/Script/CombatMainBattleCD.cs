using Holoville.HOTween;
using System;
using UnityEngine;

public class CombatMainBattleCD : MonoBehaviour
{
	private delegate int GetRemainTimeDelegate();

	private UILabel mCDTimeTxt;

	private UISprite mCDSprite;

	private Transform mExpandBtn;

	private float timerRefresh;

	private CombatMainBattleCD.GetRemainTimeDelegate GetRemainTime;

	private UIEventListener eventListener;

	private bool expand;

	public void InitWithBaseScene()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mCDSprite = base.transform.Find("cdSprite").GetComponent<UISprite>();
		this.mCDSprite.spriteName = "Countdown";
		this.mExpandBtn = base.transform.Find("ExpandBtn");
		this.mExpandBtn.gameObject.SetActive(false);
		if (HOTween.IsTweening(this.mCDSprite))
		{
			HOTween.Kill(this.mCDSprite);
		}
		this.mCDTimeTxt = base.transform.Find("cdTimeTxt").GetComponent<UILabel>();
		this.mCDTimeTxt.color = new Color(0.9843137f, 0.996078432f, 0.266666681f, 1f);
		this.eventListener = UIEventListener.Get(base.gameObject);
		this.expand = false;
	}

	public void SetState(int nState)
	{
		this.GetRemainTime = null;
		this.eventListener.onClick = null;
		switch (nState)
		{ 
		case 0:
			this.GetRemainTime = (CombatMainBattleCD.GetRemainTimeDelegate)Delegate.Combine(this.GetRemainTime, new CombatMainBattleCD.GetRemainTimeDelegate(this.GetSceneRemainTime));
			break;
		case 1:
			this.GetRemainTime = (CombatMainBattleCD.GetRemainTimeDelegate)Delegate.Combine(this.GetRemainTime, new CombatMainBattleCD.GetRemainTimeDelegate(this.GetSceneRemainTime));
			break;
		case 3:
		{
			WorldBossCombatRank instance = WorldBossCombatRank.GetInstance();
			instance.Init(base.transform, new Vector4(-270f, -6f, -115f, -435f), true, "worldBoss");
			this.GetRemainTime = (CombatMainBattleCD.GetRemainTimeDelegate)Delegate.Combine(this.GetRemainTime, new CombatMainBattleCD.GetRemainTimeDelegate(this.GetWorldBossRemainTime));
			UIEventListener expr_B8 = this.eventListener;
			expr_B8.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_B8.onClick, new UIEventListener.VoidDelegate(this.OnWorldBossBattleCDClicked));
			UIEventListener expr_E9 = UIEventListener.Get(this.mExpandBtn.gameObject);
			expr_E9.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_E9.onClick, new UIEventListener.VoidDelegate(this.OnWorldBossBattleCDClicked));
			this.mExpandBtn.gameObject.SetActive(true);
			this.expand = true;
			break;
		}
		case 5:
			this.GetRemainTime = (CombatMainBattleCD.GetRemainTimeDelegate)Delegate.Combine(this.GetRemainTime, new CombatMainBattleCD.GetRemainTimeDelegate(this.GetSceneRemainTime));
			break;
		}
	}

	private void Update()
	{
		if (Time.time - this.timerRefresh > 1f)
		{
			this.timerRefresh = Time.time;
			this.Refresh();
		}
	}

	private int GetSceneRemainTime()
	{
		if (Globals.Instance.ActorMgr.CurScene != null)
		{
			return (int)Globals.Instance.ActorMgr.CurScene.GetCombatTimer();
		}
		return 0;
	}

	private int GetWorldBossRemainTime()
	{
		int timeStamp = Globals.Instance.Player.GetTimeStamp();
		int timeStamp2 = Globals.Instance.Player.WorldBossSystem.TimeStamp;
		int num = timeStamp2 - timeStamp;
		if (num < 0)
		{
			num = 0;
		}
		return num;
	}

	private void Refresh()
	{
		int num = this.GetRemainTime();
		if ((float)num < 1.401298E-45f)
		{
			return;
		}
		this.mCDTimeTxt.text = UIEnergyTooltip.FormatTime(num);
		if (num == 10)
		{
			this.mCDTimeTxt.color = Color.red;
			this.mCDSprite.spriteName = "Countdown_red";
			HOTween.To(this.mCDSprite, 0.5f, new TweenParms().Prop("color", new Color(this.mCDSprite.color.r, this.mCDSprite.color.g, this.mCDSprite.color.b, 0.5f)).Loops(-1, LoopType.Yoyo));
		}
	}

	public void OnWorldBossBattleCDClicked(GameObject go)
	{
		this.expand = !this.expand;
		if (this.expand)
		{
			WorldBossCombatRank.GetInstance().ShowCombatRank();
			this.mExpandBtn.localRotation = Quaternion.Euler(0f, 0f, -180f);
			this.mExpandBtn.localPosition = new Vector3(-23f, -193f, 0f);
		}
		else
		{
			WorldBossCombatRank.GetInstance().HideCombatRank();
			this.mExpandBtn.localRotation = Quaternion.identity;
			this.mExpandBtn.localPosition = new Vector3(0f, -23f, 0f);
		}
	}
}
