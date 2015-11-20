using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_QueryGuildEvent")]
	[Serializable]
	public class MC2S_QueryGuildEvent : IExtensible
	{
		private int _MaxEventID;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "MaxEventID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MaxEventID
		{
			get
			{
				return this._MaxEventID;
			}
			set
			{
				this._MaxEventID = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
