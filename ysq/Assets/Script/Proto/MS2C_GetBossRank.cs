using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_GetBossRank")]
	[Serializable]
	public class MS2C_GetBossRank : IExtensible
	{
		private uint _BossRankVersion;

		private readonly List<RankData> _RData = new List<RankData>();

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

		[ProtoMember(2, Name = "RData", DataFormat = DataFormat.Default)]
		public List<RankData> RData
		{
			get
			{
				return this._RData;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
