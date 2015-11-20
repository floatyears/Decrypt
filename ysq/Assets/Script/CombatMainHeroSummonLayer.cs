using Att;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class CombatMainHeroSummonLayer : MonoBehaviour
{
	public class ActorUIInfo
	{
		public ActorController Actor;

		public UIGrid BuffGrid;

		public Dictionary<int, GameObject> BuffIcons = new Dictionary<int, GameObject>();

		public GameObject OriginalBuffItem;

		public UISlider HPBar;

		public UISprite Icon;

		public UISprite RiskMask;

		public void OnIconClick(GameObject go)
		{
			Globals.Instance.CameraMgr.dynamicCam = false;
			Globals.Instance.CameraMgr.SelectActor = this.Actor;
		}
	}

	private class SummmonInfo : CombatMainHeroSummonLayer.ActorUIInfo
	{
		public GameObject InfoRoot;

		public UISprite MaskImage;

		public UISprite Quality;

		public UILabel Countdown;

		public int ResurrectTime;

		public string SummonName = string.Empty;
	}

	public enum ActorIndex
	{
		player,
		pet1,
		pet2,
		pet3,
		pet4,
		boss,
		Max
	}

	private ESceneType mCurSceneType;

	private UILabel hpBarTxt;

	private UILabel mpBarTxt;

	private UISlider mpBar;

	private long maxHP;

	private long curHP;

	private long maxMP;

	private long curMP;

	private float updateTimer;

	private float refreshTimer;

	private CombatMainHeroSummonLayer.ActorUIInfo[] actorUIInfos = new CombatMainHeroSummonLayer.ActorUIInfo[6];

	private StringBuilder sb = new StringBuilder();

	private CombatTargetInfoLayer mTargetInfoLayer;

	private CombatMainHeroSummonLayer.SummmonInfo tempInfo;

	private int BuildSummmonInfo(ESceneType type = ESceneType.EScene_World)
	{
		int num = 1;
		for (int i = 0; i < 5; i++)
		{
			ActorController actor = Globals.Instance.ActorMgr.GetActor(i + 1);
			if (!(actor == null))
			{
				CombatMainHeroSummonLayer.SummmonInfo summmonInfo = new CombatMainHeroSummonLayer.SummmonInfo();
				summmonInfo.InfoRoot = base.transform.Find(string.Format("pet{0}", num)).gameObject;
				summmonInfo.Icon = summmonInfo.InfoRoot.transform.Find("pet_pic").GetComponent<UISprite>();
				if (type == ESceneType.EScene_Arena || type == ESceneType.EScene_OrePillage || type == ESceneType.EScene_GuildPvp)
				{
					UIEventListener expr_A3 = UIEventListener.Get(summmonInfo.Icon.gameObject);
					expr_A3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_A3.onClick, new UIEventListener.VoidDelegate(summmonInfo.OnIconClick));
				}
				summmonInfo.Icon.spriteName = actor.petInfo.Icon;
				summmonInfo.BuffGrid = summmonInfo.InfoRoot.transform.Find("petbuff").GetComponent<UIGrid>();
				summmonInfo.OriginalBuffItem = summmonInfo.BuffGrid.transform.Find("buffItem").gameObject;
				summmonInfo.OriginalBuffItem.SetActive(false);
				summmonInfo.HPBar = summmonInfo.InfoRoot.transform.Find("pet_pic/hp").GetComponent<UISlider>();
				summmonInfo.HPBar.value = 1f;
				summmonInfo.MaskImage = summmonInfo.InfoRoot.transform.Find("death_mask").GetComponent<UISprite>();
				summmonInfo.MaskImage.gameObject.SetActive(false);
				summmonInfo.Countdown = summmonInfo.InfoRoot.transform.Find("Countdown").GetComponent<UILabel>();
				if (type == ESceneType.EScene_MemoryGear)
				{
					summmonInfo.Countdown.gameObject.SetActive(true);
					summmonInfo.Countdown.enabled = false;
				}
				else
				{
					summmonInfo.Countdown.gameObject.SetActive(false);
				}
				summmonInfo.RiskMask = summmonInfo.InfoRoot.transform.Find("riskMask").GetComponent<UISprite>();
				summmonInfo.Quality = summmonInfo.InfoRoot.GetComponent<UISprite>();
				summmonInfo.Quality.spriteName = Tools.GetItemQualityIcon(actor.petInfo.Quality);
				summmonInfo.Actor = actor;
				summmonInfo.SummonName = actor.GetName();
				this.actorUIInfos[i + 1] = summmonInfo;
				num++;
			}
		}
		return num;
	}

	public void SetState(int nState)
	{
		int i = 1;
		this.mCurSceneType = (ESceneType)nState;
		switch (nState)
		{
		case 0:
		case 1:
		case 6:
			i = this.BuildSummmonInfo(ESceneType.EScene_World);
			this.mTargetInfoLayer = CombatTargetInfoLayer.CreateCombatTarget("GUI/BossTargetInfo", base.transform.parent.Find("center-top").gameObject);
			this.mTargetInfoLayer.Init(this.actorUIInfos[5]);
			break;
		case 2:
		case 8:
		case 9:
		{
			i = this.BuildSummmonInfo((ESceneType)nState);
			UIEventListener expr_164 = UIEventListener.Get(this.actorUIInfos[0].Icon.gameObject);
			expr_164.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_164.onClick, new UIEventListener.VoidDelegate(this.actorUIInfos[0].OnIconClick));
			break;
		}
		case 3:
		case 5:
		{
			i = this.BuildSummmonInfo(ESceneType.EScene_World);
			string path;
			if (Globals.Instance.Player.WorldBossSystem.CurSlot == 5)
			{
				path = "GUI/WorldBossTargetInfo";
			}
			else
			{
				path = "GUI/BossTargetInfo";
			}
			this.mTargetInfoLayer = CombatTargetInfoLayer.CreateCombatTarget(path, base.transform.parent.Find("center-top").gameObject);
			this.mTargetInfoLayer.Init(this.actorUIInfos[5]);
			break;
		}
		case 4:
			i = this.BuildSummmonInfo(ESceneType.EScene_Pillage);
			break;
		case 7:
			i = this.BuildSummmonInfo((ESceneType)nState);
			this.mTargetInfoLayer = CombatTargetInfoLayer.CreateCombatTarget("GUI/BossTargetInfo", base.transform.parent.Find("center-top").gameObject);
			this.mTargetInfoLayer.Init(this.actorUIInfos[5]);
			break;
		}
		while (i < 5)
		{
			GameObject gameObject = base.transform.Find(string.Format("pet{0}", i)).gameObject;
			gameObject.SetActive(false);
			i++;
		}
		for (int j = 1; j < 5; j++)
		{
			if (this.actorUIInfos[j] != null && !(this.actorUIInfos[j].Actor == null))
			{
				for (int k = 0; k < this.actorUIInfos[j].Actor.Buffs.Count; k++)
				{
					Buff buff = this.actorUIInfos[j].Actor.Buffs[k];
					if (buff != null)
					{
						this.OnBuffAddEvent(j, buff.SerialID, buff.Info, buff.Duration, buff.StackCount);
					}
				}
			}
		}
	}

	public void UpdatePlayerHPMP()
	{
		if (this.actorUIInfos[0] == null || this.actorUIInfos[0].Actor == null)
		{
			this.actorUIInfos[0].RiskMask.gameObject.SetActive(false);
			return;
		}
		if (this.maxHP == 0L || this.maxHP != this.actorUIInfos[0].Actor.MaxHP || this.curHP != this.actorUIInfos[0].Actor.CurHP)
		{
			this.maxHP = this.actorUIInfos[0].Actor.MaxHP;
			this.curHP = this.actorUIInfos[0].Actor.CurHP;
			float num = (float)this.curHP / (float)this.maxHP;
			this.actorUIInfos[0].HPBar.value = num;
			this.sb.Remove(0, this.sb.Length);
			this.sb.Append(this.curHP).Append("/").Append(this.maxHP);
			this.hpBarTxt.text = this.sb.ToString();
			this.actorUIInfos[0].RiskMask.gameObject.SetActive(num <= 0.2f && 0f < num);
		}
		if (this.maxMP == 0L || this.maxMP != this.actorUIInfos[0].Actor.MaxMP || this.curMP != this.actorUIInfos[0].Actor.CurMP)
		{
			this.maxMP = this.actorUIInfos[0].Actor.MaxMP;
			this.curMP = this.actorUIInfos[0].Actor.CurMP;
			this.mpBar.value = (float)this.curMP / (float)this.maxMP;
			this.sb.Remove(0, this.sb.Length);
			this.sb.Append(this.curMP).Append("/").Append(this.maxMP);
			this.mpBarTxt.text = this.sb.ToString();
		}
	}

	public void UpdateHPBar(int slot)
	{
		if (slot < 1 || slot >= 5 || this.actorUIInfos[slot] == null)
		{
			return;
		}
		if (this.actorUIInfos[slot].Actor == null)
		{
			this.actorUIInfos[slot].RiskMask.gameObject.SetActive(false);
			return;
		}
		float num = (float)this.actorUIInfos[slot].Actor.CurHP / (float)this.actorUIInfos[slot].Actor.MaxHP;
		this.actorUIInfos[slot].HPBar.value = num;
		this.actorUIInfos[slot].RiskMask.gameObject.SetActive(num <= 0.2f && 0f < num);
	}

	public void SetSummonDeath(int slot, bool bDeath)
	{
		if (slot < 1 || slot >= 5 || this.actorUIInfos[slot] == null)
		{
			return;
		}
		this.tempInfo = (CombatMainHeroSummonLayer.SummmonInfo)this.actorUIInfos[slot];
		if (bDeath && !this.tempInfo.MaskImage.gameObject.activeInHierarchy)
		{
			GameUIManager.mInstance.ShowGameNewPopUp(Singleton<StringManager>.Instance.GetString("summonKilledNew", new object[]
			{
				this.tempInfo.SummonName
			}), 1f, 50f, 0.25f);
			if (this.mCurSceneType == ESceneType.EScene_MemoryGear)
			{
				this.tempInfo.ResurrectTime = 20;
				this.tempInfo.Countdown.enabled = true;
			}
		}
		if (this.tempInfo.MaskImage.gameObject.activeInHierarchy != bDeath)
		{
			this.tempInfo.MaskImage.gameObject.SetActive(bDeath);
			if (this.mCurSceneType == ESceneType.EScene_MemoryGear)
			{
				if (Globals.Instance.ActorMgr.Actors[0].IsDead)
				{
					this.tempInfo.Countdown.enabled = false;
				}
				else
				{
					this.tempInfo.Countdown.enabled = bDeath;
				}
			}
		}
		if (bDeath)
		{
			this.tempInfo.RiskMask.gameObject.SetActive(false);
		}
	}

	private void Awake()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.actorUIInfos[0] = new CombatMainHeroSummonLayer.SummmonInfo();
		this.actorUIInfos[0].Icon = base.transform.Find("stat_bm/icon").GetComponent<UISprite>();
		this.actorUIInfos[0].Icon.spriteName = Globals.Instance.Player.GetPlayerIcon();
		this.actorUIInfos[0].RiskMask = base.transform.Find("stat_bm/riskMask").GetComponent<UISprite>();
		this.actorUIInfos[0].BuffGrid = base.transform.Find("stat_bm/herobuff").GetComponent<UIGrid>();
		this.actorUIInfos[0].OriginalBuffItem = this.actorUIInfos[0].BuffGrid.transform.Find("buffItem").gameObject;
		this.actorUIInfos[0].OriginalBuffItem.SetActive(false);
		this.hpBarTxt = base.transform.Find("stat_bm/headBg2/hpMpBg/hpHero/hptxt").GetComponent<UILabel>();
		this.mpBarTxt = base.transform.Find("stat_bm/headBg2/hpMpBg/mpHero/mptxt").GetComponent<UILabel>();
		this.actorUIInfos[0].HPBar = base.transform.Find("stat_bm/headBg2/hpMpBg/hpHero").GetComponent<UISlider>();
		this.mpBar = base.transform.Find("stat_bm/headBg2/hpMpBg/mpHero").GetComponent<UISlider>();
		this.actorUIInfos[0].HPBar.value = 1f;
		this.mpBar.value = 1f;
		this.actorUIInfos[0].Actor = Globals.Instance.ActorMgr.GetActor(0);
		if (this.actorUIInfos[0].Actor != null)
		{
			UILabel component = base.transform.Find("stat_bm/headBg2/headBg/stat_lv").GetComponent<UILabel>();
			component.text = this.actorUIInfos[0].Actor.Level.ToString();
		}
		this.actorUIInfos[5] = new CombatMainHeroSummonLayer.SummmonInfo();
		for (int i = 0; i < 6; i++)
		{
			if (this.actorUIInfos[i] != null && !(this.actorUIInfos[i].Actor == null))
			{
				for (int j = 0; j < this.actorUIInfos[i].Actor.Buffs.Count; j++)
				{
					Buff buff = this.actorUIInfos[i].Actor.Buffs[j];
					if (buff != null)
					{
						this.OnBuffAddEvent(i, buff.SerialID, buff.Info, buff.Duration, buff.StackCount);
					}
				}
			}
		}
	}

	public void OnBuffAddEvent(int slot, int serialID, BuffInfo info, float duration, int stackCount)
	{
		if (slot < 0)
		{
			slot = 5;
		}
		if (slot >= 6)
		{
			return;
		}
		if (string.IsNullOrEmpty(info.Icon))
		{
			return;
		}
		GameObject gameObject = null;
		if (!this.actorUIInfos[slot].BuffIcons.TryGetValue(serialID, out gameObject))
		{
			this.actorUIInfos[slot].OriginalBuffItem.SetActive(true);
			gameObject = Tools.AddChild(this.actorUIInfos[slot].BuffGrid.gameObject, this.actorUIInfos[slot].OriginalBuffItem);
			this.actorUIInfos[slot].OriginalBuffItem.SetActive(false);
			UISprite component = gameObject.transform.Find("buffImage").GetComponent<UISprite>();
			component.spriteName = info.Icon;
			UISprite component2 = component.gameObject.transform.Find("buffCD").GetComponent<UISprite>();
			if (info.MaxDuration <= 0f)
			{
				component2.fillAmount = 0f;
			}
			else
			{
				component2.gameObject.AddComponent<GameUIAutoCoolTimer>().PlayCool(component2, info.MaxDuration - duration, info.MaxDuration);
			}
			this.actorUIInfos[slot].BuffIcons.Add(serialID, gameObject);
			this.actorUIInfos[slot].BuffGrid.repositionNow = true;
		}
		else
		{
			Transform transform = gameObject.transform.Find("buffImage/buffCD");
			if (transform != null)
			{
				GameUIAutoCoolTimer component3 = transform.GetComponent<GameUIAutoCoolTimer>();
				if (component3 != null)
				{
					component3.PlayCool(transform.GetComponent<UISprite>(), info.MaxDuration - duration, info.MaxDuration);
				}
			}
		}
	}

	public void OnBuffRemoveEvent(int slot, int serialID)
	{
		if (slot < 0)
		{
			slot = 5;
		}
		if (slot >= 6)
		{
			return;
		}
		GameObject obj = null;
		if (this.actorUIInfos[slot].BuffIcons.TryGetValue(serialID, out obj))
		{
			this.actorUIInfos[slot].BuffIcons.Remove(serialID);
			NGUITools.Destroy(obj);
			this.actorUIInfos[slot].BuffGrid.repositionNow = true;
		}
	}

	private void Update()
	{
		this.updateTimer += Time.deltaTime;
		this.refreshTimer += Time.deltaTime;
		if (this.mCurSceneType == ESceneType.EScene_MemoryGear && this.refreshTimer >= 1f)
		{
			this.refreshTimer = 0f;
			for (int i = 1; i < 5; i++)
			{
				if (this.actorUIInfos[i] != null && this.actorUIInfos[i].Actor.IsDead)
				{
					CombatMainHeroSummonLayer.SummmonInfo summmonInfo = (CombatMainHeroSummonLayer.SummmonInfo)this.actorUIInfos[i];
					summmonInfo.ResurrectTime--;
					if (summmonInfo.ResurrectTime < 0)
					{
						summmonInfo.Countdown.text = string.Empty;
					}
					else
					{
						summmonInfo.Countdown.text = string.Format("{0}s", summmonInfo.ResurrectTime);
					}
				}
			}
		}
		if (this.updateTimer < 0.5f)
		{
			return;
		}
		this.updateTimer = 0f;
		this.UpdatePlayerHPMP();
		for (int j = 1; j < 5; j++)
		{
			if (this.actorUIInfos[j] != null)
			{
				if (this.actorUIInfos[j].Actor == null || this.actorUIInfos[j].Actor.IsDead)
				{
					this.actorUIInfos[j].HPBar.value = 0f;
					this.SetSummonDeath(j, true);
				}
				else
				{
					this.SetSummonDeath(j, false);
					this.UpdateHPBar(j);
				}
			}
		}
	}

	public void OnResurrect()
	{
		for (int i = 0; i <= 4; i++)
		{
			if (this.actorUIInfos[i] != null && !(this.actorUIInfos[i].Actor != null))
			{
				this.actorUIInfos[i].Actor = Globals.Instance.ActorMgr.GetActor(i);
				if (!(this.actorUIInfos[i].Actor == null))
				{
					this.SetSummonDeath(i, false);
					for (int j = 0; j < this.actorUIInfos[i].Actor.Buffs.Count; j++)
					{
						Buff buff = this.actorUIInfos[i].Actor.Buffs[j];
						if (buff != null)
						{
							this.OnBuffAddEvent(i, buff.SerialID, buff.Info, buff.Duration, buff.StackCount);
						}
					}
				}
			}
		}
	}
}
