using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_GetMailData")]
	[Serializable]
	public class MC2S_GetMailData : IExtensible
	{
		private uint _MinMailID;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "MinMailID", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint MinMailID
		{
			get
			{
				return this._MinMailID;
			}
			set
			{
				this._MinMailID = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
