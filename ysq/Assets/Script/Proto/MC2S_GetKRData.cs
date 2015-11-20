using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_GetKRData")]
	[Serializable]
	public class MC2S_GetKRData : IExtensible
	{
		private bool _Refresh;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Refresh", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool Refresh
		{
			get
			{
				return this._Refresh;
			}
			set
			{
				this._Refresh = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
