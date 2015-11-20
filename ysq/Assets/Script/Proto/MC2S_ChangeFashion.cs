using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_ChangeFashion")]
	[Serializable]
	public class MC2S_ChangeFashion : IExtensible
	{
		private int _FashionID;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "FashionID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int FashionID
		{
			get
			{
				return this._FashionID;
			}
			set
			{
				this._FashionID = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
