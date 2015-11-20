using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_ChangeFashion")]
	[Serializable]
	public class MS2C_ChangeFashion : IExtensible
	{
		private int _Result;

		private int _FashionID;

		private uint _SocketVersion;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Result", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Result
		{
			get
			{
				return this._Result;
			}
			set
			{
				this._Result = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "FashionID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(3, IsRequired = false, Name = "SocketVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint SocketVersion
		{
			get
			{
				return this._SocketVersion;
			}
			set
			{
				this._SocketVersion = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
