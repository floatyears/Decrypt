using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_RemoveFashion")]
	[Serializable]
	public class MS2C_RemoveFashion : IExtensible
	{
		private int _FashionID;

		private uint _FashionVersion;

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

		[ProtoMember(2, IsRequired = false, Name = "FashionVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint FashionVersion
		{
			get
			{
				return this._FashionVersion;
			}
			set
			{
				this._FashionVersion = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
