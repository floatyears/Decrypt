using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_GetGGOreRankList")]
	[Serializable]
	public class MS2C_GetGGOreRankList : IExtensible
	{
		private int _Result;

		private uint _Version;

		private readonly List<GuildRank> _Data = new List<GuildRank>();

		private uint _Rank;

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

		[ProtoMember(2, IsRequired = false, Name = "Version", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint Version
		{
			get
			{
				return this._Version;
			}
			set
			{
				this._Version = value;
			}
		}

		[ProtoMember(3, Name = "Data", DataFormat = DataFormat.Default)]
		public List<GuildRank> Data
		{
			get
			{
				return this._Data;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "Rank", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint Rank
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
