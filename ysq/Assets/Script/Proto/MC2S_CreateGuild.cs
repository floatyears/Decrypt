using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_CreateGuild")]
	[Serializable]
	public class MC2S_CreateGuild : IExtensible
	{
		private string _Name = string.Empty;

		private string _Manifesto = string.Empty;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Name", DataFormat = DataFormat.Default), DefaultValue("")]
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

		[ProtoMember(2, IsRequired = false, Name = "Manifesto", DataFormat = DataFormat.Default), DefaultValue("")]
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
