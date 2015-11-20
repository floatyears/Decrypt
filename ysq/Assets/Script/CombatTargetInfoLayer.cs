using System;
using UnityEngine;

public class CombatTargetInfoLayer : MonoBehaviour
{
	private Transform winBg;

	private GameObject targetEffect;

	private UILabel targetNameLabel;

	private float effectShowTimer;

	private UISlider HPBar1;

	private UISlider HPBar2;

	private UILabel HPLabel;

	private long lastHP;

	private long lastMaxHP;

	private long cacheHP;

	private long curHP;

	private float cacheFactor;

	private CombatMainHeroSummonLayer.ActorUIInfo actorUIInfo;

	private void CreateObjects()
	{
		this.winBg = base.transform.Find("winBg");
		this.targetNameLabel = this.winBg.Find("bossName").GetComponent<UILabel>();
		this.targetEffect = this.winBg.Find("bgEffect").gameObject;
	}

	public static CombatTargetInfoLayer CreateCombatTarget(string path, GameObject parent)
	{
		GameObject prefab = Res.LoadGUI(path);
		GameObject gameObject = NGUITools.AddChild(parent, prefab);
		return gameObject.AddComponent<CombatTargetInfoLayer>();
	}

	public void Init(CombatMainHeroSummonLayer.ActorUIInfo actorUI)
	{
		this.actorUIInfo = actorUI;
		this.CreateObjects();
		this.actorUIInfo.HPBar = this.winBg.Find("HpBg/HpFore").GetComponent<UISlider>();
		this.actorUIInfo.BuffGrid = this.winBg.Find("targetBuff").GetComponent<UIGrid>();
		this.actorUIInfo.OriginalBuffItem = this.actorUIInfo.BuffGrid.transform.Find("buffItem").gameObject;
		this.actorUIInfo.OriginalBuffItem.SetActive(false);
		this.HPBar1 = this.winBg.Find("HpBg/HpFore1").GetComponent<UISlider>();
		this.HPBar1.gameObject.SetActive(false);
		this.HPBar2 = this.winBg.Find("HpBg/HpFore2").GetComponent<UISlider>();
		this.HPBar2.gameObject.SetActive(false);
		this.HPLabel = this.winBg.Find("HpBg/Label").GetComponent<UILabel>();
		this.HPLabel.gameObject.SetActive(false);
		this.winBg.gameObject.SetActive(false);
		this.InitMutiHP();
	}

	private void InitMutiHP()
	{
		this.lastMaxHP = 0L;
		this.lastHP = 0L;
		this.cacheHP = 0L;
		this.curHP = 0L;
		this.HPBar1.gameObject.SetActive(true);
	}

	private void UpdateHPBarValue(long HPCount)
	{
		if (this.curHP == this.lastHP)
		{
			this.cacheFactor = 0f;
			this.cacheHP = this.lastHP;
			return;
		}
		this.cacheFactor += 2f * Time.deltaTime;
		if (this.cacheFactor > 1f)
		{
			this.cacheFactor = 1f;
		}
		float num = Mathf.Pow(2f, 5f * (this.cacheFactor - 1f));
		float num2 = (float)this.cacheHP + (float)(this.lastHP - this.cacheHP) * num;
		long num3 = this.curHP;
		this.curHP = (long)num2;
		if (this.curHP == 0L)
		{
			this.winBg.gameObject.SetActive(false);
			NGUITools.SetActive(this.targetEffect, false);
			return;
		}
		long num4;
		long num5;
		long num6;
		float value;
		this.GetCurHPValues(this.curHP, this.lastMaxHP, HPCount, out num4, out num5, out num6, out value);
		long num7 = (num3 + num4 - 1L) / num4;
		if (num7 != num5)
		{
			this.SetHPForeground(num5, HPCount);
		}
		long num8 = (this.lastHP + num4 - 1L) / num4;
		if (num8 == num5)
		{
			this.actorUIInfo.HPBar.gameObject.SetActive(true);
			long num9 = this.lastHP % num4;
			float value2 = (float)num9 / (float)num4;
			this.actorUIInfo.HPBar.value = value2;
		}
		else
		{
			this.actorUIInfo.HPBar.gameObject.SetActive(false);
		}
		this.HPBar1.value = value;
	}

	private void GetCurHPValues(long currentHP, long maxHP, long HPCount, out long perHp, out long fraHp, out long remHp, out float value)
	{
		if (HPCount <= 0L)
		{
			HPCount = 1L;
		}
		perHp = (maxHP + HPCount - 1L) / HPCount;
		remHp = currentHP % perHp;
		fraHp = (currentHP + perHp - 1L) / perHp;
		value = ((remHp == 0L) ? 1f : ((float)remHp / (float)perHp));
	}

	private void SetHPForeground(long fraHp, long hpCount)
	{
		if (fraHp > hpCount)
		{
			fraHp = hpCount;
		}
		if (hpCount > 1L)
		{
			this.HPLabel.text = string.Format("{0}x", fraHp);
			this.HPLabel.gameObject.SetActive(true);
		}
		else
		{
			this.HPLabel.gameObject.SetActive(false);
		}
		long num;
		if (fraHp > 1L)
		{
			num = (fraHp + 3L) % 4L;
			if (num == 0L)
			{
				num = 4L;
			}
		}
		else
		{
			num = 0L;
		}
		UISprite uISprite = this.actorUIInfo.HPBar.foregroundWidget as UISprite;
		uISprite.spriteName = string.Format("bossHp{0}", num);
		UISprite uISprite2 = this.HPBar1.foregroundWidget as UISprite;
		uISprite2.spriteName = uISprite.spriteName;
		if (num > 0L)
		{
			long num2 = num - 1L;
			if (fraHp > 2L)
			{
				num2 %= 4L;
				if (num2 == 0L)
				{
					num2 = 4L;
				}
			}
			UISprite uISprite3 = this.HPBar2.foregroundWidget as UISprite;
			uISprite3.spriteName = string.Format("bossHp{0}", num2);
			this.HPBar2.gameObject.SetActive(true);
		}
		else
		{
			this.HPBar2.gameObject.SetActive(false);
		}
	}

	public void SetTargetHPBar(long currentHP, long maxHP, long HPCount)
	{
		if (this.actorUIInfo == null || maxHP == 0L)
		{
			return;
		}
		if (this.lastMaxHP == 0L)
		{
			this.cacheHP = currentHP;
			this.lastHP = this.cacheHP;
			this.curHP = this.lastHP;
			this.cacheFactor = 0f;
			this.lastMaxHP = maxHP;
			long num;
			long fraHp;
			long num2;
			float value;
			this.GetCurHPValues(this.curHP, this.lastMaxHP, HPCount, out num, out fraHp, out num2, out value);
			this.SetHPForeground(fraHp, HPCount);
			this.actorUIInfo.HPBar.value = value;
			this.HPBar1.value = value;
		}
		else
		{
			if (currentHP != this.lastHP)
			{
				if (this.lastHP - currentHP > this.curHP - this.lastHP)
				{
					this.cacheFactor = 0f;
					this.cacheHP = this.lastHP;
				}
				this.lastHP = currentHP;
			}
			this.UpdateHPBarValue(HPCount);
		}
	}

	private void Update()
	{
		ActorController bossActor = Globals.Instance.ActorMgr.BossActor;
		if (bossActor != null && bossActor.monsterInfo != null && bossActor.CurHP != 0L && Globals.Instance.ActorMgr.IsInBossAttackDistance())
		{
			if (!this.winBg.gameObject.activeSelf)
			{
				this.winBg.gameObject.SetActive(true);
				NGUITools.SetActive(this.targetEffect, true);
				this.effectShowTimer = Time.time + 5f;
			}
			if (this.actorUIInfo.Actor != bossActor)
			{
				this.actorUIInfo.Actor = bossActor;
				this.targetNameLabel.text = bossActor.GetName();
				this.lastMaxHP = 0L;
			}
			this.SetTargetHPBar(bossActor.CurHP, bossActor.MaxHP, (long)bossActor.monsterInfo.HPBarCount);
			if (Time.time >= this.effectShowTimer && this.targetEffect.activeInHierarchy)
			{
				NGUITools.SetActive(this.targetEffect, false);
			}
		}
		else if (this.actorUIInfo.Actor != null && this.actorUIInfo.Actor.monsterInfo != null && this.actorUIInfo.Actor.isDead)
		{
			this.SetTargetHPBar(this.actorUIInfo.Actor.CurHP, this.actorUIInfo.Actor.MaxHP, (long)this.actorUIInfo.Actor.monsterInfo.HPBarCount);
		}
		else if (this.winBg.gameObject.activeSelf)
		{
			this.winBg.gameObject.SetActive(false);
			NGUITools.SetActive(this.targetEffect, false);
		}
	}
}
