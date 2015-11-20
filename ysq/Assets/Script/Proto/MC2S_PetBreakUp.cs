using ProtoBuf;
using System;
using System.Collections.Generic;

namespace Proto
{
	[ProtoContract(Name = "MC2S_PetBreakUp")]
	[Serializable]
	public class MC2S_PetBreakUp : IExtensible
	{
		private readonly List<ulong> _PetID = new List<ulong>();

		private IExtension extensionObject;

		[ProtoMember(1, Name = "PetID", DataFormat = DataFormat.TwosComplement)]
		public List<ulong> PetID
		{
			get
			{
				return this._PetID;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
