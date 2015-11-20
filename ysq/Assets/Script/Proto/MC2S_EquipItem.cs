using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_EquipItem")]
	[Serializable]
	public class MC2S_EquipItem : IExtensible
	{
		private int _SocketSlot;

		private int _EquipSlot;

		private ulong _ItemID;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "SocketSlot", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(2, IsRequired = false, Name = "EquipSlot", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(3, IsRequired = false, Name = "ItemID", DataFormat = DataFormat.TwosComplement), DefaultValue(0f)]
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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
