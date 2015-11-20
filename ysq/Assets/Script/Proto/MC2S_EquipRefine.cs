using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_EquipRefine")]
	[Serializable]
	public class MC2S_EquipRefine : IExtensible
	{
		private ulong _EquipID;

		private ulong _ItemID;

		private int _Count;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "EquipID", DataFormat = DataFormat.TwosComplement), DefaultValue(0f)]
		public ulong EquipID
		{
			get
			{
				return this._EquipID;
			}
			set
			{
				this._EquipID = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "ItemID", DataFormat = DataFormat.TwosComplement), DefaultValue(0f)]
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

		[ProtoMember(3, IsRequired = false, Name = "Count", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Count
		{
			get
			{
				return this._Count;
			}
			set
			{
				this._Count = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
