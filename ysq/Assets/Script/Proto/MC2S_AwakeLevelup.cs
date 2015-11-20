using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_AwakeLevelup")]
	[Serializable]
	public class MC2S_AwakeLevelup : IExtensible
	{
		private ulong _PetID;

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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
