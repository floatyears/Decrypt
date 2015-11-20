using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_SetGuildManifesto")]
	[Serializable]
	public class MC2S_SetGuildManifesto : IExtensible
	{
		private string _Manifesto = string.Empty;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Manifesto", DataFormat = DataFormat.Default), DefaultValue("")]
		public string Manifesto
		{
			get
			{
				return this._Manifesto;
			}
			set
			{
				this._Manifesto = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
