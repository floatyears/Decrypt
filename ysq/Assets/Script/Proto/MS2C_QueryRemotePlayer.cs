using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_QueryRemotePlayer")]
	[Serializable]
	public class MS2C_QueryRemotePlayer : IExtensible
	{
		private int _Result;

		private int _Type;

		private RemotePlayer _Data1;

		private RemotePlayerDetail _Data2;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Result", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Result
		{
			get
			{
				return this._Result;
			}
			set
			{
				this._Result = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "Type", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(3, IsRequired = false, Name = "Data1", DataFormat = DataFormat.Default), DefaultValue(null)]
		public RemotePlayer Data1
		{
			get
			{
				return this._Data1;
			}
			set
			{
				this._Data1 = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "Data2", DataFormat = DataFormat.Default), DefaultValue(null)]
		public RemotePlayerDetail Data2
		{
			get
			{
				return this._Data2;
			}
			set
			{
				this._Data2 = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
