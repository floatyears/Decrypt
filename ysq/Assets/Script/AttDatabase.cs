using System;
using UnityEngine;

public sealed class AttDatabase : MonoBehaviour
{
	public AchievementInfoDictionary AchievementDict = new AchievementInfoDictionary();

	public AreaEffectInfoDictionary AreaEffectDict = new AreaEffectInfoDictionary();

	public AwakeInfoDictionary AwakeDict = new AwakeInfoDictionary();

	public AwakeRecipeInfoDictionary AwakeRecipeDict = new AwakeRecipeInfoDictionary();

	public BuffInfoDictionary BuffDict = new BuffInfoDictionary();

	public ConstellationInfoDictionary ConstellationDict = new ConstellationInfoDictionary();

	public ConstInfoDictionary ConstDict = new ConstInfoDictionary();

	public CostumePartyInfoDictionary CostumePartyDict = new CostumePartyInfoDictionary();

	public CultivateInfoDictionary CultivateDict = new CultivateInfoDictionary();

	public D2MInfoDictionary D2MDict = new D2MInfoDictionary();

	public DayHotInfoDictionary DayHotDict = new DayHotInfoDictionary();

	public DialogInfoDictionary DialogDict = new DialogInfoDictionary();

	public FashionInfoDictionary FashionDict = new FashionInfoDictionary();

	public FDSInfoDictionary FDSDict = new FDSInfoDictionary();

	public GuildInfoDictionary GuildDict = new GuildInfoDictionary();

	public ItemInfoDictionary ItemDict = new ItemInfoDictionary();

	public ItemSetInfoDictionary ItemSetDict = new ItemSetInfoDictionary();

	public KRQuestInfoDictionary KRQuestDict = new KRQuestInfoDictionary();

	public KRRewardInfoDictionary KRRewardDict = new KRRewardInfoDictionary();

	public LegendInfoDictionary LegendDict = new LegendInfoDictionary();

	public LevelInfoDictionary LevelDict = new LevelInfoDictionary();

	public LopetInfoDictionary LopetDict = new LopetInfoDictionary();

	public LopetShopInfoDictionary LopetShopDict = new LopetShopInfoDictionary();

	public LuckyRollInfoDictionary LuckyRollDict = new LuckyRollInfoDictionary();

	public MagicLoveInfoDictionary MagicLoveDict = new MagicLoveInfoDictionary();

	public MapInfoDictionary MapDict = new MapInfoDictionary();

	public MasterInfoDictionary MasterDict = new MasterInfoDictionary();

	public MGInfoDictionary MGDict = new MGInfoDictionary();

	public MGRespawnInfoDictionary MGRespawnDict = new MGRespawnInfoDictionary();

	public MiscInfoDictionary MiscDict = new MiscInfoDictionary();

	public MonsterInfoDictionary MonsterDict = new MonsterInfoDictionary();

	public OreInfoDictionary OreDict = new OreInfoDictionary();

	public PayInfoDictionary PayDict = new PayInfoDictionary();

	public PetInfoDictionary PetDict = new PetInfoDictionary();

	public PetSceneInfoDictionary PetSceneDict = new PetSceneInfoDictionary();

	public PillageRewardInfoDictionary PillageRewardDict = new PillageRewardInfoDictionary();

	public PvpInfoDictionary PvpDict = new PvpInfoDictionary();

	public QualityInfoDictionary QualityDict = new QualityInfoDictionary();

	public QuestInfoDictionary QuestDict = new QuestInfoDictionary();

	public RecipeInfoDictionary RecipeDict = new RecipeInfoDictionary();

	public RelationInfoDictionary RelationDict = new RelationInfoDictionary();

	public SayInfoDictionary SayDict = new SayInfoDictionary();

	public SceneInfoDictionary SceneDict = new SceneInfoDictionary();

	public SevenDayInfoDictionary SevenDayDict = new SevenDayInfoDictionary();

	public ShareAchievementInfoDictionary ShareAchievementDict = new ShareAchievementInfoDictionary();

	public ShopInfoDictionary ShopDict = new ShopInfoDictionary();

	public SignInInfoDictionary SignInDict = new SignInInfoDictionary();

	public SkillInfoDictionary SkillDict = new SkillInfoDictionary();

	public TalentInfoDictionary TalentDict = new TalentInfoDictionary();

	public TinyLevelInfoDictionary TinyLevelDict = new TinyLevelInfoDictionary();

	public TrialInfoDictionary TrialDict = new TrialInfoDictionary();

	public TrialRespawnInfoDictionary TrialRespawnDict = new TrialRespawnInfoDictionary();

	public VipLevelInfoDictionary VipLevelDict = new VipLevelInfoDictionary();

	public WorldBossInfoDictionary WorldBossDict = new WorldBossInfoDictionary();

	public WorldRespawnInfoDictionary WorldRespawnDict = new WorldRespawnInfoDictionary();

	public void Init()
	{
		this.AchievementDict.LoadFromFile();
		this.AreaEffectDict.LoadFromFile();
		this.AwakeDict.LoadFromFile();
		this.AwakeRecipeDict.LoadFromFile();
		this.BuffDict.LoadFromFile();
		this.ConstellationDict.LoadFromFile();
		this.ConstDict.LoadFromFile();
		this.CostumePartyDict.LoadFromFile();
		this.CultivateDict.LoadFromFile();
		this.D2MDict.LoadFromFile();
		this.DayHotDict.LoadFromFile();
		this.DialogDict.LoadFromFile();
		this.FashionDict.LoadFromFile();
		this.FDSDict.LoadFromFile();
		this.GuildDict.LoadFromFile();
		this.ItemDict.LoadFromFile();
		this.ItemSetDict.LoadFromFile();
		this.KRQuestDict.LoadFromFile();
		this.KRRewardDict.LoadFromFile();
		this.LegendDict.LoadFromFile();
		this.LevelDict.LoadFromFile();
		this.LopetDict.LoadFromFile();
		this.LopetShopDict.LoadFromFile();
		this.LuckyRollDict.LoadFromFile();
		this.MagicLoveDict.LoadFromFile();
		this.MapDict.LoadFromFile();
		this.MasterDict.LoadFromFile();
		this.MGDict.LoadFromFile();
		this.MGRespawnDict.LoadFromFile();
		this.MiscDict.LoadFromFile();
		this.MonsterDict.LoadFromFile();
		this.OreDict.LoadFromFile();
		this.PayDict.LoadFromFile();
		this.PetDict.LoadFromFile();
		this.PetSceneDict.LoadFromFile();
		this.PillageRewardDict.LoadFromFile();
		this.PvpDict.LoadFromFile();
		this.QualityDict.LoadFromFile();
		this.QuestDict.LoadFromFile();
		this.RecipeDict.LoadFromFile();
		this.RelationDict.LoadFromFile();
		this.SayDict.LoadFromFile();
		this.SceneDict.LoadFromFile();
		this.SevenDayDict.LoadFromFile();
		this.ShareAchievementDict.LoadFromFile();
		this.ShopDict.LoadFromFile();
		this.SignInDict.LoadFromFile();
		this.SkillDict.LoadFromFile();
		this.TalentDict.LoadFromFile();
		this.TinyLevelDict.LoadFromFile();
		this.TrialDict.LoadFromFile();
		this.TrialRespawnDict.LoadFromFile();
		this.VipLevelDict.LoadFromFile();
		this.WorldBossDict.LoadFromFile();
		this.WorldRespawnDict.LoadFromFile();
		global::Debug.Log(new object[]
		{
			"AttDatabase Load OK!"
		});
		AttProcess.OnPostInitAtt();
	}
}
