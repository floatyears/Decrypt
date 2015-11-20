using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_ChangeSocket")]
	[Serializable]
	public class MS2C_ChangeSocket : IExtensible
	{
		private int _Result;

		private int _Slot1;

		private int _Slot2;

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

		[ProtoMember(2, IsRequired = false, Name = "Slot1", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Slot1
		{
			get
			{
				return this._Slot1;
			}
			set
			{
				this._Slot1 = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "Slot2", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Slot2
		{
			get
			{
				return this._Slot2;
			}
			set
			{
				this._Slot2 = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "SocketVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
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
