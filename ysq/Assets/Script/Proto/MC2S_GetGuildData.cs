using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "MC2S_GetGuildData")]
	[Serializable]
	public class MC2S_GetGuildData : IExtensible
	{
		private IExtension extensionObject;

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
