using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_UpdateBuyFundNum")]
	[Serializable]
	public class MS2C_UpdateBuyFundNum : IExtensible
	{
		private int _Num;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Num", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Num
		{
			get
			{
				return this._Num;
			}
			set
			{
				this._Num = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
