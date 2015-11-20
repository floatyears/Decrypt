using ProtoBuf;
using System;
using System.Collections.Generic;

namespace Att
{
	[ProtoContract(Name = "PetShopInfoDict")]
	[Serializable]
	public class PetShopInfoDict : IExtensible
	{
		private readonly List<PetShopInfo> _Data = new List<PetShopInfo>();

		private IExtension extensionObject;

		[ProtoMember(1, Name = "Data", DataFormat = DataFormat.Default)]
		public List<PetShopInfo> Data
		{
			get
			{
				return this._Data;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
