using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MG
{
	[ProtoContract(Name = "CacheData")]
	[Serializable]
	public class CacheData : IExtensible
	{
		private bool _Joystick;

		private bool _EnableAI;

		private readonly List<int> _ShowedDialogID = new List<int>();

		private int _GameSpeed;

		private bool _AutoSkill;

		private int _PayCardStamp;

		private int _VIPRewardStamp;

		private int _VIPWeekRewardStamp;

		private int _SongID;

		private readonly List<int> _GuardLevels = new List<int>();

		private bool _HasNewFashion;

		private bool _HasShowChangePetMark = true;

		private int _HasReadedWorldMsgTimeStamp;

		private int _HasReadedGuildMsgTimeStamp;

		private int _HasReadedSiLiaoMsgTimeStamp;

		private int _HasReadedWuHuiMsgTimeStamp;

		private bool _IsVoiceChat;

		private readonly List<ConfigurableActivityData> _ActivityData = new List<ConfigurableActivityData>();

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Joystick", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool Joystick
		{
			get
			{
				return this._Joystick;
			}
			set
			{
				this._Joystick = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "EnableAI", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool EnableAI
		{
			get
			{
				return this._EnableAI;
			}
			set
			{
				this._EnableAI = value;
			}
		}

		[ProtoMember(3, Name = "ShowedDialogID", DataFormat = DataFormat.TwosComplement)]
		public List<int> ShowedDialogID
		{
			get
			{
				return this._ShowedDialogID;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "GameSpeed", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int GameSpeed
		{
			get
			{
				return this._GameSpeed;
			}
			set
			{
				this._GameSpeed = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "AutoSkill", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool AutoSkill
		{
			get
			{
				return this._AutoSkill;
			}
			set
			{
				this._AutoSkill = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "PayCardStamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int PayCardStamp
		{
			get
			{
				return this._PayCardStamp;
			}
			set
			{
				this._PayCardStamp = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "VIPRewardStamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int VIPRewardStamp
		{
			get
			{
				return this._VIPRewardStamp;
			}
			set
			{
				this._VIPRewardStamp = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "VIPWeekRewardStamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int VIPWeekRewardStamp
		{
			get
			{
				return this._VIPWeekRewardStamp;
			}
			set
			{
				this._VIPWeekRewardStamp = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "SongID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int SongID
		{
			get
			{
				return this._SongID;
			}
			set
			{
				this._SongID = value;
			}
		}

		[ProtoMember(10, Name = "GuardLevels", DataFormat = DataFormat.TwosComplement)]
		public List<int> GuardLevels
		{
			get
			{
				return this._GuardLevels;
			}
		}

		[ProtoMember(11, IsRequired = false, Name = "HasNewFashion", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool HasNewFashion
		{
			get
			{
				return this._HasNewFashion;
			}
			set
			{
				this._HasNewFashion = value;
			}
		}

		[ProtoMember(12, IsRequired = false, Name = "HasShowChangePetMark", DataFormat = DataFormat.Default), DefaultValue(true)]
		public bool HasShowChangePetMark
		{
			get
			{
				return this._HasShowChangePetMark;
			}
			set
			{
				this._HasShowChangePetMark = value;
			}
		}

		[ProtoMember(13, IsRequired = false, Name = "HasReadedWorldMsgTimeStamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int HasReadedWorldMsgTimeStamp
		{
			get
			{
				return this._HasReadedWorldMsgTimeStamp;
			}
			set
			{
				this._HasReadedWorldMsgTimeStamp = value;
			}
		}

		[ProtoMember(14, IsRequired = false, Name = "HasReadedGuildMsgTimeStamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int HasReadedGuildMsgTimeStamp
		{
			get
			{
				return this._HasReadedGuildMsgTimeStamp;
			}
			set
			{
				this._HasReadedGuildMsgTimeStamp = value;
			}
		}

		[ProtoMember(15, IsRequired = false, Name = "HasReadedSiLiaoMsgTimeStamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int HasReadedSiLiaoMsgTimeStamp
		{
			get
			{
				return this._HasReadedSiLiaoMsgTimeStamp;
			}
			set
			{
				this._HasReadedSiLiaoMsgTimeStamp = value;
			}
		}

		[ProtoMember(16, IsRequired = false, Name = "HasReadedWuHuiMsgTimeStamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int HasReadedWuHuiMsgTimeStamp
		{
			get
			{
				return this._HasReadedWuHuiMsgTimeStamp;
			}
			set
			{
				this._HasReadedWuHuiMsgTimeStamp = value;
			}
		}

		[ProtoMember(17, IsRequired = false, Name = "IsVoiceChat", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool IsVoiceChat
		{
			get
			{
				return this._IsVoiceChat;
			}
			set
			{
				this._IsVoiceChat = value;
			}
		}

		[ProtoMember(18, Name = "ActivityData", DataFormat = DataFormat.Default)]
		public List<ConfigurableActivityData> ActivityData
		{
			get
			{
				return this._ActivityData;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
