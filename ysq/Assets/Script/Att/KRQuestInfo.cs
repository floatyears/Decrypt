using ProtoBuf;
using System;
using System.ComponentModel;

namespace Att
{
	[ProtoContract(Name = "KRQuestInfo")]
	[Serializable]
	public class KRQuestInfo : IExtensible
	{
		private int _ID;

		private string _Name = string.Empty;

		private int _Star;

		private string _Desc = string.Empty;

		private int _SceneID;

		private int _MaxHPScale;

		private int _AttackScale;

		private int _StartID;

		private int _GroupID;

		private int _PhysicDefenseScale;

		private int _MagicDefenseScale;

		private int _HitScale;

		private int _DodgeScale;

		private int _CritScale;

		private int _CritResisScale;

		private int _DamagePlusScale;

		private int _DamageMinusScale;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = true, Name = "ID", DataFormat = DataFormat.TwosComplement)]
		public int ID
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

		[ProtoMember(3, IsRequired = false, Name = "Star", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Star
		{
			get
			{
				return this._Star;
			}
			set
			{
				this._Star = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "Desc", DataFormat = DataFormat.Default), DefaultValue("")]
		public string Desc
		{
			get
			{
				return this._Desc;
			}
			set
			{
				this._Desc = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "SceneID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int SceneID
		{
			get
			{
				return this._SceneID;
			}
			set
			{
				this._SceneID = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "MaxHPScale", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MaxHPScale
		{
			get
			{
				return this._MaxHPScale;
			}
			set
			{
				this._MaxHPScale = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "AttackScale", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int AttackScale
		{
			get
			{
				return this._AttackScale;
			}
			set
			{
				this._AttackScale = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "StartID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int StartID
		{
			get
			{
				return this._StartID;
			}
			set
			{
				this._StartID = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "GroupID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int GroupID
		{
			get
			{
				return this._GroupID;
			}
			set
			{
				this._GroupID = value;
			}
		}

		[ProtoMember(10, IsRequired = false, Name = "PhysicDefenseScale", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int PhysicDefenseScale
		{
			get
			{
				return this._PhysicDefenseScale;
			}
			set
			{
				this._PhysicDefenseScale = value;
			}
		}

		[ProtoMember(11, IsRequired = false, Name = "MagicDefenseScale", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MagicDefenseScale
		{
			get
			{
				return this._MagicDefenseScale;
			}
			set
			{
				this._MagicDefenseScale = value;
			}
		}

		[ProtoMember(12, IsRequired = false, Name = "HitScale", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int HitScale
		{
			get
			{
				return this._HitScale;
			}
			set
			{
				this._HitScale = value;
			}
		}

		[ProtoMember(13, IsRequired = false, Name = "DodgeScale", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int DodgeScale
		{
			get
			{
				return this._DodgeScale;
			}
			set
			{
				this._DodgeScale = value;
			}
		}

		[ProtoMember(14, IsRequired = false, Name = "CritScale", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int CritScale
		{
			get
			{
				return this._CritScale;
			}
			set
			{
				this._CritScale = value;
			}
		}

		[ProtoMember(15, IsRequired = false, Name = "CritResisScale", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int CritResisScale
		{
			get
			{
				return this._CritResisScale;
			}
			set
			{
				this._CritResisScale = value;
			}
		}

		[ProtoMember(16, IsRequired = false, Name = "DamagePlusScale", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int DamagePlusScale
		{
			get
			{
				return this._DamagePlusScale;
			}
			set
			{
				this._DamagePlusScale = value;
			}
		}

		[ProtoMember(17, IsRequired = false, Name = "DamageMinusScale", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int DamageMinusScale
		{
			get
			{
				return this._DamageMinusScale;
			}
			set
			{
				this._DamageMinusScale = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
