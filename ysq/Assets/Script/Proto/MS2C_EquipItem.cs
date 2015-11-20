using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_EquipItem")]
	[Serializable]
	public class MS2C_EquipItem : IExtensible
	{
		private int _Result;

		private int _SocketSlot;

		private int _EquipSlot;

		private ulong _ItemID;

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

		[ProtoMember(3, IsRequired = false, Name = "EquipSlot", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int EquipSlot
		{
			get
			{
				return this._EquipSlot;
			}
			set
			{
				this._EquipSlot = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "ItemID", DataFormat = DataFormat.TwosComplement), DefaultValue(0f)]
		public ulong ItemID
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

		[ProtoMember(5, IsRequired = false, Name = "SocketVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
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
