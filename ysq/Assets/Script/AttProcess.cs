using System;

public class AttProcess
{
	public static void OnPostInitAtt()
	{
		GameConst.Init();
		ConLevelInfo.Init();
		LevelExp.Init();
		PlayerPetInfo.Init();
		Master.Init();
		Achievement.Init();
		PetFragment.Init();
		LopetFragment.Init();
		Awake.Init();
		TinyLevel.Init();
		MiscTable.Init();
		MagicLoveTable.Init();
	}
}
