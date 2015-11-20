using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "GuildWarPlayerData")]
	[Serializable]
	public class GuildWarPlayerData : IExtensible
	{
		private EGuildWarReward _Reward;

		private int _Timestamp;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Reward", DataFormat = DataFormat.TwosComplement), DefaultValue(EGuildWarReward.EGWR_No)]
		public EGuildWarReward Reward
		{
			get
			{
				return this._Reward;
			}
			set
			{
				this._Reward = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "Timestamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Timestamp
		{
			get
			{
				return this._Timestamp;
			}
			set
			{
				this._Timestamp = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
