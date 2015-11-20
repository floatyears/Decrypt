using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_Diamond2Money")]
	[Serializable]
	public class MC2S_Diamond2Money : IExtensible
	{
		private int _Count;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Count", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Count
		{
			get
			{
				return this._Count;
			}
			set
			{
				this._Count = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
