using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_AddFashion")]
	[Serializable]
	public class MS2C_AddFashion : IExtensible
	{
		private int _FashionID;

		private uint _FashionVersion;

		private int _FashionTime;

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

		[ProtoMember(3, IsRequired = false, Name = "FashionTime", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int FashionTime
		{
			get
			{
				return this._FashionTime;
			}
			set
			{
				this._FashionTime = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
