using Att;
using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldBossCombatTip : MonoBehaviour
{
	private const int TIP_NUM = 2;

	private static WorldBossCombatTip mInstance;

	private UIWidget mBg;

	private UILabel[] mBossTips = new UILabel[2];

	private UITweener[] mTipsTweenPos = new UITweener[2];

	private UITweener[] mTipsTweenAlpha = new UITweener[2];

	private List<string> textList = new List<string>();

	private int curTweenPosIndex;

	private int mESceneType;

	public void Init(Transform parent, int sceneState)
	{
		this.mESceneType = sceneState;
		this.CreateObjects();
		Transform transform = base.transform;
		transform.parent = parent;
		transform.localPosition = Vector3.zero;
		transform.localScale = Vector3.one;
		this.mBg.leftAnchor.absolute = -50;
		this.mBg.rightAnchor.absolute = 50;
		this.mBg.topAnchor.absolute = -270;
		this.mBg.bottomAnchor.absolute = -370;
		this.mBg.SetAnchor(GameUIManager.mInstance.uiCamera.transform);
		if (this.mESceneType == 3)
		{
			WorldBossSubSystem worldBossSystem = Globals.Instance.Player.WorldBossSystem;
			WorldBossSubSystem expr_B6 = worldBossSystem;
			expr_B6.BossDeadEvent = (WorldBossSubSystem.BossDeadCallback)Delegate.Combine(expr_B6.BossDeadEvent, new WorldBossSubSystem.BossDeadCallback(this.OnBossDeadEvent));
			WorldBossSubSystem expr_D8 = worldBossSystem;
			expr_D8.BossRespawnEvent = (WorldBossSubSystem.BossRespawnCallback)Delegate.Combine(expr_D8.BossRespawnEvent, new WorldBossSubSystem.BossRespawnCallback(this.OnBossRespawnEvent));
			WorldBossSubSystem expr_FA = worldBossSystem;
			expr_FA.DoBossDamageEvent = (WorldBossSubSystem.DoBossDamageCallback)Delegate.Combine(expr_FA.DoBossDamageEvent, new WorldBossSubSystem.DoBossDamageCallback(this.OnDoBossDamageEvent));
		}
		else if (this.mESceneType == 5)
		{
			GuildSubSystem guildSystem = Globals.Instance.Player.GuildSystem;
			GuildSubSystem expr_13D = guildSystem;
			expr_13D.GuildBossDeadEvent = (GuildSubSystem.GuildBossDeadCallback)Delegate.Combine(expr_13D.GuildBossDeadEvent, new GuildSubSystem.GuildBossDeadCallback(this.OnGuildBossDeadEvent));
			GuildSubSystem expr_15F = guildSystem;
			expr_15F.DoGuildBossDamageEvent = (GuildSubSystem.DoGuildBossDamageCallback)Delegate.Combine(expr_15F.DoGuildBossDamageEvent, new GuildSubSystem.DoGuildBossDamageCallback(this.OnDoGuildBossDamageEvent));
		}
	}

	private void OnDestroy()
	{
		if (Globals.Instance == null)
		{
			return;
		}
		if (this.mESceneType == 3)
		{
			WorldBossSubSystem worldBossSystem = Globals.Instance.Player.WorldBossSystem;
			WorldBossSubSystem expr_2E = worldBossSystem;
			expr_2E.BossDeadEvent = (WorldBossSubSystem.BossDeadCallback)Delegate.Remove(expr_2E.BossDeadEvent, new WorldBossSubSystem.BossDeadCallback(this.OnBossDeadEvent));
			WorldBossSubSystem expr_50 = worldBossSystem;
			expr_50.BossRespawnEvent = (WorldBossSubSystem.BossRespawnCallback)Delegate.Remove(expr_50.BossRespawnEvent, new WorldBossSubSystem.BossRespawnCallback(this.OnBossRespawnEvent));
			WorldBossSubSystem expr_72 = worldBossSystem;
			expr_72.DoBossDamageEvent = (WorldBossSubSystem.DoBossDamageCallback)Delegate.Remove(expr_72.DoBossDamageEvent, new WorldBossSubSystem.DoBossDamageCallback(this.OnDoBossDamageEvent));
		}
		else if (this.mESceneType == 5)
		{
			GuildSubSystem guildSystem = Globals.Instance.Player.GuildSystem;
			GuildSubSystem expr_B5 = guildSystem;
			expr_B5.GuildBossDeadEvent = (GuildSubSystem.GuildBossDeadCallback)Delegate.Remove(expr_B5.GuildBossDeadEvent, new GuildSubSystem.GuildBossDeadCallback(this.OnGuildBossDeadEvent));
			GuildSubSystem expr_D7 = guildSystem;
			expr_D7.DoGuildBossDamageEvent = (GuildSubSystem.DoGuildBossDamageCallback)Delegate.Remove(expr_D7.DoGuildBossDamageEvent, new GuildSubSystem.DoGuildBossDamageCallback(this.OnDoGuildBossDamageEvent));
		}
	}

	private void CreateObjects()
	{
		this.mBg = base.transform.GetComponent<UIWidget>();
		for (int i = 0; i < 2; i++)
		{
			this.mBossTips[i] = base.transform.FindChild(string.Format("Tip{0}", i + 1)).GetComponent<UILabel>();
			this.mTipsTweenPos[i] = this.mBossTips[i].gameObject.GetComponent<TweenPosition>();
			this.mTipsTweenAlpha[i] = this.mBossTips[i].gameObject.GetComponent<TweenAlpha>();
			this.mBossTips[i].name = i.ToString();
			this.mBossTips[i].gameObject.SetActive(false);
			this.mBossTips[i].text = string.Empty;
			EventDelegate.Add(this.mTipsTweenPos[i].onFinished, new EventDelegate.Callback(this.TipTweenPosEnd));
			EventDelegate.Add(this.mTipsTweenAlpha[i].onFinished, new EventDelegate.Callback(this.TipsTweenAlphaEnd));
		}
	}

	public static WorldBossCombatTip GetInstance()
	{
		if (WorldBossCombatTip.mInstance == null)
		{
			GameObject original = Res.LoadGUI("GUI/WorldBossCombatTip");
			GameObject gameObject = UnityEngine.Object.Instantiate(original) as GameObject;
			WorldBossCombatTip.mInstance = gameObject.AddComponent<WorldBossCombatTip>();
		}
		return WorldBossCombatTip.mInstance;
	}

	private void Update()
	{
		if (this.textList.Count > 0 && this.FindAvalidBossTip() != 0)
		{
			this.ShowBossTipText(this.textList[0]);
			this.textList.RemoveAt(0);
		}
	}

	public void OnBossDeadEvent(int slot, MonsterInfo info, string playerName)
	{
		if (slot == Globals.Instance.Player.WorldBossSystem.CurSlot || slot == 5)
		{
			return;
		}
		this.ShowBossTipText(Singleton<StringManager>.Instance.GetString("worldBossTxt14", new object[]
		{
			info.Name,
			playerName
		}));
	}

	private void OnGuildBossDeadEvent(int id, MonsterInfo info, string playerName)
	{
		this.ShowBossTipText(Singleton<StringManager>.Instance.GetString("worldBossTxt14", new object[]
		{
			info.Name,
			playerName
		}));
	}

	public void OnBossRespawnEvent(int slot, MonsterInfo info)
	{
		this.ShowBossTipText(Singleton<StringManager>.Instance.GetString("worldBossTxt15", new object[]
		{
			info.Name
		}));
	}

	public void OnDoBossDamageEvent(int slot, MonsterInfo info, string playerName, long damage, int type)
	{
		this.ShowBossTipText(Singleton<StringManager>.Instance.GetString("worldBossTxt16", new object[]
		{
			playerName,
			info.Name,
			damage
		}));
	}

	private void OnDoGuildBossDamageEvent(int id, MonsterInfo info, string playerName, long damage)
	{
		this.ShowBossTipText(Singleton<StringManager>.Instance.GetString("worldBossTxt16", new object[]
		{
			playerName,
			info.Name,
			damage
		}));
	}

	private int FindAvalidBossTip()
	{
		int num = 0;
		for (int i = 0; i < 2; i++)
		{
			if (this.mBossTips[i].text.Length <= 0)
			{
				num |= 1 << i;
			}
		}
		return num;
	}

	private void ResetBossTip(int index)
	{
		this.mTipsTweenPos[index].ResetToBeginning();
		this.mTipsTweenAlpha[index].ResetToBeginning();
		this.mTipsTweenAlpha[index].enabled = true;
		this.mTipsTweenPos[index].enabled = false;
		this.mBossTips[index].gameObject.SetActive(true);
	}

	public void ShowBossTipText(string text)
	{
		int num = this.FindAvalidBossTip();
		if (num == 0)
		{
			this.textList.Add(text);
			return;
		}
		int num2 = 0;
		if (num > 2)
		{
			this.ResetBossTip(num2);
		}
		else
		{
			this.mTipsTweenPos[num % 2].enabled = true;
			num2 = num - 1;
			this.curTweenPosIndex = num2;
		}
		this.mBossTips[num2].text = text;
	}

	private void TipTweenPosEnd()
	{
		this.ResetBossTip(this.curTweenPosIndex);
	}

	private void TipsTweenAlphaEnd()
	{
		for (int i = 0; i < 2; i++)
		{
			if (!this.mTipsTweenAlpha[i].enabled)
			{
				this.mBossTips[i].gameObject.SetActive(false);
				this.mBossTips[i].text = string.Empty;
			}
		}
	}
}
