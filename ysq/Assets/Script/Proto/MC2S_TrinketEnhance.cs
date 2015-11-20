using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_TrinketEnhance")]
	[Serializable]
	public class MC2S_TrinketEnhance : IExtensible
	{
		private ulong _TrinketID;

		private readonly List<ulong> _ItemID = new List<ulong>();

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

		[ProtoMember(2, Name = "ItemID", DataFormat = DataFormat.TwosComplement)]
		public List<ulong> ItemID
		{
			get
			{
				return this._ItemID;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
