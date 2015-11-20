using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_HotTimeData")]
	[Serializable]
	public class MC2S_HotTimeData : IExtensible
	{
		private int _Version;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Version", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Version
		{
			get
			{
				return this._Version;
			}
			set
			{
				this._Version = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
