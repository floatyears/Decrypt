using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_GuildDataUpdate")]
	[Serializable]
	public class MS2C_GuildDataUpdate : IExtensible
	{
		private string _Name = string.Empty;

		private int _Level;

		private int _Exp;

		private int _Money;

		private int _Prosperity;

		private bool _HasManifesto;

		private string _Manifesto = string.Empty;

		private int _ImpeachTimeStamp;

		private ulong _ImpeachID;

		private int _ApplyLevel;

		private int _Verify;

		private int _Score;

		private int _SignNum;

		private int _MaxAcademyID;

		private int _AttackAcademyID1;

		private int _AttackAcademyID2;

		private int _KickCount;

		private int _CombatValue;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Name", DataFormat = DataFormat.Default), DefaultValue("")]
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

		[ProtoMember(2, IsRequired = false, Name = "Level", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(3, IsRequired = false, Name = "Exp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Exp
		{
			get
			{
				return this._Exp;
			}
			set
			{
				this._Exp = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "Money", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Money
		{
			get
			{
				return this._Money;
			}
			set
			{
				this._Money = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "Prosperity", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Prosperity
		{
			get
			{
				return this._Prosperity;
			}
			set
			{
				this._Prosperity = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "HasManifesto", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool HasManifesto
		{
			get
			{
				return this._HasManifesto;
			}
			set
			{
				this._HasManifesto = value;
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

		[ProtoMember(8, IsRequired = false, Name = "ImpeachTimeStamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ImpeachTimeStamp
		{
			get
			{
				return this._ImpeachTimeStamp;
			}
			set
			{
				this._ImpeachTimeStamp = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "ImpeachID", DataFormat = DataFormat.TwosComplement), DefaultValue(0f)]
		public ulong ImpeachID
		{
			get
			{
				return this._ImpeachID;
			}
			set
			{
				this._ImpeachID = value;
			}
		}

		[ProtoMember(10, IsRequired = false, Name = "ApplyLevel", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(11, IsRequired = false, Name = "Verify", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Verify
		{
			get
			{
				return this._Verify;
			}
			set
			{
				this._Verify = value;
			}
		}

		[ProtoMember(13, IsRequired = false, Name = "Score", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Score
		{
			get
			{
				return this._Score;
			}
			set
			{
				this._Score = value;
			}
		}

		[ProtoMember(14, IsRequired = false, Name = "SignNum", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int SignNum
		{
			get
			{
				return this._SignNum;
			}
			set
			{
				this._SignNum = value;
			}
		}

		[ProtoMember(15, IsRequired = false, Name = "MaxAcademyID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MaxAcademyID
		{
			get
			{
				return this._MaxAcademyID;
			}
			set
			{
				this._MaxAcademyID = value;
			}
		}

		[ProtoMember(16, IsRequired = false, Name = "AttackAcademyID1", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int AttackAcademyID1
		{
			get
			{
				return this._AttackAcademyID1;
			}
			set
			{
				this._AttackAcademyID1 = value;
			}
		}

		[ProtoMember(17, IsRequired = false, Name = "AttackAcademyID2", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int AttackAcademyID2
		{
			get
			{
				return this._AttackAcademyID2;
			}
			set
			{
				this._AttackAcademyID2 = value;
			}
		}

		[ProtoMember(18, IsRequired = false, Name = "KickCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int KickCount
		{
			get
			{
				return this._KickCount;
			}
			set
			{
				this._KickCount = value;
			}
		}

		[ProtoMember(19, IsRequired = false, Name = "CombatValue", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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
