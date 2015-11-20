using Att;
using PathologicalGames;
using Proto;
using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class ActorManager : MonoBehaviour
{
	public delegate void BuffAddCallback(int slot, int serialID, BuffInfo info, float duration, int stackCount);

	public delegate void BuffRemoveCallback(int slot, int serialID);

	public delegate void LootMoneyCallback(ActorController actor, int money);

	public delegate void TrialScoreCallback(int wave, int score);

	public delegate void KillActorCallback(string killerName, string killedName, bool isKilledYour);

	public const int MAX_GROUP_SIZE = 5;

	public ActorManager.BuffAddCallback BuffAddEvent;

	public ActorManager.BuffAddCallback BuffUpdateEvent;

	public ActorManager.BuffRemoveCallback BuffRemoveEvent;

	public ActorManager.LootMoneyCallback LootMoneyEvent;

	public ActorManager.TrialScoreCallback TrialScoreEvent;

	public ActorManager.KillActorCallback KillActorEvent;

	private PlayerController playerCtrler;

	private List<ActorController> actors = new List<ActorController>();

	private SceneInfo senceInfo;

	private Vector3 bornPosition = Vector3.zero;

	private float bornRotationY;

	private bool[] poseOK = new bool[5];

	private bool combat;

	private GameObject selectEffect;

	private ParticleSystem[] selectPS;

	private float preSelectScale = 1f;

	private ActorController selectActor;

	private OutlineShader bossOutLine;

	private ActorController bossActor;

	public bool ForceShowBossActor;

	private float combatTime;

	private bool win;

	public int Key;

	public int RecvStartTime;

	private int lootMoney;

	private BaseScene baseScene;

	private WorldScene worldScene;

	private DefenseScene defenseScene;

	private TrialScene trialScene;

	private WorldBossScene worldBossScene;

	private ArenaScene arenaScene;

	private PillageScene pillageScene;

	private GuildBossScene guildBossScene;

	private MemoryGearScene memoryGearScene;

	private OrePillageScene orePillageScene;

	private GuildPvpScene guildPvpScene;

	private DelayInvoker invoker = new DelayInvoker();

	private bool enableUpdate = true;

	private int assistPetID;

	private int assistPetAttID;

	private CombatLog combatLog;

	private float pauseTimer;

	public int ServerKey;

	public ServerActorData ServerData;

	private float checkTimer;

	public ActorController LopetActor;

	private float hideLopetTimer;

	public ActorController RemoteLopetActor;

	private float hideRemoteLopetTimer;

	public PlayerController PlayerCtrler
	{
		get
		{
			return this.playerCtrler;
		}
	}

	public List<ActorController> Actors
	{
		get
		{
			return this.actors;
		}
	}

	public Vector3 BornPosition
	{
		get
		{
			return this.bornPosition;
		}
	}

	public float BornRotationY
	{
		get
		{
			return this.bornRotationY;
		}
	}

	public bool Combat
	{
		get
		{
			return this.combat;
		}
	}

	public ActorController BossActor
	{
		get
		{
			if (this.ForceShowBossActor)
			{
				return this.bossActor;
			}
			if (this.selectActor && this.selectActor.IsBoss)
			{
				return this.selectActor;
			}
			return null;
		}
	}

	public int PlayerDead
	{
		get;
		private set;
	}

	public int PetDead
	{
		get;
		private set;
	}

	public int MonsterDead
	{
		get;
		private set;
	}

	public int Score
	{
		get
		{
			int num = 3;
			num -= this.PlayerDead;
			num -= this.PetDead;
			if (num <= 0)
			{
				return 1;
			}
			return num;
		}
	}

	public BaseScene CurScene
	{
		get
		{
			return this.baseScene;
		}
	}

	public TrialScene TrialScene
	{
		get
		{
			return this.trialScene;
		}
	}

	public WorldBossScene WorldBossScene
	{
		get
		{
			return this.worldBossScene;
		}
	}

	public ArenaScene ArenaScene
	{
		get
		{
			return this.arenaScene;
		}
	}

	public PillageScene PillageScene
	{
		get
		{
			return this.pillageScene;
		}
	}

	public GuildBossScene GuildBossScene
	{
		get
		{
			return this.guildBossScene;
		}
	}

	public MemoryGearScene MemoryGearScene
	{
		get
		{
			return this.memoryGearScene;
		}
	}

	public OrePillageScene OrePillageScene
	{
		get
		{
			return this.orePillageScene;
		}
	}

	public GuildPvpScene GuildPvpScene
	{
		get
		{
			return this.guildPvpScene;
		}
	}

	public bool ForceFollow
	{
		get;
		private set;
	}

	private void Start()
	{
		for (int i = 0; i < 5; i++)
		{
			this.actors.Add(null);
		}
		Globals.Instance.SenceMgr.SencePreLoadedEvent += OnSencePreLoadedEvent;
	}

	private void FixedUpdate()
	{
		for (int i = 0; i < this.actors.Count; i++)
		{
			if (!(this.actors[i] == null))
			{
				this.actors[i].OnUpdate(Time.fixedDeltaTime);
			}
		}
		if (this.LopetActor != null)
		{
			this.LopetActor.OnUpdate(Time.fixedDeltaTime);
		}
		if (this.RemoteLopetActor != null)
		{
			this.RemoteLopetActor.OnUpdate(Time.fixedDeltaTime);
		}
		if (this.playerCtrler != null)
		{
			this.playerCtrler.OnUpdate(Time.fixedDeltaTime);
		}
	}

	private void Update()
	{
		if (!this.enableUpdate)
		{
			return;
		}
		this.invoker.Update(Time.deltaTime);
		if (this.baseScene != null)
		{
			if (this.baseScene.CurStatus == 2)
			{
				this.checkTimer += Time.deltaTime;
				if (this.checkTimer > 5f)
				{
					this.checkTimer = 0f;
					if (!this.CheckData())
					{
						this.baseScene.OnFailed();
						return;
					}
				}
			}
			this.baseScene.Update(Time.deltaTime);
			Singleton<ActionMgr>.Instance.Update(Time.deltaTime);
		}
		this.UpdateLopet(Time.deltaTime);
	}

	private void UpdateLopet(float elapse)
	{
		if (this.LopetActor != null)
		{
			if (this.hideLopetTimer > 0f)
			{
				this.hideLopetTimer -= elapse;
				if (this.LopetActor.gameObject.activeInHierarchy)
				{
					this.LopetActor.gameObject.SetActive(false);
				}
			}
			else if (!this.LopetActor.gameObject.activeInHierarchy)
			{
				this.LopetActor.gameObject.SetActive(true);
				if (this.actors[0] != null)
				{
					int slot = 9;
					Vector3 vector = CombatHelper.GetSlotPos(this.actors[0].transform.position, this.actors[0].transform, slot, false, false);
					NavMeshHit navMeshHit;
					if (NavMesh.SamplePosition(vector, out navMeshHit, 0.5f, -1))
					{
						vector = navMeshHit.position;
					}
					this.LopetActor.NavAgent.Warp(vector);
				}
			}
		}
		if (this.RemoteLopetActor != null)
		{
			if (this.hideRemoteLopetTimer > 0f)
			{
				this.hideRemoteLopetTimer -= elapse;
				if (this.RemoteLopetActor.gameObject.activeInHierarchy)
				{
					this.RemoteLopetActor.gameObject.SetActive(false);
				}
			}
			else if (!this.RemoteLopetActor.gameObject.activeInHierarchy)
			{
				this.RemoteLopetActor.gameObject.SetActive(true);
				ActorController remotePlayerActor = this.GetRemotePlayerActor();
				if (remotePlayerActor != null)
				{
					int slot2 = 9;
					Vector3 vector2 = CombatHelper.GetSlotPos(remotePlayerActor.transform.position, remotePlayerActor.transform, slot2, false, false);
					NavMeshHit navMeshHit2;
					if (NavMesh.SamplePosition(vector2, out navMeshHit2, 0.5f, -1))
					{
						vector2 = navMeshHit2.position;
					}
					this.RemoteLopetActor.NavAgent.Warp(vector2);
				}
			}
		}
	}

	public ActorController GetActor(int index)
	{
		if (index >= 5)
		{
			return null;
		}
		if (index >= this.actors.Count)
		{
			return null;
		}
		return this.actors[index];
	}

	public ActorController CreateActor(SocketDataEx socket, ActorController.EFactionType factionType, Vector3 postion, Quaternion rotation)
	{
		if (socket == null || socket.GetPet() == null)
		{
			return null;
		}
		PetDataEx pet = socket.GetPet();
		if (pet == null)
		{
			return null;
		}
		string resLoc = socket.GetResLoc();
		GameObject gameObject = Res.Load<GameObject>(resLoc, false);
		if (gameObject == null)
		{
			global::Debug.LogErrorFormat("Res.Load error, path = {0}", new object[]
			{
				resLoc
			});
			return null;
		}
		GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject, postion, rotation) as GameObject;
		if (gameObject2 == null)
		{
			global::Debug.LogErrorFormat("Instantiate error, path = {0}", new object[]
			{
				pet.Info.ResLoc
			});
			return null;
		}
		if (this.senceInfo.Type == 0 || this.senceInfo.Type == 6 || this.senceInfo.Type == 7)
		{
			NJGMapItem safeComponent = Tools.GetSafeComponent<NJGMapItem>(gameObject2);
			safeComponent.type = ((!socket.IsPlayer()) ? 2 : 5);
		}
		NavMeshAgentEx safeComponent2 = Tools.GetSafeComponent<NavMeshAgentEx>(gameObject2);
		Tools.GetSafeComponent<AnimationController>(gameObject2);
		AIController safeComponent3 = Tools.GetSafeComponent<AIController>(gameObject2);
		ActorController safeComponent4 = Tools.GetSafeComponent<ActorController>(gameObject2);
		if (factionType == ActorController.EFactionType.ERed)
		{
			gameObject2.layer = LayerDefine.MonsterLayer;
		}
		if (socket.IsPlayer())
		{
			string weaponResLoc = socket.GetWeaponResLoc();
			if (!string.IsNullOrEmpty(weaponResLoc))
			{
				gameObject = Res.Load<GameObject>(weaponResLoc, false);
				if (gameObject == null)
				{
					global::Debug.LogErrorFormat("Res.Load error, path = {0}", new object[]
					{
						weaponResLoc
					});
					return null;
				}
				GameObject gameObject3 = UnityEngine.Object.Instantiate(gameObject, postion, rotation) as GameObject;
				if (gameObject3 == null)
				{
					global::Debug.LogErrorFormat("Instantiate error, path = {0}", new object[]
					{
						weaponResLoc
					});
					return null;
				}
				GameObject gameObject4 = ObjectUtil.FindChildObject(gameObject2, "W_Rhand");
				if (gameObject4 == null)
				{
					global::Debug.LogError(new object[]
					{
						"Can not find socket : W_Rhand"
					});
					return null;
				}
				gameObject3.transform.parent = gameObject4.transform;
				gameObject3.transform.localPosition = Vector3.zero;
				gameObject3.transform.localRotation = Quaternion.identity;
				gameObject3.transform.localScale = Vector3.one;
			}
			safeComponent2.speed = 2.8f;
			safeComponent4.MaxRunSpeed = safeComponent2.speed;
			safeComponent4.SetData(socket, factionType);
			safeComponent3.EnableAI = false;
			safeComponent3.AttackDistance = 3f - safeComponent4.GetBoundsRadius();
			safeComponent3.Init();
			if (factionType == ActorController.EFactionType.EBlue)
			{
				this.playerCtrler = Tools.GetSafeComponent<PlayerController>(gameObject2);
				this.playerCtrler.Init();
			}
			this.CachePlayerSound(pet.Info.ElementType);
		}
		else
		{
			safeComponent2.speed = pet.Info.Speed;
			safeComponent4.MaxRunSpeed = safeComponent2.speed;
			safeComponent4.SetData(socket, factionType);
			safeComponent3.FindEnemyDistance = 6f;
			safeComponent3.AttackDistance = pet.Info.AttackDistance;
			safeComponent3.Init();
			PoolMgr.CreatePetPrefabPool(pet.Info);
		}
		return safeComponent4;
	}

	public ActorController CreateLocalActor(int slot, Vector3 postion, Quaternion rotation)
	{
		SocketDataEx socket = Globals.Instance.Player.TeamSystem.GetSocket(slot);
		if (socket == null)
		{
			return null;
		}
		return this.CreateActor(socket, ActorController.EFactionType.EBlue, postion, rotation);
	}

	public ActorController CreateRemoteActor(int slot, Vector3 postion, Quaternion rotation)
	{
		SocketDataEx remoteSocket = Globals.Instance.Player.TeamSystem.GetRemoteSocket(slot);
		if (remoteSocket == null)
		{
			return null;
		}
		return this.CreateActor(remoteSocket, ActorController.EFactionType.ERed, postion, rotation);
	}

	public ActorController CreateAssist(int petInfoID, int monsterInfoID, Vector3 position, Quaternion rotation, bool addSkill = true)
	{
		PetInfo info = Globals.Instance.AttDB.PetDict.GetInfo(petInfoID);
		if (info == null)
		{
			global::Debug.LogErrorFormat("MonsterDict.GetInfo error, {0}", new object[]
			{
				petInfoID
			});
			return null;
		}
		MonsterInfo info2 = Globals.Instance.AttDB.MonsterDict.GetInfo(monsterInfoID);
		if (info2 == null)
		{
			global::Debug.LogErrorFormat("MasterDict.GetInfo error, {0}", new object[]
			{
				monsterInfoID
			});
			return null;
		}
		GameObject gameObject = Res.Load<GameObject>(info.ResLoc, false);
		if (gameObject == null)
		{
			global::Debug.LogErrorFormat("Res.Load error, path = {0}", new object[]
			{
				info.ResLoc
			});
			return null;
		}
		GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject, position, rotation) as GameObject;
		if (gameObject2 == null)
		{
			global::Debug.LogErrorFormat("Instantiate error, path = {0}", new object[]
			{
				info.ResLoc
			});
			return null;
		}
		if (this.senceInfo.Type == 0 || this.senceInfo.Type == 6 || this.senceInfo.Type == 7)
		{
			NJGMapItem safeComponent = Tools.GetSafeComponent<NJGMapItem>(gameObject2);
			safeComponent.type = 2;
		}
		NavMeshAgentEx safeComponent2 = Tools.GetSafeComponent<NavMeshAgentEx>(gameObject2);
		safeComponent2.speed = info.Speed;
		Tools.GetSafeComponent<AnimationController>(gameObject2);
		AIController safeComponent3 = Tools.GetSafeComponent<AIController>(gameObject2);
		safeComponent3.FindEnemyDistance = 6f;
		safeComponent3.AttackDistance = info.AttackDistance;
		ActorController safeComponent4 = Tools.GetSafeComponent<ActorController>(gameObject2);
		safeComponent4.MaxRunSpeed = safeComponent2.speed;
		safeComponent4.SetAssistInfo(info, info2, addSkill);
		safeComponent3.Init();
		PoolMgr.CreatePetPrefabPool(info);
		PoolMgr.CreateMonsterPrefabPool(info2);
		return safeComponent4;
	}

	public ActorController CreateMonster(int infoID, Vector3 position, Quaternion rotation, Vector3 scale, int attScale = 10000)
	{
		MonsterInfo info = Globals.Instance.AttDB.MonsterDict.GetInfo(infoID);
		if (info == null)
		{
			global::Debug.LogErrorFormat("MonsterDict.GetInfo error, {0}", new object[]
			{
				infoID
			});
			return null;
		}
		GameObject gameObject = Res.Load<GameObject>(info.ResLoc, false);
		if (gameObject == null)
		{
			global::Debug.LogErrorFormat("Res.Load error, path = {0}", new object[]
			{
				info.ResLoc
			});
			return null;
		}
		GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject, position, rotation) as GameObject;
		if (gameObject2 == null)
		{
			global::Debug.LogErrorFormat("Instantiate error, path = {0}", new object[]
			{
				info.ResLoc
			});
			return null;
		}
		gameObject2.layer = LayerDefine.MonsterLayer;
		gameObject2.transform.localScale = scale;
		if (this.senceInfo.Type == 0 || this.senceInfo.Type == 6)
		{
			NJGMapItem safeComponent = Tools.GetSafeComponent<NJGMapItem>(gameObject2);
			if (info.BossType != 0)
			{
				safeComponent.type = 4;
				safeComponent.showDeath = true;
			}
			else
			{
				safeComponent.type = 3;
			}
		}
		NavMeshAgentEx safeComponent2 = Tools.GetSafeComponent<NavMeshAgentEx>(gameObject2);
		safeComponent2.speed = info.Speed;
		Tools.GetSafeComponent<AnimationController>(gameObject2);
		AIController aIController;
		if (string.IsNullOrEmpty(info.AIScript))
		{
			aIController = gameObject2.AddComponent<AIController>();
		}
		else
		{
			aIController = (gameObject2.AddComponent(info.AIScript) as AIController);
		}
		aIController.EnableAI = true;
		aIController.AttackDistance = info.AttackDistance;
		if (aIController.AttackDistance > 1.5f)
		{
			aIController.FindEnemyDistance = 7f;
		}
		if (info.FindEnemyDistance > aIController.FindEnemyDistance)
		{
			aIController.FindEnemyDistance = info.FindEnemyDistance;
		}
		ActorController safeComponent3 = Tools.GetSafeComponent<ActorController>(gameObject2);
		safeComponent3.MaxRunSpeed = safeComponent2.speed;
		safeComponent3.SetMonsterInfo(info, this.senceInfo, attScale, ActorController.EFactionType.ERed);
		if (info.BossType != 0)
		{
			if (info.OutLine)
			{
				this.bossOutLine = Tools.GetSafeComponent<OutlineShader>(gameObject2);
				this.bossOutLine.Outline = false;
			}
			if (info.BossType == 1)
			{
				this.bossActor = safeComponent3;
				safeComponent3.CorpseDecayTime = 5f;
			}
			if (this.combatLog != null)
			{
				this.combatLog.BossInfoID = info.ID;
				this.combatLog.Attack = safeComponent3.GetInitAtt(EAttID.EAID_Attack);
				this.combatLog.PhysicDefense = safeComponent3.GetInitAtt(EAttID.EAID_PhysicDefense);
				this.combatLog.MagicDefense = safeComponent3.GetInitAtt(EAttID.EAID_MagicDefense);
				this.combatLog.MaxHP = safeComponent3.GetLongInitAtt(EAttID.EAID_MaxHP);
			}
		}
		if (info.LootMoney != 0u)
		{
			aIController.Locked = true;
		}
		aIController.Init();
		PoolMgr.CreateMonsterPrefabPool(info);
		return safeComponent3;
	}

	public ActorController CreateBuilding(int infoID, Vector3 position, Quaternion rotation, Vector3 scale, ActorController.EFactionType factionType, bool showMiniMap, bool showHP)
	{
		MonsterInfo info = Globals.Instance.AttDB.MonsterDict.GetInfo(infoID);
		if (info == null)
		{
			global::Debug.LogErrorFormat("MonsterDict.GetInfo error, {0}", new object[]
			{
				infoID
			});
			return null;
		}
		GameObject gameObject = Res.Load<GameObject>(info.ResLoc, false);
		if (gameObject == null)
		{
			global::Debug.LogErrorFormat("Res.Load error, path = {0}", new object[]
			{
				info.ResLoc
			});
			return null;
		}
		GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject, position, rotation) as GameObject;
		if (gameObject2 == null)
		{
			global::Debug.LogErrorFormat("Instantiate error, path = {0}", new object[]
			{
				info.ResLoc
			});
			return null;
		}
		if (factionType == ActorController.EFactionType.EBlue)
		{
			NGUITools.SetLayer(gameObject2, LayerDefine.CharLayer);
		}
		else
		{
			NGUITools.SetLayer(gameObject2, LayerDefine.MonsterLayer);
		}
		gameObject2.transform.localScale = scale;
		if (showMiniMap)
		{
			NJGMapItem safeComponent = Tools.GetSafeComponent<NJGMapItem>(gameObject2);
			if (factionType == ActorController.EFactionType.EBlue)
			{
				safeComponent.type = 2;
			}
			else
			{
				safeComponent.type = 3;
			}
		}
		Tools.GetSafeComponent<AnimationController>(gameObject2);
		AIController aIController;
		if (string.IsNullOrEmpty(info.AIScript))
		{
			aIController = gameObject2.AddComponent<AIController>();
		}
		else
		{
			aIController = (gameObject2.AddComponent(info.AIScript) as AIController);
		}
		aIController.EnableAI = true;
		aIController.AttackDistance = info.AttackDistance;
		if (aIController.AttackDistance > 1.5f)
		{
			aIController.FindEnemyDistance = 7f;
		}
		if (info.FindEnemyDistance > aIController.FindEnemyDistance)
		{
			aIController.FindEnemyDistance = info.FindEnemyDistance;
		}
		ActorController safeComponent2 = Tools.GetSafeComponent<ActorController>(gameObject2);
		safeComponent2.SetMonsterInfo(info, this.senceInfo, 10000, ActorController.EFactionType.EBlue);
		safeComponent2.CorpseDecayTime = -1f;
		safeComponent2.IsBuilding = true;
		aIController.Init();
		PoolMgr.CreateMonsterPrefabPool(info);
		safeComponent2.SetRoateable(false);
		if (!showHP)
		{
			safeComponent2.PlayerheadTip.enabled = false;
		}
		return safeComponent2;
	}

	public ActorController CreateLopet(LopetDataEx lpData, ActorController.EFactionType factionType, Vector3 position, Quaternion rotation)
	{
		if (lpData == null || lpData.Info == null)
		{
			global::Debug.LogError(new object[]
			{
				"LopetDataEx error"
			});
			return null;
		}
		GameObject gameObject = Res.Load<GameObject>(lpData.Info.ResLoc, false);
		if (gameObject == null)
		{
			global::Debug.LogErrorFormat("Res.Load error, path = {0}", new object[]
			{
				lpData.Info.ResLoc
			});
			return null;
		}
		GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject, position, rotation) as GameObject;
		if (gameObject2 == null)
		{
			global::Debug.LogErrorFormat("Instantiate error, path = {0}", new object[]
			{
				lpData.Info.ResLoc
			});
			return null;
		}
		NGUITools.SetLayer(gameObject2, LayerDefine.CharLayer);
		Tools.GetSafeComponent<NavMeshAgentEx>(gameObject2);
		Tools.GetSafeComponent<AnimationController>(gameObject2);
		AIController safeComponent = Tools.GetSafeComponent<AIController>(gameObject2);
		safeComponent.EnableAI = false;
		ActorController safeComponent2 = Tools.GetSafeComponent<ActorController>(gameObject2);
		safeComponent2.SetLopetData(lpData, factionType);
		safeComponent2.PlayerheadTip.enabled = false;
		safeComponent.Init();
		return safeComponent2;
	}

	public ActorController CreateLocalLopet(Vector3 postion, Quaternion rotation)
	{
		LopetDataEx lopet = Globals.Instance.Player.TeamSystem.GetLopet(true);
		if (lopet == null)
		{
			return null;
		}
		return this.CreateLopet(lopet, ActorController.EFactionType.EBlue, postion, rotation);
	}

	public ActorController CreateRemoteLopet(Vector3 postion, Quaternion rotation)
	{
		LopetDataEx lopet = Globals.Instance.Player.TeamSystem.GetLopet(false);
		if (lopet == null)
		{
			return null;
		}
		return this.CreateLopet(lopet, ActorController.EFactionType.ERed, postion, rotation);
	}

	public ActorController SummonMonster(int monsterID, Vector3 position, Quaternion rotation)
	{
		ActorController actorController = this.CreateMonster(monsterID, position, rotation, Vector3.one, 10000);
		if (actorController != null)
		{
			this.actors.Add(actorController);
		}
		return actorController;
	}

	public void ClearActor()
	{
		GameUIManager.mInstance.HUDTextMgr.Clear();
		Globals.Instance.EffectSoundMgr.ClearCache();
		this.invoker.Clear();
		if (this.baseScene != null)
		{
			this.combatTime = this.baseScene.GetMaxCombatTimer() - this.baseScene.GetCombatTimer();
			this.baseScene.Destroy();
			this.baseScene = null;
		}
		this.senceInfo = null;
		this.actors.Clear();
		this.LopetActor = null;
		this.RemoteLopetActor = null;
		for (int i = 0; i < 5; i++)
		{
			this.actors.Add(null);
		}
		this.assistPetID = 0;
		this.assistPetAttID = 0;
		this.combatLog = null;
		this.ServerData = null;
	}

	public void OnSencePreLoadedEvent(SceneInfo _senceInfo)
	{
		GameUIManager.mInstance.HUDTextMgr.Clear();
		Globals.Instance.EffectSoundMgr.ClearCache();
		int uiLayer = LayerDefine.uiLayer;
		int uiMeshLayer = LayerDefine.uiMeshLayer;
		Camera.main.cullingMask &= ~(1 << uiLayer | 1 << uiMeshLayer);
		this.senceInfo = _senceInfo;
		this.ForceFollow = false;
		this.enableUpdate = true;
		this.preSelectScale = 1f;
		this.invoker.SetEnable(true);
		PoolMgr.spawnPool = PoolManager.Pools.Create("prefab");
		PoolMgr.spawnPoolByName = PoolManager.Pools.Create("name");
		PoolMgr.CreatePrefabPool("Skill/st_011", 5, 10);
		PoolMgr.CreatePrefabPool("Skill/st_010", 5, 10);
		PoolMgr.CreatePrefabPool("Skill/misc_001", 5, 10);
		Transform transform = PoolMgr.Spawn("Skill/misc_001");
		PoolMgr.CreateActionPrefabPool(transform);
		PoolMgr.Despawn(transform);
		PoolMgr.CreatePrefabPool("Skill/misc_002", 5, 10);
		transform = PoolMgr.Spawn("Skill/misc_002");
		PoolMgr.CreateActionPrefabPool(transform);
		PoolMgr.Despawn(transform);
		PoolMgr.CreatePrefabPool("Skill/misc_003", 1, 5);
		transform = PoolMgr.Spawn("Skill/misc_003");
		PoolMgr.CreateActionPrefabPool(transform);
		PoolMgr.Despawn(transform);
		PoolMgr.CreatePrefabPool("Skill/misc_004", 1, 5);
		transform = PoolMgr.Spawn("Skill/misc_004");
		PoolMgr.CreateActionPrefabPool(transform);
		PoolMgr.Despawn(transform);
		PoolMgr.CreatePrefabPool("Skill/Tan", 6, 6);
		transform = PoolMgr.Spawn("Skill/Tan");
		PoolMgr.CreateActionPrefabPool(transform);
		PoolMgr.Despawn(transform);
		GameObject gameObject = Res.Load<GameObject>("Skill/st_006", false);
		if (gameObject == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load st_006 error"
			});
		}
		else
		{
			this.selectEffect = (UnityEngine.Object.Instantiate(gameObject) as GameObject);
		}
		if (this.selectEffect == null)
		{
			global::Debug.LogError(new object[]
			{
				"Instantiate st_006 error"
			});
		}
		else
		{
			this.selectPS = this.selectEffect.GetComponentsInChildren<ParticleSystem>();
			this.selectEffect.SetActive(false);
		}
		GameUIManager.mInstance.GameStateChange(GUIGameStateTip.EGAMEING_STATE.NONE, 0);
		CameraShake.Instance.IsShaking();
		this.combatLog = new CombatLog();
		this.checkTimer = 10f;
		this.InitScene();
	}

	private void InitScene()
	{
		switch (this.senceInfo.Type)
		{
		case 0:
			if (GameConst.GetInt32(110) == this.senceInfo.ID)
			{
				this.baseScene = new StartScene(this);
			}
			else if (this.senceInfo.SubType == 0)
			{
				if (this.worldScene == null)
				{
					this.worldScene = new WorldScene(this);
				}
				this.baseScene = this.worldScene;
			}
			else
			{
				if (this.defenseScene == null)
				{
					this.defenseScene = new DefenseScene(this);
				}
				this.baseScene = this.defenseScene;
			}
			break;
		case 1:
			if (this.trialScene == null)
			{
				this.trialScene = new TrialScene(this);
			}
			this.baseScene = this.trialScene;
			break;
		case 2:
			if (this.arenaScene == null)
			{
				this.arenaScene = new ArenaScene(this);
			}
			this.baseScene = this.arenaScene;
			break;
		case 3:
			if (this.worldBossScene == null)
			{
				this.worldBossScene = new WorldBossScene(this);
			}
			this.baseScene = this.worldBossScene;
			break;
		case 4:
			if (this.pillageScene == null)
			{
				this.pillageScene = new PillageScene(this);
			}
			this.baseScene = this.pillageScene;
			break;
		case 5:
			if (this.guildBossScene == null)
			{
				this.guildBossScene = new GuildBossScene(this);
			}
			this.baseScene = this.guildBossScene;
			break;
		case 6:
			if (this.senceInfo.SubType == 0 || this.senceInfo.SubType == 1)
			{
				this.baseScene = new KingRewardScene1(this);
			}
			else
			{
				this.baseScene = new KingRewardScene2(this);
			}
			break;
		case 7:
			if (this.memoryGearScene == null)
			{
				this.memoryGearScene = new MemoryGearScene(this);
			}
			this.baseScene = this.memoryGearScene;
			break;
		case 8:
			if (this.orePillageScene == null)
			{
				this.orePillageScene = new OrePillageScene(this);
			}
			this.baseScene = this.orePillageScene;
			break;
		case 9:
			if (this.guildPvpScene == null)
			{
				this.guildPvpScene = new GuildPvpScene(this);
			}
			this.baseScene = this.guildPvpScene;
			break;
		default:
			global::Debug.LogErrorFormat("senceInfo type error, type = {0}", new object[]
			{
				this.senceInfo.Type
			});
			break;
		}
		if (this.baseScene == null)
		{
			global::Debug.LogError(new object[]
			{
				"baseScene is null"
			});
			return;
		}
		this.baseScene.SetSceneInfo(this.senceInfo);
		this.baseScene.Init();
		this.LoadRespawnInfo();
		this.InitEnvironment();
	}

	private void LoadRespawnInfo()
	{
		string assetFileName = string.Format("Respawn/{0}", (this.senceInfo.RespawnInfoID != 0) ? this.senceInfo.RespawnInfoID : this.senceInfo.ID);
		SceneRespawnData sceneRespawnData = SceneRespawnData.LoadFromFile(assetFileName);
		if (sceneRespawnData != null)
		{
			this.bornPosition.x = (float)sceneRespawnData.BPx;
			this.bornPosition.y = (float)sceneRespawnData.BPy;
			this.bornPosition.z = (float)sceneRespawnData.BPz;
			this.bornRotationY = (float)sceneRespawnData.BRy;
			for (int i = 0; i < sceneRespawnData.Respawn.Count; i++)
			{
				Respawn respawn = sceneRespawnData.Respawn[i];
				if (respawn.Sx < 1.4012984643248171E-45)
				{
					respawn.Sx = 1.0;
				}
				if (respawn.Sy < 1.4012984643248171E-45)
				{
					respawn.Sy = 1.0;
				}
				if (respawn.Sz < 1.4012984643248171E-45)
				{
					respawn.Sz = 1.0;
				}
				this.baseScene.OnLoadRespawnPoint(respawn.ID, respawn.GroupID, new Vector3((float)respawn.Px, (float)respawn.Py, (float)respawn.Pz), (float)respawn.Ry, new Vector3((float)respawn.Sx, (float)respawn.Sy, (float)respawn.Sz));
			}
			this.baseScene.OnLoadRespawnOK();
		}
	}

	public void CreateLocalActors()
	{
		int[] array = new int[6];
		Quaternion rotation = Quaternion.Euler(0f, this.bornRotationY, 0f);
		for (int i = 1; i < 4; i++)
		{
			this.actors[i] = this.CreateLocalActor(i, this.bornPosition, rotation);
			if (this.actors[i] != null)
			{
				array[i] = this.actors[i].GetPlayerSkillID();
			}
		}
		if (this.assistPetID != 0 && this.assistPetAttID != 0)
		{
			this.actors[4] = this.CreateAssist(this.assistPetID, this.assistPetAttID, this.bornPosition, rotation, true);
		}
		this.LopetActor = this.CreateLocalLopet(this.bornPosition, rotation);
		if (this.LopetActor != null)
		{
			array[4] = this.LopetActor.GetPlayerSkillID();
		}
		array[0] = 16;
		this.actors[0] = this.CreateLocalActor(0, this.bornPosition, rotation);
		if (this.actors[0] != null)
		{
			for (int j = 0; j < array.Length; j++)
			{
				if (array[j] != 0)
				{
					this.actors[0].AddSkill(j, array[j], true);
				}
			}
		}
		this.ResetAI();
		this.actors[0].AiCtrler.EnableAI = false;
		this.playerCtrler.SetControlLocked(true);
	}

	public void ResetAI()
	{
		if (this.actors[0] == null)
		{
			return;
		}
		int num = 0;
		int num2 = 0;
		for (int i = 1; i < 5; i++)
		{
			if (!(this.actors[i] == null))
			{
				this.actors[i].CastPassiveSkill();
				if (i <= 3)
				{
					if (this.actors[i].IsMelee)
					{
						num++;
					}
					else
					{
						num2++;
					}
				}
			}
		}
		int num3 = 0;
		int num4 = 0;
		for (int j = 1; j < 5; j++)
		{
			if (!(this.actors[j] == null))
			{
				this.actors[j].CurHP = this.actors[j].MaxHP;
				int num5;
				if (j >= 4)
				{
					if (this.LopetActor == null)
					{
						num5 = 7;
					}
					else
					{
						num5 = 8;
					}
				}
				else if (this.actors[j].IsMelee)
				{
					num3++;
					num5 = 100 + num * 10 + num3;
				}
				else
				{
					num4++;
					num5 = 200 + num2 * 10 + num4;
				}
				Vector3 vector = CombatHelper.GetSlotPos(this.actors[0].transform.position, this.actors[0].transform, num5, false, false);
				NavMeshHit navMeshHit;
				if (NavMesh.SamplePosition(vector, out navMeshHit, 0.5f, -1))
				{
					vector = navMeshHit.position;
				}
				this.actors[j].NavAgent.Warp(vector);
				this.actors[j].AiCtrler.SetFellowSlot(num5);
				this.actors[j].AiCtrler.ForceFollow = this.ForceFollow;
				if (this.ForceFollow)
				{
					this.actors[j].AiCtrler.FollowForce(this.actors[0], num5);
					this.actors[j].AiCtrler.SetSelectTarget(null);
				}
				else
				{
					this.actors[j].AiCtrler.Follow(this.actors[0], num5);
				}
			}
		}
		if (this.LopetActor != null)
		{
			int num5 = 9;
			Vector3 vector = CombatHelper.GetSlotPos(this.actors[0].transform.position, this.actors[0].transform, num5, false, false);
			NavMeshHit navMeshHit;
			if (NavMesh.SamplePosition(vector, out navMeshHit, 0.5f, -1))
			{
				vector = navMeshHit.position;
			}
			this.LopetActor.NavAgent.Warp(vector);
			this.LopetActor.AiCtrler.SetFellowSlot(num5);
			this.LopetActor.AiCtrler.ForceFollow = this.ForceFollow;
			if (this.ForceFollow)
			{
				this.LopetActor.AiCtrler.FollowForce(this.actors[0], num5);
				this.LopetActor.AiCtrler.SetSelectTarget(null);
			}
			else
			{
				this.LopetActor.AiCtrler.Follow(this.actors[0], num5);
			}
		}
		this.actors[0].CurHP = this.actors[0].MaxHP;
		this.playerCtrler.SetAttackArea(true);
		this.UnlockAllActorAI();
	}

	public void OnUILoadingFinish()
	{
		float preStartDelay = this.baseScene.GetPreStartDelay();
		float num = this.baseScene.GetStartDelay();
		if (preStartDelay >= num)
		{
			num = preStartDelay + 0.5f;
		}
		this.invoker.DoInvoke(this, "OnPreStart", preStartDelay);
		this.invoker.DoInvoke(this, "OnStart", num);
		this.baseScene.OnUILoaded();
		Tools.PlaySceneBGM(Globals.Instance.SenceMgr.sceneInfo);
	}

	private void OnPreStart()
	{
		this.baseScene.OnPreStart();
	}

	private void OnStart()
	{
		if (this.playerCtrler != null)
		{
			this.playerCtrler.SetControlLocked(false);
		}
		this.win = false;
		this.lootMoney = 0;
		this.PlayerDead = 0;
		this.PetDead = 0;
		this.MonsterDead = 0;
		this.combatLog.StartTime = Globals.Instance.Player.GetTimeStamp();
		this.baseScene.OnStart();
	}

	public void SetSelectTarget(ActorController actor)
	{
		if (actor == null)
		{
			return;
		}
		for (int i = 0; i < 5; i++)
		{
			if (!(this.actors[i] == null))
			{
				this.actors[i].AiCtrler.SetSelectTarget(actor);
			}
		}
	}

	public void OnPlayerFindEnemy(ActorController actor)
	{
		this.combat = true;
		this.baseScene.OnPlayerFindEnemy(actor);
	}

	public void OnActorDead(ActorController actor, ActorController killer)
	{
		if (actor == null)
		{
			return;
		}
		if (actor.ActorType == ActorController.EActorType.EPlayer)
		{
			if (actor.FactionType == ActorController.EFactionType.EBlue)
			{
				this.AddActorCombatLog(actor);
			}
			this.OnPlayerDead(actor);
		}
		else if (actor.ActorType == ActorController.EActorType.EPet)
		{
			if (actor.FactionType == ActorController.EFactionType.EBlue)
			{
				this.AddActorCombatLog(actor);
			}
			this.OnPetDead(actor);
		}
		else
		{
			this.OnMonsterDead(actor);
		}
		if (this.KillActorEvent != null && killer != null)
		{
			this.KillActorEvent(killer.GetName(), actor.GetName(), actor.FactionType == ActorController.EFactionType.EBlue);
		}
	}

	private void OnPlayerDead(ActorController actor)
	{
		if (actor.FactionType == ActorController.EFactionType.ERed)
		{
			this.baseScene.OnRemoteActorDead();
			return;
		}
		if (actor == this.playerCtrler.ActorCtrler)
		{
			this.playerCtrler.SetAttackArea(false);
		}
		if (this.senceInfo.Type != 7)
		{
			this.LockAllActorAI();
		}
		this.ClearAllThreat();
		if (this.senceInfo.DayReset)
		{
			this.PlayerDead++;
		}
		if (!this.win)
		{
			this.baseScene.OnPlayerDead();
		}
		else
		{
			actor.Resurrect(false);
		}
	}

	private void OnPetDead(ActorController actor)
	{
		if (actor.FactionType == ActorController.EFactionType.ERed)
		{
			this.baseScene.OnRemoteActorDead();
			return;
		}
		if (this.senceInfo.DayReset)
		{
			this.PetDead++;
		}
		this.baseScene.OnPetDead();
	}

	private void OnMonsterDead(ActorController actor)
	{
		if (actor == null)
		{
			return;
		}
		if (!actor.IsBox)
		{
			this.MonsterDead++;
		}
		if (!actor.IsBoss && !actor.IsBox && UtilFunc.RangeRandom(0f, 100f) <= this.senceInfo.LootHPMPCoef)
		{
			this.LootHPMP(actor.transform.position, actor.Level);
		}
		if (this.selectActor == actor)
		{
			if (this.selectActor.IsBoss)
			{
				if (this.bossOutLine != null)
				{
					this.bossOutLine.Outline = false;
				}
			}
			else if (this.selectEffect != null)
			{
				this.selectEffect.transform.parent = null;
				this.selectEffect.SetActive(false);
			}
		}
		if (actor.IsBox && this.senceInfo.MinLootMoney > 0)
		{
			int num = this.senceInfo.MinLootMoney;
			if (this.senceInfo.MinLootMoney < this.senceInfo.MaxLootMoney)
			{
				num = UtilFunc.RangeRandom(this.senceInfo.MinLootMoney, this.senceInfo.MaxLootMoney);
			}
			this.lootMoney += num;
			if (this.LootMoneyEvent != null)
			{
				this.LootMoneyEvent(actor, num);
			}
		}
		if (this.actors.Remove(actor))
		{
			bool flag = true;
			for (int i = 5; i < this.actors.Count; i++)
			{
				if (this.actors[i] != null && this.actors[i].ActorType == ActorController.EActorType.EMonster && !this.actors[i].IsBox)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				this.combat = false;
				this.baseScene.OnAllMonsterDead();
			}
		}
	}

	private void AddActorCombatLog(ActorController actor)
	{
		if (actor == null || this.combatLog == null)
		{
			return;
		}
		if (actor.HighestDamage > this.combatLog.HighestDamage)
		{
			this.combatLog.HighestDamage = actor.HighestDamage;
		}
		if (actor.HighestHeal > this.combatLog.HighestHeal)
		{
			this.combatLog.HighestHeal = actor.HighestHeal;
		}
		for (int i = 0; i < this.combatLog.Data.Count; i++)
		{
			if (this.combatLog.Data[i].PetID == actor.GetPetInfoID() && this.combatLog.Data[i].SocketSlot == actor.SocketSlot)
			{
				this.combatLog.Data[i].HPPct = (int)actor.CurHP * 100 / (int)actor.MaxHP;
				this.combatLog.Data[i].DamageTaken += actor.DamageTaken;
				this.combatLog.Data[i].DamageTakenCount += actor.DamageTakenCount;
				this.combatLog.Data[i].HealTaken += actor.HealTaken;
				this.combatLog.Data[i].HealTakenCount += actor.HealTakenCount;
				this.combatLog.Data[i].Damage += actor.TotalDamage;
				this.combatLog.Data[i].Heal += actor.TotalHeal;
				for (int j = 0; j < actor.Skills.Length; j++)
				{
					if (actor.Skills[j] != null && actor.Skills[j].Info != null)
					{
						bool flag = false;
						for (int k = 0; k < this.combatLog.Data[i].Data.Count; k++)
						{
							if (actor.Skills[j].Info.ID == this.combatLog.Data[i].Data[k].SkillID)
							{
								this.combatLog.Data[i].Data[k].Count += actor.Skills[j].Count;
								this.combatLog.Data[i].Data[k].Damage += actor.Skills[j].Damage;
								this.combatLog.Data[i].Data[k].Heal += actor.Skills[j].Heal;
								if (actor.Skills[j].HighDamage > this.combatLog.Data[i].Data[k].HighDamage)
								{
									this.combatLog.Data[i].Data[k].HighDamage = actor.Skills[j].HighDamage;
								}
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							SkillLog skillLog = new SkillLog();
							skillLog.SkillID = actor.Skills[j].Info.ID;
							skillLog.Count = actor.Skills[j].Count;
							skillLog.Damage = actor.Skills[j].Damage;
							skillLog.Heal = actor.Skills[j].Heal;
							skillLog.HighDamage = actor.Skills[j].HighDamage;
							this.combatLog.Data[i].Data.Add(skillLog);
						}
					}
				}
				return;
			}
		}
		ActorCombatLog actorCombatLog = new ActorCombatLog();
		actorCombatLog.PetID = actor.GetPetInfoID();
		actorCombatLog.HPPct = (int)actor.CurHP * 100 / (int)actor.MaxHP;
		actorCombatLog.DamageTaken += actor.DamageTaken;
		actorCombatLog.DamageTakenCount += actor.DamageTakenCount;
		actorCombatLog.HealTaken += actor.HealTaken;
		actorCombatLog.HealTakenCount += actor.HealTakenCount;
		actorCombatLog.Damage += actor.TotalDamage;
		actorCombatLog.Heal += actor.TotalHeal;
		actorCombatLog.SocketSlot = actor.SocketSlot;
		for (int l = 0; l < actor.Skills.Length; l++)
		{
			if (actor.Skills[l] != null && actor.Skills[l].Info != null)
			{
				SkillLog skillLog2 = new SkillLog();
				skillLog2.SkillID = actor.Skills[l].Info.ID;
				skillLog2.Count = actor.Skills[l].Count;
				skillLog2.Damage = actor.Skills[l].Damage;
				skillLog2.Heal = actor.Skills[l].Heal;
				skillLog2.HighDamage = actor.Skills[l].HighDamage;
				actorCombatLog.Data.Add(skillLog2);
			}
		}
		this.combatLog.Data.Add(actorCombatLog);
	}

	public void OnWin()
	{
		this.win = true;
		for (int i = 0; i < 5; i++)
		{
			if (this.actors[i] == null || this.actors[i].IsDead)
			{
				this.poseOK[i] = true;
			}
			else
			{
				this.poseOK[i] = false;
				this.actors[i].InterruptSkill(0);
				this.actors[i].AiCtrler.Win = true;
				this.actors[i].AiCtrler.Locked = false;
			}
		}
		this.playerCtrler.SetControlLocked(true);
		this.playerCtrler.SetAttackArea(false);
		this.playerCtrler.HideTargetDirection();
		Globals.Instance.CameraMgr.Pause();
	}

	public void GoPosePoint()
	{
		Vector3 camCurBasePos = Globals.Instance.CameraMgr.GetCamCurBasePos();
		Vector3 right = Camera.main.transform.right;
		NavMeshPath navMeshPath = new NavMeshPath();
		Vector3 vector = camCurBasePos;
		for (int i = 0; i < 5; i++)
		{
			if (!this.poseOK[i])
			{
				switch (i)
				{
				case 0:
					vector = camCurBasePos;
					break;
				case 1:
					vector = camCurBasePos - right * 1f;
					break;
				case 2:
					vector = camCurBasePos + right * 1f;
					break;
				case 3:
					vector = camCurBasePos + right * 1f * 2f;
					break;
				case 4:
					vector = camCurBasePos - right * 1f * 2f;
					break;
				}
				if (!this.actors[i].NavAgent.CalculatePath(vector, navMeshPath) || navMeshPath.status != NavMeshPathStatus.PathComplete)
				{
					this.OnArrivedPosePoint(i);
				}
				else
				{
					this.actors[i].AiCtrler.GoPoint(vector, i, 2f);
				}
			}
		}
	}

	public void OnArrivedPosePoint(int pointID)
	{
		if (pointID < 0 || pointID >= 5)
		{
			return;
		}
		this.poseOK[pointID] = true;
		if (this.actors[pointID] != null)
		{
			this.actors[pointID].AiCtrler.Locked = true;
			Vector3 forward = -Camera.main.transform.forward;
			Quaternion rotation = Quaternion.LookRotation(forward);
			rotation.x = 0f;
			rotation.z = 0f;
			this.actors[pointID].RotateTo(rotation);
		}
		bool flag = true;
		for (int i = 0; i < this.poseOK.Length; i++)
		{
			if (!this.poseOK[i])
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			Globals.Instance.CameraMgr.Resume();
			Globals.Instance.CameraMgr.resultCamTest = true;
			this.invoker.DoInvoke(this, "OnPlayWin", 0.7f);
		}
	}

	private void OnPlayWin()
	{
		this.PlayWin(false, true);
	}

	private void OnPlayWinOver()
	{
		this.baseScene.OnPlayWinOver();
	}

	public void PlayWin(bool pvp, bool win)
	{
		Quaternion rotation = Quaternion.identity;
		if (!pvp)
		{
			Globals.Instance.GameMgr.ClearSpeedMod();
		}
		else
		{
			Vector3 forward = -Camera.main.transform.forward;
			rotation = Quaternion.LookRotation(forward);
			rotation.x = 0f;
			rotation.z = 0f;
		}
		PlayAnimation playAnimation = new PlayAnimation();
		playAnimation.AnimName = "win";
		playAnimation.PlayMode = PlayMode.StopAll;
		playAnimation.WrapMode = WrapMode.Loop;
		playAnimation.priority = 10;
		int num = 0;
		int num2 = 5;
		if (!win)
		{
			num = 5;
			num2 = this.actors.Count;
		}
		for (int i = num; i < num2; i++)
		{
			if (this.actors[i] != null && !this.actors[i].IsDead)
			{
				if (pvp)
				{
					this.actors[i].RotateTo(rotation);
				}
				if (this.actors[i].PlayerheadTip != null)
				{
					this.actors[i].PlayerheadTip.IsWin = true;
				}
				this.actors[i].AnimationCtrler.PlayAnimation(playAnimation);
				this.AddActorCombatLog(this.actors[i]);
			}
		}
		if (win)
		{
			if (this.LopetActor != null)
			{
				this.LopetActor.RotateTo(rotation);
			}
		}
		else if (this.RemoteLopetActor != null)
		{
			this.RemoteLopetActor.RotateTo(rotation);
		}
		this.combatLog.EndTime = Globals.Instance.Player.GetTimeStamp();
		this.combatLog.KillMonsterCount = this.MonsterDead;
		this.combatLog.Win = ((!win) ? 0 : 1);
		this.invoker.DoInvoke(this, "OnPlayWinOver", 1.5f);
	}

	public void OnSelect(ActorController actor)
	{
		if (this.selectEffect == null)
		{
			return;
		}
		if (actor != null && actor.IsDead)
		{
			return;
		}
		if (actor == this.selectActor)
		{
			return;
		}
		this.selectActor = actor;
		if (this.selectActor == null)
		{
			this.selectEffect.transform.parent = null;
			this.selectEffect.SetActive(false);
		}
		else if (this.selectActor.IsBoss)
		{
			this.selectEffect.transform.parent = null;
			this.selectEffect.SetActive(false);
			if (this.bossOutLine != null)
			{
				this.bossOutLine.Outline = true;
			}
		}
		else
		{
			this.selectEffect.transform.parent = this.selectActor.gameObject.transform;
			this.selectEffect.transform.localPosition = new Vector3(0f, 0.04f, 0f);
			this.selectEffect.transform.localRotation = Quaternion.identity;
			this.selectEffect.transform.localScale = Vector3.one;
			this.selectEffect.SetActive(true);
			float num = this.selectActor.GetBoundsMinRadius() / 0.4f;
			if (this.preSelectScale != num && num > 0f && this.selectPS != null)
			{
				float num2 = num / this.preSelectScale;
				for (int i = 0; i < this.selectPS.Length; i++)
				{
					this.selectPS[i].startSize *= num2;
					this.selectPS[i].Simulate(0f, false, true);
					this.selectPS[i].Play();
				}
				this.preSelectScale = num;
			}
			if (this.bossOutLine != null)
			{
				this.bossOutLine.Outline = false;
			}
		}
	}

	public static void CancelCreateUIActorAsync(ResourceEntity Entity)
	{
		if (Globals.Instance == null)
		{
			return;
		}
		Globals.Instance.ResourceCache.CancelAsyncResource(Entity);
	}

	public static ResourceEntity CreateUIActorAsync(string prefabPath, string weaponPrefabPath, float scale = 1f, int depth = 0, bool playAction = true, bool canRotate = true, GameObject parent = null, int gender = 0, string uiAction = "", float rotation = 180f, Action<GameObject> callback = null)
	{
		return Globals.Instance.ResourceCache.LoadResourceAsync(prefabPath, typeof(GameObject), delegate(UnityEngine.Object resActorPrefab)
		{
			GameObject obj = null;
			if (resActorPrefab == null)
			{
				global::Debug.LogErrorFormat("CreateUIActorAsync error, name = {0}", new object[]
				{
					prefabPath
				});
			}
			else
			{
				UnityEngine.Object weaponPrefab = (!string.IsNullOrEmpty(weaponPrefabPath)) ? Globals.Instance.ResourceCache.LoadResource<GameObject>(weaponPrefabPath, 0f) : null;
				obj = ActorManager.CreateUIActor(resActorPrefab, weaponPrefab, scale, depth, playAction, canRotate, parent, gender, uiAction, rotation);
			}
			if (callback != null)
			{
				callback(obj);
			}
		}, 0f);
	}

	public static GameObject CreateUIActor(UnityEngine.Object actorPrefab, UnityEngine.Object weaponPrefab, float scale = 1f, int depth = 0, bool playAction = true, bool canRotate = true, GameObject parent = null, int gender = 0, string uiAction = "", float rotation = 180f)
	{
		if (actorPrefab == null)
		{
			global::Debug.LogError(new object[]
			{
				"[CreateUIActor] error, param null."
			});
			return null;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(actorPrefab) as GameObject;
		if (gameObject == null)
		{
			global::Debug.LogErrorFormat("Instantiate error, name = {0}", new object[]
			{
				actorPrefab.name
			});
			return null;
		}
		NGUITools.SetLayer(gameObject, LayerDefine.uiLayer);
		UIActorController uIActorController = gameObject.AddComponent<UIActorController>();
		uIActorController.Gender = gender;
		uIActorController.UIAction = uiAction;
		uIActorController.PlayAction = playAction;
		uIActorController.CanRotate = canRotate;
		GameObject gameObject2 = new GameObject("Empty");
		gameObject2.layer = LayerDefine.uiLayer;
		UIWidget uIWidget = gameObject2.AddComponent<UIWidget>();
		uIWidget.depth = depth;
		gameObject2.transform.parent = gameObject.transform;
		gameObject2.transform.localPosition = Vector3.zero;
		gameObject2.transform.localScale = Vector3.zero;
		if (weaponPrefab != null)
		{
			uIActorController.IsPlayer = true;
			GameObject gameObject3 = UnityEngine.Object.Instantiate(weaponPrefab) as GameObject;
			if (gameObject3 == null)
			{
				global::Debug.LogErrorFormat("[CreateUIActor] Instantiate weapon error, path = {0}", new object[]
				{
					weaponPrefab.name
				});
			}
			else
			{
				NGUITools.SetLayer(gameObject3, LayerDefine.uiLayer);
				GameObject gameObject4 = ObjectUtil.FindChildObject(gameObject, "W_Rhand");
				if (gameObject4 == null)
				{
					global::Debug.LogError(new object[]
					{
						"Can not find socket : W_Rhand"
					});
				}
				else
				{
					gameObject3.transform.parent = gameObject4.transform;
					gameObject3.transform.localPosition = Vector3.zero;
					gameObject3.transform.localRotation = Quaternion.identity;
					gameObject3.transform.localScale = Vector3.one;
				}
			}
		}
		ParticleScaler particleScaler = gameObject.AddComponent<ParticleScaler>();
		particleScaler.scaleByUIRoot = false;
		particleScaler.scaleByParent = true;
		particleScaler.renderQueue = 4000;
		if (parent != null)
		{
			gameObject.transform.parent = parent.transform;
			gameObject.transform.localPosition = Vector3.zero;
			scale *= 240f;
			gameObject.transform.localScale = new Vector3(scale, scale, scale);
			gameObject.transform.rotation = Quaternion.Euler(0f, rotation, 0f);
		}
		return gameObject;
	}

	public static ResourceEntity CreateLocalUIActor(int slot, int depth = 0, bool playAction = true, bool canRotate = true, GameObject parent = null, float scale = 1f, Action<GameObject> callback = null)
	{
		SocketDataEx socket = Globals.Instance.Player.TeamSystem.GetSocket(slot);
		if (socket == null || socket.GetPet() == null)
		{
			if (callback != null)
			{
				callback(null);
			}
			return null;
		}
		if (socket.IsPlayer())
		{
			return ActorManager.CreateUIPlayer(socket.GetResLoc(), socket.GetWeaponResLoc(), socket.GetGender(), depth, playAction, canRotate, parent, scale, callback);
		}
		return ActorManager.CreateUIPet(socket.GetPet().Info, depth, playAction, canRotate, parent, scale, 1, callback);
	}

	public static ResourceEntity CreateRemoteUIActor(int slot, int depth = 0, bool playAction = true, bool canRotate = true, GameObject parent = null, float scale = 1f, Action<GameObject> callback = null)
	{
		SocketDataEx remoteSocket = Globals.Instance.Player.TeamSystem.GetRemoteSocket(slot);
		if (remoteSocket == null || remoteSocket.GetPet() == null)
		{
			if (callback != null)
			{
				callback(null);
			}
			return null;
		}
		if (remoteSocket.IsPlayer())
		{
			return ActorManager.CreateUIPlayer(remoteSocket.GetResLoc(), remoteSocket.GetWeaponResLoc(), remoteSocket.GetGender(), depth, playAction, canRotate, parent, scale, callback);
		}
		return ActorManager.CreateUIPet(remoteSocket.GetPet().Info, depth, playAction, canRotate, parent, scale, 1, callback);
	}

	public static ResourceEntity CreateUIPlayer(string armorResLoc, string weaponResLoc, int gender = 0, int depth = 0, bool playAction = true, bool canRotate = true, GameObject parent = null, float scale = 1f, Action<GameObject> callback = null)
	{
		return ActorManager.CreateUIActorAsync(armorResLoc, weaponResLoc, scale, depth, playAction, canRotate, parent, gender, "Skill/U1000", 180f, callback);
	}

	public static ResourceEntity CreateUIFashion(FashionInfo fInfo, int depth = 0, bool playAction = false, bool canRotate = false, GameObject parent = null, float scale = 1f, Action<GameObject> callback = null)
	{
		if (fInfo == null)
		{
			if (callback != null)
			{
				callback(null);
			}
			return null;
		}
		return ActorManager.CreateUIActorAsync(fInfo.ResLoc, fInfo.WeaponResLoc, scale, depth, playAction, canRotate, parent, fInfo.Gender - 1, string.Empty, 180f, callback);
	}

	public static ResourceEntity CreateUIPet(PetInfo petInfo, int depth = 0, bool playAction = true, bool canRotate = true, GameObject parent = null, float scale = 1f, int uiType = 0, Action<GameObject> callback = null)
	{
        //ActorManager.<CreateUIPet>c__AnonStoreyB5 <CreateUIPet>c__AnonStoreyB = new ActorManager.<CreateUIPet>c__AnonStoreyB5();
        //<CreateUIPet>c__AnonStoreyB.uiType = uiType;
        //<CreateUIPet>c__AnonStoreyB.callback = callback;
		if (petInfo == null)
		{
			if (callback != null)
			{
				callback(null);
			}
			return null;
		}
		float rotation = 180f;
		float offsetY = petInfo.OffsetYInCard * scale;
		if (uiType == 0)
		{
			scale *= petInfo.ScaleInUI;
		}
		else
		{
			scale *= petInfo.ScaleInCard;
			if (uiType == 2)
			{
				rotation = 170f;
			}
		}
		return ActorManager.CreateUIActorAsync(petInfo.ResLoc, string.Empty, scale, depth, playAction, canRotate, parent, 0, string.Empty, rotation, delegate(GameObject obj)
		{
			if (uiType != 0 && obj != null && offsetY != 0f)
			{
				obj.transform.localPosition = new Vector3(0f, offsetY, 0f);
			}
			if (callback != null)
			{
				callback(obj);
			}
		});
	}

	public static ResourceEntity CreateUIPet(int infoID, int depth = 0, bool playAction = true, bool canRotate = true, GameObject parent = null, float scale = 1f, int uiType = 0, Action<GameObject> callback = null)
	{
		PetInfo info = Globals.Instance.AttDB.PetDict.GetInfo(infoID);
		if (info == null)
		{
			global::Debug.LogErrorFormat("PetDict GetInfo error, id = {0}", new object[]
			{
				infoID
			});
			if (callback != null)
			{
				callback(null);
			}
			return null;
		}
		return ActorManager.CreateUIPet(info, depth, playAction, canRotate, parent, scale, uiType, callback);
	}

	public static ResourceEntity CreateUIMonster(MonsterInfo mInfo, int depth = 0, bool playAction = true, bool canRotate = true, GameObject parent = null, float scale = 1f, Action<GameObject> callback = null)
	{
		if (mInfo == null)
		{
			global::Debug.LogError(new object[]
			{
				"MonsterInfo == null"
			});
			if (callback != null)
			{
				callback(null);
			}
			return null;
		}
		return ActorManager.CreateUIActorAsync(mInfo.ResLoc, string.Empty, mInfo.ScaleInUI * scale, depth, playAction, canRotate, parent, 0, string.Empty, 180f, callback);
	}

	public static ResourceEntity CreateUIMonster(int infoID, int depth = 0, bool playAction = true, bool canRotate = true, GameObject parent = null, float scale = 1f, Action<GameObject> callback = null)
	{
		MonsterInfo info = Globals.Instance.AttDB.MonsterDict.GetInfo(infoID);
		if (info == null)
		{
			global::Debug.LogErrorFormat("MonsterDict GetInfo error, id = {0}", new object[]
			{
				infoID
			});
			if (callback != null)
			{
				callback(null);
			}
			return null;
		}
		return ActorManager.CreateUIMonster(info, depth, playAction, canRotate, parent, scale, callback);
	}

	public static ResourceEntity CreateUILopet(LopetInfo lpInfo, int depth = 0, bool playAction = true, bool canRotate = true, GameObject parent = null, float scale = 1f, Action<GameObject> callback = null)
	{
		if (lpInfo == null)
		{
			global::Debug.LogError(new object[]
			{
				"LopetInfo == null"
			});
			if (callback != null)
			{
				callback(null);
			}
			return null;
		}
		return ActorManager.CreateUIActorAsync(lpInfo.ResLoc, string.Empty, lpInfo.ScaleInUI * scale, depth, playAction, canRotate, parent, 0, string.Empty, 180f, delegate(GameObject obj)
		{
			if (lpInfo.OffsetYInUI != 0f)
			{
				obj.transform.localPosition = new Vector3(0f, lpInfo.OffsetYInUI, 0f);
			}
			if (callback != null)
			{
				callback(obj);
			}
		});
	}

	public static ResourceEntity CreateUILopet(int infoID, int depth = 0, bool playAction = true, bool canRotate = true, GameObject parent = null, float scale = 1f, Action<GameObject> callback = null)
	{
		LopetInfo info = Globals.Instance.AttDB.LopetDict.GetInfo(infoID);
		if (info == null)
		{
			global::Debug.LogError(new object[]
			{
				"LopetInfo == null"
			});
			if (callback != null)
			{
				callback(null);
			}
			return null;
		}
		return ActorManager.CreateUILopet(info, depth, playAction, canRotate, parent, scale, callback);
	}

	public static ResourceEntity CreateUILopet(bool local, int depth = 0, bool playAction = true, bool canRotate = true, GameObject parent = null, float scale = 1f, Action<GameObject> callback = null)
	{
		LopetDataEx lopet = Globals.Instance.Player.TeamSystem.GetLopet(local);
		if (lopet == null)
		{
			if (callback != null)
			{
				callback(null);
			}
			return null;
		}
		return ActorManager.CreateUILopet(lopet.Info, depth, playAction, canRotate, parent, scale, callback);
	}

	public void LootHPMP(Vector3 position, uint level)
	{
		if (this.senceInfo.MinLootHPMPCount <= 0 || (this.senceInfo.LootHPValue <= 0 && this.senceInfo.LootMPValue <= 0))
		{
			return;
		}
		int num = this.senceInfo.MinLootHPMPCount;
		if (this.senceInfo.MinLootHPMPCount < this.senceInfo.MaxLootHPMPCount)
		{
			num = UtilFunc.RangeRandom(this.senceInfo.MinLootHPMPCount, this.senceInfo.MaxLootHPMPCount);
		}
		if (this.senceInfo.Type != 1)
		{
			if (this.senceInfo.Type != 6)
			{
				level = 1u;
			}
			else
			{
				level = this.playerCtrler.ActorCtrler.Level;
			}
		}
		for (int i = 0; i < num; i++)
		{
			Vector3 vector = position;
			float f = UtilFunc.RangeRandom(0f, 6.28318548f);
			vector.x += 1f * Mathf.Sin(f);
			vector.z += 1f * Mathf.Cos(f);
			NavMeshHit navMeshHit;
			if (NavMesh.SamplePosition(vector, out navMeshHit, 1f, -1))
			{
				vector = navMeshHit.position;
			}
			vector.y += 0.3f;
			if (UtilFunc.RangeRandom(0, 100) > 50)
			{
				Transform transform = PoolMgr.Spawn("Skill/st_011", vector, Quaternion.identity);
				if (transform != null)
				{
					HMRecover safeComponent = Tools.GetSafeComponent<HMRecover>(transform.gameObject);
					safeComponent.HPValue = (uint)(this.senceInfo.LootHPValue * (int)level);
				}
			}
			else
			{
				Transform transform2 = PoolMgr.Spawn("Skill/st_010", vector, Quaternion.identity);
				if (transform2 != null)
				{
					HMRecover safeComponent2 = Tools.GetSafeComponent<HMRecover>(transform2.gameObject);
					safeComponent2.MPValue = (uint)(this.senceInfo.LootMPValue * (int)level);
				}
			}
		}
	}

	public void AddHPMP(uint hpValue, uint mpValue)
	{
		if (this.actors[0] == null)
		{
			return;
		}
		Singleton<ActionMgr>.Instance.PlayAction(this.actors[0], "Skill/misc_003", this.actors[0]);
		for (int i = 0; i < 5; i++)
		{
			if (!(this.actors[i] == null) && !this.actors[i].IsDead)
			{
				if (hpValue > 0u)
				{
					this.actors[i].AddHP(hpValue);
				}
				if (i == 0 && mpValue > 0u)
				{
					this.actors[i].AddMP(mpValue);
				}
			}
		}
	}

	private int GetActorSlot(ActorController actor)
	{
		if (actor == null || this.senceInfo == null)
		{
			return -2;
		}
		if (actor.FactionType == ActorController.EFactionType.EBlue)
		{
			for (int i = 0; i < 5; i++)
			{
				if (this.actors[i] == actor)
				{
					return i;
				}
			}
		}
		else
		{
			if (actor == this.BossActor)
			{
				return -1;
			}
			if ((this.senceInfo.Type == 2 || this.senceInfo.Type == 4 || this.senceInfo.Type == 8 || this.senceInfo.Type == 9) && this.arenaScene != null)
			{
				for (int j = 0; j < 4; j++)
				{
					if (actor == this.arenaScene.GetRemoteActor(j))
					{
						return j + 5;
					}
				}
			}
		}
		return -2;
	}

	public void OnBuffAdd(ActorController actor, int serialID, BuffInfo info, float duration, int stackCount)
	{
		if (this.BuffAddEvent == null)
		{
			return;
		}
		int actorSlot = this.GetActorSlot(actor);
		if (actorSlot != -2)
		{
			this.BuffAddEvent(actorSlot, serialID, info, duration, stackCount);
		}
	}

	public void OnBuffUpdate(ActorController actor, int serialID, BuffInfo info, float duration, int stackCount)
	{
		if (this.BuffUpdateEvent == null)
		{
			return;
		}
		int actorSlot = this.GetActorSlot(actor);
		if (actorSlot != -2)
		{
			this.BuffUpdateEvent(actorSlot, serialID, info, duration, stackCount);
		}
	}

	public void OnBuffRemove(ActorController actor, int serialID)
	{
		if (this.BuffRemoveEvent == null)
		{
			return;
		}
		int actorSlot = this.GetActorSlot(actor);
		if (actorSlot != -2)
		{
			this.BuffRemoveEvent(actorSlot, serialID);
		}
	}

	public void Resurrect(bool flag = true)
	{
		Quaternion rotation = Quaternion.Euler(0f, this.bornRotationY, 0f);
		if (flag)
		{
			PoolMgr.CreateBuffPrefabPool(205801);
		}
		for (int i = 0; i < 4; i++)
		{
			if (this.actors[i] != null && (i == 0 || !this.actors[i].IsDead))
			{
				this.actors[i].Resurrect(flag);
			}
			else
			{
				if (this.actors[i] != null)
				{
					UnityEngine.Object.DestroyImmediate(this.actors[i].gameObject);
				}
				this.actors[i] = this.CreateLocalActor(i, this.bornPosition, rotation);
			}
			if (flag && this.actors[i] != null)
			{
				this.actors[i].AddBuff(205801, this.actors[i]);
			}
		}
		this.ResetAI();
		if (flag)
		{
			this.baseScene.OnPlayerResurrect();
		}
	}

	public void PetResurrect(int slot)
	{
		if (slot < 0 || slot >= 4)
		{
			return;
		}
		if (this.actors[slot] != null && (slot == 0 || !this.actors[slot].IsDead))
		{
			this.actors[slot].Resurrect(true);
		}
		else
		{
			if (this.actors[slot] != null)
			{
				UnityEngine.Object.DestroyImmediate(this.actors[slot].gameObject);
			}
			this.actors[slot] = this.CreateLocalActor(slot, this.bornPosition, Quaternion.Euler(0f, this.bornRotationY, 0f));
			if (this.actors[slot] == null)
			{
				return;
			}
		}
		this.actors[slot].CurHP = this.actors[slot].MaxHP;
		if (this.actors[0] == null)
		{
			return;
		}
		int num = 0;
		int num2 = 0;
		for (int i = 1; i < 5; i++)
		{
			if (!(this.actors[i] == null))
			{
				this.actors[i].CastPassiveSkill();
				if (i <= 3)
				{
					if (this.actors[i].IsMelee)
					{
						num++;
					}
					else
					{
						num2++;
					}
				}
			}
		}
		int num3 = 0;
		int num4 = 0;
		for (int j = 1; j < 5; j++)
		{
			if (!(this.actors[j] == null))
			{
				int num5;
				if (j >= 4)
				{
					num5 = 7;
				}
				else if (this.actors[j].IsMelee)
				{
					num3++;
					num5 = 100 + num * 10 + num3;
				}
				else
				{
					num4++;
					num5 = 200 + num2 * 10 + num4;
				}
				if (j == slot)
				{
					Vector3 vector = CombatHelper.GetSlotPos(this.actors[0].transform.position, this.actors[0].transform, num5, false, false);
					NavMeshHit navMeshHit;
					if (NavMesh.SamplePosition(vector, out navMeshHit, 0.5f, -1))
					{
						vector = navMeshHit.position;
					}
					this.actors[j].NavAgent.Warp(vector);
					this.actors[j].AiCtrler.SetFellowSlot(num5);
					this.actors[j].AiCtrler.ForceFollow = this.ForceFollow;
					if (this.ForceFollow)
					{
						this.actors[j].AiCtrler.FollowForce(this.actors[0], num5);
						this.actors[j].AiCtrler.SetSelectTarget(null);
					}
					else
					{
						this.actors[j].AiCtrler.Follow(this.actors[0], num5);
					}
					this.actors[j].PlayAction("Skill/misc_004", null);
				}
				else
				{
					this.actors[j].AiCtrler.SetFellowSlot(num5);
				}
			}
		}
	}

	public void SendPveResultMsg()
	{
		MC2S_PveResult mC2S_PveResult = new MC2S_PveResult();
		mC2S_PveResult.Score = this.Score;
		mC2S_PveResult.LootMoney = this.lootMoney;
		if (this.win)
		{
			mC2S_PveResult.ResultKey = (this.Key ^ 2014);
		}
		else
		{
			mC2S_PveResult.ResultKey = (this.Key ^ 2010);
		}
		mC2S_PveResult.Log = this.GetCombatLog();
		mC2S_PveResult.SceneID = GameUIManager.mInstance.uiState.PveSceneID;
		mC2S_PveResult.Value = GameUIManager.mInstance.uiState.PveSceneValue;
		Globals.Instance.CliSession.Send(602, mC2S_PveResult);
	}

	public void RemoveBuff(int buffID, ActorController caster)
	{
		for (int i = 0; i < 5; i++)
		{
			if (this.actors[i] != null)
			{
				this.actors[i].RemoveBuff(buffID, caster);
			}
		}
	}

	public void ChangeAIMode(bool value)
	{
		if (this.actors[0] == null)
		{
			return;
		}
		this.actors[0].AiCtrler.EnableAI = value;
		GameCache.Data.EnableAI = value;
		GameCache.UpdateNow = true;
		this.baseScene.OnChangeAIMode();
	}

	public void ChangeForceFollow()
	{
		if (this.actors[0] == null || this.baseScene == null || this.baseScene.CurStatus != 2)
		{
			return;
		}
		this.ForceFollow = !this.ForceFollow;
		ActorController actorController = null;
		if (!this.ForceFollow)
		{
			actorController = AIController.FindMinDistEnemy(this.actors[0], 3.40282347E+38f);
		}
		for (int i = 1; i < 5; i++)
		{
			if (!(this.actors[i] == null))
			{
				this.actors[i].AiCtrler.ForceFollow = this.ForceFollow;
				if (this.ForceFollow)
				{
					this.actors[i].AiCtrler.FollowForce(this.actors[0], this.actors[i].AiCtrler.FollowSlot);
					this.actors[i].AiCtrler.SetSelectTarget(null);
				}
				else if (this.actors[i].AiCtrler.Target != null)
				{
					this.actors[i].AiCtrler.Chase(this.actors[i].AiCtrler.Target);
				}
				else if (actorController != null)
				{
					this.actors[i].AiCtrler.SetTarget(actorController);
				}
				else
				{
					this.actors[i].AiCtrler.Follow(this.actors[0], this.actors[i].AiCtrler.FollowSlot);
				}
			}
		}
	}

	public void OnDoDamage(MonsterInfo info, long damage)
	{
		this.baseScene.OnDoDamage(info, damage);
	}

	public void LockAllActorAI()
	{
		if (this.playerCtrler != null)
		{
			this.playerCtrler.SetControlLocked(true);
		}
		for (int i = 0; i < this.actors.Count; i++)
		{
			if (!(this.actors[i] == null))
			{
				this.actors[i].AiCtrler.Locked = true;
			}
		}
	}

	public void UnlockAllActorAI()
	{
		if (this.playerCtrler != null)
		{
			this.playerCtrler.SetControlLocked(false);
		}
		for (int i = 0; i < this.actors.Count; i++)
		{
			if (!(this.actors[i] == null))
			{
				this.actors[i].AiCtrler.Locked = false;
			}
		}
	}

	public void ClearAllThreat()
	{
		for (int i = 0; i < this.actors.Count; i++)
		{
			if (!(this.actors[i] == null))
			{
				this.actors[i].AiCtrler.ThreatMgr.Clear();
			}
		}
	}

	public ActorController GetBossActor()
	{
		return this.bossActor;
	}

	public void Pause(bool value)
	{
		this.enableUpdate = !value;
		this.playerCtrler.enabled = !value;
		this.invoker.SetEnable(!value);
		for (int i = 0; i < this.actors.Count; i++)
		{
			if (this.actors[i] != null)
			{
				this.actors[i].SetPlayStatus(!value);
			}
		}
		Singleton<ActionMgr>.Instance.Pause(value, null);
	}

	public void DelayPause(float time)
	{
		base.Invoke("ActorsPause", time);
	}

	private void Pause()
	{
		this.Pause(true);
		Globals.Instance.TutorialMgr.InitializationCompleted(null, new GUIPlotDialog.FinishCallback(this.Resume));
	}

	private void Resume()
	{
		this.Pause(false);
	}

	public void PauseActor(bool value, ActorController actor)
	{
		if (actor == null)
		{
			return;
		}
		if (actor == this.playerCtrler.ActorCtrler)
		{
			this.playerCtrler.enabled = !value;
		}
		actor.SetPlayStatus(!value);
		Singleton<ActionMgr>.Instance.Pause(value, actor);
	}

	public bool HasAssistant()
	{
		return this.assistPetID != 0 && this.assistPetAttID != 0;
	}

	public void SetAssistant(int petID, int attID)
	{
		this.assistPetID = petID;
		this.assistPetAttID = attID;
	}

	public void SetPlayerController(PlayerController playerCtrl)
	{
		this.playerCtrler = playerCtrl;
	}

	public void SetWalkableMask(int layer, bool walkable)
	{
		for (int i = 0; i < this.actors.Count; i++)
		{
			ActorController actorController = this.actors[i];
			if (actorController)
			{
				if (walkable)
				{
					actorController.NavAgent.walkableMask |= 1 << layer;
				}
				else
				{
					actorController.NavAgent.walkableMask &= ~(1 << layer);
				}
			}
		}
	}

	public void BuildCombatLog(bool win)
	{
		if (this.combatLog == null || this.combatLog.EndTime != 0)
		{
			return;
		}
		for (int i = 0; i < 5; i++)
		{
			if (this.actors[i] != null && !this.actors[i].IsDead)
			{
				this.AddActorCombatLog(this.actors[i]);
			}
		}
		this.combatLog.EndTime = Globals.Instance.Player.GetTimeStamp();
		this.combatLog.KillMonsterCount = this.MonsterDead;
		this.combatLog.Win = ((!win) ? 0 : 1);
	}

	public void OutputRecount()
	{
		if (this.combatLog == null)
		{
			return;
		}
		global::Debug.Log(new object[]
		{
			"---------- Damage Recount ---------"
		});
		for (int i = 0; i < this.combatLog.Data.Count; i++)
		{
			PetInfo info = Globals.Instance.AttDB.PetDict.GetInfo(this.combatLog.Data[i].PetID);
			if (info == null)
			{
				global::Debug.LogErrorFormat("PetDict.GetInfo error, id = {0}", new object[]
				{
					this.combatLog.Data[i].PetID
				});
			}
			else
			{
				global::Debug.LogFormat("{0} damage: {1}, heal: {2}", new object[]
				{
					info.Name,
					this.combatLog.Data[i].Damage,
					this.combatLog.Data[i].Heal
				});
			}
		}
		global::Debug.Log(new object[]
		{
			"------------- Damage Recount ---------------------"
		});
	}

	private void CachePlayerSound(int gender)
	{
		if (!EffectSoundManager.IsEffectSoundOptionOn())
		{
			return;
		}
		if (gender == 0)
		{
			for (int i = 0; i < ConstSound.PlayerHitSounds.Length; i++)
			{
				Globals.Instance.EffectSoundMgr.CacheSoundResourceSync(ConstSound.PlayerHitSounds[i] + "m", 360f);
			}
		}
		else
		{
			for (int j = 0; j < ConstSound.PlayerHitSounds.Length; j++)
			{
				Globals.Instance.EffectSoundMgr.CacheSoundResourceSync(ConstSound.PlayerHitSounds[j], 360f);
			}
		}
	}

	public ActorController GetRemotePlayerActor()
	{
		return this.baseScene.GetRemotePlayerActor();
	}

	private void InitEnvironment()
	{
		if (!string.IsNullOrEmpty(this.senceInfo.WeatherEffect))
		{
			GameObject gameObject = Res.Load<GameObject>(this.senceInfo.WeatherEffect, false);
			if (gameObject == null)
			{
				global::Debug.LogErrorFormat("Res.Load error, Name = {0}", new object[]
				{
					this.senceInfo.WeatherEffect
				});
				return;
			}
			if (this.senceInfo.WEMode == 0 && this.playerCtrler != null)
			{
				Tools.AddChild(Camera.main.gameObject, gameObject);
			}
			else
			{
				UnityEngine.Object.Instantiate(gameObject);
			}
		}
		GameObject gameObject2 = GameObject.Find("/Wall");
		if (gameObject2 != null)
		{
			if (this.senceInfo.Wall)
			{
				gameObject2.SetActive(true);
				this.SetWalkableMask(4, false);
			}
			else
			{
				gameObject2.SetActive(false);
			}
		}
	}

	public bool IsInBossAttackDistance()
	{
		if (this.BossActor == null)
		{
			return false;
		}
		float distance2D = this.playerCtrler.ActorCtrler.GetDistance2D(this.BossActor);
		return distance2D <= this.playerCtrler.ActorCtrler.AiCtrler.AttackDistance * 2.5f;
	}

	public bool IsPvpScene()
	{
		return this.baseScene == this.arenaScene || this.baseScene == this.pillageScene || this.baseScene == this.orePillageScene || this.baseScene == this.guildPvpScene;
	}

	public bool IsControlLocked()
	{
		return this.baseScene == this.arenaScene || this.baseScene == this.orePillageScene || this.baseScene == this.guildPvpScene;
	}

	public ActorController GetRemoteActor(int index)
	{
		if (this.senceInfo.Type == 2)
		{
			return this.arenaScene.GetRemoteActor(index);
		}
		if (this.senceInfo.Type == 4)
		{
			return this.pillageScene.GetRemoteActor(index);
		}
		if (this.senceInfo.Type == 8)
		{
			return this.orePillageScene.GetRemoteActor(index);
		}
		if (this.senceInfo.Type == 9)
		{
			return this.guildPvpScene.GetRemoteActor(index);
		}
		return null;
	}

	public CombatLog GetCombatLog()
	{
		this.combatLog.RecvStartTime = this.RecvStartTime;
		this.combatLog.SendResultTime = Globals.Instance.Player.GetTimeStamp();
		return this.combatLog;
	}

	public void OnPauseStart()
	{
		if (this.combatLog == null)
		{
			return;
		}
		this.pauseTimer = Time.realtimeSinceStartup;
	}

	public void OnPauseStop()
	{
		if (this.combatLog == null || this.pauseTimer <= 0f || Time.realtimeSinceStartup < this.pauseTimer)
		{
			return;
		}
		this.combatLog.PauseCount++;
		this.combatLog.PauseTime += (int)(Time.realtimeSinceStartup - this.pauseTimer);
	}

	public void SetServerData(int key, ServerActorData data)
	{
		this.Key = key;
		this.ServerKey = key;
		this.ServerData = data;
	}

	public bool CheckData()
	{
		if (this.ServerData == null)
		{
			return true;
		}
		for (int i = 0; i < 4; i++)
		{
			ActorController actor = this.GetActor(i);
			if (!(actor == null))
			{
				bool flag = false;
				int j = 0;
				while (j < this.ServerData.Data.Count)
				{
					if (this.ServerData.Data[j].Slot == i)
					{
						if ((ulong)actor.Level != (ulong)((long)(this.ServerData.Data[j].Level ^ this.ServerKey)))
						{
							return false;
						}
						if (this.ServerData.Flag != 0)
						{
							if (actor.GetInitAtt(EAttID.EAID_MaxHP) > (this.ServerData.Data[j].MaxHP ^ this.ServerKey) + 1)
							{
								return false;
							}
							if (actor.GetInitAtt(EAttID.EAID_Attack) > (this.ServerData.Data[j].Attack ^ this.ServerKey) + 1)
							{
								return false;
							}
							if (actor.GetInitAtt(EAttID.EAID_PhysicDefense) > (this.ServerData.Data[j].PhysicDefense ^ this.ServerKey) + 1)
							{
								return false;
							}
							if (actor.GetInitAtt(EAttID.EAID_MagicDefense) > (this.ServerData.Data[j].MagicDefense ^ this.ServerKey) + 1)
							{
								return false;
							}
							if (actor.GetInitAtt(EAttID.EAID_Hit) > (this.ServerData.Data[j].Hit ^ this.ServerKey) + 1)
							{
								return false;
							}
							if (actor.GetInitAtt(EAttID.EAID_Dodge) > (this.ServerData.Data[j].Dodge ^ this.ServerKey) + 1)
							{
								return false;
							}
							if (actor.GetInitAtt(EAttID.EAID_Crit) > (this.ServerData.Data[j].Crit ^ this.ServerKey) + 1)
							{
								return false;
							}
							if (actor.GetInitAtt(EAttID.EAID_CritResis) > (this.ServerData.Data[j].CritResis ^ this.ServerKey) + 1)
							{
								return false;
							}
							if (actor.GetInitAtt(EAttID.EAID_DamagePlus) > (this.ServerData.Data[j].DamagePlus ^ this.ServerKey) + 1)
							{
								return false;
							}
							if (actor.GetInitAtt(EAttID.EAID_DamageMinus) > (this.ServerData.Data[j].DamageMinus ^ this.ServerKey) + 1)
							{
								return false;
							}
							int num = this.ServerData.Data[j].PlayerSkillID - 10000;
							if (actor.GetPlayerSkillID() != num)
							{
								return false;
							}
							if (num != 0)
							{
								SkillInfo info = Globals.Instance.AttDB.SkillDict.GetInfo(num);
								if (info != null)
								{
									if (info.ManaCost < (this.ServerData.Data[j].ManaCost - 10000) / 2)
									{
										return false;
									}
									if (info.CoolDown < this.ServerData.Data[j].CoolDown / 20000f)
									{
										return false;
									}
								}
							}
						}
						flag = true;
						break;
					}
					else
					{
						j++;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
		}
		for (int k = 4; k < this.actors.Count; k++)
		{
			if (!(this.actors[k] == null) && !this.actors[k].IsBox)
			{
				if (this.actors[k].GetLongInitAtt(EAttID.EAID_MaxHP) < (long)this.ServerData.MaxHP)
				{
					return false;
				}
			}
		}
		return true;
	}

	public void SkipCombat()
	{
		long num = 0L;
		long num2 = 0L;
		long num3 = 0L;
		long num4 = 0L;
		long num5 = 0L;
		long num6 = 0L;
		long num7 = 0L;
		long num8 = 0L;
		long num9 = 0L;
		long num10 = 0L;
		long num11 = 0L;
		long num12 = 0L;
		long num13 = 0L;
		long num14 = 0L;
		long num15 = 0L;
		long num16 = 0L;
		long num17 = 0L;
		long num18 = 0L;
		long num19 = 0L;
		long num20 = 0L;
		uint num21 = 0u;
		uint num22 = 0u;
		int num23 = 0;
		for (int i = 0; i < 5; i++)
		{
			ActorController actorController = this.actors[i];
			if (!(actorController == null))
			{
				num23++;
				num21 += actorController.Level;
				num += actorController.MaxHP;
				num2 += (long)actorController.GetAtt(EAttID.EAID_Attack);
				num3 += (long)actorController.GetAtt(EAttID.EAID_PhysicDefense);
				num4 += (long)actorController.GetAtt(EAttID.EAID_MagicDefense);
				num5 += (long)actorController.GetAtt(EAttID.EAID_Hit);
				num6 += (long)actorController.GetAtt(EAttID.EAID_Dodge);
				num7 += (long)actorController.GetAtt(EAttID.EAID_Crit);
				num8 += (long)actorController.GetAtt(EAttID.EAID_CritResis);
				num9 += (long)actorController.GetAtt(EAttID.EAID_DamagePlus);
				num10 += (long)actorController.GetAtt(EAttID.EAID_DamageMinus);
			}
		}
		num21 = (uint)((ulong)num21 / (ulong)((long)num23));
		num2 /= (long)num23;
		num3 /= (long)num23;
		num4 /= (long)num23;
		num5 /= (long)num23;
		num6 /= (long)num23;
		num7 /= (long)num23;
		num8 /= (long)num23;
		num9 /= (long)num23;
		num10 /= (long)num23;
		num23 = 0;
		for (int j = 5; j < this.actors.Count; j++)
		{
			ActorController actorController = this.actors[j];
			if (!(actorController == null))
			{
				num23++;
				num22 += actorController.Level;
				num11 += actorController.MaxHP;
				num12 += (long)actorController.GetAtt(EAttID.EAID_Attack);
				num13 += (long)actorController.GetAtt(EAttID.EAID_PhysicDefense);
				num14 += (long)actorController.GetAtt(EAttID.EAID_MagicDefense);
				num15 += (long)actorController.GetAtt(EAttID.EAID_Hit);
				num16 += (long)actorController.GetAtt(EAttID.EAID_Dodge);
				num17 += (long)actorController.GetAtt(EAttID.EAID_Crit);
				num18 += (long)actorController.GetAtt(EAttID.EAID_CritResis);
				num19 += (long)actorController.GetAtt(EAttID.EAID_DamagePlus);
				num20 += (long)actorController.GetAtt(EAttID.EAID_DamageMinus);
			}
		}
		num22 = (uint)((ulong)num22 / (ulong)((long)num23));
		num12 /= (long)num23;
		num13 /= (long)num23;
		num14 /= (long)num23;
		num15 /= (long)num23;
		num16 /= (long)num23;
		num17 /= (long)num23;
		num18 /= (long)num23;
		num19 /= (long)num23;
		num20 /= (long)num23;
		long num24 = 90L + num5 / 100L + (long)((num21 - num22) * 20u / num22) - num16 / 100L;
		long num25 = 90L + num15 / 100L + (long)((num22 - num21) * 20u / num21) - num6 / 100L;
		long num26 = num7 / 100L + (long)((num21 - num22) * 10u / num22) - num18 / 100L;
		long num27 = num17 / 100L + (long)((num22 - num21) * 10u / num21) - num8 / 100L;
		long num28 = (num2 * 2L - num13 - num14 / 2L) * (10000L + num9 - num20) / 10000L;
		if (num28 < 1L)
		{
			num28 = 1L;
		}
		long num29 = (num12 * 2L - num3 - num4 / 2L) * (10000L + num19 - num10) / 10000L;
		if (num29 < 1L)
		{
			num29 = 1L;
		}
		num23 = 0;
		while (num > 0L && num11 > 0L)
		{
			if ((long)UtilFunc.RangeRandom(0, 100) < num24)
			{
				if ((long)UtilFunc.RangeRandom(0, 100) < num26)
				{
					num -= num29 * 150L / 100L;
				}
				else
				{
					num -= num29;
				}
			}
			if ((long)UtilFunc.RangeRandom(0, 100) < num25)
			{
				if ((long)UtilFunc.RangeRandom(0, 100) < num27)
				{
					num11 -= num28 * 150L / 100L;
				}
				else
				{
					num11 -= num28;
				}
			}
			num23++;
			if (num23 > 500)
			{
				break;
			}
		}
		this.baseScene.OnSkip(num11 < 0L);
	}

	public void AddLocalPlayerEP(int value)
	{
		if (this.LopetActor == null)
		{
			return;
		}
		if (this.actors[0] == null || this.actors[0].IsDead)
		{
			return;
		}
		this.actors[0].AddEP(value);
	}

	public void AddRemotePlayerEP(int value)
	{
		if (this.RemoteLopetActor == null)
		{
			return;
		}
		ActorController remoteActor = this.GetRemoteActor(0);
		if (remoteActor == null || remoteActor.IsDead)
		{
			return;
		}
		remoteActor.AddEP(value);
	}

	public long DoShareDamage(ActorController actor, long damage, int rate)
	{
		if (actor == null || rate <= 0)
		{
			return damage;
		}
		long num = damage * (long)rate / 10000L;
		long num2 = damage - num;
		long num3 = 0L;
		long num4 = 0L;
		if (actor.FactionType == ActorController.EFactionType.EBlue)
		{
			for (int i = 0; i < 5; i++)
			{
				if (!(this.actors[i] == null) && !this.actors[i].IsDead)
				{
					if (this.actors[i] == actor)
					{
						if (num2 < this.actors[i].CurHP)
						{
							num3 += this.actors[i].CurHP - num2;
						}
					}
					else
					{
						num3 += this.actors[i].CurHP;
					}
				}
			}
			if (num3 < num)
			{
				num = num3 - 10L;
				if (num < 0L)
				{
					num = 0L;
				}
			}
			if (num > 0L)
			{
				for (int j = 0; j < 5; j++)
				{
					if (!(this.actors[j] == null) && !this.actors[j].IsDead)
					{
						if (this.actors[j] != actor)
						{
							long num5 = this.actors[j].CurHP * num / num3;
							if (num5 > 0L && num5 < this.actors[j].CurHP)
							{
								num4 += num5;
								GameUIManager.mInstance.HUDTextMgr.RequestShow(this.actors[j], EShowType.EST_Damage, -(int)num5, null, 2);
								this.actors[j].DoDamage(num5, null, false);
							}
						}
					}
				}
			}
		}
		else
		{
			for (int k = 5; k < this.actors.Count; k++)
			{
				if (!(this.actors[k] == null) && !this.actors[k].IsDead)
				{
					if (this.actors[k] == actor)
					{
						if (num2 < this.actors[k].CurHP)
						{
							num3 += this.actors[k].CurHP - num2;
						}
					}
					else
					{
						num3 += this.actors[k].CurHP;
					}
				}
			}
			if (num3 < num)
			{
				num = num3 - 10L;
				if (num < 0L)
				{
					num = 0L;
				}
			}
			if (num > 0L)
			{
				for (int l = 5; l < this.actors.Count; l++)
				{
					if (!(this.actors[l] == null) && !this.actors[l].IsDead)
					{
						if (this.actors[l] != actor)
						{
							long num5 = this.actors[l].CurHP * num / num3;
							if (num5 > 0L && num5 < this.actors[l].CurHP)
							{
								num4 += num5;
								GameUIManager.mInstance.HUDTextMgr.RequestShow(this.actors[l], EShowType.EST_Damage, (int)num5, null, 0);
								this.actors[l].DoDamage(num5, null, false);
							}
						}
					}
				}
			}
		}
		return damage - num4;
	}

	public void HideLopet(bool local, float timer)
	{
		if (timer <= 0f)
		{
			return;
		}
		if (local)
		{
			if (this.LopetActor != null)
			{
				this.LopetActor.gameObject.SetActive(false);
				this.hideLopetTimer = timer;
			}
		}
		else if (this.RemoteLopetActor != null)
		{
			this.RemoteLopetActor.gameObject.SetActive(false);
			this.hideRemoteLopetTimer = timer;
		}
	}

	public float GetCombatTime(bool resetTime = true)
	{
		float result = this.combatTime;
		if (resetTime)
		{
			this.combatTime = 0f;
		}
		return result;
	}
}
