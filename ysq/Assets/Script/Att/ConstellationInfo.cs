using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Att
{
	[ProtoContract(Name = "ConstellationInfo")]
	[Serializable]
	public class ConstellationInfo : IExtensible
	{
		private int _ID;

		private string _Name = string.Empty;

		private string _Icon = string.Empty;

		private string _ResLoc = string.Empty;

		private int _PreID;

		private int _NextID;

		private readonly List<int> _Type = new List<int>();

		private readonly List<int> _Value1 = new List<int>();

		private readonly List<int> _Value2 = new List<int>();

		private readonly List<int> _Cost = new List<int>();

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

		[ProtoMember(3, IsRequired = false, Name = "Icon", DataFormat = DataFormat.Default), DefaultValue("")]
		public string Icon
		{
			get
			{
				return this._Icon;
			}
			set
			{
				this._Icon = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "ResLoc", DataFormat = DataFormat.Default), DefaultValue("")]
		public string ResLoc
		{
			get
			{
				return this._ResLoc;
			}
			set
			{
				this._ResLoc = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "PreID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int PreID
		{
			get
			{
				return this._PreID;
			}
			set
			{
				this._PreID = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "NextID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int NextID
		{
			get
			{
				return this._NextID;
			}
			set
			{
				this._NextID = value;
			}
		}

		[ProtoMember(7, Name = "Type", DataFormat = DataFormat.TwosComplement)]
		public List<int> Type
		{
			get
			{
				return this._Type;
			}
		}

		[ProtoMember(8, Name = "Value1", DataFormat = DataFormat.TwosComplement)]
		public List<int> Value1
		{
			get
			{
				return this._Value1;
			}
		}

		[ProtoMember(9, Name = "Value2", DataFormat = DataFormat.TwosComplement)]
		public List<int> Value2
		{
			get
			{
				return this._Value2;
			}
		}

		[ProtoMember(10, Name = "Cost", DataFormat = DataFormat.TwosComplement)]
		public List<int> Cost
		{
			get
			{
				return this._Cost;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
