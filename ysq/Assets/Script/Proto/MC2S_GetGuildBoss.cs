using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "MC2S_GetGuildBoss")]
	[Serializable]
	public class MC2S_GetGuildBoss : IExtensible
	{
		private IExtension extensionObject;

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
