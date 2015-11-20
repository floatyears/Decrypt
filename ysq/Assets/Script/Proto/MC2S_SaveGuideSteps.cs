using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_SaveGuideSteps")]
	[Serializable]
	public class MC2S_SaveGuideSteps : IExtensible
	{
		private int _index;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "index", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int index
		{
			get
			{
				return this._index;
			}
			set
			{
				this._index = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
