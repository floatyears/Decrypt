using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_GetBossRank")]
	[Serializable]
	public class MC2S_GetBossRank : IExtensible
	{
		private uint _BossRankVersion;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "BossRankVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint BossRankVersion
		{
			get
			{
				return this._BossRankVersion;
			}
			set
			{
				this._BossRankVersion = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
