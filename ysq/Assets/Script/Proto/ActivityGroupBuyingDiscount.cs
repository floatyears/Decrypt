using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "ActivityGroupBuyingDiscount")]
	[Serializable]
	public class ActivityGroupBuyingDiscount : IExtensible
	{
		private int _BuyCount;

		private int _Discount;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "BuyCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int BuyCount
		{
			get
			{
				return this._BuyCount;
			}
			set
			{
				this._BuyCount = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "Discount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Discount
		{
			get
			{
				return this._Discount;
			}
			set
			{
				this._Discount = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
