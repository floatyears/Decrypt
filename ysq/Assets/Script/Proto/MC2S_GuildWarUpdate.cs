using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_GuildWarUpdate")]
	[Serializable]
	public class MC2S_GuildWarUpdate : IExtensible
	{
		private EGuildWarId _WarID;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "WarID", DataFormat = DataFormat.TwosComplement), DefaultValue(EGuildWarId.EGWI_None)]
		public EGuildWarId WarID
		{
			get
			{
				return this._WarID;
			}
			set
			{
				this._WarID = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
