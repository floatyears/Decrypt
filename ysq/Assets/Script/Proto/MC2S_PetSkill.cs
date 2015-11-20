using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_PetSkill")]
	[Serializable]
	public class MC2S_PetSkill : IExtensible
	{
		private ulong _PetID;

		private int _Index;

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

		[ProtoMember(2, IsRequired = false, Name = "Index", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Index
		{
			get
			{
				return this._Index;
			}
			set
			{
				this._Index = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
