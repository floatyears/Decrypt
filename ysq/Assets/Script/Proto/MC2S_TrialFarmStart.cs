using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "MC2S_TrialFarmStart")]
	[Serializable]
	public class MC2S_TrialFarmStart : IExtensible
	{
		private IExtension extensionObject;

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
