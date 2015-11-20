using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_UpdateItem")]
	[Serializable]
	public class MS2C_UpdateItem : IExtensible
	{
		private readonly List<ItemUpdate> _Data = new List<ItemUpdate>();

		private uint _ItemVersion;

		private IExtension extensionObject;

		[ProtoMember(1, Name = "Data", DataFormat = DataFormat.Default)]
		public List<ItemUpdate> Data
		{
			get
			{
				return this._Data;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "ItemVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint ItemVersion
		{
			get
			{
				return this._ItemVersion;
			}
			set
			{
				this._ItemVersion = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
