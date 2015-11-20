using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_QueryRoseRank")]
	[Serializable]
	public class MS2C_QueryRoseRank : IExtensible
	{
		private uint _Version;

		private readonly List<RankData> _Data = new List<RankData>();

		private uint _SelfRank;

		private uint _Count;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Version", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
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

		[ProtoMember(4, IsRequired = false, Name = "Count", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint Count
		{
			get
			{
				return this._Count;
			}
			set
			{
				this._Count = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
