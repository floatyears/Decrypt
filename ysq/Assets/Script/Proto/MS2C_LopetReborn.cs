using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_LopetReborn")]
	[Serializable]
	public class MS2C_LopetReborn : IExtensible
	{
		private ELopetResult _Result;

		private uint _Version;

		private LopetData _SrcLopet;

		private readonly List<LopetData> _AddLopet = new List<LopetData>();

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Result", DataFormat = DataFormat.TwosComplement), DefaultValue(ELopetResult.ELR_Success)]
		public ELopetResult Result
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

		[ProtoMember(2, IsRequired = false, Name = "Version", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint Version
		{
			get
			{
				return this._Version;
			}
			set
			{
				this._Version = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "SrcLopet", DataFormat = DataFormat.Default), DefaultValue(null)]
		public LopetData SrcLopet
		{
			get
			{
				return this._SrcLopet;
			}
			set
			{
				this._SrcLopet = value;
			}
		}

		[ProtoMember(4, Name = "AddLopet", DataFormat = DataFormat.Default)]
		public List<LopetData> AddLopet
		{
			get
			{
				return this._AddLopet;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
