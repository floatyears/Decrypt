using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_QueryTrialRank")]
	[Serializable]
	public class MS2C_QueryTrialRank : IExtensible
	{
		private uint _TrialRankVersion;

		private readonly List<RankData> _Data = new List<RankData>();

		private uint _SelfRank;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "TrialRankVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint TrialRankVersion
		{
			get
			{
				return this._TrialRankVersion;
			}
			set
			{
				this._TrialRankVersion = value;
			}
		}

		[ProtoMember(2, Name = "Data", DataFormat = DataFormat.Default)]
		public List<RankData> Data
		{
			get
			{
				return this._Data;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "SelfRank", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint SelfRank
		{
			get
			{
				return this._SelfRank;
			}
			set
			{
				this._SelfRank = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
