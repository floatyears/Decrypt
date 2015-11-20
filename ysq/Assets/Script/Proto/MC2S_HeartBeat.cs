using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "MC2S_HeartBeat")]
	[Serializable]
	public class MC2S_HeartBeat : IExtensible
	{
		private IExtension extensionObject;

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
