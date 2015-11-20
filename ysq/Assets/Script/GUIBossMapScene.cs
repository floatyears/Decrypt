using Att;
using Proto;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class GUIBossMapScene : GameUISession
{
	public class BossItemInfo
	{
		public UISlider hpSlider;

		public UILabel bossName;

		public Transform bossBg;

		public UISprite bossIcon;

		public UIButton slot;

		public GameObject bossActor;
	}

	private const int MAX_DAMAGE_TEXT_NUM = 6;

	private const float START_DAMAGE_TEXT_POS = -22f;

	private const float END_DAMAGE_TEXT_POS = 8f;

	private UILabel resurrectTimeLb;

	private Vector3[] WORLD_BOSS_POS = new Vector3[]
	{
		new Vector3(-162.3f, 0.8642101f, -462f),
		new Vector3(-289.2144f, -151.4336f, -732f),
		new Vector3(66.4f, -105f, -532f),
		new Vector3(-67.08579f, -214.9f, -632f)
	};

	public GUIBossMapScene.BossItemInfo[] mBossItems = new GUIBossMapScene.BossItemInfo[5];

	private bool updateTime;

	private UIPanel combatDamagePanel;

	private UISprite combatDamageBg;

	private Transform combatDamageExpand;

	private UILabel combatDamageLb;

	private bool expand;

	private List<string> combatDamageTxtList = new List<string>();

	private StringBuilder mStringBuilder = new StringBuilder();

	private bool isPlayDamageTextAnim;

	private float refreshAnimTime;

	private int itemTextCount;

	private GameObject mTakenBtnRedFlag;

	protected override void OnPostLoadGUI()
	{
		Globals.Instance.BackgroundMusicMgr.PlayLobbyMusic("bg/bg_003", true);
		GameUIManager.mInstance.DestroyGameUIOptionPopUp();
		TopGoods topGoods = GameUIManager.mInstance.GetTopGoods();
		topGoods.Show("worldBossTxt5");
		UIPanel component = topGoods.gameObject.GetComponent<UIPanel>();
		component.transform.localPosition = new Vector3(component.transform.localPosition.x, component.transform.localPosition.y, component.transform.localPosition.z - 600f);
		Transform transform = base.transform;
		GameObject gameObject = transform.Find("rulesBtn").gameObject;
		UIEventListener expr_B1 = UIEventListener.Get(gameObject);
		expr_B1.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_B1.onClick, new UIEventListener.VoidDelegate(this.OnRulesBtnClicked));
		GameObject gameObject2 = transform.Find("takenBtn").gameObject;
		UIEventListener expr_EB = UIEventListener.Get(gameObject2);
		expr_EB.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_EB.onClick, new UIEventListener.VoidDelegate(this.OnTakenBtnClick));
		this.mTakenBtnRedFlag = gameObject2.transform.Find("redFlag").gameObject;
		this.mTakenBtnRedFlag.SetActive(Tools.IsFDSCanTaken() || Tools.IsWBRewardCanTaken());
		UILabel component2 = gameObject2.transform.Find("Label").GetComponent<UILabel>();
		component2.overflowMethod = UILabel.Overflow.ShrinkContent;
		component2.width = 158;
		this.resurrectTimeLb = transform.Find("resurrectTime").GetComponent<UILabel>();
		this.resurrectTimeLb.gameObject.SetActive(false);
		UIEventListener expr_1B7 = UIEventListener.Get(this.resurrectTimeLb.transform.Find("resetTipInfo/againInfo/resetBtn").gameObject);
		expr_1B7.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_1B7.onClick, new UIEventListener.VoidDelegate(this.OnResurrectClick));
		WorldBossSubSystem worldBossSystem = Globals.Instance.Player.WorldBossSystem;
		for (int i = 1; i <= 5; i++)
		{
			BossData bossData = worldBossSystem.GetBossData(i);
			Transform transform2 = transform.Find(string.Format("targetInfo{0}", i));
			GUIBossMapScene.BossItemInfo bossItemInfo = new GUIBossMapScene.BossItemInfo
			{
				hpSlider = transform2.Find("HpFore").GetComponent<UISlider>(),
				bossName = transform2.Find("bossName").GetComponent<UILabel>(),
				bossBg = transform2.Find("bossBg"),
				bossIcon = transform2.Find("bossBg/bossIcon").GetComponent<UISprite>(),
				slot = transform2.Find("slot").GetComponent<UIButton>()
			};
			bossItemInfo.hpSlider.value = bossData.HealthPct;
			this.mBossItems[i - 1] = bossItemInfo;
			bossItemInfo.slot.name = i.ToString();
			UIEventListener expr_2DA = UIEventListener.Get(bossItemInfo.slot.gameObject);
			expr_2DA.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_2DA.onClick, new UIEventListener.VoidDelegate(this.OnSlotBtnClicked));
			if (bossData.InfoID == 0)
			{
				transform2.gameObject.SetActive(false);
			}
		}
		this.combatDamageBg = transform.Find("CombatDamageBg").GetComponent<UISprite>();
		this.combatDamagePanel = this.combatDamageBg.transform.Find("CombatDamage").GetComponent<UIPanel>();
		this.combatDamageLb = this.combatDamagePanel.transform.Find("DamageLb").GetComponent<UILabel>();
		this.combatDamageExpand = this.combatDamageBg.transform.Find("DamageExpand");
		this.combatDamageLb.text = string.Empty;
		UIEventListener expr_3B3 = UIEventListener.Get(this.combatDamageExpand.gameObject);
		expr_3B3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_3B3.onClick, new UIEventListener.VoidDelegate(this.OnCombatDamageExpandClick));
		this.expand = false;
		this.RefreshCombatDamage();
		WorldBossCombatRank.GetInstance().Init(base.transform, new Vector4(-274f, -10f, -80f, -398f), false, "worldBoss");
		WorldBossSubSystem expr_412 = worldBossSystem;
		expr_412.GetBossDataEvent = (WorldBossSubSystem.VoidCallback)Delegate.Combine(expr_412.GetBossDataEvent, new WorldBossSubSystem.VoidCallback(this.OnGetBossDataEvent));
		WorldBossSubSystem expr_435 = worldBossSystem;
		expr_435.BossRespawnEvent = (WorldBossSubSystem.BossRespawnCallback)Delegate.Combine(expr_435.BossRespawnEvent, new WorldBossSubSystem.BossRespawnCallback(this.OnBossRespawnEvent));
		WorldBossSubSystem expr_458 = worldBossSystem;
		expr_458.DoBossDamageEvent = (WorldBossSubSystem.DoBossDamageCallback)Delegate.Combine(expr_458.DoBossDamageEvent, new WorldBossSubSystem.DoBossDamageCallback(this.OnDoBossDamageEvent));
		WorldBossSubSystem expr_47B = worldBossSystem;
		expr_47B.BossDeadEvent = (WorldBossSubSystem.BossDeadCallback)Delegate.Combine(expr_47B.BossDeadEvent, new WorldBossSubSystem.BossDeadCallback(this.OnBossDeadEvent));
		Globals.Instance.CliSession.Register(623, new ClientSession.MsgHandler(this.OnMsgWorldBossResurrect));
		Globals.Instance.CliSession.Register(648, new ClientSession.MsgHandler(this.OnMsgTakeFDSReward));
		Globals.Instance.CliSession.Register(651, new ClientSession.MsgHandler(this.OnMsgTakeKWBReward));
		GameObject gameObject3 = base.transform.Find("ui68").gameObject;
		Tools.SetParticleRenderQueue2(gameObject3, 5500);
		NGUITools.SetActive(gameObject3, true);
		this.updateTime = false;
		GUIBossReadyScene.SendGetBossDataMsg();
	}

	protected override void OnPreDestroyGUI()
	{
		this.updateTime = false;
		TopGoods topGoods = GameUIManager.mInstance.GetTopGoods();
		UIPanel component = topGoods.gameObject.GetComponent<UIPanel>();
		component.transform.localPosition = new Vector3(component.transform.localPosition.x, component.transform.localPosition.y, component.transform.localPosition.z + 600f);
		topGoods.Hide();
		WorldBossSubSystem worldBossSystem = Globals.Instance.Player.WorldBossSystem;
		WorldBossSubSystem expr_86 = worldBossSystem;
		expr_86.GetBossDataEvent = (WorldBossSubSystem.VoidCallback)Delegate.Remove(expr_86.GetBossDataEvent, new WorldBossSubSystem.VoidCallback(this.OnGetBossDataEvent));
		WorldBossSubSystem expr_A8 = worldBossSystem;
		expr_A8.BossRespawnEvent = (WorldBossSubSystem.BossRespawnCallback)Delegate.Remove(expr_A8.BossRespawnEvent, new WorldBossSubSystem.BossRespawnCallback(this.OnBossRespawnEvent));
		WorldBossSubSystem expr_CA = worldBossSystem;
		expr_CA.DoBossDamageEvent = (WorldBossSubSystem.DoBossDamageCallback)Delegate.Remove(expr_CA.DoBossDamageEvent, new WorldBossSubSystem.DoBossDamageCallback(this.OnDoBossDamageEvent));
		WorldBossSubSystem expr_EC = worldBossSystem;
		expr_EC.BossDeadEvent = (WorldBossSubSystem.BossDeadCallback)Delegate.Remove(expr_EC.BossDeadEvent, new WorldBossSubSystem.BossDeadCallback(this.OnBossDeadEvent));
		Globals.Instance.CliSession.Unregister(623, new ClientSession.MsgHandler(this.OnMsgWorldBossResurrect));
		Globals.Instance.CliSession.Unregister(648, new ClientSession.MsgHandler(this.OnMsgTakeFDSReward));
		Globals.Instance.CliSession.Unregister(651, new ClientSession.MsgHandler(this.OnMsgTakeKWBReward));
	}

	private void OnMsgTakeFDSReward(MemoryStream stream)
	{
		MS2C_TakeFDSReward mS2C_TakeFDSReward = Serializer.NonGeneric.Deserialize(typeof(MS2C_TakeFDSReward), stream) as MS2C_TakeFDSReward;
		if (mS2C_TakeFDSReward.Result == 51)
		{
			Globals.Instance.Player.ShowFrozenFunctionMsg();
			return;
		}
		if (mS2C_TakeFDSReward.Result == 0)
		{
			this.mTakenBtnRedFlag.SetActive(Tools.IsFDSCanTaken() || Tools.IsWBRewardCanTaken());
		}
	}

	private void OnMsgTakeKWBReward(MemoryStream stream)
	{
		MS2C_TakeKillWorldBossReward mS2C_TakeKillWorldBossReward = Serializer.NonGeneric.Deserialize(typeof(MS2C_TakeKillWorldBossReward), stream) as MS2C_TakeKillWorldBossReward;
		if (mS2C_TakeKillWorldBossReward.Result == 51)
		{
			Globals.Instance.Player.ShowFrozenFunctionMsg();
			return;
		}
		if (mS2C_TakeKillWorldBossReward.Result == 0)
		{
			Globals.Instance.Player.WorldBossSystem.SetWBRewardFlag(mS2C_TakeKillWorldBossReward.HasReward);
			this.mTakenBtnRedFlag.SetActive(Tools.IsFDSCanTaken() || Tools.IsWBRewardCanTaken());
		}
	}

	private void OnCombatDamageExpandClick(GameObject go)
	{
		this.expand = !this.expand;
		this.RefreshCombatDamage();
	}

	private void RefreshCombatDamage()
	{
		if (this.expand)
		{
			this.combatDamageExpand.localRotation = Quaternion.identity;
			this.combatDamageBg.topAnchor.absolute = 201;
			Vector4 baseClipRegion = this.combatDamagePanel.baseClipRegion;
			baseClipRegion.w = 180f;
			this.combatDamagePanel.baseClipRegion = baseClipRegion;
			this.combatDamagePanel.clipOffset = new Vector2(280f, -106f);
		}
		else
		{
			this.combatDamageExpand.localRotation = Quaternion.Euler(0f, 0f, -180f);
			this.combatDamageBg.topAnchor.absolute = 83;
			Vector4 baseClipRegion2 = this.combatDamagePanel.baseClipRegion;
			baseClipRegion2.w = 62f;
			this.combatDamagePanel.baseClipRegion = baseClipRegion2;
			this.combatDamagePanel.clipOffset = new Vector2(280f, -47f);
		}
		this.refreshAnimTime = 0f;
		this.combatDamageBg.gameObject.SetActive(this.itemTextCount > 0);
	}

	private void Update()
	{
		if (!this.updateTime)
		{
			return;
		}
		WorldBossSubSystem worldBossSystem = Globals.Instance.Player.WorldBossSystem;
		float t = Time.deltaTime * 2.5f;
		for (int i = 1; i <= 5; i++)
		{
			GUIBossMapScene.BossItemInfo bossItemInfo = this.mBossItems[i - 1];
			BossData bossData = worldBossSystem.GetBossData(i);
			float value = bossItemInfo.hpSlider.value;
			float healthPct = bossData.HealthPct;
			if (value != healthPct)
			{
				if (Mathf.Abs(value - healthPct) > 0.01f)
				{
					bossItemInfo.hpSlider.value = Mathf.Lerp(value, healthPct, t);
				}
				else
				{
					bossItemInfo.hpSlider.value = healthPct;
				}
			}
		}
		int timeStamp = Globals.Instance.Player.GetTimeStamp();
		int timeStamp2 = Globals.Instance.Player.WorldBossSystem.TimeStamp;
		if (timeStamp >= timeStamp2)
		{
			this.updateTime = false;
			this.BackBossScene();
			return;
		}
		if (Globals.Instance.Player.GetTimeStamp() < Globals.Instance.Player.WorldBossSystem.AutoResurrectTimeStamp)
		{
			this.resurrectTimeLb.text = Tools.FormatTime2(Globals.Instance.Player.WorldBossSystem.AutoResurrectTimeStamp - Globals.Instance.Player.GetTimeStamp());
			if (!this.resurrectTimeLb.gameObject.activeSelf)
			{
				this.resurrectTimeLb.gameObject.SetActive(true);
			}
		}
		else if (this.resurrectTimeLb.gameObject.activeSelf)
		{
			this.resurrectTimeLb.gameObject.SetActive(false);
		}
		if (this.isPlayDamageTextAnim)
		{
			Vector3 localPosition = this.combatDamageLb.transform.localPosition;
			localPosition.y += Time.deltaTime * 100f;
			if (localPosition.y >= 8f)
			{
				localPosition.y = -22f;
				this.isPlayDamageTextAnim = false;
				this.refreshAnimTime = 0f;
				this.RemoveFirstCombatDamageText();
			}
			this.combatDamageLb.transform.localPosition = localPosition;
		}
		else if (this.combatDamageTxtList.Count > 0 && Time.time - this.refreshAnimTime > 1.5f)
		{
			this.ShowCombatDamageText(this.combatDamageTxtList[0]);
			this.combatDamageTxtList.RemoveAt(0);
			this.refreshAnimTime = Time.time;
		}
	}

	public void RemoveFirstCombatDamageText()
	{
		string text = this.combatDamageLb.text;
		this.combatDamageLb.text = text.Substring(text.IndexOf('\n') + 1);
		this.itemTextCount--;
	}

	public void ShowCombatDamageText(string text)
	{
		if (this.isPlayDamageTextAnim)
		{
			this.combatDamageTxtList.Add(text);
			return;
		}
		this.mStringBuilder.Remove(0, this.mStringBuilder.Length);
		this.mStringBuilder.Append(this.combatDamageLb.text);
		this.mStringBuilder.AppendLine(text);
		this.combatDamageLb.text = this.mStringBuilder.ToString();
		this.itemTextCount++;
		this.combatDamageBg.gameObject.SetActive(this.itemTextCount > 0);
		this.isPlayDamageTextAnim = (this.itemTextCount > ((!this.expand) ? 2 : 6));
	}

	private void RefreshBossItemInfo(BossData data, MonsterInfo info, bool createActor = false)
	{
		GUIBossMapScene.BossItemInfo item = this.mBossItems[data.Slot - 1];
		if (info != null)
		{
			item.bossName.text = string.Format("[fd8e00]Lv{0}[-] {1}", info.Level, info.Name);
		}
		if (data.Slot == 5)
		{
			if (info != null)
			{
				item.hpSlider.gameObject.SetActive(true);
				item.bossBg.localPosition = new Vector3(190f, -35f, 0f);
			}
			else
			{
				item.hpSlider.gameObject.SetActive(false);
				item.bossBg.localPosition = new Vector3(190f, -14f, 0f);
			}
		}
		if (info != null && data.HealthPct > 0f)
		{
			item.slot.normalSprite = ((data.Slot != 5) ? "easy" : "hard");
		}
		else
		{
			item.slot.normalSprite = "Disable";
		}
		item.slot.gameObject.SetActive(info != null);
		item.bossBg.parent.gameObject.SetActive(info != null);
		if (info != null && createActor && data.Slot != 5)
		{
			if (item.bossActor != null)
			{
				UnityEngine.Object.DestroyImmediate(item.bossActor);
				item.bossActor = null;
			}
			GUIWorldMap.CreateWorldMapActorAsnc(info.ResLoc, string.Empty, base.transform, info.ScaleInUI * 1.25f, 180f, delegate(GameObject bossActor)
			{
				if (bossActor != null)
				{
					bossActor.transform.localPosition = this.WORLD_BOSS_POS[data.Slot - 1];
					bossActor.animation.clip = bossActor.animation.GetClip("std");
					bossActor.animation.wrapMode = WrapMode.Loop;
					bossActor.SetActive(true);
					item.bossActor = bossActor;
				}
			});
			item.bossIcon.spriteName = Tools.GetPropertyIconWithBorder((EElementType)info.ElementType);
		}
	}

	public void OnBackClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.ChangeSession<GUIBossReadyScene>(null, false, true);
	}

	public void OnRulesBtnClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GUIWBRuleInfoPopUp.ShowMe();
	}

	private void OnTakenBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GUILongLinRewardPopUp.ShowMe();
	}

	public void OnSlotBtnClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (Globals.Instance.Player.GetTimeStamp() <= Globals.Instance.Player.WorldBossSystem.AutoResurrectTimeStamp)
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("worldBossTxt17"), 0f, 0f);
			return;
		}
		int num = Convert.ToInt32(go.name);
		WorldBossSubSystem worldBossSystem = Globals.Instance.Player.WorldBossSystem;
		BossData bossData = worldBossSystem.GetBossData(num);
		if (worldBossSystem.GetBossInfo(num) == null)
		{
			return;
		}
		if (bossData.HealthPct <= 0f)
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("worldBossTxt13"), 0f, 0f);
			return;
		}
		GameUIManager.mInstance.uiState.WorldBossSlot = num;
		MC2S_WorldBossStart mC2S_WorldBossStart = new MC2S_WorldBossStart();
		mC2S_WorldBossStart.Slot = num;
		Globals.Instance.CliSession.Send(624, mC2S_WorldBossStart);
	}

	public void OnGetBossDataEvent()
	{
		WorldBossSubSystem worldBossSystem = Globals.Instance.Player.WorldBossSystem;
		for (int i = 1; i <= 5; i++)
		{
			this.RefreshBossItemInfo(worldBossSystem.GetBossData(i), worldBossSystem.GetBossInfo(i), true);
		}
		this.updateTime = true;
		GameUIManager.mInstance.TryCommend(ECommentType.EComment_WorldBossKill, 0f);
	}

	public void OnBossRespawnEvent(int slot, MonsterInfo info)
	{
		WorldBossSubSystem worldBossSystem = Globals.Instance.Player.WorldBossSystem;
		this.RefreshBossItemInfo(worldBossSystem.GetBossData(slot), info, true);
	}

	public void OnDoBossDamageEvent(int slot, MonsterInfo info, string playerName, long damage, int type)
	{
		WorldBossSubSystem worldBossSystem = Globals.Instance.Player.WorldBossSystem;
		this.RefreshBossItemInfo(worldBossSystem.GetBossData(slot), info, false);
		this.ShowCombatDamageText((type == 0) ? Singleton<StringManager>.Instance.GetString("worldBossTxt18", new object[]
		{
			playerName,
			info.Name,
			damage
		}) : Singleton<StringManager>.Instance.GetString("worldBossTxt19", new object[]
		{
			playerName,
			info.Name,
			damage
		}));
	}

	public void OnBossDeadEvent(int slot, MonsterInfo info, string playerName)
	{
		WorldBossSubSystem worldBossSystem = Globals.Instance.Player.WorldBossSystem;
		this.RefreshBossItemInfo(worldBossSystem.GetBossData(slot), info, false);
		if (slot == 5)
		{
			this.updateTime = false;
			this.BackBossScene();
		}
	}

	private void BackBossScene()
	{
		WorldBossSubSystem worldBossSystem = Globals.Instance.Player.WorldBossSystem;
		if (worldBossSystem.WorldBossIsOver())
		{
			GameUIManager.mInstance.uiState.WorldBossIsOver = true;
			GameUIManager.mInstance.ChangeSession<GUIBossReadyScene>(null, false, true);
		}
	}

	private void OnResurrectClick(GameObject go)
	{
		if (Tools.MoneyNotEnough(ECurrencyType.ECurrencyT_Diamond, GameConst.GetInt32(47), 0))
		{
			return;
		}
		MC2S_WorldBossResurrect mC2S_WorldBossResurrect = new MC2S_WorldBossResurrect();
		mC2S_WorldBossResurrect.Type = 1;
		Globals.Instance.CliSession.Send(622, mC2S_WorldBossResurrect);
	}

	public void OnMsgWorldBossResurrect(MemoryStream stream)
	{
		MS2C_WorldBossResurrect mS2C_WorldBossResurrect = Serializer.NonGeneric.Deserialize(typeof(MS2C_WorldBossResurrect), stream) as MS2C_WorldBossResurrect;
		if (mS2C_WorldBossResurrect.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PveR", mS2C_WorldBossResurrect.Result);
			return;
		}
		if (mS2C_WorldBossResurrect.Type != 0)
		{
			Globals.Instance.Player.WorldBossSystem.AutoResurrectTimeStamp = 0;
		}
	}
}
