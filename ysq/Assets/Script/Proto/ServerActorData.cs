using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "ServerActorData")]
	[Serializable]
	public class ServerActorData : IExtensible
	{
		private readonly List<ActorData> _Data = new List<ActorData>();

		private int _Flag;

		private int _MaxHP;

		private IExtension extensionObject;

		[ProtoMember(1, Name = "Data", DataFormat = DataFormat.Default)]
		public List<ActorData> Data
		{
			get
			{
				return this._Data;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "Flag", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Flag
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

		[ProtoMember(3, IsRequired = false, Name = "MaxHP", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MaxHP
		{
			get
			{
				return this._MaxHP;
			}
			set
			{
				this._MaxHP = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
