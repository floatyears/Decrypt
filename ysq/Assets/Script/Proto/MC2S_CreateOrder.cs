using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_CreateOrder")]
	[Serializable]
	public class MC2S_CreateOrder : IExtensible
	{
		private int _ProductID;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "ProductID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ProductID
		{
			get
			{
				return this._ProductID;
			}
			set
			{
				this._ProductID = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
