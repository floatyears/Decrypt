using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_GMCommand")]
	[Serializable]
	public class MC2S_GMCommand : IExtensible
	{
		private string _Command = string.Empty;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Command", DataFormat = DataFormat.Default), DefaultValue("")]
		public string Command
		{
			get
			{
				return this._Command;
			}
			set
			{
				this._Command = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
