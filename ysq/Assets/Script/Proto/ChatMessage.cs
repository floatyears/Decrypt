using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "ChatMessage")]
	[Serializable]
	public class ChatMessage : IExtensible
	{
		private ulong _PlayerID;

		private string _Name = string.Empty;

		private uint _Level;

		private string _GuildName = string.Empty;

		private int _ConstellationLevel;

		private uint _VipLevel;

		private int _FashionID;

		private int _Gender;

		private string _Message = string.Empty;

		private int _Channel;

		private uint _Type;

		private uint _Value1;

		private int _TimeStamp;

		private bool _Voice;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "PlayerID", DataFormat = DataFormat.TwosComplement), DefaultValue(0f)]
		public ulong PlayerID
		{
			get
			{
				return this._PlayerID;
			}
			set
			{
				this._PlayerID = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "Name", DataFormat = DataFormat.Default), DefaultValue("")]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				this._Name = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "Level", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint Level
		{
			get
			{
				return this._Level;
			}
			set
			{
				this._Level = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "GuildName", DataFormat = DataFormat.Default), DefaultValue("")]
		public string GuildName
		{
			get
			{
				return this._GuildName;
			}
			set
			{
				this._GuildName = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "ConstellationLevel", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ConstellationLevel
		{
			get
			{
				return this._ConstellationLevel;
			}
			set
			{
				this._ConstellationLevel = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "VipLevel", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint VipLevel
		{
			get
			{
				return this._VipLevel;
			}
			set
			{
				this._VipLevel = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "FashionID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int FashionID
		{
			get
			{
				return this._FashionID;
			}
			set
			{
				this._FashionID = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "Gender", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Gender
		{
			get
			{
				return this._Gender;
			}
			set
			{
				this._Gender = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "Message", DataFormat = DataFormat.Default), DefaultValue("")]
		public string Message
		{
			get
			{
				return this._Message;
			}
			set
			{
				this._Message = value;
			}
		}

		[ProtoMember(10, IsRequired = false, Name = "Channel", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Channel
		{
			get
			{
				return this._Channel;
			}
			set
			{
				this._Channel = value;
			}
		}

		[ProtoMember(11, IsRequired = false, Name = "Type", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint Type
		{
			get
			{
				return this._Type;
			}
			set
			{
				this._Type = value;
			}
		}

		[ProtoMember(12, IsRequired = false, Name = "Value1", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint Value1
		{
			get
			{
				return this._Value1;
			}
			set
			{
				this._Value1 = value;
			}
		}

		[ProtoMember(13, IsRequired = false, Name = "TimeStamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int TimeStamp
		{
			get
			{
				return this._TimeStamp;
			}
			set
			{
				this._TimeStamp = value;
			}
		}

		[ProtoMember(14, IsRequired = false, Name = "Voice", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool Voice
		{
			get
			{
				return this._Voice;
			}
			set
			{
				this._Voice = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
