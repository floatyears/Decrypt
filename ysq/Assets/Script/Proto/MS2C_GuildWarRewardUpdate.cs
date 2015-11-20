using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_GuildWarRewardUpdate")]
	[Serializable]
	public class MS2C_GuildWarRewardUpdate : IExtensible
	{
		private int _Reward;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Reward", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Reward
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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
