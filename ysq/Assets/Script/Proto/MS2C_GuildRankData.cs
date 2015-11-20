using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_GuildRankData")]
	[Serializable]
	public class MS2C_GuildRankData : IExtensible
	{
		private int _Result;

		private readonly List<GuildRank> _Data = new List<GuildRank>();

		private int _Rank;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Result", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Result
		{
			get
			{
				return this._Result;
			}
			set
			{
				this._Result = value;
			}
		}

		[ProtoMember(2, Name = "Data", DataFormat = DataFormat.Default)]
		public List<GuildRank> Data
		{
			get
			{
				return this._Data;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "Rank", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Rank
		{
			get
			{
				return this._Rank;
			}
			set
			{
				this._Rank = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
