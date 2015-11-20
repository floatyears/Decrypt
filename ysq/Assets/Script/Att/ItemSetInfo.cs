using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Att
{
	[ProtoContract(Name = "ItemSetInfo")]
	[Serializable]
	public class ItemSetInfo : IExtensible
	{
		private int _ID;

		private string _Name = string.Empty;

		private string _Desc = string.Empty;

		private readonly List<int> _ItemID = new List<int>();

		private readonly List<int> _Count = new List<int>();

		private readonly List<int> _AttID1 = new List<int>();

		private readonly List<int> _AttValue1 = new List<int>();

		private readonly List<int> _AttID2 = new List<int>();

		private readonly List<int> _AttValue2 = new List<int>();

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

		[ProtoMember(4, Name = "ItemID", DataFormat = DataFormat.TwosComplement)]
		public List<int> ItemID
		{
			get
			{
				return this._ItemID;
			}
		}

		[ProtoMember(5, Name = "Count", DataFormat = DataFormat.TwosComplement)]
		public List<int> Count
		{
			get
			{
				return this._Count;
			}
		}

		[ProtoMember(6, Name = "AttID1", DataFormat = DataFormat.TwosComplement)]
		public List<int> AttID1
		{
			get
			{
				return this._AttID1;
			}
		}

		[ProtoMember(7, Name = "AttValue1", DataFormat = DataFormat.TwosComplement)]
		public List<int> AttValue1
		{
			get
			{
				return this._AttValue1;
			}
		}

		[ProtoMember(8, Name = "AttID2", DataFormat = DataFormat.TwosComplement)]
		public List<int> AttID2
		{
			get
			{
				return this._AttID2;
			}
		}

		[ProtoMember(9, Name = "AttValue2", DataFormat = DataFormat.TwosComplement)]
		public List<int> AttValue2
		{
			get
			{
				return this._AttValue2;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
