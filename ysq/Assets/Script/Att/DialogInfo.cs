using ProtoBuf;
using System;
using System.ComponentModel;

namespace Att
{
	[ProtoContract(Name = "DialogInfo")]
	[Serializable]
	public class DialogInfo : IExtensible
	{
		private int _ID;

		private int _NextID;

		private int _Style;

		private string _Text = string.Empty;

		private int _HeadType;

		private int _HeadValue;

		private float _Scale;

		private float _OffsetY;

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

		[ProtoMember(2, IsRequired = false, Name = "NextID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(4, IsRequired = false, Name = "Style", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Style
		{
			get
			{
				return this._Style;
			}
			set
			{
				this._Style = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "Text", DataFormat = DataFormat.Default), DefaultValue("")]
		public string Text
		{
			get
			{
				return this._Text;
			}
			set
			{
				this._Text = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "HeadType", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int HeadType
		{
			get
			{
				return this._HeadType;
			}
			set
			{
				this._HeadType = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "HeadValue", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int HeadValue
		{
			get
			{
				return this._HeadValue;
			}
			set
			{
				this._HeadValue = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "Scale", DataFormat = DataFormat.FixedSize), DefaultValue(0f)]
		public float Scale
		{
			get
			{
				return this._Scale;
			}
			set
			{
				this._Scale = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "OffsetY", DataFormat = DataFormat.FixedSize), DefaultValue(0f)]
		public float OffsetY
		{
			get
			{
				return this._OffsetY;
			}
			set
			{
				this._OffsetY = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
