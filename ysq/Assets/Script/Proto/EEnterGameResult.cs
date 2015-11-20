using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "EEnterGameResult")]
	public enum EEnterGameResult
	{
		[ProtoEnum(Name = "EEGR_Success", Value = 0)]
		EEGR_Success,
		[ProtoEnum(Name = "EEGR_PlatformAuthFailed", Value = 1)]
		EEGR_PlatformAuthFailed,
		[ProtoEnum(Name = "EEGR_NoActor", Value = 2)]
		EEGR_NoActor,
		[ProtoEnum(Name = "EEGR_StatusError", Value = 3)]
		EEGR_StatusError,
		[ProtoEnum(Name = "EEGR_GenderInvalid", Value = 4)]
		EEGR_GenderInvalid,
		[ProtoEnum(Name = "EEGR_NewPlayerInfoFailed", Value = 5)]
		EEGR_NewPlayerInfoFailed,
		[ProtoEnum(Name = "EEGR_CreatPlayerFailed", Value = 6)]
		EEGR_CreatPlayerFailed,
		[ProtoEnum(Name = "EEGR_PrivilegeError", Value = 7)]
		EEGR_PrivilegeError,
		[ProtoEnum(Name = "EEGR_PlatformTimedout", Value = 8)]
		EEGR_PlatformTimedout,
		[ProtoEnum(Name = "EEGR_PlatformKeyError", Value = 9)]
		EEGR_PlatformKeyError,
		[ProtoEnum(Name = "EEGR_PlayerLock", Value = 10)]
		EEGR_PlayerLock,
		[ProtoEnum(Name = "EEGR_DBError", Value = 11)]
		EEGR_DBError,
		[ProtoEnum(Name = "EEGR_AccessTokenExpires", Value = 12)]
		EEGR_AccessTokenExpires,
		[ProtoEnum(Name = "EEGR_RefreshTokenFailed", Value = 13)]
		EEGR_RefreshTokenFailed,
		[ProtoEnum(Name = "EEGR_GMKickPlayer", Value = 14)]
		EEGR_GMKickPlayer,
		[ProtoEnum(Name = "EEGR_OtherDeviceLogin", Value = 15)]
		EEGR_OtherDeviceLogin,
		[ProtoEnum(Name = "EEGR_ClientVersionError", Value = 16)]
		EEGR_ClientVersionError,
		[ProtoEnum(Name = "EEGR_CkeckingAccount", Value = 17)]
		EEGR_CkeckingAccount,
		[ProtoEnum(Name = "EEGR_ServerFull", Value = 18)]
		EEGR_ServerFull,
		[ProtoEnum(Name = "EEGR_ServerMaintenance", Value = 19)]
		EEGR_ServerMaintenance,
		[ProtoEnum(Name = "EEGR_BlackUDID", Value = 20)]
		EEGR_BlackUDID
	}
}
