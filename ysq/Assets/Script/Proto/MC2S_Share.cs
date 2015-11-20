using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_Share")]
	[Serializable]
	public class MC2S_Share : IExtensible
	{
		private int _ShareChannel;

		private int _SharePoint;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "ShareChannel", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ShareChannel
		{
			get
			{
				return this._ShareChannel;
			}
			set
			{
				this._ShareChannel = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "SharePoint", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int SharePoint
		{
			get
			{
				return this._SharePoint;
			}
			set
			{
				this._SharePoint = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
