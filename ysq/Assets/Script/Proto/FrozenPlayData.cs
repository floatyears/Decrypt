using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "FrozenPlayData")]
	[Serializable]
	public class FrozenPlayData : IExtensible
	{
		private int _Play;

		private int _TimeStamp;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Play", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Play
		{
			get
			{
				return this._Play;
			}
			set
			{
				this._Play = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "TimeStamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int TimeStamp
		{
			get
			{
				return this._TimeStamp;
			}
			set
			{
				this._TimeStamp = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
