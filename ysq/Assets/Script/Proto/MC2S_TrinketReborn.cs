using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_TrinketReborn")]
	[Serializable]
	public class MC2S_TrinketReborn : IExtensible
	{
		private ulong _TrinketID;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "TrinketID", DataFormat = DataFormat.TwosComplement), DefaultValue(0f)]
		public ulong TrinketID
		{
			get
			{
				return this._TrinketID;
			}
			set
			{
				this._TrinketID = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
