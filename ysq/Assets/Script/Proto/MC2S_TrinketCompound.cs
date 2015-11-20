using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_TrinketCompound")]
	[Serializable]
	public class MC2S_TrinketCompound : IExtensible
	{
		private int _InfoID;

		private readonly List<ulong> _ItemID = new List<ulong>();

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "InfoID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int InfoID
		{
			get
			{
				return this._InfoID;
			}
			set
			{
				this._InfoID = value;
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
