using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "MC2S_ActivityPayTodayNoPopup")]
	[Serializable]
	public class MC2S_ActivityPayTodayNoPopup : IExtensible
	{
		private IExtension extensionObject;

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
