using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_StartCarnival")]
	[Serializable]
	public class MC2S_StartCarnival : IExtensible
	{
		private int _CarnivalType;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "CarnivalType", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int CarnivalType
		{
			get
			{
				return this._CarnivalType;
			}
			set
			{
				this._CarnivalType = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
