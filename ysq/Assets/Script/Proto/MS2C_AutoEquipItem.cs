using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_AutoEquipItem")]
	[Serializable]
	public class MS2C_AutoEquipItem : IExtensible
	{
		private int _Result;

		private int _SocketSlot;

		private readonly List<ulong> _ItemID = new List<ulong>();

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

		[ProtoMember(2, IsRequired = false, Name = "SocketSlot", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int SocketSlot
		{
			get
			{
				return this._SocketSlot;
			}
			set
			{
				this._SocketSlot = value;
			}
		}

		[ProtoMember(3, Name = "ItemID", DataFormat = DataFormat.TwosComplement)]
		public List<ulong> ItemID
		{
			get
			{
				return this._ItemID;
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
