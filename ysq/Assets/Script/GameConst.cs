using Att;
using System;

public class GameConst
{
	public const float PVE_COMBAT_SPEEDUP = 0.15f;

	public const float PVP_COMBAT_SPEEDUP = 0.2f;

	public const float ATTACK_DISTANCE = 3f;

	public const float FIND_ENEMY_DISTANCE = 6f;

	public const float FORCE_FOLLOW_DISTANCE = 20f;

	public const int MAX_PVE_MONSTER_NUM = 3;

	public const int MAX_SCENE_SCORE_NUM = 3;

	public const int MAX_REWARD_ITEM_NUM = 3;

	public const int MAX_SCENE_REWARD_ITEM_NUM = 4;

	public const int MAX_NAME_LENGTH = 12;

	public const int MAX_CHAT_CACHE_NUM = 50;

	public const int MAX_SYSTEM_CHAT_CACHE_NUM = 20;

	public const int MAX_REWARD_NUM = 4;

	public const int MAX_CHAT_CHAR_NUM = 45;

	public const int MAX_CHAT_CHAR_NUM_LINE = 24;

	public const int MAX_TRIAL_RESPAWN_POINT = 15;

	public const int MAX_TRIAL_SUB_WAVE = 5;

	public const int MIN_PET_EFFECT_QUALITY = 2;

	public const int MIN_GET_PET_QUALITY_SHOW_NEWS = 3;

	public const int ROLL_TEN_TIMES_REWARDS_NUM = 10;

	public const int SOULRELIQUARY_ROLL_ONCE_REWARDS_NUM = 6;

	public const int WOLRD_BOSS_TOTAL_TIME = 30;

	public const int MAX_PVP4_BILLBOARD_RANK = 15000;

	public const int MAX_PVP_RESPAWN_POINT = 7;

	public const int MAX_OPEN_MAP_NUM = 18;

	public const int MAX_AWAKE_OPEN_MAP_NUM = 32;

	public const int Guild_SignIn_JinDu0 = 1;

	public const int Guild_SignIn_JinDu1 = 3;

	public const int Guild_SignIn_JinDu2 = 5;

	public const int PVP_MAXHP_MOD = 10000;

	public const int PVP_MAXMP_MOD = 200;

	public const int MAX_KINGREWARD_QUEST_REWARDS_NUM = 3;

	public const int MAX_KINGREWARD_QUEST_STARS = 5;

	public const int MIN_KINGREWARD_REWARD_QUELITY_LEVEL = 3;

	public const int DART_PIECE_NUM = 6;

	public const int MSG_WINDOW_MAX_TXT_NUM = 30;

	public const int MAX_COSTUMEPARTY_INTERACTION_COUNT = 96;

	public const int COSTUMEPARTY_INTERACTION_COUNT_REWARD_NUM = 3;

	public const int TRIAL_SHOP_TABS_COUNT = 4;

	public const int GUILD_MINES_TARGETS_NUM = 4;

	public const float FARM_COMBAT_MULTIPLE = 1.5f;

	public const int TREE_APPLES_NUM = 5;

	public const int TREE_ONE_ITEM_COST = 1;

	public const int GUARD_TARGET_NUM = 3;

	public const int GUARD_TARGET_LEVEL_NUM = 3;

	public const int GUARD_SUMMON_RESURRECT_TIME = 20;

	public const int GUARD_ACTOR_RESURRECT_TIME = 5;

	public const int MAX_MG_WAY_COUNT = 3;

	public const int MAX_MG_POINT_COUNT = 5;

	public const int MAX_MG_WAVE = 6;

	public const int MAX_TRINKET_RECIPE_ITEM_NUM = 6;

	public const int PILLAGE_CASTING_QUALITY = 4;

	public const int PILLAGE_CASTING_NUM = 4;

	public const int MAX_SHOW_REWARD_NUM = 10;

	public const float Trail_SaoDang_Time = 30f;

	public const int SIGN_IN_ITEM_NUM = 30;

	public const int PeiYang5_Vip = 4;

	public const int PeiYang10_Vip = 5;

	public const int MAX_TRINKETEXPBOX_USE_NUM = 5;

	public const int GuildWar_Kill_Score = 20;

	public const int MAX_LOPET_STARS_NUM = 5;

	public const int MAX_LOPET_SET_PET_NUM = 3;

	public const int MAX_COMBAT_LOPET_NUM = 1;

	public const int MAX_LEVEL_NUM = 150;

	public const int MAX_EQUIP_ENHANCE_LEVEL = 300;

	public const int MAX_EQUIP_REFINE_LEVEL = 80;

	public const int MAX_TRINKET_ENHANCE_LEVEL = 150;

	public const int MAX_TRINKET_REFINE_LEVEL = 30;

	public const int MAX_FURTHER_LEVEL = 15;

	public const int MAX_SKILL_LEVEL = 15;

	public const int MAX_AWAKE_LEVEL = 80;

	public const int MAX_LOPET_LEVEL = 120;

	public const int MAX_MAGIC_LOVE_PET_NUM = 4;

	public const float Max_Voice_Record_Time = 30f;

	public const int MIN_AUTOCOMBAT_LEVEL = 3;

	public const int EQUIP_SHOP_SPECIAL_ITEM_ID = 1000;

	public static readonly int[] PET_EXP_ITEM_ID = new int[4];

	public static readonly int[] EQUIP_REFINE_ITEM_ID = new int[4];

	public static readonly int[] TRINKET_ENHANCE_EXP_ITEM_ID = new int[3];

	public static readonly int[] TRINKET_ENHANCE_EXP = new int[3];

	public static readonly int[] LOW_QUALITY_EQUIP_ID = new int[6];

	public static readonly int[] LOPET_EXP_ITEM_ID = new int[3];

	public static readonly string[] COSTUMEPARTY_SONG_NAME = new string[3];

	private static int[] constValue = new int[280];

	public static void Init()
	{
		Array.Clear(GameConst.constValue, 0, GameConst.constValue.Length);
		foreach (ConstInfo current in Globals.Instance.AttDB.ConstDict.Values)
		{
			if (current.ID < 0 || current.ID >= 280)
			{
				Debug.LogErrorFormat("ConstInfo error, id = {0}", new object[]
				{
					current.ID
				});
			}
			else
			{
				GameConst.constValue[current.ID] = current.IntValue;
			}
		}
		GameConst.PET_EXP_ITEM_ID[0] = GameConst.GetInt32(83);
		GameConst.PET_EXP_ITEM_ID[1] = GameConst.GetInt32(84);
		GameConst.PET_EXP_ITEM_ID[2] = GameConst.GetInt32(85);
		GameConst.PET_EXP_ITEM_ID[3] = GameConst.GetInt32(86);
		GameConst.EQUIP_REFINE_ITEM_ID[0] = GameConst.GetInt32(87);
		GameConst.EQUIP_REFINE_ITEM_ID[1] = GameConst.GetInt32(88);
		GameConst.EQUIP_REFINE_ITEM_ID[2] = GameConst.GetInt32(89);
		GameConst.EQUIP_REFINE_ITEM_ID[3] = GameConst.GetInt32(90);
		GameConst.TRINKET_ENHANCE_EXP_ITEM_ID[0] = GameConst.GetInt32(91);
		GameConst.TRINKET_ENHANCE_EXP_ITEM_ID[1] = GameConst.GetInt32(92);
		GameConst.TRINKET_ENHANCE_EXP_ITEM_ID[2] = GameConst.GetInt32(93);
		ItemInfo info = Globals.Instance.AttDB.ItemDict.GetInfo(GameConst.TRINKET_ENHANCE_EXP_ITEM_ID[0]);
		if (info != null)
		{
			GameConst.TRINKET_ENHANCE_EXP[0] = info.Value1;
		}
		info = Globals.Instance.AttDB.ItemDict.GetInfo(GameConst.TRINKET_ENHANCE_EXP_ITEM_ID[1]);
		if (info != null)
		{
			GameConst.TRINKET_ENHANCE_EXP[1] = info.Value1;
		}
		info = Globals.Instance.AttDB.ItemDict.GetInfo(GameConst.TRINKET_ENHANCE_EXP_ITEM_ID[2]);
		if (info != null)
		{
			GameConst.TRINKET_ENHANCE_EXP[2] = info.Value1;
		}
		GameConst.LOW_QUALITY_EQUIP_ID[0] = GameConst.GetInt32(94);
		GameConst.LOW_QUALITY_EQUIP_ID[1] = GameConst.GetInt32(95);
		GameConst.LOW_QUALITY_EQUIP_ID[2] = GameConst.GetInt32(96);
		GameConst.LOW_QUALITY_EQUIP_ID[3] = GameConst.GetInt32(97);
		GameConst.LOW_QUALITY_EQUIP_ID[4] = GameConst.GetInt32(98);
		GameConst.LOW_QUALITY_EQUIP_ID[5] = GameConst.GetInt32(99);
		GameConst.COSTUMEPARTY_SONG_NAME[0] = "bg/ui_020";
		GameConst.COSTUMEPARTY_SONG_NAME[1] = "bg/bg_110";
		GameConst.COSTUMEPARTY_SONG_NAME[2] = "bg/bg_113";
		GameConst.LOPET_EXP_ITEM_ID[0] = GameConst.GetInt32(202);
		GameConst.LOPET_EXP_ITEM_ID[1] = GameConst.GetInt32(203);
		GameConst.LOPET_EXP_ITEM_ID[2] = GameConst.GetInt32(204);
	}

	public static int GetInt32(int index)
	{
		if (index < 0 || index >= 280)
		{
			return 0;
		}
		return GameConst.constValue[index];
	}
}
