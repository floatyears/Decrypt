using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "MS2C_HeartBeat")]
	[Serializable]
	public class MS2C_HeartBeat : IExtensible
	{
		private IExtension extensionObject;

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
