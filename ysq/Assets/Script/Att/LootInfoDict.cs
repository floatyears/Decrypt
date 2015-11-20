using ProtoBuf;
using System;
using System.Collections.Generic;

namespace Att
{
	[ProtoContract(Name = "LootInfoDict")]
	[Serializable]
	public class LootInfoDict : IExtensible
	{
		private readonly List<LootInfo> _Data = new List<LootInfo>();

		private IExtension extensionObject;

		[ProtoMember(1, Name = "Data", DataFormat = DataFormat.Default)]
		public List<LootInfo> Data
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
