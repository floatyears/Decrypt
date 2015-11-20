using Att;
using System;
using System.Collections.Generic;
using UnityEngine;
using Util;

[AddComponentMenu("Game/Character/ActorController")]
public sealed class ActorController : MonoBehaviour
{
	public enum EActorType
	{
		EPlayer,
		EPet,
		EMonster,
		ELopet
	}

	public enum EFactionType
	{
		EBlue,
		ERed
	}

	internal class DelayTarget
	{
		public float Delay;

		public ActorController Target;

		public SkillInfo Info;

		public bool Deleted;

		public int Index;
	}

	private Vector3 initLocalScale = Vector3.one;

	private float actionScale = 1f;

	private float cacheActionScaleTimer;

	private float cacheActionScale = 1f;

	private List<CharacterMeshInfo> meshInfos = new List<CharacterMeshInfo>();

	private bool meshChanged = true;

	private NavMeshAgentEx navAgent;

	private AIController aiCtrler;

	private AnimationController animationCtrler;

	private static PlayAnimation deadAnimation;

	private HUDText uiText;

	private UIIngameActorHPMP playerheadTip;

	private static int HeadTipDepth;

	private Transform spineTransform;

	private GameObject feetEffect; 

	private GameObject runEffect;

	private ActorController.EActorType actorType;

	private ActorController.EFactionType factionType;

	private ObscuredLongArray attInit = new ObscuredLongArray(11);

	private int[] attValueMod = new int[11];

	private int[] attPctMod = new int[11];

	private ObscuredLongArray AttData = new ObscuredLongArray(11);

	private int[] resist = new int[7];

	private TalentInfo talentEnhanceDamage;

	private TalentInfo talentReduceDamage;

	private TalentInfo talentHeal;

	private float talentHealCD;

	private TalentInfo talentImmuneControlled;

	private float talentImmuneCD;

	private TalentInfo talentResurrect;

	private float talentResurrectCD;

	private int playerGender;

	private string playerName = string.Empty;

	public int ElementType;

	public long MaxMP;

	private ObscuredLong curHP = new ObscuredLong(0);

	private ObscuredLong curMP = new ObscuredLong(0);

	private ObscuredLong curEP = new ObscuredLong(0);

	private float mpTimer;

	public float MaxRunSpeed = 2.8f;

	private ObscuredFloat speedScale = new ObscuredFloat(1f);

	public ObscuredInt level = new ObscuredInt(1);

	public bool isDead = true;

	public SkillData[] Skills = new SkillData[7];

	private int skillSerialID;

	private int skillCastCache = -1;

	private ActorController skillCacheTarget;

	private Vector3 skillCachePos = Vector3.zero;

	public float SkillValue;

	private bool isTrigger;

	private SkillInfo triggerInfo;

	private ActorController triggerTarget;

	private bool lockSkillCast;

	private bool lockMove;

	private int lockSkillIndex = -1;

	private bool lockSkillCD;

	private bool immuneControl;

	private List<ActorController.DelayTarget> delayTargets = new List<ActorController.DelayTarget>();

	private List<Buff> buffs = new List<Buff>();

	private int buffSerialID;

	private int rootBuff;

	private int silenceBuff;

	private int stunBuff;

	private int immunityBuff;

	private int absorbBuff;

	private int changeFactionBuff;

	private int fearBuff;

	private int speedMod;

	private int damageMod;

	private int damageTakenMod;

	private int healMod;

	private int healTakenMod;

	private int attackSpeedMod;

	private int damageReflect;

	private int resurrectRate;

	private float resurrectCD;

	private float resurrectCDTimer;

	private bool autoResurrect;

	private int suckPct;

	private int hitEffect;

	private int superShield;

	private int shareDamage;

	private float rotateTimer;

	private Quaternion newRotation = Quaternion.identity;

	private bool canRotate = true;

	public float CorpseDecayTime = 3f;

	private int playerSkillID;

	public int SocketSlot = -1;

	private float soundTimer;

	public bool IsBuilding;

	public bool Undead;

	public bool Unattacked;

	public bool Unhealed;

	public bool DamageRecount;

	public bool PlayMatinee;

	private List<Transform> socketTrans = new List<Transform>();

	private List<ActorController> summons = new List<ActorController>();

	private long totalDamage;

	private long highestDamage;

	private long totalHeal;

	private long highestHeal;

	private long damageTaken;

	private int damageTakenCount;

	private long healTaken;

	private int healTakenCount;

	private GameObject boxExplode;

	private LegendSkillData doubleDamage;

	private LegendSkillData reflexDamage;

	private LegendSkillData reduceDamage;

	private LegendSkillData damageToHeal;

	private LegendSkillData ignoreDefense;

	private LegendSkillData attackToHeal;

	public Transform BuffAction;

	public int SkillCountIndex = -1;

	public float ActionScale
	{
		get
		{
			return this.actionScale;
		}
		set
		{
			this.cacheActionScale = this.actionScale;
			this.cacheActionScaleTimer = Time.time;
			this.actionScale = value;
		}
	}

	public List<CharacterMeshInfo> MeshInfos
	{
		get
		{
			this.VerifyMeshChanged();
			return this.meshInfos;
		}
	}

	public NavMeshAgentEx NavAgent
	{
		get
		{
			return this.navAgent;
		}
	}

	public AIController AiCtrler
	{
		get
		{
			return this.aiCtrler;
		}
	}

	public AnimationController AnimationCtrler
	{
		get
		{
			return this.animationCtrler;
		}
	}

	public HUDText UIText
	{
		get
		{
			return this.uiText;
		}
	}

	public UIIngameActorHPMP PlayerheadTip
	{
		get
		{
			return this.playerheadTip;
		}
	}

	public Transform SpineTransform
	{
		get
		{
			return this.spineTransform;
		}
	}

	public ActorController.EActorType ActorType
	{
		get
		{
			return this.actorType;
		}
	}

	public ActorController.EFactionType FactionType
	{
		get
		{
			return this.factionType;
		}
	}

	public long MaxHP
	{
		get
		{
			return this.AttData[1];
		}
	}

	public long CurHP
	{
		get
		{
			return this.curHP.Value;
		}
		set
		{
			this.curHP.Value = value;
		}
	}

	public long CurMP
	{
		get
		{
			return this.curMP.Value;
		}
		set
		{
			this.curMP.Value = value;
		}
	}

	public long CurEP
	{
		get
		{
			return this.curEP.Value;
		}
		set
		{
			this.curEP.Value = value;
		}
	}

	public float SpeedScale
	{
		get
		{
			return this.speedScale.Value;
		}
	}

	public uint Level
	{
		get
		{
			return (uint)this.level.Value;
		}
		private set
		{
			this.level.Value = (int)value;
		}
	}

	public bool IsDead
	{
		get
		{
			return this.isDead;
		}
	}

	public int SkillSerialID
	{
		get
		{
			return this.skillSerialID;
		}
	}

	public int SkillCastCache
	{
		get
		{
			return this.skillCastCache;
		}
	}

	public int LockSkillIndex
	{
		get
		{
			return this.lockSkillIndex;
		}
	}

	public List<Buff> Buffs
	{
		get
		{
			return this.buffs;
		}
	}

	public bool IsRoot
	{
		get
		{
			return this.rootBuff > 0;
		}
	}

	public bool IsSilence
	{
		get
		{
			return this.silenceBuff > 0;
		}
	}

	public bool IsStun
	{
		get
		{
			return this.stunBuff > 0;
		}
	}

	public bool IsImmunity
	{
		get
		{
			return this.immunityBuff > 0;
		}
	}

	public bool IsAbsorb
	{
		get
		{
			return this.absorbBuff > 0;
		}
	}

	public bool IsChangeFaction
	{
		get
		{
			return this.changeFactionBuff > 0;
		}
	}

	public bool IsFear
	{
		get
		{
			return this.fearBuff > 0;
		}
	}

	public int DamageMod
	{
		get
		{
			return this.damageMod;
		}
	}

	public int HealMod
	{
		get
		{
			return this.healMod;
		}
	}

	public float AttackSpeed
	{
		get
		{
			float num = 1f + (float)this.attackSpeedMod / 10000f;
			if (num <= 0f)
			{
				num = 0.001f;
			}
			return num;
		}
	}

	public PetInfo petInfo
	{
		get;
		private set;
	}

	public MonsterInfo monsterInfo
	{
		get;
		private set;
	}

	public bool IsBox
	{
		get
		{
			return this.actorType == ActorController.EActorType.EMonster && this.monsterInfo.LootMoney != 0u;
		}
	}

	public bool IsBoss
	{
		get
		{
			return this.actorType == ActorController.EActorType.EMonster && this.monsterInfo.BossType != 0;
		}
	}

	public bool IsMelee
	{
		get
		{
			return this.aiCtrler.AttackDistance < 1.5f;
		}
	}

	public long TotalDamage
	{
		get
		{
			return this.totalDamage;
		}
	}

	public long HighestDamage
	{
		get
		{
			return this.highestDamage;
		}
	}

	public long TotalHeal
	{
		get
		{
			return this.totalHeal;
		}
	}

	public long HighestHeal
	{
		get
		{
			return this.highestHeal;
		}
	}

	public long DamageTaken
	{
		get
		{
			return this.damageTaken;
		}
	}

	public int DamageTakenCount
	{
		get
		{
			return this.damageTakenCount;
		}
	}

	public long HealTaken
	{
		get
		{
			return this.healTaken;
		}
	}

	public int HealTakenCount
	{
		get
		{
			return this.healTakenCount;
		}
	}

	private void Init()
	{
		this.initLocalScale = base.transform.localScale;
		this.navAgent = base.GetComponent<NavMeshAgentEx>();
		this.aiCtrler = base.GetComponent<AIController>();
		this.animationCtrler = base.GetComponent<AnimationController>();
		if (ActorController.deadAnimation == null)
		{
			ActorController.deadAnimation = new PlayAnimation();
			ActorController.deadAnimation.AnimName = "die";
			ActorController.deadAnimation.PlayMode = PlayMode.StopAll;
			ActorController.deadAnimation.WrapMode = WrapMode.ClampForever;
			ActorController.deadAnimation.priority = 10;
		}
		GameObject gameObject = Res.LoadGUI("GUI/HUDText");
		if (gameObject != null && GameUIManager.mInstance != null)
		{
			GameObject gameObject2 = NGUITools.AddChild(GameUIManager.mInstance.HUDTextMgr.gameObject, gameObject);
			this.uiText = gameObject2.GetComponent<HUDText>();
			this.uiText.Init();
			Vector3 localPosition = this.uiText.transform.localPosition;
			localPosition.z += 5000f;
			this.uiText.transform.localPosition = localPosition;
			gameObject2.AddComponent<UIFollowTarget>().target = base.gameObject.transform;
		}
		this.playerheadTip = base.gameObject.AddComponent<UIIngameActorHPMP>();
		this.playerheadTip.Init(this);
		this.playerheadTip.SpriteDepth = (int)((ActorController.HeadTipDepth += 4) % 100 + (1000 - (int)this.actorType * 100));
		GameObject gameObject3 = ObjectUtil.FindChildObject(base.gameObject, "Spine1");
		if (gameObject3 != null)
		{
			this.spineTransform = gameObject3.transform;
		}
		float num = 1f;
		if (this.actorType != ActorController.EActorType.ELopet)
		{
			if (this.factionType == ActorController.EFactionType.ERed)
			{
				if (this.IsBox)
				{
					gameObject = Res.Load<GameObject>("Skill/st_027", false);
					this.boxExplode = Res.Load<GameObject>("Skill/st_028", false);
				}
				else if (this.IsBoss)
				{
					gameObject = Res.Load<GameObject>("Skill/st_007", false);
					num = this.GetBoundsMinRadius() / 0.5f;
				}
				else
				{
					gameObject = Res.Load<GameObject>("Skill/st_009", false);
					num = this.GetBoundsMinRadius() / 0.275f;
				}
			}
			else if (this.ActorType == ActorController.EActorType.EPet)
			{
				gameObject = Res.Load<GameObject>("Skill/st_008", false);
				num = this.GetBoundsMinRadius() / 0.275f;
			}
			else
			{
				gameObject = Res.Load<GameObject>("Skill/st_008a", false);
			}
			if (gameObject != null)
			{
				this.feetEffect = Tools.AddChild(base.gameObject, gameObject);
			}
			if (this.feetEffect == null)
			{
				global::Debug.LogError(new object[]
				{
					"Instantiate st_008 or st_009 or st_027 error"
				});
				return;
			}
			this.feetEffect.transform.rotation = Quaternion.identity;
			ParticleSystem[] componentsInChildren = this.feetEffect.GetComponentsInChildren<ParticleSystem>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].startSize *= num;
			}
		}
		gameObject = Res.Load<GameObject>("Skill/st_012", false);
		if (!(gameObject != null))
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load Skill/st_012 error"
			});
			return;
		}
		this.runEffect = Tools.AddChild(base.gameObject, gameObject);
		if (this.runEffect == null)
		{
			global::Debug.LogError(new object[]
			{
				"Instantiate st_012 error"
			});
			return;
		}
		this.runEffect.SetActive(false);
		this.isDead = false;
		for (int j = 1; j < 11; j++)
		{
			this.UpdateAtt(j);
		}
		this.CurHP = this.MaxHP;
		this.CurMP = this.MaxMP;
		this.meshChanged = true;
	}

	public void OnUpdate(float deltaTime)
	{
		if (this.cacheActionScale != this.actionScale)
		{
			float d = this.actionScale;
			float num = Time.time - this.cacheActionScaleTimer;
			if (num > 0.2f)
			{
				this.cacheActionScale = this.actionScale;
			}
			else
			{
				d = Mathf.Lerp(this.cacheActionScale, this.actionScale, num / 0.2f);
			}
			base.transform.localScale = this.initLocalScale * d;
		}
		if (this.navAgent != null)
		{
			this.navAgent.OnUpdate(deltaTime);
		}
		if (this.navAgent != null && this.navAgent.hasPath && this.navAgent.velocity.sqrMagnitude > 0.1f)
		{
			if (!this.lockMove)
			{
				Quaternion to = Quaternion.LookRotation(this.navAgent.velocity);
				base.transform.rotation = Quaternion.Slerp(base.transform.rotation, to, Time.deltaTime * 15f);
				this.runEffect.SetActive(true);
				if (this.feetEffect != null)
				{
					this.feetEffect.transform.rotation = Quaternion.identity;
				}
			}
		}
		else if (this.rotateTimer > 0f)
		{
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, this.newRotation, Time.deltaTime * 10f);
			this.rotateTimer -= Time.deltaTime;
			this.runEffect.SetActive(false);
			if (this.feetEffect != null)
			{
				this.feetEffect.transform.rotation = Quaternion.identity;
			}
		}
		else
		{
			this.runEffect.SetActive(false);
		}
		if (this.delayTargets.Count > 0)
		{
			for (int i = 0; i < this.delayTargets.Count; i++)
			{
				if (!this.delayTargets[i].Deleted)
				{
					this.delayTargets[i].Delay -= Time.deltaTime;
					if (this.delayTargets[i].Delay <= 0f)
					{
						this.DoEffectOnTarget(this.delayTargets[i].Info, this.delayTargets[i].Target, this.delayTargets[i].Index);
						this.delayTargets[i].Deleted = true;
					}
				}
			}
			this.delayTargets.RemoveAll(new Predicate<ActorController.DelayTarget>(ActorController.IsDeletedDelayTarget));
		}
		if (this.isTrigger)
		{
			this.OnSkillCast(this.triggerInfo, this.triggerTarget, (!(this.triggerTarget != null)) ? base.transform.position : this.triggerTarget.transform.position, 0);
			this.isTrigger = false;
			this.triggerInfo = null;
			this.triggerTarget = null;
		}
		if (this.buffs.Count > 0)
		{
			for (int j = 0; j < this.buffs.Count; j++)
			{
				if (!this.buffs[j].Deleted)
				{
					this.buffs[j].Update(Time.deltaTime);
				}
			}
			this.buffs.RemoveAll(new Predicate<Buff>(ActorController.IsDeletedBuff));
		}
		if (this.soundTimer > 0f)
		{
			this.soundTimer -= Time.deltaTime;
		}
		this.mpTimer += Time.deltaTime;
		if (this.mpTimer >= 3f)
		{
			this.mpTimer -= 3f;
			if (this.CurMP < this.MaxMP)
			{
				this.CurMP += this.MaxMP / 100L * 3L;
				if (this.CurMP > this.MaxMP)
				{
					this.CurMP = this.MaxMP;
				}
			}
		}
		for (int k = 0; k < this.Skills.Length; k++)
		{
			if (this.Skills[k] != null && this.Skills[k].Cooldown > 0f)
			{
				this.Skills[k].Cooldown -= Time.deltaTime;
			}
		}
	}

	private void FixedUpdate()
	{
		for (int i = 0; i < this.meshInfos.Count; i++)
		{
			this.meshInfos[i].FixedUpdate();
		}
	}

	private void VerifyMeshChanged()
	{
		if (!this.meshChanged)
		{
			return;
		}
		this.meshInfos.Clear();
		Renderer[] componentsInChildren = base.gameObject.GetComponentsInChildren<Renderer>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			Renderer renderer = componentsInChildren[i];
			if (!(renderer == null))
			{
				if (!(renderer is ParticleSystemRenderer))
				{
					if (!(renderer is ParticleRenderer))
					{
						if (!(renderer is LineRenderer))
						{
							if (!(renderer is TrailRenderer))
							{
								CharacterMeshInfo item = new CharacterMeshInfo(renderer);
								this.meshInfos.Add(item);
							}
						}
					}
				}
			}
		}
		this.meshChanged = false;
	}

	private static bool IsDeletedDelayTarget(ActorController.DelayTarget data)
	{
		return data.Deleted;
	}

	private static bool IsDeletedBuff(Buff data)
	{
		return data.Deleted;
	}

	private void OnDestroy()
	{
		if (this.uiText != null)
		{
			UnityEngine.Object.Destroy(this.uiText.gameObject);
			this.uiText = null;
		}
		this.ClearPoolSocket();
	}

	public void SetData(SocketDataEx socket, ActorController.EFactionType faction)
	{
		if (socket == null)
		{
			global::Debug.LogError(new object[]
			{
				"socket == null"
			});
			return;
		}
		PetDataEx pet = socket.GetPet();
		if (pet == null)
		{
			global::Debug.LogError(new object[]
			{
				"socket pet == null"
			});
			return;
		}
		this.SocketSlot = pet.GetSocketSlot();
		if (socket.IsPlayer())
		{
			this.actorType = ActorController.EActorType.EPlayer;
			this.ElementType = 0;
			this.playerGender = pet.Info.Type;
			this.playerName = pet.Info.Name;
		}
		else
		{
			this.actorType = ActorController.EActorType.EPet;
			this.ElementType = pet.Info.ElementType;
			this.petInfo = pet.Info;
			this.playerSkillID = pet.GetPlayerSkillID();
			int skillID = pet.GetSkillID(0);
			if (skillID != 0)
			{
				this.AddSkill(0, skillID, false);
			}
			skillID = pet.GetSkillID(1);
			if (skillID != 0)
			{
				this.AddSkill(1, skillID, false);
			}
			skillID = pet.GetSkillID(2);
			if (skillID != 0 && pet.Data.Further >= 3u)
			{
				this.AddSkill(2, skillID, false);
			}
			skillID = pet.GetSkillID(3);
			if (skillID != 0 && pet.Data.Further >= 4u)
			{
				this.AddSkill(3, skillID, false);
			}
		}
		this.factionType = faction;
		this.Level = pet.Data.Level;
		for (int i = 1; i < 11; i++)
		{
			this.attInit[i] = (long)socket.GetAtt(i);
		}
		for (int j = 1; j < 7; j++)
		{
			this.resist[j] = socket.GetResist(j);
		}
		LevelInfo info = Globals.Instance.AttDB.LevelDict.GetInfo((int)this.Level);
		if (info == null)
		{
			global::Debug.LogError(new object[]
			{
				"socket pet == null"
			});
			return;
		}
		this.MaxMP = (long)((ulong)info.MaxMP);
		for (int k = 0; k < 6; k++)
		{
			LegendSkillData legendSkill = socket.GetLegendSkill(k);
			if (legendSkill != null && legendSkill.EffectType != 0)
			{
				legendSkill.Cooldown = 0f;
				switch (legendSkill.EffectType)
				{
				case 3:
					this.doubleDamage = legendSkill;
					break;
				case 4:
					this.reflexDamage = legendSkill;
					break;
				case 5:
					this.reduceDamage = legendSkill;
					break;
				case 6:
					this.damageToHeal = legendSkill;
					break;
				case 7:
					this.ignoreDefense = legendSkill;
					break;
				case 8:
					this.attackToHeal = legendSkill;
					break;
				}
			}
		}
		this.Init();
	}

	public void SetAssistInfo(PetInfo pInfo, MonsterInfo mInfo, bool addSkill = true)
	{
		if (pInfo == null || mInfo == null)
		{
			global::Debug.LogError(new object[]
			{
				"pInfo == null || mInfo == null"
			});
			return;
		}
		this.actorType = ActorController.EActorType.EPet;
		this.petInfo = pInfo;
		this.factionType = ActorController.EFactionType.EBlue;
		this.Level = mInfo.Level;
		this.ElementType = pInfo.ElementType;
		this.attInit[1] = (long)mInfo.MaxHP;
		this.attInit[2] = (long)mInfo.Attack;
		this.attInit[3] = (long)mInfo.PhysicDefense;
		this.attInit[4] = (long)mInfo.MagicDefense;
		this.attInit[5] = (long)mInfo.Hit;
		this.attInit[6] = (long)mInfo.Dodge;
		this.attInit[7] = (long)mInfo.Crit;
		this.attInit[8] = (long)mInfo.CritResist;
		this.attInit[9] = (long)((ulong)mInfo.DamagePlus);
		this.attInit[10] = (long)((ulong)mInfo.DamageMinus);
		this.resist[1] = mInfo.StunResist;
		this.resist[2] = mInfo.RootResist;
		this.resist[3] = mInfo.FearResist;
		this.resist[4] = mInfo.HitBackResist;
		this.resist[5] = mInfo.HitDownResist;
		this.resist[6] = mInfo.SilenceResist;
		if (addSkill)
		{
			for (int i = 0; i < mInfo.SkillID.Count; i++)
			{
				if (mInfo.SkillID[i] != 0)
				{
					this.AddSkill(i, mInfo.SkillID[i], false);
				}
			}
		}
		this.Init();
	}

	public void SetMonsterInfo(MonsterInfo mInfo, SceneInfo sceneInfo, int attScale = 10000, ActorController.EFactionType faction = ActorController.EFactionType.ERed)
	{
		if (mInfo == null)
		{
			global::Debug.LogError(new object[]
			{
				"mInfo == null"
			});
			return;
		}
		this.actorType = ActorController.EActorType.EMonster;
		this.monsterInfo = mInfo;
		this.factionType = faction;
		this.Level = this.monsterInfo.Level;
		this.ElementType = this.monsterInfo.ElementType;
		if (this.MaxRunSpeed <= 0f)
		{
			this.canRotate = false;
		}
		if (sceneInfo != null && sceneInfo.Type == 6 && GameUIManager.mInstance.uiState.KRQuest != null)
		{
			this.Level = Globals.Instance.Player.Data.Level;
			KRQuestInfo kRQuest = GameUIManager.mInstance.uiState.KRQuest;
			this.attInit[1] = (long)mInfo.MaxHP * (long)kRQuest.MaxHPScale / 10000L;
			this.attInit[2] = (long)mInfo.Attack * (long)kRQuest.AttackScale / 10000L;
			this.attInit[3] = (long)mInfo.PhysicDefense * (long)kRQuest.PhysicDefenseScale / 10000L;
			this.attInit[4] = (long)mInfo.MagicDefense * (long)kRQuest.MagicDefenseScale / 10000L;
			this.attInit[5] = (long)mInfo.Hit * (long)kRQuest.HitScale / 10000L;
			this.attInit[6] = (long)mInfo.Dodge * (long)kRQuest.DodgeScale / 10000L;
			this.attInit[7] = (long)mInfo.Crit * (long)kRQuest.CritScale / 10000L;
			this.attInit[8] = (long)mInfo.CritResist * (long)kRQuest.CritResisScale / 10000L;
			this.attInit[9] = (long)mInfo.DamagePlus * (long)kRQuest.DamagePlusScale / 10000L;
			this.attInit[10] = (long)mInfo.DamageMinus * (long)kRQuest.DamageMinusScale / 10000L;
		}
		else
		{
			this.attInit[1] = (long)mInfo.MaxHP * (long)attScale / 10000L;
			this.attInit[2] = (long)mInfo.Attack * (long)attScale / 10000L;
			this.attInit[3] = (long)mInfo.PhysicDefense * (long)attScale / 10000L;
			this.attInit[4] = (long)mInfo.MagicDefense * (long)attScale / 10000L;
			this.attInit[5] = (long)mInfo.Hit * (long)attScale / 10000L;
			this.attInit[6] = (long)mInfo.Dodge * (long)attScale / 10000L;
			this.attInit[7] = (long)mInfo.Crit * (long)attScale / 10000L;
			this.attInit[8] = (long)mInfo.CritResist * (long)attScale / 10000L;
			this.attInit[9] = (long)mInfo.DamagePlus * (long)attScale / 10000L;
			this.attInit[10] = (long)mInfo.DamageMinus * (long)attScale / 10000L;
		}
		this.resist[1] = mInfo.StunResist;
		this.resist[2] = mInfo.RootResist;
		this.resist[3] = mInfo.FearResist;
		this.resist[4] = mInfo.HitBackResist;
		this.resist[5] = mInfo.HitDownResist;
		this.resist[6] = mInfo.SilenceResist;
		for (int i = 0; i < this.monsterInfo.SkillID.Count; i++)
		{
			if (this.monsterInfo.SkillID[i] != 0)
			{
				this.AddSkill(i, this.monsterInfo.SkillID[i], false);
			}
		}
		this.Init();
		this.CastPassiveSkill();
	}

	public void SetLopetData(LopetDataEx lpData, ActorController.EFactionType faction)
	{
		if (lpData == null)
		{
			global::Debug.LogError(new object[]
			{
				"lpData == null"
			});
			return;
		}
		this.playerSkillID = lpData.Info.PlayerSkillID;
		this.actorType = ActorController.EActorType.ELopet;
		this.factionType = faction;
		this.Unattacked = true;
		this.Unhealed = true;
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		lpData.GetAttribute(ref num, ref num2, ref num3, ref num4);
		this.attInit[1] = (long)num;
		this.attInit[2] = (long)num2;
		this.attInit[3] = (long)num3;
		this.attInit[4] = (long)num4;
		this.Init();
	}

	public int GetPlayerSkillID()
	{
		return this.playerSkillID;
	}

	public int GetAtt(EAttID type)
	{
		return (int)this.AttData[(int)type];
	}

	public int GetInitAtt(EAttID type)
	{
		return (int)this.attInit[(int)type];
	}

	public long GetLongInitAtt(EAttID type)
	{
		return this.attInit[(int)type];
	}

	private void UpdateAtt(int attID)
	{
		if (attID <= 0 || attID >= 11)
		{
			global::Debug.LogErrorFormat("UpdateAtt Invalid attID = {0}", new object[]
			{
				attID
			});
			return;
		}
		long num = (this.attInit[attID] + (long)this.attValueMod[attID]) * (long)(10000 + this.attPctMod[attID]) / 10000L;
		if (num < 0L)
		{
			num = 0L;
		}
		this.AttData[attID] = num;
	}

	public void HandleAttMod(EAttMod modType, int attID, int attValue, bool apply)
	{
		if (attID <= 0)
		{
			global::Debug.LogErrorFormat("HandleAttMod Invalid attID = {0}", new object[]
			{
				attID
			});
			return;
		}
		if (attID < 11)
		{
			if (modType != EAttMod.EAM_Value)
			{
				if (modType != EAttMod.EAM_Pct)
				{
					global::Debug.LogErrorFormat("Error ModType = {0}", new object[]
					{
						modType
					});
					return;
				}
				this.attPctMod[attID] += ((!apply) ? (-attValue) : attValue);
			}
			else
			{
				this.attValueMod[attID] += ((!apply) ? (-attValue) : attValue);
			}
			this.UpdateAtt(attID);
		}
		else
		{
			if (attID == 20)
			{
				this.HandleAttMod(modType, 3, attValue, apply);
				this.HandleAttMod(modType, 4, attValue, apply);
				this.UpdateAtt(3);
				this.UpdateAtt(4);
				return;
			}
			int num = attID - 300;
			if (num <= 0 || num >= 7)
			{
				global::Debug.LogErrorFormat("attID error, attID = {0}", new object[]
				{
					attID
				});
				return;
			}
			this.resist[num] += ((!apply) ? (-attValue) : attValue);
		}
	}

	public float GetSpeedMod()
	{
		return this.speedScale.Value * (1f + (float)this.speedMod / 10000f);
	}

	public void UpdateSpeedScale(float value)
	{
		if (this.speedScale.Value == value)
		{
			return;
		}
		this.speedScale.Value = value;
		this.UpdateSpeedEffect();
	}

	public void BuffModSpeed(int value, bool apply)
	{
		this.speedMod += ((!apply) ? (-value) : value);
		this.UpdateSpeedEffect();
	}

	private void UpdateSpeedEffect()
	{
		if (this.navAgent == null)
		{
			return;
		}
		this.navAgent.speed = this.MaxRunSpeed * this.GetSpeedMod();
		this.animationCtrler.UpdateSpeed(this.GetSpeedMod());
	}

	public bool IsHostileTo(ActorController actor)
	{
		if (this == actor)
		{
			return false;
		}
		if (actor.IsChangeFaction)
		{
			return true;
		}
		if (!this.IsChangeFaction)
		{
			return this.FactionType != actor.FactionType;
		}
		return this.FactionType == actor.FactionType;
	}

	public bool IsFriendlyTo(ActorController actor)
	{
		return this.FactionType == actor.FactionType;
	}

	public void AddSkill(int index, int skillID, bool prefabPool = true)
	{
		if (index >= this.Skills.Length)
		{
			global::Debug.LogErrorFormat("[AddSkill] index overflow, index = {0}", new object[]
			{
				index
			});
			return;
		}
		SkillInfo info = Globals.Instance.AttDB.SkillDict.GetInfo(skillID);
		if (info == null)
		{
			global::Debug.LogErrorFormat("[AddSkill] GetInfo error, id = {0}", new object[]
			{
				skillID
			});
			return;
		}
		if (this.Skills[index] != null)
		{
			global::Debug.LogError(new object[]
			{
				"[AddSkill] index slot exit skill!"
			});
			return;
		}
		SkillData skillData = new SkillData();
		skillData.Info = info;
		this.Skills[index] = skillData;
		if (prefabPool)
		{
			PoolMgr.CreateSkillPrefabPool(skillID);
		}
	}

	public void InsertSkill(int index, int skillID)
	{
		if (index < 1 || index >= this.Skills.Length)
		{
			global::Debug.LogErrorFormat("[InsertSkill] index error, index = {0}", new object[]
			{
				index
			});
			return;
		}
		for (int i = index; i < this.Skills.Length; i++)
		{
			if (this.Skills[i] == null)
			{
				for (int j = i; j > index; j--)
				{
					this.Skills[j] = this.Skills[j - 1];
				}
				this.Skills[index] = null;
				this.AddSkill(index, skillID, true);
				return;
			}
		}
		global::Debug.LogError(new object[]
		{
			"[InsertSkill] no empty slot"
		});
	}

	public void ClearSkillCD()
	{
		for (int i = 0; i < this.Skills.Length; i++)
		{
			if (this.Skills[i] != null)
			{
				this.Skills[i].Cooldown = 0f;
			}
		}
	}

	public void ClearSkillCD(int index)
	{
		if (this.Skills[index] != null)
		{
			this.Skills[index].Cooldown = 0f;
		}
	}

	public bool IsValidSkill(int index)
	{
		return index > 0 && index < this.Skills.Length && this.Skills[index] != null;
	}

	private void CountSkillDamageHeal(SkillInfo info, long damage, long heal)
	{
		if (this.actorType != ActorController.EActorType.EPlayer && this.actorType != ActorController.EActorType.EPet)
		{
			return;
		}
		int num = this.SkillCountIndex;
		for (int i = 0; i < this.Skills.Length; i++)
		{
			if (this.Skills[i] != null && this.Skills[i].Info == info)
			{
				num = i;
				break;
			}
		}
		if (num < 0 || num >= this.Skills.Length)
		{
			return;
		}
		this.Skills[num].Damage += damage;
		this.Skills[num].Heal += heal;
		if (damage > this.Skills[num].HighDamage)
		{
			this.Skills[num].HighDamage = damage;
		}
	}

	public void BuffRoot(bool apply)
	{
		if (apply)
		{
			this.StopMove();
			this.rootBuff++;
		}
		else
		{
			this.rootBuff--;
		}
	}

	public void BuffStun(bool apply)
	{
		if (apply)
		{
			this.StopMove();
			this.InterruptSkill(0);
			this.stunBuff++;
		}
		else
		{
			this.stunBuff--;
			if (this.stunBuff == 0)
			{
				this.AnimationCtrler.StopAnimation();
			}
		}
	}

	public void BuffSilence(bool apply)
	{
		if (apply)
		{
			this.InterruptSkill(0);
			this.silenceBuff++;
		}
		else
		{
			this.silenceBuff--;
		}
	}

	public void BuffImmunity(bool apply)
	{
		this.immunityBuff += ((!apply) ? -1 : 1);
	}

	public void BuffAbsorb(bool apply)
	{
		this.absorbBuff += ((!apply) ? -1 : 1);
	}

	public void BuffChangeFaction(bool apply)
	{
		if (this.changeFactionBuff == 0)
		{
			this.aiCtrler.SetTarget(null);
			this.aiCtrler.SetSelectTarget(null);
			this.aiCtrler.ThreatMgr.Clear();
			if (apply && this == Globals.Instance.ActorMgr.PlayerCtrler.ActorCtrler)
			{
				this.StopMove();
				Globals.Instance.ActorMgr.PlayerCtrler.SetControlLocked(true);
			}
		}
		this.changeFactionBuff += ((!apply) ? -1 : 1);
		if (this.changeFactionBuff == 0)
		{
			this.aiCtrler.SetTarget(null);
			this.aiCtrler.SetSelectTarget(null);
			this.aiCtrler.ThreatMgr.Clear();
			if (!apply && this == Globals.Instance.ActorMgr.PlayerCtrler.ActorCtrler && !Globals.Instance.ActorMgr.IsControlLocked())
			{
				Globals.Instance.ActorMgr.PlayerCtrler.SetControlLocked(false);
			}
		}
	}

	public void BuffFear(bool apply)
	{
		if (apply)
		{
			if (this.fearBuff == 0)
			{
				this.aiCtrler.Fear(1.2f);
			}
			this.fearBuff++;
		}
		else
		{
			this.fearBuff--;
			if (this.fearBuff == 0)
			{
				this.aiCtrler.CancelFear();
			}
		}
	}

	public void BuffDamageMod(int value, bool apply)
	{
		this.damageMod += ((!apply) ? (-value) : value);
	}

	public void BuffDamageTakenMod(int value, bool apply)
	{
		this.damageTakenMod += ((!apply) ? (-value) : value);
	}

	public void BuffHealMod(int value, bool apply)
	{
		this.healMod += ((!apply) ? (-value) : value);
	}

	public void BuffHealTakenMod(int value, bool apply)
	{
		this.healTakenMod += ((!apply) ? (-value) : value);
	}

	public void BuffAttackSpeedMod(int value, bool apply)
	{
		this.attackSpeedMod += ((!apply) ? (-value) : value);
	}

	public void BuffDamageReflect(int value, bool apply)
	{
		this.damageReflect += ((!apply) ? (-value) : value);
	}

	public void BuffResurrectRate(int value, int coolDown, bool apply)
	{
		this.resurrectRate += ((!apply) ? (-value) : value);
		this.resurrectCD = (float)coolDown;
	}

	public void BuffSuck(int value, bool apply)
	{
		this.suckPct += ((!apply) ? (-value) : value);
	}

	public void BuffModResist(int index, int value, bool apply)
	{
		if (index <= 0 || index >= 7)
		{
			global::Debug.LogErrorFormat("Resist index error, index = {0}", new object[]
			{
				index
			});
		}
		this.resist[index] += ((!apply) ? (-value) : value);
	}

	public void BuffModAutoAttack(int skillID, bool apply)
	{
		this.ChangeSkill(0, skillID, apply);
	}

	public void BuffModHitEffect(int value, bool apply)
	{
		this.hitEffect = ((!apply) ? 0 : value);
		if (apply)
		{
			PoolMgr.CreatePrefabPool("Skill/hit_26", 1, 5);
		}
	}

	public void BuffSuperShield(int value, bool apply)
	{
		this.superShield += ((!apply) ? (-value) : value);
	}

	public void BuffChangeModel(BuffInfo info, int effectIndex, bool apply)
	{
		if (info == null || effectIndex < 0 || effectIndex >= info.EffectType.Count)
		{
			return;
		}
		if (apply)
		{
			ModelController component = base.GetComponent<ModelController>();
			if (component != null)
			{
				component.SetModelState(1);
				this.animationCtrler.UpdateAnimCtrl();
			}
		}
		else
		{
			if (!string.IsNullOrEmpty(info.RemoveAction))
			{
				this.PlayAction(info.RemoveAction, null);
			}
			ModelController component2 = base.GetComponent<ModelController>();
			if (component2 != null)
			{
				component2.SetModelState(0);
				this.animationCtrler.UpdateAnimCtrl();
			}
		}
		this.ChangeSkill(0, info.Value1[effectIndex], apply);
		this.ChangeSkill(1, info.Value2[effectIndex], apply);
	}

	public void BuffShareDamage(int value, bool apply)
	{
		this.shareDamage += ((!apply) ? (-value) : value);
	}

	private void ChangeSkill(int index, int newSkillID, bool apply)
	{
		if (this.Skills == null || newSkillID == 0 || index < 0 || index >= this.Skills.Length)
		{
			global::Debug.LogError(new object[]
			{
				"ChangeSkill param error"
			});
			return;
		}
		if (apply)
		{
			if (this.Skills[index] == null)
			{
				this.AddSkill(index, newSkillID, true);
			}
			else
			{
				this.Skills[index].Info = Globals.Instance.AttDB.SkillDict.GetInfo(newSkillID);
				if (this.Skills[index].Info == null)
				{
					global::Debug.LogErrorFormat("[ChangeSkill] GetInfo error, newSkillID = {0}", new object[]
					{
						newSkillID
					});
					return;
				}
				PoolMgr.CreateSkillPrefabPool(newSkillID);
			}
		}
		else
		{
			int num = 0;
			switch (this.actorType)
			{
			case ActorController.EActorType.EPlayer:
				num = 16;
				break;
			case ActorController.EActorType.EPet:
				if (index >= 0 && index < this.petInfo.SkillID.Count)
				{
					num = this.petInfo.SkillID[index];
				}
				break;
			case ActorController.EActorType.EMonster:
				if (index >= 0 && index < this.monsterInfo.SkillID.Count)
				{
					num = this.monsterInfo.SkillID[index];
				}
				break;
			}
			if (num == 0)
			{
				this.Skills[index] = null;
			}
			else
			{
				this.Skills[index].Info = Globals.Instance.AttDB.SkillDict.GetInfo(num);
				if (this.Skills[index].Info == null)
				{
					global::Debug.LogErrorFormat("[ChangeSkill] GetInfo error, oldSkillID = {0}", new object[]
					{
						newSkillID
					});
					return;
				}
			}
		}
	}

	public bool CanMove(bool ignoreLock = false, bool ignoreFear = false)
	{
		if (this.IsDead || this.IsRoot || this.IsStun || (!ignoreFear && this.IsFear))
		{
			return false;
		}
		if (ignoreLock)
		{
			if (this.IsValidSkill(this.lockSkillIndex))
			{
				return false;
			}
		}
		else if (this.lockMove)
		{
			return false;
		}
		return true;
	}

	public void StartMove(Vector3 targetPos)
	{
		if (this.navAgent != null && this.CanMove(false, false))
		{
			this.navAgent.destination = targetPos;
		}
	}

	public void StopMove()
	{
		if (this.navAgent != null)
		{
			this.navAgent.Stop();
		}
	}

	public void RotateTo(Quaternion rotation)
	{
		if (!this.canRotate)
		{
			return;
		}
		this.newRotation = rotation;
		this.rotateTimer = 0.5f;
	}

	public void LookAt(Transform trans)
	{
		base.transform.LookAt(trans);
	}

	public void FaceToPosition(Vector3 targetPosition)
	{
		if (!this.canRotate)
		{
			return;
		}
		Vector3 vector = targetPosition - base.transform.position;
		if (vector == Vector3.zero)
		{
			return;
		}
		Quaternion rotation = Quaternion.LookRotation(vector);
		rotation.x = 0f;
		rotation.z = 0f;
		base.transform.rotation = rotation;
	}

	public float GetBoundsRadius()
	{
		return Mathf.Max(((BoxCollider)base.transform.collider).size.x, ((BoxCollider)base.transform.collider).size.z) * 0.5f * base.transform.localScale.y;
	}

	public float GetBoundsMinRadius()
	{
		return Mathf.Min(((BoxCollider)base.transform.collider).size.x, ((BoxCollider)base.transform.collider).size.z) * 0.5f * base.transform.localScale.y;
	}

	public float GetDistance2D(ActorController actor)
	{
		float num = CombatHelper.Distance2D(actor.transform.position, base.transform.position);
		num -= this.GetBoundsRadius() + actor.GetBoundsRadius();
		return (num <= 0f) ? 0f : num;
	}

	public float GetDistance2D(Vector3 pos)
	{
		float num = CombatHelper.Distance2D(pos, base.transform.position);
		num -= this.GetBoundsRadius();
		return (num <= 0f) ? 0f : num;
	}

	public bool InterruptSkill(int serialID = 0)
	{
		if (serialID != 0 && this.skillSerialID != serialID)
		{
			return false;
		}
		if (this.lockSkillIndex == -1)
		{
			return false;
		}
		this.skillSerialID++;
		this.lockSkillCast = false;
		this.lockMove = false;
		this.lockSkillIndex = -1;
		this.lockSkillCD = false;
		this.skillCastCache = -1;
		return true;
	}

	public int GenerateSkillSerialID()
	{
		return ++this.skillSerialID;
	}

	public ECastSkillResult TryCastSkill(int skillIndex, ActorController target)
	{
		return this.TryCastSkill(skillIndex, target, (!(target != null)) ? Vector3.zero : target.transform.position);
	}

	public ECastSkillResult TryCastSkill(SkillInfo info, ActorController target)
	{
		return this.TryCastSkill(info, target, (!(target != null)) ? Vector3.zero : target.transform.position);
	}

	public ECastSkillResult TryCastSkill(int skillIndex, Vector3 targetPosition)
	{
		return this.TryCastSkill(skillIndex, null, targetPosition);
	}

	public ECastSkillResult TryCastSkill(SkillInfo info, Vector3 targetPosition)
	{
		return this.TryCastSkill(info, null, targetPosition);
	}

	private ECastSkillResult TryCastSkill(int skillIndex, ActorController target, Vector3 targetPosition)
	{
		if (skillIndex >= this.Skills.Length || skillIndex < 0)
		{
			return ECastSkillResult.ECSR_NoSkill;
		}
		if (this.Skills[skillIndex] == null || this.Skills[skillIndex].Info == null)
		{
			return ECastSkillResult.ECSR_InvalidSkillInfo;
		}
		if (!this.Skills[skillIndex].IsCooldown)
		{
			return ECastSkillResult.ECSR_NotCoolDown;
		}
		if (this.lockSkillCast)
		{
			if (!SkillData.IsAutoAttack(this.lockSkillIndex) || SkillData.IsAutoAttack(skillIndex) || this.actorType != ActorController.EActorType.EPlayer)
			{
				this.skillCastCache = skillIndex;
				this.skillCacheTarget = target;
				this.skillCachePos = targetPosition;
				return ECastSkillResult.ECSR_Cache;
			}
			this.InterruptSkill(0);
		}
		this.lockSkillIndex = skillIndex;
		ECastSkillResult eCastSkillResult = this.TryCastSkill(this.Skills[skillIndex].Info, target, targetPosition);
		if (eCastSkillResult != ECastSkillResult.ECSR_Sucess)
		{
			this.lockSkillIndex = -1;
		}
		return eCastSkillResult;
	}

	private ECastSkillResult TryCastSkill(SkillInfo info, ActorController target, Vector3 targetPosition)
	{
		if (info == null)
		{
			return ECastSkillResult.ECSR_InvalidSkillInfo;
		}
		if (this.IsDead)
		{
			return ECastSkillResult.ECSR_YouDead;
		}
		if (this.IsSilence)
		{
			return ECastSkillResult.ECSR_InSilence;
		}
		if (this.IsStun)
		{
			return ECastSkillResult.ECSR_InStun;
		}
		if (this.IsFear)
		{
			return ECastSkillResult.ECSR_InFear;
		}
		if (this.actorType == ActorController.EActorType.EPlayer && this.CurMP < (long)info.ManaCost)
		{
			return ECastSkillResult.ECSR_NotEnoughMP;
		}
		switch (info.CastTargetType)
		{
		case 0:
			break;
		case 1:
		case 2:
			if (!(target != null))
			{
				return ECastSkillResult.ECSR_NotCastTargetType;
			}
			if (target.IsDead)
			{
				return ECastSkillResult.ECSR_TargetDead;
			}
			if (info.CastTargetType == 1 && !this.IsHostileTo(target))
			{
				return ECastSkillResult.ECSR_NotCastTargetType;
			}
			if (info.CastTargetType == 2 && !this.IsFriendlyTo(target))
			{
				return ECastSkillResult.ECSR_NotCastTargetType;
			}
			if (target != null)
			{
				this.FaceToPosition(target.transform.position);
			}
			break;
		case 3:
			break;
		default:
			global::Debug.LogErrorFormat("SkillID = {0}, invalid CastTargetType = {1}", new object[]
			{
				info.ID,
				info.CastTargetType
			});
			return ECastSkillResult.ECSR_InvalidSkillInfo;
		}
		Singleton<ActionMgr>.Instance.PlayCastAction(this, info, target, targetPosition);
		return ECastSkillResult.ECSR_Sucess;
	}

	public void OnLockingStart(bool canMove, bool noControl)
	{
		this.lockSkillCast = true;
		this.immuneControl = noControl;
		if (!canMove)
		{
			this.lockMove = true;
			this.StopMove();
		}
	}

	public void OnLockingStop(SkillInfo info)
	{
		int num = (info == null) ? 0 : info.ComboSkillID;
		if (this.lockSkillIndex >= 0 && this.lockSkillIndex < this.Skills.Length && this.Skills[this.lockSkillIndex] != null && this.Skills[this.lockSkillIndex].Info != null)
		{
			if (SkillData.IsAutoAttack(this.lockSkillIndex) && this.Skills[this.lockSkillIndex].Info.CoolDown > 0f)
			{
				float num2 = this.Skills[this.lockSkillIndex].Info.CoolDown / this.AttackSpeed;
				this.Skills[this.lockSkillIndex].Cooldown = num2;
				if (this.Skills[this.lockSkillIndex].Cooldown != num2)
				{
					Globals.Instance.ActorMgr.CurScene.OnFailed();
					return;
				}
			}
			num = this.Skills[this.lockSkillIndex].Info.ComboSkillID;
		}
		this.immuneControl = false;
		this.lockSkillCast = false;
		this.lockMove = false;
		this.lockSkillIndex = -1;
		this.lockSkillCD = false;
		if (num != 0)
		{
			SkillInfo info2 = Globals.Instance.AttDB.SkillDict.GetInfo(num);
			if (info2 != null)
			{
				if (this.actorType == ActorController.EActorType.EMonster && this.monsterInfo != null && this.monsterInfo.SkillToPlayer)
				{
					ActorController actor = Globals.Instance.ActorMgr.GetActor(0);
					if (actor != null)
					{
						this.TryCastSkill(info2, actor, actor.transform.position);
						return;
					}
				}
				else if (this.aiCtrler.Target != null)
				{
					this.TryCastSkill(info2, this.aiCtrler.Target, this.aiCtrler.Target.transform.position);
					return;
				}
			}
		}
		if (this.skillCastCache >= 0 && this.skillCastCache < this.Skills.Length && this.Skills[this.skillCastCache].Info != null)
		{
			this.TryCastSkill(this.skillCastCache, this.skillCacheTarget, this.skillCachePos);
			this.skillCastCache = -1;
		}
	}

	public void OnSkillStart(SkillInfo info)
	{
		if (info == null || this.lockSkillIndex < 0 || this.lockSkillIndex >= this.Skills.Length || this.Skills[this.lockSkillIndex].Info != info)
		{
			return;
		}
		if (this.lockSkillCD)
		{
			return;
		}
		this.lockSkillCD = true;
		if (SkillData.IsAutoAttack(this.lockSkillIndex) && this.Skills[this.lockSkillIndex].Info.CoolDown > 0f)
		{
			this.Skills[this.lockSkillIndex].Cooldown = this.Skills[this.lockSkillIndex].Info.CoolDown;
			if (this.Skills[this.lockSkillIndex].Cooldown != this.Skills[this.lockSkillIndex].Info.CoolDown)
			{
				Globals.Instance.ActorMgr.CurScene.OnFailed();
				return;
			}
		}
		else
		{
			this.Skills[this.lockSkillIndex].Cooldown = this.Skills[this.lockSkillIndex].Info.CoolDown;
			this.Skills[this.lockSkillIndex].Rate = 0;
			if (this.Skills[this.lockSkillIndex].Cooldown != this.Skills[this.lockSkillIndex].Info.CoolDown)
			{
				Globals.Instance.ActorMgr.CurScene.OnFailed();
				return;
			}
			if (this.actorType == ActorController.EActorType.EPlayer && (this.aiCtrler.AssistSkill & 1) == 0 && this.Skills[this.lockSkillIndex].Info.ManaCost > 0)
			{
				this.CurMP -= (long)((ulong)this.Skills[this.lockSkillIndex].Info.ManaCost);
			}
			if (this.actorType == ActorController.EActorType.EPlayer || this.actorType == ActorController.EActorType.EPet)
			{
				if (this.actorType == ActorController.EActorType.EPlayer && this.lockSkillIndex == 4)
				{
					this.CurEP = 0L;
				}
				else if (this.Skills[this.lockSkillIndex].Info.EPValue > 0)
				{
					if (this.factionType == ActorController.EFactionType.EBlue)
					{
						Globals.Instance.ActorMgr.AddLocalPlayerEP(this.Skills[this.lockSkillIndex].Info.EPValue);
					}
					else
					{
						Globals.Instance.ActorMgr.AddRemotePlayerEP(this.Skills[this.lockSkillIndex].Info.EPValue);
					}
				}
			}
		}
		this.Skills[this.lockSkillIndex].Count++;
	}

	public void OnSkillCast(SkillInfo info, ActorController target, Vector3 targetPosition, int index)
	{
		switch (info.EffectTargetType)
		{
		case 0:
			if (info.CastTargetType != 3)
			{
				this.DoEffectOnTarget(info, target, index);
			}
			else
			{
				this.DoEffectOnPosition(info, targetPosition);
			}
			break;
		case 1:
			this.DoEffectOnTarget(info, this, index);
			break;
		case 2:
		{
			List<ActorController> actors = Globals.Instance.ActorMgr.Actors;
			for (int i = 0; i < actors.Count; i++)
			{
				ActorController actorController = actors[i];
				if (actorController && !actorController.IsDead && this.IsHostileTo(actorController))
				{
					if (CombatHelper.DistanceSquared2D(base.transform.position, actorController.transform.position) <= (info.Radius + actorController.GetBoundsRadius()) * (info.Radius + actorController.GetBoundsRadius()))
					{
						if (info.RadiateSpeed == 0)
						{
							this.DoEffectOnTarget(info, actorController, index);
						}
						else
						{
							ActorController.DelayTarget delayTarget = new ActorController.DelayTarget();
							delayTarget.Delay = Vector3.Distance(base.transform.position, actorController.transform.position) / (float)info.RadiateSpeed * 100f;
							delayTarget.Target = actorController;
							delayTarget.Info = info;
							delayTarget.Index = index;
							this.delayTargets.Add(delayTarget);
						}
					}
				}
			}
			break;
		}
		case 3:
		{
			List<ActorController> actors2 = Globals.Instance.ActorMgr.Actors;
			for (int j = 0; j < actors2.Count; j++)
			{
				ActorController actorController = actors2[j];
				if (actorController && !actorController.IsDead && this.IsFriendlyTo(actorController))
				{
					if (CombatHelper.DistanceSquared2D(base.transform.position, actorController.transform.position) <= (info.Radius + actorController.GetBoundsRadius()) * (info.Radius + actorController.GetBoundsRadius()))
					{
						if (info.RadiateSpeed == 0)
						{
							this.DoEffectOnTarget(info, actorController, index);
						}
						else
						{
							ActorController.DelayTarget delayTarget2 = new ActorController.DelayTarget();
							delayTarget2.Delay = Vector3.Distance(base.transform.position, actorController.transform.position) / (float)info.RadiateSpeed * 100f;
							delayTarget2.Target = actorController;
							delayTarget2.Info = info;
							delayTarget2.Index = index;
							this.delayTargets.Add(delayTarget2);
						}
					}
				}
			}
			break;
		}
		case 4:
		{
			List<ActorController> actors3 = Globals.Instance.ActorMgr.Actors;
			for (int k = 0; k < actors3.Count; k++)
			{
				ActorController actorController = actors3[k];
				if (actorController && !actorController.IsDead && this.IsHostileTo(actorController))
				{
					if (CombatHelper.DistanceSquared2D(base.transform.position, actorController.transform.position) <= (info.Radius + actorController.GetBoundsRadius()) * (info.Radius + actorController.GetBoundsRadius()))
					{
						Quaternion rotation = base.transform.rotation;
						rotation.x = 0f;
						rotation.z = 0f;
						Quaternion b = Quaternion.LookRotation(actorController.transform.position - base.transform.position);
						b.x = 0f;
						b.z = 0f;
						float num = Quaternion.Angle(rotation, b);
						if (num <= info.Angle / 2f)
						{
							if (info.RadiateSpeed == 0)
							{
								this.DoEffectOnTarget(info, actorController, index);
							}
							else
							{
								ActorController.DelayTarget delayTarget3 = new ActorController.DelayTarget();
								delayTarget3.Delay = Vector3.Distance(base.transform.position, actorController.transform.position) / (float)info.RadiateSpeed * 100f;
								delayTarget3.Target = actorController;
								delayTarget3.Info = info;
								delayTarget3.Index = index;
								this.delayTargets.Add(delayTarget3);
							}
						}
					}
				}
			}
			break;
		}
		case 5:
		{
			float num2 = Mathf.Sqrt(info.Weight / 2f * (info.Weight / 2f) + info.Length * info.Length);
			List<ActorController> actors4 = Globals.Instance.ActorMgr.Actors;
			for (int l = 0; l < actors4.Count; l++)
			{
				ActorController actorController = actors4[l];
				if (actorController && !actorController.IsDead && this.IsHostileTo(actorController))
				{
					float distance2D = actorController.GetDistance2D(base.transform.position);
					if (distance2D <= num2)
					{
						Quaternion rotation2 = base.transform.rotation;
						rotation2.x = 0f;
						rotation2.z = 0f;
						Quaternion b2 = Quaternion.LookRotation(actorController.transform.position - base.transform.position);
						b2.x = 0f;
						b2.z = 0f;
						float num3 = Quaternion.Angle(rotation2, b2);
						float num4 = distance2D * Mathf.Cos(0.0174532924f * num3);
						if (num4 >= 0f && num4 <= info.Length && distance2D * Mathf.Abs(Mathf.Sin(0.0174532924f * num3)) <= info.Weight / 2f)
						{
							if (info.RadiateSpeed == 0)
							{
								this.DoEffectOnTarget(info, actorController, index);
							}
							else
							{
								ActorController.DelayTarget delayTarget4 = new ActorController.DelayTarget();
								delayTarget4.Delay = distance2D / (float)info.RadiateSpeed * 100f;
								delayTarget4.Target = actorController;
								delayTarget4.Info = info;
								delayTarget4.Index = index;
								this.delayTargets.Add(delayTarget4);
							}
						}
					}
				}
			}
			break;
		}
		case 6:
		{
			float num5 = Mathf.Sqrt(info.Weight / 2f * (info.Weight / 2f) + info.Length / 2f * (info.Length / 2f));
			List<ActorController> actors5 = Globals.Instance.ActorMgr.Actors;
			for (int m = 0; m < actors5.Count; m++)
			{
				ActorController actorController = actors5[m];
				if (actorController && !actorController.IsDead && this.IsHostileTo(actorController))
				{
					float distance2D2 = actorController.GetDistance2D(base.transform.position);
					if (distance2D2 <= num5)
					{
						Quaternion rotation3 = base.transform.rotation;
						rotation3.x = 0f;
						rotation3.z = 0f;
						Quaternion b3 = Quaternion.LookRotation(actorController.transform.position - base.transform.position);
						b3.x = 0f;
						b3.z = 0f;
						float num6 = Quaternion.Angle(rotation3, b3);
						if (distance2D2 * Mathf.Abs(Mathf.Sin(0.0174532924f * num6)) <= info.Weight / 2f && Mathf.Abs(distance2D2 * Mathf.Cos(0.0174532924f * num6)) <= info.Length / 2f)
						{
							this.DoEffectOnTarget(info, actorController, index);
						}
					}
				}
			}
			break;
		}
		case 7:
		{
			Vector3 a = targetPosition;
			if (target != null && (info.CastTargetType == 1 || info.CastTargetType == 2))
			{
				a = target.transform.position;
			}
			List<ActorController> actors6 = Globals.Instance.ActorMgr.Actors;
			for (int n = 0; n < actors6.Count; n++)
			{
				ActorController actorController = actors6[n];
				if (actorController && !actorController.IsDead && this.IsHostileTo(actorController))
				{
					if (CombatHelper.DistanceSquared2D(a, actorController.transform.position) <= (info.Radius + actorController.GetBoundsRadius()) * (info.Radius + actorController.GetBoundsRadius()))
					{
						this.DoEffectOnTarget(info, actorController, index);
					}
				}
			}
			break;
		}
		case 8:
		{
			Vector3 a2 = targetPosition;
			if (target != null && (info.CastTargetType == 1 || info.CastTargetType == 2))
			{
				a2 = target.transform.position;
			}
			List<ActorController> actors7 = Globals.Instance.ActorMgr.Actors;
			for (int num7 = 0; num7 < actors7.Count; num7++)
			{
				ActorController actorController = actors7[num7];
				if (actorController && !actorController.IsDead && this.IsFriendlyTo(actorController))
				{
					if (CombatHelper.DistanceSquared2D(a2, actorController.transform.position) <= (info.Radius + actorController.GetBoundsRadius()) * (info.Radius + actorController.GetBoundsRadius()))
					{
						this.DoEffectOnTarget(info, actorController, index);
					}
				}
			}
			break;
		}
		default:
			global::Debug.LogErrorFormat("SkillID = {0}, invalid EffectTargetType = {1}", new object[]
			{
				info.ID,
				info.EffectTargetType
			});
			break;
		}
	}

	public void OnMissileHit(SkillInfo info, ActorController target, Vector3 targetPosition, int index)
	{
		EEffectTargetType effectTargetType = (EEffectTargetType)info.EffectTargetType;
		if (effectTargetType != EEffectTargetType.EETT_AllEnemyInArea2)
		{
			if (effectTargetType != EEffectTargetType.EETT_AllFriendInArea2)
			{
				if (effectTargetType != EEffectTargetType.EETT_Target)
				{
					global::Debug.LogErrorFormat("SkillID = {0}, invalid EffectTargetType = {1}", new object[]
					{
						info.ID,
						info.EffectTargetType
					});
				}
				else
				{
					this.DoEffectOnTarget(info, target, index);
				}
			}
			else
			{
				Vector3 a = targetPosition;
				if (target != null && (info.CastTargetType == 1 || info.CastTargetType == 2))
				{
					a = target.transform.position;
				}
				List<ActorController> actors = Globals.Instance.ActorMgr.Actors;
				for (int i = 0; i < actors.Count; i++)
				{
					ActorController actorController = actors[i];
					if (actorController && !actorController.IsDead && this.IsFriendlyTo(actorController))
					{
						if (CombatHelper.DistanceSquared2D(a, actorController.transform.position) <= (info.Radius + actorController.GetBoundsRadius()) * (info.Radius + actorController.GetBoundsRadius()))
						{
							this.DoEffectOnTarget(info, actorController, index);
						}
					}
				}
			}
		}
		else
		{
			Vector3 a2 = targetPosition;
			if (target != null && (info.CastTargetType == 1 || info.CastTargetType == 2))
			{
				a2 = target.transform.position;
			}
			List<ActorController> actors2 = Globals.Instance.ActorMgr.Actors;
			for (int j = 0; j < actors2.Count; j++)
			{
				ActorController actorController2 = actors2[j];
				if (actorController2 && !actorController2.IsDead && this.IsHostileTo(actorController2))
				{
					if (CombatHelper.DistanceSquared2D(a2, actorController2.transform.position) <= (info.Radius + actorController2.GetBoundsRadius()) * (info.Radius + actorController2.GetBoundsRadius()))
					{
						this.DoEffectOnTarget(info, actorController2, index);
					}
				}
			}
		}
	}

	private void DoReflectDamage(long damage, ActorController caster)
	{
		if (damage <= 0L)
		{
			return;
		}
		int num = this.DoDamage(damage, caster, false);
		if (this.uiText != null && num > 0)
		{
			if (caster.factionType == ActorController.EFactionType.EBlue)
			{
				GameUIManager.mInstance.HUDTextMgr.RequestShow(this, EShowType.EST_Damage, num, null, 0);
			}
			else
			{
				GameUIManager.mInstance.HUDTextMgr.RequestShow(this, EShowType.EST_Damage, -num, null, 2);
			}
		}
	}

	private void DoSuckHeal(long value, ActorController caster, bool effectPlayer)
	{
		int num = this.DoHeal(value, caster);
		if (num > 0)
		{
			if (this.uiText != null && this.factionType == ActorController.EFactionType.EBlue)
			{
				GameUIManager.mInstance.HUDTextMgr.RequestShow(this, EShowType.EST_Heal, num, null, 0);
			}
			if (effectPlayer)
			{
				ActorController actorController;
				if (this.factionType == ActorController.EFactionType.EBlue)
				{
					actorController = Globals.Instance.ActorMgr.GetActor(0);
				}
				else
				{
					actorController = Globals.Instance.ActorMgr.GetRemotePlayerActor();
				}
				if (actorController != null && !actorController.IsDead && actorController.IsFriendlyTo(this))
				{
					num = actorController.DoHeal(value, caster);
					if (num > 0 && actorController.uiText != null && actorController.FactionType == ActorController.EFactionType.EBlue)
					{
						GameUIManager.mInstance.HUDTextMgr.RequestShow(actorController, EShowType.EST_Heal, num, null, 0);
					}
				}
			}
		}
	}

	public void DoEffectOnTarget(SkillInfo info, ActorController target, int index)
	{
		if (target == null || target.IsDead || info == null)
		{
			return;
		}
		if (this.IsHostileTo(target) && target.IsImmunity)
		{
			if (target.uiText != null)
			{
				if (target.factionType == ActorController.EFactionType.EBlue)
				{
					GameUIManager.mInstance.HUDTextMgr.RequestShow(target, EShowType.EST_Text, 0, ConstString.ImmunityTxt, 2);
				}
				else
				{
					GameUIManager.mInstance.HUDTextMgr.RequestShow(target, EShowType.EST_Text, 0, ConstString.ImmunityTxt, 0);
				}
			}
			return;
		}
		if (!info.AlwaysHit && !this.CanHit(target))
		{
			if (target.uiText != null)
			{
				if (target.factionType == ActorController.EFactionType.EBlue)
				{
					GameUIManager.mInstance.HUDTextMgr.RequestShow(target, EShowType.EST_Text, 0, ConstString.MissTxt, 2);
				}
				else
				{
					GameUIManager.mInstance.HUDTextMgr.RequestShow(target, EShowType.EST_Text, 0, ConstString.MissTxt, 0);
				}
			}
			return;
		}
		long num = 0L;
		int num2 = 0;
		int num3 = 0;
		long num4 = 0L;
		int num5 = 10000;
		for (int i = 0; i < info.EffectType.Count; i++)
		{
			if (info.EffectType[i] != 0)
			{
				if (info.Rate[i] >= 10000 || UtilFunc.RangeRandom(0, 10000) <= info.Rate[i])
				{
					switch (info.EffectType[i])
					{
					case 1:
						this.HandleTriggerSkill(info, i, target);
                        continue;
					case 2:
						this.HandleTriggerBuff(info, i, target);
                        continue;
					case 4:
						num += (long)this.CalculateDamage(info.Value1[i], info.Value2[i], target);
                        continue;
					case 5:
						num += (long)this.CalculateDamage(info.Value1[i], info.Value2[i], target);
                        continue;
					case 6:
						num4 += (long)this.CalculateHeal(info.Value1[i], info.Value2[i], target);
                        continue;
					case 7:
						this.HandleSummon(info, i, target.transform.position);
                        continue;
					case 8:
						this.HandleSummonMaelstrom(info, i, target.transform.position);
                        continue;
					case 9:
						this.HandleSummonMonster(info, i, target.transform.position);
                        continue;
					case 10:
					{
						int value = 0;
						int value2 = info.Value4[i];
						switch (index)
						{
						case 0:
							value = info.Value1[i];
							break;
						case 1:
							value = info.Value2[i];
							break;
						case 2:
							value = info.Value3[i];
							break;
						case 3:
							value = info.Value3[i];
							break;
						}
						num += (long)this.CalculateDamage(value, value2, target);
                        continue;

					}
					case 11:
					{
						num += (long)this.CalculateDamage(info.Value1[i], info.Value4[i], target);
						int num6 = info.Value2[i] + info.Value3[i] * index;
						if (num6 > 0)
						{
							num5 = num6;
						}
                        continue;
					}
					case 12:
					{
						long num7 = 0L;
						if (this.factionType == ActorController.EFactionType.EBlue)
						{
							if (Globals.Instance.ActorMgr.LopetActor != null)
							{
								num7 = (long)Globals.Instance.ActorMgr.LopetActor.GetAtt(EAttID.EAID_Attack);
							}
						}
						else if (Globals.Instance.ActorMgr.RemoteLopetActor != null)
						{
							num7 = (long)Globals.Instance.ActorMgr.RemoteLopetActor.GetAtt(EAttID.EAID_Attack);
						}
						num += num7 * (long)info.Value1[i] + (long)info.Value2[i];
                        continue;
					}
					}
					global::Debug.LogErrorFormat("SkillID = {0}, invalid EffectType = {1}", new object[]
					{
						info.ID,
						info.EffectType[i]
					});
				}
			}
            continue;
		}
		if (target.hitEffect == 0)
		{
			Singleton<ActionMgr>.Instance.PlayHitAction(this, info, target);
		}
		else
		{
			Singleton<ActionMgr>.Instance.PlayAction(target, "Skill/hit_26", this);
		}
		float num8 = info.ThreatBase;
		if (num > 0L)
		{
			bool flag = this.CanCrit(target);
			if (flag)
			{
				num = num * 150L / 100L;
			}
			num = num * (long)(10000 + this.damageMod) / 10000L;
			num = num * (long)num5 / 10000L;
			if (this.talentEnhanceDamage != null)
			{
				int num9 = (int)(target.CurHP * 10000L / target.MaxHP);
				if (num9 <= this.talentEnhanceDamage.Value2)
				{
					num = num * (long)(10000 + this.talentEnhanceDamage.Value1) / 10000L;
				}
			}
			if (!flag && info.TriggerDoubleDamage && this.doubleDamage != null && Time.time >= this.doubleDamage.Cooldown && UtilFunc.RangeRandom(0, 10000) < this.doubleDamage.Value1)
			{
				num = num * (long)this.doubleDamage.Value2 / 10000L;
				this.doubleDamage.Cooldown = Time.time + (float)this.doubleDamage.Value3;
			}
			if (target.shareDamage > 0)
			{
				num = Globals.Instance.ActorMgr.DoShareDamage(target, num, target.shareDamage);
			}
			num2 = target.DoDamage(num, this, ActorController.CanHitBack(info));
			if (num2 > 0 && target.damageReflect > 0)
			{
				long damage = (long)num2 * (long)target.damageReflect / 10000L;
				this.DoReflectDamage(damage, target);
			}
			if (num2 > 0 && target.reflexDamage != null && Time.time >= target.reflexDamage.Cooldown && UtilFunc.RangeRandom(0, 10000) < target.reflexDamage.Value1)
			{
				long num10 = (long)num2 * (long)target.reflexDamage.Value2 / 10000L;
				if (num10 > 0L)
				{
					target.reflexDamage.Cooldown = Time.time + (float)target.reflexDamage.Value3;
					this.DoReflectDamage(num10, target);
				}
			}
			if (target.uiText != null && num2 > 0)
			{
				if (target.factionType == ActorController.EFactionType.ERed)
				{
					if (flag)
					{
						GameUIManager.mInstance.HUDTextMgr.RequestShow(target, EShowType.EST_CriticalDamage, num2, null, 0);
					}
					else if (this.actorType == ActorController.EActorType.EPlayer && this.Skills[0] != null && this.Skills[0].Info != info)
					{
						GameUIManager.mInstance.HUDTextMgr.RequestShow(target, EShowType.EST_SkillDamage, num2, null, 0);
					}
					else
					{
						GameUIManager.mInstance.HUDTextMgr.RequestShow(target, EShowType.EST_Damage, num2, null, 0);
					}
				}
				else
				{
					GameUIManager.mInstance.HUDTextMgr.RequestShow(target, EShowType.EST_Damage, -num2, null, 2);
				}
			}
			num8 += (float)num2 * info.ThreatCoef;
			if (this.suckPct > 0)
			{
				long value3 = (long)num2 * (long)this.suckPct / 10000L;
				this.DoSuckHeal(value3, this, true);
			}
			if (num2 > 0 && this.damageToHeal != null && Time.time >= this.damageToHeal.Cooldown && UtilFunc.RangeRandom(0, 10000) < this.damageToHeal.Value1)
			{
				this.damageToHeal.Cooldown = Time.time + (float)this.damageToHeal.Value3;
				long value4 = (long)num2 * (long)this.damageToHeal.Value2 / 10000L;
				this.DoSuckHeal(value4, this, false);
			}
			if (this.attackToHeal != null && Time.time >= this.attackToHeal.Cooldown && UtilFunc.RangeRandom(0, 10000) < this.attackToHeal.Value1)
			{
				this.attackToHeal.Cooldown = Time.time + (float)this.attackToHeal.Value3;
				long value5 = (long)this.GetAtt(EAttID.EAID_Attack) * (long)this.attackToHeal.Value2 / 10000L;
				this.DoSuckHeal(value5, this, false);
			}
		}
		if (num4 > 0L)
		{
			num4 = num4 * (long)(10000 + this.healMod) / 10000L;
			num3 = target.DoHeal(num4, this);
			if (num3 > 0 && target.uiText != null && this.factionType == ActorController.EFactionType.EBlue)
			{
				GameUIManager.mInstance.HUDTextMgr.RequestShow(target, EShowType.EST_Heal, num3, null, 0);
			}
		}
		if (num8 > 0f && this.IsHostileTo(target))
		{
			target.AiCtrler.ThreatMgr.AddThreat(this, num8);
		}
		if ((this.actorType == ActorController.EActorType.EPlayer || this.actorType == ActorController.EActorType.EPet) && target.AiCtrler.ThreatMgr.TargetActor != null && target.AiCtrler.ThreatMgr.TargetActor.IsBuilding)
		{
			target.AiCtrler.ThreatMgr.Clear();
			target.AiCtrler.SetInitTarget(this);
		}
		if (info.CastType == 0 || info.CastType == 1)
		{
			this.CastTriggerSkill(target);
		}
		this.CountSkillDamageHeal(info, (long)num2, (long)num3);
	}

	private void DoEffectOnPosition(SkillInfo info, Vector3 targetPosition)
	{
		for (int i = 0; i < info.EffectType.Count; i++)
		{
			if (info.EffectType[i] != 0)
			{
				switch (info.EffectType[i])
				{
				case 7:
					this.HandleSummon(info, i, targetPosition);
					break;
				case 8:
					this.HandleSummonMaelstrom(info, i, targetPosition);
					break;
				case 9:
					this.HandleSummonMonster(info, i, targetPosition);
					break;
				default:
					global::Debug.LogErrorFormat("SkillID = {0}, invalid EffectType = {1}", new object[]
					{
						info.ID,
						info.EffectType[i]
					});
					break;
				}
			}
		}
	}

	private int GetAttackType()
	{
		if (this.petInfo != null)
		{
			return this.petInfo.Type;
		}
		if (this.monsterInfo != null)
		{
			return this.monsterInfo.Type;
		}
		return 0;
	}

	public int CalculateDamage(int value1, int value2, ActorController target)
	{
		int attackType = this.GetAttackType();
		long num = 0L;
		if (this.ignoreDefense != null && Time.time >= this.ignoreDefense.Cooldown && UtilFunc.RangeRandom(0, 10000) < this.ignoreDefense.Value1)
		{
			this.ignoreDefense.Cooldown = Time.time + (float)this.ignoreDefense.Value3;
			num = (long)this.GetAtt(EAttID.EAID_Attack);
		}
		else if (attackType == 1)
		{
			num = (long)(this.GetAtt(EAttID.EAID_Attack) - target.GetAtt(EAttID.EAID_PhysicDefense));
		}
		else if (attackType == 3)
		{
			num = (long)(this.GetAtt(EAttID.EAID_Attack) - target.GetAtt(EAttID.EAID_MagicDefense));
		}
		else
		{
			num += (long)(this.GetAtt(EAttID.EAID_Attack) - target.GetAtt(EAttID.EAID_MagicDefense));
			num += (long)(this.GetAtt(EAttID.EAID_Attack) - target.GetAtt(EAttID.EAID_PhysicDefense));
			num /= 2L;
		}
		int num2 = this.GetAtt(EAttID.EAID_DamagePlus) - target.GetAtt(EAttID.EAID_DamageMinus);
		if (num2 < -8000)
		{
			num2 = -8000;
		}
		num = num * (long)(10000 + num2) / 10000L;
		num = num * (long)value1 / 10000L;
		num += (long)value2;
		if (num < 1L)
		{
			num = 1L;
		}
		return (int)num;
	}

	public int CalculateHeal(int value1, int value2, ActorController target)
	{
		long num = (long)this.GetAtt(EAttID.EAID_Attack) * (long)value1 / 10000L + (long)value2;
		if (num < 1L)
		{
			num = 1L;
		}
		return (int)num;
	}

	private bool CanHit(ActorController target)
	{
		if (target == null)
		{
			return false;
		}
		if (this.IsFriendlyTo(target))
		{
			return true;
		}
		int num = 90 + this.GetAtt(EAttID.EAID_Hit) / 100 + (int)((this.Level - target.Level) * 20u / target.Level) - target.GetAtt(EAttID.EAID_Dodge) / 100;
		return num > 0 && UtilFunc.RangeRandom(0, 100) <= num;
	}

	public bool CanCrit(ActorController target)
	{
		if (target == null)
		{
			return false;
		}
		int num = this.GetAtt(EAttID.EAID_Crit) / 100 + (int)((this.Level - target.Level) * 10u / this.Level) - target.GetAtt(EAttID.EAID_CritResis) / 100;
		return num > 0 && UtilFunc.RangeRandom(0, 100) <= num;
	}

	public int DoDamage(long damage, ActorController caster = null, bool hitBack = false)
	{
		if (this.isDead || this.aiCtrler.Win || this.Unattacked)
		{
			return 0;
		}
		if (this.superShield > 0)
		{
			int num = this.DoHeal(damage * (long)this.superShield / 10000L, this);
			if (num > 0 && this.uiText != null && this.factionType == ActorController.EFactionType.EBlue)
			{
				GameUIManager.mInstance.HUDTextMgr.RequestShow(this, EShowType.EST_Heal, num, null, 0);
			}
			return 0;
		}
		damage = damage * (long)(10000 + this.damageTakenMod) / 10000L;
		if (this.IsAbsorb)
		{
			int num2 = 0;
			for (int i = 0; i < this.buffs.Count; i++)
			{
				this.buffs[i].OnDamage((int)damage, out num2);
				damage -= (long)num2;
				if (damage <= 0L)
				{
					return 0;
				}
			}
		}
		if (this.talentReduceDamage != null)
		{
			int num3 = (int)(this.CurHP * 10000L / this.MaxHP);
			if (num3 <= this.talentReduceDamage.Value2)
			{
				if (this.talentReduceDamage.Value1 >= 10000)
				{
					damage = 1L;
				}
				else
				{
					damage = damage * (long)(10000 - this.talentReduceDamage.Value1) / 10000L;
				}
			}
		}
		if (this.reduceDamage != null && Time.time >= this.reduceDamage.Cooldown && UtilFunc.RangeRandom(0, 10000) < this.reduceDamage.Value1)
		{
			if (this.reduceDamage.Value2 >= 10000)
			{
				damage = 1L;
			}
			else
			{
				damage = damage * (long)(10000 - this.reduceDamage.Value2) / 10000L;
			}
			this.reduceDamage.Cooldown = Time.time + (float)this.reduceDamage.Value3;
		}
		int num4 = UtilFunc.RangeRandom(95, 105);
		damage = damage * (long)num4 / 100L;
		long num5 = damage;
		if (this.DamageRecount && this.actorType == ActorController.EActorType.EMonster)
		{
			Globals.Instance.ActorMgr.OnDoDamage(this.monsterInfo, num5);
		}
		if (caster != null && caster.factionType == ActorController.EFactionType.EBlue)
		{
			caster.totalDamage += num5;
			if (num5 > caster.highestDamage)
			{
				caster.highestDamage = num5;
			}
		}
		if (this.factionType == ActorController.EFactionType.EBlue)
		{
			this.damageTaken += num5;
			this.damageTakenCount++;
		}
		if (this.Undead)
		{
			if (this.CurHP - num5 < this.MaxHP / 100L)
			{
				this.CurHP = this.MaxHP / 100L;
			}
			else
			{
				this.CurHP -= num5;
				this.PlayerHitSound();
			}
			return (int)damage;
		}
		this.aiCtrler.OnDamageTaken();
		if (num5 >= this.CurHP)
		{
			if (this.actorType == ActorController.EActorType.EPet)
			{
				if (this.resurrectRate > 0 && Time.time >= this.resurrectCDTimer && UtilFunc.RangeRandom(0, 10000) <= this.resurrectRate)
				{
					this.autoResurrect = true;
					this.resurrectCDTimer = Time.time + this.resurrectCD;
				}
				else
				{
					this.autoResurrect = false;
				}
			}
			this.StopMove();
			this.InterruptSkill(0);
			this.RemoveAllBuff();
			if (this.actorType == ActorController.EActorType.EPet)
			{
				this.CancelPassiveSkill();
			}
			this.aiCtrler.OnDead();
			this.CurHP = 0L;
			this.isDead = true;
			if (this.IsBoss && this.monsterInfo != null && Globals.Instance.TutorialMgr.IsMatineeBoss(this.monsterInfo.ID))
			{
				this.PlayMatinee = true;
				if (this.animationCtrler.AnimCtrl != null)
				{
					this.animationCtrler.AnimCtrl.CrossFade("std");
					this.animationCtrler.AnimCtrl.wrapMode = WrapMode.Loop;
				}
				List<ActorController> list = new List<ActorController>();
				List<ActorController> actors = Globals.Instance.ActorMgr.Actors;
				for (int j = 5; j < actors.Count; j++)
				{
					ActorController actorController = actors[j];
					if (actorController && !(actorController == this) && !actorController.IsDead)
					{
						list.Add(actorController);
					}
				}
				for (int k = 0; k < list.Count; k++)
				{
					list[k].DoDamage(list[k].MaxHP, null, false);
				}
				if (this.feetEffect != null)
				{
					this.feetEffect.SetActive(false);
				}
				Globals.Instance.ActorMgr.OnActorDead(this, caster);
			}
			else
			{
				NJGMapItem nJGMapItem = base.gameObject.GetComponent<NJGMapItem>();
				if (nJGMapItem != null)
				{
					bool showDeath = nJGMapItem.showDeath;
					UnityEngine.Object.DestroyImmediate(nJGMapItem);
					if (showDeath)
					{
						nJGMapItem = Tools.GetSafeComponent<NJGMapItem>(base.gameObject);
						nJGMapItem.type = 1;
					}
				}
				this.PlayerDeadSound();
				this.OnDead(caster);
				this.PlayDeadEffect();
				if (hitBack && !this.IsBox && !this.IsBoss && UtilFunc.RangeRandom(0, 100) < 50)
				{
					GameObject gameObject = new GameObject("deadFly");
					gameObject.transform.localPosition = base.transform.position;
					gameObject.transform.localRotation = base.transform.rotation;
					BeatBackAction beatBackAction = gameObject.AddComponent<BeatBackAction>();
					beatBackAction.initSpeed = 12f;
					gameObject.SendMessage("OnInit", new SkillVariables
					{
						skillCaster = this,
						skillTarget = caster
					}, SendMessageOptions.DontRequireReceiver);
					UnityEngine.Object.Destroy(gameObject, 3f);
				}
			}
		}
		else
		{
			this.CurHP -= num5;
			this.PlayerHitSound();
			if (this.talentHeal != null && Time.time >= this.talentHealCD)
			{
				int num6 = (int)(this.CurHP * 10000L / this.MaxHP);
				if (num6 <= this.talentHeal.Value2)
				{
					this.AddBuff(this.talentHeal.Value1, this);
					this.talentHealCD = (float)this.talentHeal.Value3 + Time.time;
				}
			}
			if (this.talentImmuneControlled != null && Time.time >= this.talentImmuneCD)
			{
				int num7 = (int)(this.CurHP * 10000L / this.MaxHP);
				if (num7 <= this.talentImmuneControlled.Value2)
				{
					this.AddBuff(this.talentImmuneControlled.Value1, this);
					this.talentImmuneCD = (float)this.talentImmuneControlled.Value3 + Time.time;
				}
			}
		}
		return (int)damage;
	}

	public int DoHeal(long heal, ActorController caster = null)
	{
		if (this.Unhealed)
		{
			return 0;
		}
		heal = heal * (long)(10000 + this.healTakenMod) / 10000L;
		int num = UtilFunc.RangeRandom(95, 105);
		heal = heal * (long)num / 100L;
		long num2 = heal;
		if (caster != null && caster.factionType == ActorController.EFactionType.EBlue)
		{
			caster.totalHeal += num2;
			if (num2 > caster.highestHeal)
			{
				caster.highestHeal = num2;
			}
		}
		if (this.factionType == ActorController.EFactionType.EBlue)
		{
			this.healTaken += num2;
			this.healTakenCount++;
		}
		if (this.CurHP + num2 > this.MaxHP)
		{
			this.CurHP = this.MaxHP;
		}
		else
		{
			this.CurHP += num2;
		}
		return (int)num2;
	}

	private void PlayDeadEffect()
	{
		if (this.animationCtrler != null && ActorController.deadAnimation != null)
		{
			this.animationCtrler.PlayAnimation(ActorController.deadAnimation);
		}
		if (this.ActorType == ActorController.EActorType.EPlayer)
		{
			return;
		}
		PlayActorControllerDead component = base.gameObject.GetComponent<PlayActorControllerDead>();
		if (component != null)
		{
			float num = component.StartDeadEffect();
			UnityEngine.Object.Destroy(base.gameObject, num + 0.1f);
		}
		else if (this.CorpseDecayTime > 0f)
		{
			base.Invoke("DestroyActor", this.CorpseDecayTime);
		}
	}

	private void PlayerHitSound()
	{
		switch (this.ActorType)
		{
		case ActorController.EActorType.EPlayer:
		{
			int num = UtilFunc.RangeRandom(0, 100) % ConstSound.PlayerHitSounds.Length;
			this.PlaySound(ConstSound.PlayerHitSounds[num], 0.75f, false, true);
			break;
		}
		case ActorController.EActorType.EPet:
			this.PlaySound(this.petInfo.HitSound, 0.5f, false, false);
			break;
		case ActorController.EActorType.EMonster:
			this.PlaySound(this.monsterInfo.HitSound, 0.4f, false, false);
			break;
		default:
			global::Debug.LogError(new object[]
			{
				"ActorType unknown!"
			});
			break;
		}
	}

	private void PlayerDeadSound()
	{
		switch (this.ActorType)
		{
		case ActorController.EActorType.EPlayer:
			this.PlaySound(ConstSound.PlayerDeadSound, 1f, true, true);
			break;
		case ActorController.EActorType.EPet:
			this.PlaySound(this.petInfo.DeadSound, 1f, true, false);
			break;
		case ActorController.EActorType.EMonster:
			this.PlaySound(this.monsterInfo.DeadSound, 1f, true, false);
			break;
		default:
			global::Debug.LogError(new object[]
			{
				"ActorType unknown!"
			});
			break;
		}
	}

	public void PlaySound(string sound, float volume, bool force, bool gender = false)
	{
		if (!force && this.soundTimer > 0f)
		{
			return;
		}
		if (!Globals.Instance.EffectSoundMgr.pause && EffectSoundManager.IsEffectSoundOptionOn())
		{
			if (string.IsNullOrEmpty(sound))
			{
				return;
			}
			if (gender && this.ActorType == ActorController.EActorType.EPlayer && this.playerGender == 0)
			{
				sound += "m";
			}
			this.soundTimer = Globals.Instance.EffectSoundMgr.Play(sound, volume, base.transform.position);
		}
	}

	private void HandleTriggerSkill(SkillInfo info, int index, ActorController target)
	{
		SkillInfo info2 = Globals.Instance.AttDB.SkillDict.GetInfo(info.Value3[index]);
		if (info2 == null)
		{
			return;
		}
		switch (info.Value4[index])
		{
		case 0:
			this.TryCastSkill(info2, target);
			break;
		case 1:
			this.TryCastSkill(info2, target.transform.position);
			break;
		case 2:
			this.TryCastSkill(info2, base.transform.position);
			break;
		}
	}

	private void HandleTriggerBuff(SkillInfo info, int index, ActorController target)
	{
		target.AddBuff(info.Value3[index], this);
	}

	private void HandleSummon(SkillInfo info, int index, Vector3 targetPosition)
	{
		AreaEffectInfo info2 = Globals.Instance.AttDB.AreaEffectDict.GetInfo(info.Value3[index]);
		if (info2 == null)
		{
			global::Debug.LogErrorFormat("AreaEffectDict.GetInfo error, id = {0}", new object[]
			{
				info.Value3[index]
			});
			return;
		}
		Quaternion quaternion = base.transform.rotation;
		if (info.Value2[index] > 0)
		{
			quaternion = Quaternion.Euler(0f, this.SkillValue, 0f);
		}
		bool pool = true;
		Transform transform;
		if (!string.IsNullOrEmpty(info2.ResLoc))
		{
			transform = PoolMgr.Spawn(info2.ResLoc, targetPosition, quaternion);
			if (transform == null)
			{
				return;
			}
		}
		else
		{
			transform = new GameObject("ae")
			{
				transform = 
				{
					position = targetPosition,
					rotation = quaternion
				}
			}.transform;
			pool = false;
		}
		ParticleSystem[] componentsInChildren = transform.gameObject.GetComponentsInChildren<ParticleSystem>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Simulate(0f, false, true);
			componentsInChildren[i].Play();
		}
		transform.gameObject.layer = LayerDefine.CharLayer;
		AreaEffect safeComponent = Tools.GetSafeComponent<AreaEffect>(transform.gameObject);
		safeComponent.caster = this;
		safeComponent.skillInfo = Globals.Instance.AttDB.SkillDict.GetInfo(info2.TriggerSkillID);
		safeComponent.TickInterval = info2.TickInterval;
		safeComponent.TickCount = info2.TickCount;
		safeComponent.Pool = pool;
		safeComponent.MaxDuration = info2.MaxDuration;
		safeComponent.OrignalSkillIndex = this.lockSkillIndex;
		if (info.Value4[index] == 1)
		{
			safeComponent.FollowCaster = true;
		}
		else if (info.Value4[index] == 2)
		{
			safeComponent.MoveForwardSpeed = ((info.Value1[index] <= 10000) ? ((float)info.Value1[index]) : ((float)info.Value1[index] / 10000f));
			safeComponent.tickTimer = safeComponent.TickInterval;
		}
	}

	private void HandleSummonMaelstrom(SkillInfo info, int index, Vector3 targetPosition)
	{
		AreaEffectInfo info2 = Globals.Instance.AttDB.AreaEffectDict.GetInfo(info.Value3[index]);
		if (info2 == null)
		{
			global::Debug.LogErrorFormat("AreaEffectDict.GetInfo error, id = {0}", new object[]
			{
				info.Value3[index]
			});
			return;
		}
		SkillInfo info3 = Globals.Instance.AttDB.SkillDict.GetInfo(info2.TriggerSkillID);
		if (info3 == null)
		{
			global::Debug.LogErrorFormat("SkillDict.GetInfo error, id = {0}", new object[]
			{
				info2.TriggerSkillID
			});
			return;
		}
		Transform transform = PoolMgr.Spawn(info2.ResLoc, targetPosition, Quaternion.identity);
		if (transform == null)
		{
			return;
		}
		ParticleSystem[] componentsInChildren = transform.gameObject.GetComponentsInChildren<ParticleSystem>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Simulate(0f, false, true);
			componentsInChildren[i].Play();
		}
		transform.gameObject.layer = LayerDefine.CharLayer;
		AreaEffect safeComponent = Tools.GetSafeComponent<AreaEffect>(transform.gameObject);
		safeComponent.caster = this;
		safeComponent.skillInfo = info3;
		safeComponent.TickInterval = info2.TickInterval;
		safeComponent.TickCount = info2.TickCount;
		safeComponent.Pool = true;
		safeComponent.OrignalSkillIndex = this.lockSkillIndex;
		MaelstromEffect safeComponent2 = Tools.GetSafeComponent<MaelstromEffect>(transform.gameObject);
		safeComponent2.caster = this;
		safeComponent2.radius = info3.Radius;
		safeComponent2.BuffID = info.Value4[index];
		safeComponent2.MaxDuration = info2.TickInterval * (float)info2.TickCount;
		safeComponent2.Speed = (float)info.Value1[index];
		safeComponent2.DelayTime = info.Value2[index];
	}

	private void HandleSummonMonster(SkillInfo info, int i, Vector3 targetPosition)
	{
		Vector3 position = base.transform.position;
		NavMeshHit navMeshHit;
		if (NavMesh.Raycast(position, targetPosition, out navMeshHit, -1))
		{
			targetPosition = navMeshHit.position;
		}
		for (int j = 0; j < info.Value4[i]; j++)
		{
			for (int k = 0; k < 3; k++)
			{
				Vector3 vector = targetPosition;
				float f = UtilFunc.RangeRandom(0f, 6.28318548f);
				vector.x += 1f * Mathf.Sin(f);
				vector.z += 1f * Mathf.Cos(f);
				if (NavMesh.SamplePosition(vector, out navMeshHit, 0.5f, -1))
				{
					vector = navMeshHit.position;
					ActorController item = Globals.Instance.ActorMgr.SummonMonster(info.Value3[i], vector, base.transform.rotation);
					this.summons.Add(item);
					break;
				}
			}
		}
	}

	public bool HasBuff(int buffID)
	{
		for (int i = 0; i < this.buffs.Count; i++)
		{
			if (this.buffs[i].Info.ID == buffID)
			{
				return true;
			}
		}
		return false;
	}

	public void AddBuff(int buffID, ActorController caster)
	{
		if (this.Unattacked || this.Unhealed)
		{
			return;
		}
		BuffInfo info = Globals.Instance.AttDB.BuffDict.GetInfo(buffID);
		if (info == null)
		{
			global::Debug.LogErrorFormat("Get BuffInfo Error ID = {0}", new object[]
			{
				buffID
			});
			return;
		}
		if (this.IsDead)
		{
			return;
		}
		if (this.IsBuffResist(info))
		{
			return;
		}
		for (int i = 0; i < this.buffs.Count; i++)
		{
			if (!this.buffs[i].Deleted)
			{
				if (this.buffs[i].Info.ID == buffID)
				{
					int num;
					if (this.buffs[i].Caster == caster)
					{
						num = info.SSrcStackType;
					}
					else
					{
						num = info.DSrcStackType;
					}
					switch (num)
					{
					case 0:
					{
						this.buffs[i].Duration = info.MaxDuration;
						bool flag = false;
						if (!this.buffs[i].Periodic && this.buffs[i].StackCount != info.InitStackCount)
						{
							this.buffs[i].ApplyEffect(false, false);
							flag = true;
						}
						this.buffs[i].StackCount = info.InitStackCount;
						Globals.Instance.ActorMgr.OnBuffUpdate(this.buffs[i].Owner, this.buffs[i].SerialID, this.buffs[i].Info, this.buffs[i].Duration, this.buffs[i].StackCount);
						if (flag)
						{
							this.buffs[i].ApplyEffect(true, false);
						}
						this.buffs[i].OnAdd();
						return;
					}
					case 1:
					{
						this.buffs[i].Duration = info.MaxDuration;
						int num2 = this.buffs[i].StackCount + info.InitStackCount;
						if (num2 > info.MaxStackCount)
						{
							num2 = info.MaxStackCount;
						}
						bool flag2 = false;
						if (!this.buffs[i].Periodic && this.buffs[i].StackCount != num2)
						{
							this.buffs[i].ApplyEffect(false, false);
							flag2 = true;
						}
						this.buffs[i].StackCount = num2;
						Globals.Instance.ActorMgr.OnBuffUpdate(this.buffs[i].Owner, this.buffs[i].SerialID, this.buffs[i].Info, this.buffs[i].Duration, this.buffs[i].StackCount);
						if (flag2)
						{
							this.buffs[i].ApplyEffect(true, false);
						}
						this.buffs[i].OnAdd();
						return;
					}
					case 2:
						break;
					case 3:
						return;
					default:
						global::Debug.LogErrorFormat("BuffID = {0}, invalid StackType = {1}", new object[]
						{
							info.ID,
							num
						});
						break;
					}
				}
			}
		}
		if (info.ReplaceType != 0)
		{
			for (int j = 0; j < this.buffs.Count; j++)
			{
				if (!this.buffs[j].Deleted)
				{
					if (this.buffs[j].Info.ReplaceType == info.ReplaceType)
					{
						if (this.buffs[j].Info.Level < info.Level)
						{
							return;
						}
						break;
					}
				}
			}
		}
		Buff buff = new Buff();
		buff.Init(this, caster, info, ++this.buffSerialID);
		this.buffs.Add(buff);
		buff.OnAdd();
		Globals.Instance.ActorMgr.OnBuffAdd(buff.Owner, buff.SerialID, buff.Info, buff.Duration, buff.StackCount);
		if (!buff.Periodic)
		{
			buff.ApplyEffect(true, false);
		}
	}

	public void RemoveBuff(Buff data)
	{
		if (data.Deleted)
		{
			return;
		}
		data.Deleted = true;
		if (!data.Periodic)
		{
			data.ApplyEffect(false, false);
		}
		Globals.Instance.ActorMgr.OnBuffRemove(data.Owner, data.SerialID);
		data.OnRemove();
	}

	public void RemoveAllBuff()
	{
		for (int i = 0; i < this.buffs.Count; i++)
		{
			this.RemoveBuff(this.buffs[i]);
		}
	}

	public void RemoveBuff(int buffID, ActorController caster)
	{
		for (int i = 0; i < this.buffs.Count; i++)
		{
			if (this.buffs[i].Caster == caster && this.buffs[i].Info.ID == buffID)
			{
				this.RemoveBuff(this.buffs[i]);
				break;
			}
		}
	}

	private bool IsBuffResist(BuffInfo info)
	{
        if (info == null)
        {
            return true;
        }
        for (int i = 0; i < info.EffectType.Count; i++)
        {
            if (info.EffectType[i] == 0)
            {
                continue;
            }
            int num2 = 0;
            EBuffEffectType type = (EBuffEffectType)info.EffectType[i];
            switch (type)
            {
                case EBuffEffectType.EBET_Fear:
                    if (!this.immuneControl)
                    {
                        goto Label_00D0;
                    }
                    return true;

                case EBuffEffectType.EBET_HitBack:
                    if (!this.immuneControl)
                    {
                        goto Label_00EB;
                    }
                    return true;

                case EBuffEffectType.EBET_HitDown:
                    if (!this.immuneControl)
                    {
                        goto Label_0106;
                    }
                    return true;

                case EBuffEffectType.EBET_ChangeFaction:
                    goto Label_00A8;

                default:
                    switch (type)
                    {
                        case EBuffEffectType.EBET_Root:
                            if (!this.immuneControl)
                            {
                                break;
                            }
                            return true;

                        case EBuffEffectType.EBET_Silence:
                            if (!this.immuneControl)
                            {
                                break;
                            }
                            return true;

                        case EBuffEffectType.EBET_Stun:
                            goto Label_00A8;

                        default:
                            goto Label_0114;
                    }
                    num2 = this.resist[2];
                    goto Label_0114;
            }
            num2 = this.resist[6];
            goto Label_0114;
        Label_00A8:
            if (this.immuneControl)
            {
                return true;
            }
            num2 = this.resist[1];
            goto Label_0114;
        Label_00D0:
            num2 = this.resist[3];
            goto Label_0114;
        Label_00EB:
            num2 = this.resist[4];
            goto Label_0114;
        Label_0106:
            num2 = this.resist[5];
        Label_0114:
            if (UtilFunc.RangeRandom(0, 0x2710) <= num2)
            {
                return true;
            }
        }
        return false;
	}

	private void OnDead(ActorController killer)
	{
		if (this.feetEffect != null)
		{
			this.feetEffect.SetActive(false);
		}
		if (this.ActorType == ActorController.EActorType.EMonster && this.monsterInfo.LootMoney > 0u && this.boxExplode != null)
		{
			Tools.AddChild(base.gameObject, this.boxExplode);
		}
		for (int i = 0; i < this.summons.Count; i++)
		{
			if (this.summons[i] != null && !this.summons[i].IsDead)
			{
				this.summons[i].DoDamage(this.summons[i].MaxHP, null, false);
			}
		}
		Globals.Instance.ActorMgr.OnActorDead(this, killer);
	}

	public void Fadeout(float duration)
	{
		for (int i = 0; i < this.meshInfos.Count; i++)
		{
			CharacterMeshInfo characterMeshInfo = this.meshInfos[i];
			characterMeshInfo.StartFadeout(duration);
		}
	}

	public void DestroyActor()
	{
		bool flag = this.autoResurrect;
		bool flag2 = false;
		int num = 100;
		if (this.actorType == ActorController.EActorType.EPet && !flag && this.talentResurrect != null && Time.time >= this.talentResurrectCD && UtilFunc.RangeRandom(0, 10000) <= this.talentResurrect.Value1)
		{
			flag = true;
			flag2 = true;
			num = this.talentResurrect.Value2;
			this.talentResurrectCD = Time.time + (float)this.talentResurrect.Value3;
		}
		if (flag)
		{
			this.Resurrect(true);
			if (!Globals.Instance.ActorMgr.IsPvpScene())
			{
				ActorController actor = Globals.Instance.ActorMgr.GetActor(0);
				if (actor != null)
				{
					this.AiCtrler.Follow(actor, this.AiCtrler.FollowSlot);
					this.AiCtrler.SetFindEnemyTimer(2f);
				}
			}
			this.CastPassiveSkill();
			this.AiCtrler.Locked = false;
			if (flag2)
			{
				this.CurHP = this.MaxHP * (long)num / 10000L;
				if (this.CurHP > this.MaxHP)
				{
					this.CurHP = this.MaxHP;
				}
			}
			this.PlayAction("Skill/misc_004", null);
		}
		else
		{
			this.Fadeout(0.6f);
			UnityEngine.Object.Destroy(base.gameObject, 1f);
		}
	}

	public void AddHP(uint value)
	{
		if (this.CurHP + (long)((ulong)value) > this.MaxHP)
		{
			this.CurHP = this.MaxHP;
		}
		else
		{
			this.CurHP += (long)((ulong)value);
		}
		this.PlayAction("Skill/misc_001", null);
	}

	public void AddMP(uint value)
	{
		if (this.CurMP + (long)((ulong)value) > this.MaxMP)
		{
			this.CurMP = this.MaxMP;
		}
		else
		{
			this.CurMP += (long)((ulong)value);
		}
		this.PlayAction("Skill/misc_002", null);
	}

	public void PlayAction(string actionName, ActorController target = null)
	{
		Singleton<ActionMgr>.Instance.PlayAction(this, actionName, target);
	}

	public string GetName()
	{
		switch (this.actorType)
		{
		case ActorController.EActorType.EPlayer:
			return this.playerName;
		case ActorController.EActorType.EPet:
			return Tools.GetPetName(this.petInfo);
		case ActorController.EActorType.EMonster:
			return this.monsterInfo.Name;
		default:
			return string.Empty;
		}
	}

	public void Resurrect(bool flag = true)
	{
		this.CurHP = this.MaxHP;
		this.CurMP = this.MaxMP;
		this.isDead = false;
		NJGMapItem safeComponent = Tools.GetSafeComponent<NJGMapItem>(base.gameObject);
		if (this.actorType == ActorController.EActorType.EPlayer)
		{
			safeComponent.type = 5;
			for (int i = 0; i < this.Skills.Length; i++)
			{
				if (this.Skills[i] != null)
				{
					this.Skills[i].Cooldown = 0f;
					this.Skills[i].Count = 0;
					this.Skills[i].Damage = 0L;
					this.Skills[i].Heal = 0L;
				}
			}
			if (flag)
			{
				this.PlayAction("Skill/misc_004", null);
			}
		}
		else
		{
			safeComponent.type = 2;
		}
		this.totalDamage = 0L;
		this.highestDamage = 0L;
		this.totalHeal = 0L;
		this.highestHeal = 0L;
		this.damageTaken = 0L;
		this.damageTakenCount = 0;
		this.healTaken = 0L;
		this.healTakenCount = 0;
		this.aiCtrler.LeaveCombat();
	}

	public void CastPassiveSkill()
	{
		for (int i = 0; i < this.Skills.Length; i++)
		{
			if (this.Skills[i] != null && this.Skills[i].Info.CastType == 2)
			{
				this.OnSkillCast(this.Skills[i].Info, this, base.transform.position, 0);
			}
		}
	}

	private void CancelPassiveSkill()
	{
		for (int i = 0; i < this.Skills.Length; i++)
		{
			if (this.Skills[i] != null && this.Skills[i].Info.CastType == 2 && this.Skills[i].Info.EffectTargetType == 3)
			{
				for (int j = 0; j < this.Skills[i].Info.EffectType.Count; j++)
				{
					if (this.Skills[i].Info.EffectType[j] == 2)
					{
						Globals.Instance.ActorMgr.RemoveBuff(this.Skills[i].Info.Value3[j], this);
					}
				}
			}
		}
	}

	private bool CanTriggerSkill(int index, ActorController target)
	{
		if (index < 0 || index >= this.Skills.Length)
		{
			return false;
		}
		if (this.Skills[index] == null || this.Skills[index].Info == null || this.Skills[index].Info.CastType != 3 || !this.Skills[index].IsCooldown)
		{
			return false;
		}
		this.Skills[index].Rate += this.Skills[index].Info.CastRate;
		if (this.Skills[index].Rate < 10000 && UtilFunc.RangeRandom(0, 10000) > this.Skills[index].Rate)
		{
			return false;
		}
		switch (this.Skills[index].Info.CastTargetType)
		{
		case 0:
			break;
		case 1:
			if (target == null || target.IsDead || !this.IsHostileTo(target))
			{
				return false;
			}
			break;
		case 2:
			if (target == null || target.IsDead || !this.IsFriendlyTo(target))
			{
				return false;
			}
			break;
		case 3:
			break;
		default:
			global::Debug.LogErrorFormat("SkillID = {0}, invalid CastTargetType = {1}", new object[]
			{
				this.Skills[index].Info.ID,
				this.Skills[index].Info.CastTargetType
			});
			return false;
		}
		return true;
	}

	private void CastTriggerSkill(ActorController target)
	{
		if (this.isTrigger)
		{
			return;
		}
		for (int i = 0; i < this.Skills.Length; i++)
		{
			if (this.CanTriggerSkill(i, target))
			{
				this.Skills[i].Cooldown = this.Skills[i].Info.CoolDown;
				this.Skills[i].Rate = 0;
				this.isTrigger = true;
				this.triggerInfo = this.Skills[i].Info;
				this.triggerTarget = target;
				break;
			}
		}
	}

	public void InitMonsterSkillCD()
	{
		if (this.actorType == ActorController.EActorType.EMonster && this.monsterInfo != null)
		{
			for (int i = 0; i < this.monsterInfo.SkillInitCD.Count; i++)
			{
				if (this.Skills[i] != null && i != 0)
				{
					this.Skills[i].Cooldown = this.monsterInfo.SkillInitCD[i];
				}
			}
		}
	}

	public static bool CanHitBack(SkillInfo info)
	{
		return info.CastType == 0 && info.EffectTargetType != 7;
	}

	public void AddPoolSocket(Transform trans)
	{
		for (int i = 0; i < this.socketTrans.Count; i++)
		{
			if (this.socketTrans[i] == null)
			{
				this.socketTrans[i] = trans;
				return;
			}
		}
		this.socketTrans.Add(trans);
	}

	public void ClearPoolSocket()
	{
		for (int i = 0; i < this.socketTrans.Count; i++)
		{
			if (this.socketTrans[i] != null)
			{
				if (PoolMgr.spawnPool != null)
				{
					this.socketTrans[i].parent = PoolMgr.spawnPool.transform;
				}
				else
				{
					this.socketTrans[i].parent = null;
				}
				this.socketTrans[i].gameObject.SetActive(false);
			}
		}
	}

	public void ResetAtt(int attID, long attValue)
	{
		if (attID <= 0 || attID >= 11)
		{
			global::Debug.LogErrorFormat("UpdateAtt Invalid attID = {0}", new object[]
			{
				attID
			});
			return;
		}
		this.attInit[attID] = attValue;
		this.UpdateAtt(attID);
	}

	public void ResetMaxHP(long maxHP)
	{
		this.ResetAtt(1, maxHP);
	}

	public void ResetAtt(int attInfoID)
	{
		MonsterInfo info = Globals.Instance.AttDB.MonsterDict.GetInfo(attInfoID);
		if (info == null)
		{
			global::Debug.LogErrorFormat("MonsterDict.GetInfo error, {0}", new object[]
			{
				attInfoID
			});
			return;
		}
		this.ResetAtt(1, (long)info.MaxHP);
		this.ResetAtt(2, (long)info.Attack);
		this.ResetAtt(3, (long)info.PhysicDefense);
		this.ResetAtt(4, (long)info.MagicDefense);
		this.ResetAtt(5, (long)info.Hit);
		this.ResetAtt(6, (long)info.Dodge);
		this.ResetAtt(7, (long)info.Crit);
		this.ResetAtt(8, (long)info.CritResist);
		this.ResetAtt(9, (long)info.DamagePlus);
		this.ResetAtt(10, (long)info.DamageMinus);
		this.CurHP = this.MaxHP;
	}

	public void SetPlayStatus(bool value)
	{
		if (this.isDead)
		{
			return;
		}
		base.enabled = value;
		this.aiCtrler.enabled = value;
		this.animationCtrler.PauseAnimation(!value);
		this.lockMove = !value;
		if (!value)
		{
			this.StopMove();
		}
	}

	public bool IsAllSummonDead()
	{
		for (int i = 0; i < this.summons.Count; i++)
		{
			if (this.summons[i] != null && !this.summons[i].IsDead)
			{
				return false;
			}
		}
		return false;
	}

	public int GetPetInfoID()
	{
		if (this.actorType == ActorController.EActorType.EPlayer)
		{
			return 90000;
		}
		if (this.actorType == ActorController.EActorType.EPet)
		{
			return this.petInfo.ID;
		}
		return 0;
	}

	public void SetFactionType(ActorController.EFactionType type)
	{
		this.factionType = type;
	}

	public void SetRoateable(bool value)
	{
		this.canRotate = value;
	}

	public void AddEP(int value)
	{
		this.CurEP += (long)value;
		if (this.CurEP > (long)GameConst.GetInt32(217))
		{
			this.CurEP = (long)GameConst.GetInt32(217);
		}
	}

	public bool CanSkillReady(int index)
	{
		if (index < 0 || index >= this.Skills.Length)
		{
			return false;
		}
		if (this.Skills[index] == null || this.Skills[index].Info == null)
		{
			return false;
		}
		if (this.Skills[index].Info.CastType != 0)
		{
			return false;
		}
		if (!this.Skills[index].IsCooldown)
		{
			return false;
		}
		if (index == 4)
		{
			if (this.CurEP < (long)GameConst.GetInt32(217))
			{
				return false;
			}
		}
		else if (this.CurMP < (long)this.Skills[index].Info.ManaCost)
		{
			return false;
		}
		return true;
	}
}
