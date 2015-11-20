using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_GetGuildList")]
	[Serializable]
	public class MS2C_GetGuildList : IExtensible
	{
		private readonly List<BriefGuildData> _Data = new List<BriefGuildData>();

		private bool _Flag;

		private int _JoinGuildCD;

		private IExtension extensionObject;

		[ProtoMember(1, Name = "Data", DataFormat = DataFormat.Default)]
		public List<BriefGuildData> Data
		{
			get
			{
				return this._Data;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "Flag", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool Flag
		{
			get
			{
				return this._Flag;
			}
			set
			{
				this._Flag = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "JoinGuildCD", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int JoinGuildCD
		{
			get
			{
				return this._JoinGuildCD;
			}
			set
			{
				this._JoinGuildCD = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
