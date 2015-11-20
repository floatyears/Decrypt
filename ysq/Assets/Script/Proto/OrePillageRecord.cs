using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "OrePillageRecord")]
	[Serializable]
	public class OrePillageRecord : IExtensible
	{
		private ulong _GUID;

		private string _Name = string.Empty;

		private int _Amount;

		private int _Timestamp;

		private int _PillageCount;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "GUID", DataFormat = DataFormat.TwosComplement), DefaultValue(0f)]
		public ulong GUID
		{
			get
			{
				return this._GUID;
			}
			set
			{
				this._GUID = value;
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

		[ProtoMember(3, IsRequired = false, Name = "Amount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Amount
		{
			get
			{
				return this._Amount;
			}
			set
			{
				this._Amount = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "Timestamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Timestamp
		{
			get
			{
				return this._Timestamp;
			}
			set
			{
				this._Timestamp = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "PillageCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int PillageCount
		{
			get
			{
				return this._PillageCount;
			}
			set
			{
				this._PillageCount = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
