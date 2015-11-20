using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_SystemNotice")]
	[Serializable]
	public class MS2C_SystemNotice : IExtensible
	{
		private string _Content = string.Empty;

		private int _Priority;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Content", DataFormat = DataFormat.Default), DefaultValue("")]
		public string Content
		{
			get
			{
				return this._Content;
			}
			set
			{
				this._Content = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "Priority", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Priority
		{
			get
			{
				return this._Priority;
			}
			set
			{
				this._Priority = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
