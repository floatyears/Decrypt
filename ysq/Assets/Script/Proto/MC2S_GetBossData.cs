using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_GetBossData")]
	[Serializable]
	public class MC2S_GetBossData : IExtensible
	{
		private int _Status;

		private int _TimeStamp;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Status", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Status
		{
			get
			{
				return this._Status;
			}
			set
			{
				this._Status = value;
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
