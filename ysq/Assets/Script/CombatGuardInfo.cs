using System;
using UnityEngine;

public class CombatGuardInfo : MonoBehaviour
{
	private static CombatGuardInfo mInstance;

	private MemoryGearScene scene;

	private UILabel mNextWaveCD;

	private UILabel mHP;

	private UILabel mWave;

	private int maxHP;

	private int maxWave;

	private float timerRefresh;

	public static CombatGuardInfo GetInstance()
	{
		if (CombatGuardInfo.mInstance == null)
		{
			GameObject original = Res.LoadGUI("GUI/CombatGuardInfo");
			GameObject gameObject = UnityEngine.Object.Instantiate(original) as GameObject;
			CombatGuardInfo.mInstance = gameObject.AddComponent<CombatGuardInfo>();
		}
		return CombatGuardInfo.mInstance;
	}

	public void Init(Transform t)
	{
		UIWidget component = base.GetComponent<UIWidget>();
		component.SetAnchor(t);
		base.transform.parent = t;
		base.transform.localScale = Vector3.one;
		this.CreateObjects();
		this.scene = Globals.Instance.ActorMgr.MemoryGearScene;
		if (this.scene != null)
		{
			this.maxHP = this.scene.MaxDamageCount;
			this.maxWave = this.scene.MaxWave;
			this.SetHP(this.maxHP);
			this.SetWave(1);
		}
		this.mNextWaveCD.transform.parent.gameObject.SetActive(false);
		MemoryGearScene expr_A6 = this.scene;
		expr_A6.GearDamageEvent = (MemoryGearScene.VoidCallback)Delegate.Combine(expr_A6.GearDamageEvent, new MemoryGearScene.VoidCallback(this.OnGearDamageEvent));
		MemoryGearScene expr_CD = this.scene;
		expr_CD.WaveUpdateEvent = (MemoryGearScene.VoidCallback)Delegate.Combine(expr_CD.WaveUpdateEvent, new MemoryGearScene.VoidCallback(this.OnWaveUpdateEvent));
		MemoryGearScene expr_F4 = this.scene;
		expr_F4.PlayerDeadEvent = (MemoryGearScene.VoidCallback)Delegate.Combine(expr_F4.PlayerDeadEvent, new MemoryGearScene.VoidCallback(this.OnPlayerDeadEvent));
		MemoryGearScene expr_11B = this.scene;
		expr_11B.CombatEvent = (MemoryGearScene.ValueCallback)Delegate.Combine(expr_11B.CombatEvent, new MemoryGearScene.ValueCallback(this.OnCombatEvent));
	}

	private void OnGearDamageEvent()
	{
		this.SetHP(this.scene.MaxDamageCount - this.scene.DamageCount);
	}

	private void OnWaveUpdateEvent()
	{
		this.SetWave(this.scene.CurWave);
	}

	private void OnPlayerDeadEvent()
	{
		GameUIManager.mInstance.ShowCountdownDeath(5);
	}

	private void OnCombatEvent(EMGEventType type, int value)
	{
		switch (type)
		{
		case EMGEventType.EMGET_Respawn:
			GameUIManager.mInstance.ShowGameNewPopUp(Singleton<StringManager>.Instance.GetString("guardRespawn", new object[]
			{
				Singleton<StringManager>.Instance.GetString(string.Format("guardRoad{0}", value))
			}), 2f, 50f, 0.25f);
			break;
		case EMGEventType.EMGET_BossRespawn:
			GameUIManager.mInstance.ShowGameNewPopUp(Singleton<StringManager>.Instance.GetString("guardBossRespawn", new object[]
			{
				Singleton<StringManager>.Instance.GetString(string.Format("guardRoad{0}", value))
			}), 2f, 50f, 0.25f);
			break;
		case EMGEventType.EMGET_TowerDamaged:
			GameUIManager.mInstance.ShowGameNewPopUp(Singleton<StringManager>.Instance.GetString("guardTowerDamaged", new object[]
			{
				Singleton<StringManager>.Instance.GetString(string.Format("guardRoad{0}", value))
			}), 2f, 50f, 0.25f);
			break;
		case EMGEventType.EMGET_TowerDead:
			GameUIManager.mInstance.ShowGameNewPopUp(Singleton<StringManager>.Instance.GetString("guardTowerDead", new object[]
			{
				Singleton<StringManager>.Instance.GetString(string.Format("guardRoad{0}", value))
			}), 2f, 50f, 0.25f);
			break;
		case EMGEventType.EMGET_GearDamaged:
		{
			string key;
			if (this.scene.MaxDamageCount - this.scene.DamageCount < 5)
			{
				key = "guardGearDamaged0";
			}
			else
			{
				key = "guardGearDamaged";
			}
			GameUIManager.mInstance.ShowGameNewPopUp(Singleton<StringManager>.Instance.GetString(key, new object[]
			{
				value
			}), 1f, 50f, 0.25f);
			break;
		}
		case EMGEventType.EMGET_GearDead:
			GameUIManager.mInstance.ShowGameNewPopUp(Singleton<StringManager>.Instance.GetString("guardGearDead"), 2f, 50f, 0.25f);
			break;
		}
	}

	private void OnDestroy()
	{
		if (Globals.Instance == null)
		{
			return;
		}
		if (this.scene != null)
		{
			MemoryGearScene expr_22 = this.scene;
			expr_22.GearDamageEvent = (MemoryGearScene.VoidCallback)Delegate.Remove(expr_22.GearDamageEvent, new MemoryGearScene.VoidCallback(this.OnGearDamageEvent));
			MemoryGearScene expr_49 = this.scene;
			expr_49.WaveUpdateEvent = (MemoryGearScene.VoidCallback)Delegate.Remove(expr_49.WaveUpdateEvent, new MemoryGearScene.VoidCallback(this.OnWaveUpdateEvent));
			MemoryGearScene expr_70 = this.scene;
			expr_70.PlayerDeadEvent = (MemoryGearScene.VoidCallback)Delegate.Remove(expr_70.PlayerDeadEvent, new MemoryGearScene.VoidCallback(this.OnPlayerDeadEvent));
			MemoryGearScene expr_97 = this.scene;
			expr_97.CombatEvent = (MemoryGearScene.ValueCallback)Delegate.Remove(expr_97.CombatEvent, new MemoryGearScene.ValueCallback(this.OnCombatEvent));
		}
	}

	private void CreateObjects()
	{
		this.mNextWaveCD = GameUITools.FindUILabel("NextWaveCD/Label", base.gameObject);
		this.mHP = GameUITools.FindUILabel("HP/Value", base.gameObject);
		this.mWave = GameUITools.FindUILabel("Wave/Label", base.gameObject);
		this.timerRefresh = Time.time;
	}

	private void Update()
	{
		if (base.gameObject.activeInHierarchy && this.scene != null)
		{
			if (this.scene.MaxWave > this.scene.CurWave)
			{
				if (this.scene.CurStatus == 2 && Time.time - this.timerRefresh >= 1f)
				{
					this.timerRefresh = Time.time;
					int num = Mathf.FloorToInt(this.scene.RespawnTimer);
					if (num > 0)
					{
						if (!this.mNextWaveCD.gameObject.activeInHierarchy)
						{
							this.mNextWaveCD.transform.parent.gameObject.SetActive(true);
						}
						this.mNextWaveCD.text = Singleton<StringManager>.Instance.GetString("guard8", new object[]
						{
							num
						});
					}
					else
					{
						this.mNextWaveCD.transform.parent.gameObject.SetActive(false);
					}
				}
			}
			else if (this.mNextWaveCD.gameObject.activeInHierarchy)
			{
				this.mNextWaveCD.transform.parent.gameObject.SetActive(false);
			}
		}
	}

	public void SetHP(int current)
	{
		this.mHP.text = Singleton<StringManager>.Instance.GetString("guard10", new object[]
		{
			current,
			this.maxHP
		});
	}

	public void SetWave(int current)
	{
		this.mWave.text = Singleton<StringManager>.Instance.GetString("guard9", new object[]
		{
			current,
			this.maxWave
		});
	}
}
