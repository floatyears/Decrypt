using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_PvpOneKeyPillage")]
	[Serializable]
	public class MC2S_PvpOneKeyPillage : IExtensible
	{
		private int _ItemID;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "ItemID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ItemID
		{
			get
			{
				return this._ItemID;
			}
			set
			{
				this._ItemID = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
