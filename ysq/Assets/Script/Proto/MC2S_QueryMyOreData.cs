using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "MC2S_QueryMyOreData")]
	[Serializable]
	public class MC2S_QueryMyOreData : IExtensible
	{
		private IExtension extensionObject;

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
