using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "APItemData")]
	[Serializable]
	public class APItemData : IExtensible
	{
		private int _ID;

		private int _Value;

		private int _OffPrice;

		private int _Price;

		private int _MaxCount;

		private int _BuyCount;

		private int _GiftID;

		private readonly List<RewardData> _Data = new List<RewardData>();

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "ID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(2, IsRequired = false, Name = "Value", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Value
		{
			get
			{
				return this._Value;
			}
			set
			{
				this._Value = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "OffPrice", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int OffPrice
		{
			get
			{
				return this._OffPrice;
			}
			set
			{
				this._OffPrice = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "Price", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Price
		{
			get
			{
				return this._Price;
			}
			set
			{
				this._Price = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "MaxCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MaxCount
		{
			get
			{
				return this._MaxCount;
			}
			set
			{
				this._MaxCount = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "BuyCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(7, IsRequired = false, Name = "GiftID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int GiftID
		{
			get
			{
				return this._GiftID;
			}
			set
			{
				this._GiftID = value;
			}
		}

		[ProtoMember(8, Name = "Data", DataFormat = DataFormat.Default)]
		public List<RewardData> Data
		{
			get
			{
				return this._Data;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
