using Att;
using System;

public class PlayerPetInfo
{
	public const int INFO_ID = 90000;

	public const int INFO_ID2 = 90001;

	public const ulong GUID = 100uL;

	public static PetInfo Info;

	public static PetInfo Info2;

	public static void Init()
	{
		PlayerPetInfo.Info = Globals.Instance.AttDB.PetDict.GetInfo(90000);
		if (PlayerPetInfo.Info == null)
		{
			Debug.LogErrorFormat("PetDict.GetInfo error, id = {0}", new object[]
			{
				90000
			});
			return;
		}
		PlayerPetInfo.Info2 = Globals.Instance.AttDB.PetDict.GetInfo(90001);
		if (PlayerPetInfo.Info2 == null)
		{
			Debug.LogErrorFormat("PetDict.GetInfo error, id = {0}", new object[]
			{
				90001
			});
			return;
		}
		for (int i = 0; i < PlayerPetInfo.Info2.RelationID.Count; i++)
		{
			PlayerPetInfo.Info.RelationID.Add(PlayerPetInfo.Info2.RelationID[i]);
		}
		PlayerPetInfo.Info2.RelationID.Clear();
		for (int j = 0; j < PlayerPetInfo.Info.RelationID.Count; j++)
		{
			PlayerPetInfo.Info2.RelationID.Add(PlayerPetInfo.Info.RelationID[j]);
		}
		PlayerPetInfo.Info2.TalentID.Clear();
		for (int k = 0; k < PlayerPetInfo.Info.TalentID.Count; k++)
		{
			PlayerPetInfo.Info2.TalentID.Add(PlayerPetInfo.Info.TalentID[k]);
		}
	}
}
