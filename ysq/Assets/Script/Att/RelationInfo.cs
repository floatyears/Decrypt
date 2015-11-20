using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Att
{
	[ProtoContract(Name = "RelationInfo")]
	[Serializable]
	public class RelationInfo : IExtensible
	{
		private int _ID;

		private string _Name = string.Empty;

		private string _Desc = string.Empty;

		private int _Type;

		private int _ItemID;

		private readonly List<int> _PetID = new List<int>();

		private readonly List<int> _AttID = new List<int>();

		private readonly List<int> _AttPct = new List<int>();

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

		[ProtoMember(3, IsRequired = false, Name = "Desc", DataFormat = DataFormat.Default), DefaultValue("")]
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

		[ProtoMember(4, IsRequired = false, Name = "Type", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Type
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

		[ProtoMember(5, IsRequired = false, Name = "ItemID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ItemID
		{
			get
			{
				return this._ItemID;
			}
			set
			{
				this._ItemID = value;
			}
		}

		[ProtoMember(6, Name = "PetID", DataFormat = DataFormat.TwosComplement)]
		public List<int> PetID
		{
			get
			{
				return this._PetID;
			}
		}

		[ProtoMember(7, Name = "AttID", DataFormat = DataFormat.TwosComplement)]
		public List<int> AttID
		{
			get
			{
				return this._AttID;
			}
		}

		[ProtoMember(8, Name = "AttPct", DataFormat = DataFormat.TwosComplement)]
		public List<int> AttPct
		{
			get
			{
				return this._AttPct;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
