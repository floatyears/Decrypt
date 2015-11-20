using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Att
{
	[ProtoContract(Name = "BuffInfo")]
	[Serializable]
	public class BuffInfo : IExtensible
	{
		private int _ID;

		private string _Name = string.Empty;

		private string _Icon = string.Empty;

		private int _TickType;

		private float _MaxDuration;

		private float _PeriodicTime;

		private int _InitStackCount;

		private int _MaxStackCount;

		private int _AreaType;

		private float _AreaRadius;

		private int _Level;

		private int _SSrcStackType;

		private int _DSrcStackType;

		private int _ReplaceType;

		private readonly List<int> _EffectType = new List<int>();

		private readonly List<int> _Value1 = new List<int>();

		private readonly List<int> _Value2 = new List<int>();

		private readonly List<int> _Value3 = new List<int>();

		private readonly List<int> _Value4 = new List<int>();

		private string _AddAction = string.Empty;

		private string _RemoveAction = string.Empty;

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

		[ProtoMember(4, IsRequired = false, Name = "TickType", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int TickType
		{
			get
			{
				return this._TickType;
			}
			set
			{
				this._TickType = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "MaxDuration", DataFormat = DataFormat.FixedSize), DefaultValue(0f)]
		public float MaxDuration
		{
			get
			{
				return this._MaxDuration;
			}
			set
			{
				this._MaxDuration = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "PeriodicTime", DataFormat = DataFormat.FixedSize), DefaultValue(0f)]
		public float PeriodicTime
		{
			get
			{
				return this._PeriodicTime;
			}
			set
			{
				this._PeriodicTime = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "InitStackCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int InitStackCount
		{
			get
			{
				return this._InitStackCount;
			}
			set
			{
				this._InitStackCount = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "MaxStackCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MaxStackCount
		{
			get
			{
				return this._MaxStackCount;
			}
			set
			{
				this._MaxStackCount = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "AreaType", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int AreaType
		{
			get
			{
				return this._AreaType;
			}
			set
			{
				this._AreaType = value;
			}
		}

		[ProtoMember(10, IsRequired = false, Name = "AreaRadius", DataFormat = DataFormat.FixedSize), DefaultValue(0f)]
		public float AreaRadius
		{
			get
			{
				return this._AreaRadius;
			}
			set
			{
				this._AreaRadius = value;
			}
		}

		[ProtoMember(11, IsRequired = false, Name = "Level", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(12, IsRequired = false, Name = "SSrcStackType", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int SSrcStackType
		{
			get
			{
				return this._SSrcStackType;
			}
			set
			{
				this._SSrcStackType = value;
			}
		}

		[ProtoMember(13, IsRequired = false, Name = "DSrcStackType", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int DSrcStackType
		{
			get
			{
				return this._DSrcStackType;
			}
			set
			{
				this._DSrcStackType = value;
			}
		}

		[ProtoMember(14, IsRequired = false, Name = "ReplaceType", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ReplaceType
		{
			get
			{
				return this._ReplaceType;
			}
			set
			{
				this._ReplaceType = value;
			}
		}

		[ProtoMember(15, Name = "EffectType", DataFormat = DataFormat.TwosComplement)]
		public List<int> EffectType
		{
			get
			{
				return this._EffectType;
			}
		}

		[ProtoMember(16, Name = "Value1", DataFormat = DataFormat.TwosComplement)]
		public List<int> Value1
		{
			get
			{
				return this._Value1;
			}
		}

		[ProtoMember(17, Name = "Value2", DataFormat = DataFormat.TwosComplement)]
		public List<int> Value2
		{
			get
			{
				return this._Value2;
			}
		}

		[ProtoMember(18, Name = "Value3", DataFormat = DataFormat.TwosComplement)]
		public List<int> Value3
		{
			get
			{
				return this._Value3;
			}
		}

		[ProtoMember(19, Name = "Value4", DataFormat = DataFormat.TwosComplement)]
		public List<int> Value4
		{
			get
			{
				return this._Value4;
			}
		}

		[ProtoMember(20, IsRequired = false, Name = "AddAction", DataFormat = DataFormat.Default), DefaultValue("")]
		public string AddAction
		{
			get
			{
				return this._AddAction;
			}
			set
			{
				this._AddAction = value;
			}
		}

		[ProtoMember(21, IsRequired = false, Name = "RemoveAction", DataFormat = DataFormat.Default), DefaultValue("")]
		public string RemoveAction
		{
			get
			{
				return this._RemoveAction;
			}
			set
			{
				this._RemoveAction = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
