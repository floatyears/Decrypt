using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_SystemEvent")]
	[Serializable]
	public class MS2C_SystemEvent : IExtensible
	{
		private int _EventType;

		private readonly List<int> _IntValue = new List<int>();

		private string _StrValue = string.Empty;

		private ulong _PlayerID;

		private string _StrValue2 = string.Empty;

		private int _Priority;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "EventType", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int EventType
		{
			get
			{
				return this._EventType;
			}
			set
			{
				this._EventType = value;
			}
		}

		[ProtoMember(2, Name = "IntValue", DataFormat = DataFormat.TwosComplement)]
		public List<int> IntValue
		{
			get
			{
				return this._IntValue;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "StrValue", DataFormat = DataFormat.Default), DefaultValue("")]
		public string StrValue
		{
			get
			{
				return this._StrValue;
			}
			set
			{
				this._StrValue = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "PlayerID", DataFormat = DataFormat.TwosComplement), DefaultValue(0f)]
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

		[ProtoMember(5, IsRequired = false, Name = "StrValue2", DataFormat = DataFormat.Default), DefaultValue("")]
		public string StrValue2
		{
			get
			{
				return this._StrValue2;
			}
			set
			{
				this._StrValue2 = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "Priority", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Priority
		{
			get
			{
				return this._Priority;
			}
			set
			{
				this._Priority = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
