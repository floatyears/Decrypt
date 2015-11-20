using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_StartScratchOff")]
	[Serializable]
	public class MC2S_StartScratchOff : IExtensible
	{
		private bool _Free;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Free", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool Free
		{
			get
			{
				return this._Free;
			}
			set
			{
				this._Free = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
