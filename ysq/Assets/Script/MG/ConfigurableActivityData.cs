using ProtoBuf;
using System;
using System.ComponentModel;

namespace MG
{
	[ProtoContract(Name = "ConfigurableActivityData")]
	[Serializable]
	public class ConfigurableActivityData : IExtensible
	{
		private int _ID;

		private int _RewardTimeStamp;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "ID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				this._ID = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "RewardTimeStamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int RewardTimeStamp
		{
			get
			{
				return this._RewardTimeStamp;
			}
			set
			{
				this._RewardTimeStamp = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
