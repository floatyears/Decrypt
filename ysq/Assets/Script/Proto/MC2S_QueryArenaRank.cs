using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_QueryArenaRank")]
	[Serializable]
	public class MC2S_QueryArenaRank : IExtensible
	{
		private uint _RankVersion;

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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
