using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_PetExchange")]
	[Serializable]
	public class MC2S_PetExchange : IExtensible
	{
		private ulong _PetID1;

		private ulong _PetID2;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "PetID1", DataFormat = DataFormat.TwosComplement), DefaultValue(0f)]
		public ulong PetID1
		{
			get
			{
				return this._PetID1;
			}
			set
			{
				this._PetID1 = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "PetID2", DataFormat = DataFormat.TwosComplement), DefaultValue(0f)]
		public ulong PetID2
		{
			get
			{
				return this._PetID2;
			}
			set
			{
				this._PetID2 = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
