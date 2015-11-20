using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_MagicMatch")]
	[Serializable]
	public class MC2S_MagicMatch : IExtensible
	{
		private int _Index;

		private int _MagicType;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Index", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Index
		{
			get
			{
				return this._Index;
			}
			set
			{
				this._Index = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "MagicType", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MagicType
		{
			get
			{
				return this._MagicType;
			}
			set
			{
				this._MagicType = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
