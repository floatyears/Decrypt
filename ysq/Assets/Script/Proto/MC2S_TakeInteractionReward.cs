using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "MC2S_TakeInteractionReward")]
	[Serializable]
	public class MC2S_TakeInteractionReward : IExtensible
	{
		private IExtension extensionObject;

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
