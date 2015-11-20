using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "BriefGuildData")]
	[Serializable]
	public class BriefGuildData : IExtensible
	{
		private ulong _ID;

		private string _Name = string.Empty;

		private int _Level;

		private int _MemberNum;

		private int _ApplyLevel;

		private string _Manifesto = string.Empty;

		private int _Flag;

		private int _CombatValue;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "ID", DataFormat = DataFormat.TwosComplement), DefaultValue(0f)]
		public ulong ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				this._ID = value;
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

		[ProtoMember(3, IsRequired = false, Name = "Level", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Level
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

		[ProtoMember(4, IsRequired = false, Name = "MemberNum", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MemberNum
		{
			get
			{
				return this._MemberNum;
			}
			set
			{
				this._MemberNum = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "ApplyLevel", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ApplyLevel
		{
			get
			{
				return this._ApplyLevel;
			}
			set
			{
				this._ApplyLevel = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "Manifesto", DataFormat = DataFormat.Default), DefaultValue("")]
		public string Manifesto
		{
			get
			{
				return this._Manifesto;
			}
			set
			{
				this._Manifesto = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "Flag", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Flag
		{
			get
			{
				return this._Flag;
			}
			set
			{
				this._Flag = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "CombatValue", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int CombatValue
		{
			get
			{
				return this._CombatValue;
			}
			set
			{
				this._CombatValue = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
