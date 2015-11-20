using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_QueryTrialRank")]
	[Serializable]
	public class MC2S_QueryTrialRank : IExtensible
	{
		private uint _TrialRankVersion;

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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
