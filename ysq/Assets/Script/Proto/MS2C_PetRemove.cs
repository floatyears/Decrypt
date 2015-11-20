using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_PetRemove")]
	[Serializable]
	public class MS2C_PetRemove : IExtensible
	{
		private ulong _PetID;

		private uint _PetVersion;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "PetID", DataFormat = DataFormat.TwosComplement), DefaultValue(0f)]
		public ulong PetID
		{
			get
			{
				return this._PetID;
			}
			set
			{
				this._PetID = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "PetVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint PetVersion
		{
			get
			{
				return this._PetVersion;
			}
			set
			{
				this._PetVersion = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
