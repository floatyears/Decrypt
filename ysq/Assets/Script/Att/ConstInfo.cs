using ProtoBuf;
using System;
using System.ComponentModel;

namespace Att
{
	[ProtoContract(Name = "ConstInfo")]
	[Serializable]
	public class ConstInfo : IExtensible
	{
		private int _ID;

		private int _IntValue;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = true, Name = "ID", DataFormat = DataFormat.TwosComplement)]
		public int ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				this._ID = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "IntValue", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int IntValue
		{
			get
			{
				return this._IntValue;
			}
			set
			{
				this._IntValue = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
