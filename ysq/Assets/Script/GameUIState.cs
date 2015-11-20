using Att;
using Proto;
using System;

public sealed class GameUIState
{
	public ulong SelectPetID;

	public ulong SelectItemID;

	public ulong SelectFriendID;

	public ulong SelectLopetID;

	public SceneInfo PetSceneInfo;

	public int MaxHp;

	public int Attack;

	public int WuFang;

	public int FaFang;

	public int BaseMaxHp;

	public int BaseAttack;

	public int BasePhysicDefense;

	public int BaseMagicDefense;

	public SceneInfo CurSceneInfo;

	public SceneInfo AdventureSceneInfo;

	public SceneInfo ResultSceneInfo;

	public SceneInfo ResultSceneInfo2;

	public SceneInfo QuestSceneInfo;

	public bool ResetWMSceneInfo;

	public MS2C_PveResult PveResult;

	public int PveSceneID;

	public int PveSceneValue;

	public string mShowMsg;

	public uint PlayerLevel;

	public uint PlayerExp;

	public int PlayerEnergy;

	public int PlayerMoney;

	public int LastScore;

	public QuestInfo LastQuestInfo;

	public int TrialStartWave;

	public int WorldBossSlot;

	public bool WorldBossIsOver;

	public bool ArenaIsReplay;

	public bool IsPvp;

	public int ArenaHighestRank;

	public string WorldBossKillerName;

	public string GuildBossKillerName;

	public int Pvp4StartState;

	public ItemInfo PillageItem;

	public float GuildBossHp;

	public GUIRecycleScene.ERecycleT RecycleType;

	public KRQuestInfo KRQuest;

	public KRRewardInfo KRReward;

	public bool MaskTutorial;

	public int UnlockNewGameLevel;

	public int ShowMonthCardHalo;

	public PetInfo CollectionPopUpPet;

	public int CombatPetSlot;

	public bool CacheEquipBag;

	public bool CacheTrinketBag;

	public bool CachePropsBag;

	public bool CachePetBag;

	public bool CacheLopetBag;

	public int TrailCurLvl;

	public int TrailMaxRecord;

	public bool IsLocalPlayer = true;

	public MC2S_TrinketEnhance TrinketEnhanceData;

	public MC2S_PetBreakUp PetBreakUpData;

	public MC2S_EquipBreakUp EquipBreakUpData;

	public MC2S_PetReborn PetRebornData;

	public MC2S_TrinketReborn TrinketRebornData;

	public MC2S_LopetBreakUp LopetBreakData;

	public MC2S_LopetReborn LopetRebornData;

	public MS2C_CommentMsg CommentData;

	private PetDataEx tempPetDataEx;

	private LopetDataEx tempLopetDataEx;

	public int mPetTrainCurPageIndex;

	public int mPetTrainLvlPageIndex;

	public int GuildWarFightSlotIndex;

	public int GuildWarFightHoldIndex;

	public bool IsShowGuildWarDetailInfo = true;

	public int mOldHpNum;

	public int mOldAttackNum;

	public int mOldWufangNum;

	public int mOldFafangNum;

	public bool IsShowPetZhuWei;

	public bool IsShowPetZhuWeiPopUp;

	public int[] mOldRelationFlags = new int[]
	{
		-1,
		-1,
		-1,
		-1
	};

	public int[] mOldRelationPetInfoIds = new int[]
	{
		-1,
		-1,
		-1,
		-1
	};

	public bool IsShowedGuildWarResult;

	public int PropsBagSceneToTrainIndex;

	public bool SDKPlayerManagerNew;

	public PetDataEx mPetTrainCurPetDataEx
	{
		get
		{
			return this.tempPetDataEx;
		}
		set
		{
			this.tempPetDataEx = value;
			if (this.tempLopetDataEx != null)
			{
				this.tempLopetDataEx = null;
			}
		}
	}

	public LopetDataEx mLopetTrainCurLopetDataEx
	{
		get
		{
			return this.tempLopetDataEx;
		}
		set
		{
			this.tempLopetDataEx = value;
			if (this.tempPetDataEx != null)
			{
				this.tempPetDataEx = null;
			}
		}
	}

	public void SetOldFurtherData(PetDataEx data)
	{
		data.GetAttribute(ref this.MaxHp, ref this.Attack, ref this.WuFang, ref this.FaFang);
		data.GetBastAtt(ref this.BaseMaxHp, ref this.BaseAttack, ref this.BasePhysicDefense, ref this.BaseMagicDefense);
	}

	public void SetOldLopetAwakeData(LopetDataEx data)
	{
		data.GetAttribute(ref this.MaxHp, ref this.Attack, ref this.WuFang, ref this.FaFang);
	}

	public void SetOldRelationFlag()
	{
		TeamSubSystem teamSystem = Globals.Instance.Player.TeamSystem;
		if (teamSystem != null)
		{
			for (int i = 0; i < 4; i++)
			{
				SocketDataEx socket = teamSystem.GetSocket(i);
				if (socket != null)
				{
					this.mOldRelationFlags[i] = socket.RelationFlag;
					PetDataEx pet = socket.GetPet();
					if (pet != null)
					{
						this.mOldRelationPetInfoIds[i] = pet.Info.ID;
					}
					else
					{
						this.mOldRelationPetInfoIds[i] = 0;
					}
				}
				else
				{
					this.mOldRelationFlags[i] = 0;
					this.mOldRelationPetInfoIds[i] = 0;
				}
			}
		}
	}
}
