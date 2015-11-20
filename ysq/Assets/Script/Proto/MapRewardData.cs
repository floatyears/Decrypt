using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MapRewardData")]
	[Serializable]
	public class MapRewardData : IExtensible
	{
		private int _MapID;

		private int _RewardMask;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "MapID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MapID
		{
			get
			{
				return this._MapID;
			}
			set
			{
				this._MapID = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "RewardMask", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int RewardMask
		{
			get
			{
				return this._RewardMask;
			}
			set
			{
				this._RewardMask = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
