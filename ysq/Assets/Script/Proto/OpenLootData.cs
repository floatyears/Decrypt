using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "OpenLootData")]
	[Serializable]
	public class OpenLootData : IExtensible
	{
		private int _InfoID;

		private uint _Count;

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

		[ProtoMember(2, IsRequired = false, Name = "Count", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint Count
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
