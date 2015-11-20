using ProtoBuf;
using System;
using System.Collections.Generic;

namespace Proto
{
	[ProtoContract(Name = "MC2S_EquipBreakUp")]
	[Serializable]
	public class MC2S_EquipBreakUp : IExtensible
	{
		private readonly List<ulong> _EquipID = new List<ulong>();

		private IExtension extensionObject;

		[ProtoMember(2, Name = "EquipID", DataFormat = DataFormat.TwosComplement)]
		public List<ulong> EquipID
		{
			get
			{
				return this._EquipID;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
