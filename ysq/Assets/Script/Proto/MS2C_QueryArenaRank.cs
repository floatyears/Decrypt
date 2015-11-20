using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_QueryArenaRank")]
	[Serializable]
	public class MS2C_QueryArenaRank : IExtensible
	{
		private uint _RankVersion;

		private readonly List<RankData> _Data = new List<RankData>();

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "RankVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint RankVersion
		{
			get
			{
				return this._RankVersion;
			}
			set
			{
				this._RankVersion = value;
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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
