using ProtoBuf;
using System;
using System.Collections.Generic;

namespace Proto
{
	[ProtoContract(Name = "MS2C_BroadcastDamageRank")]
	[Serializable]
	public class MS2C_BroadcastDamageRank : IExtensible
	{
		private readonly List<RankData> _Data = new List<RankData>();

		private IExtension extensionObject;

		[ProtoMember(1, Name = "Data", DataFormat = DataFormat.Default)]
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
