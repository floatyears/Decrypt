using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_AutoEquipItem")]
	[Serializable]
	public class MC2S_AutoEquipItem : IExtensible
	{
		private int _SocketSlot;

		private readonly List<ulong> _ItemID = new List<ulong>();

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

		[ProtoMember(2, Name = "ItemID", DataFormat = DataFormat.TwosComplement)]
		public List<ulong> ItemID
		{
			get
			{
				return this._ItemID;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
