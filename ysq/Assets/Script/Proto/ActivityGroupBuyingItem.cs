using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "ActivityGroupBuyingItem")]
	[Serializable]
	public class ActivityGroupBuyingItem : IExtensible
	{
		private int _ID;

		private int _ItemID;

		private int _ItemCount;

		private int _Diamond;

		private int _Coupon;

		private int _Limit;

		private readonly List<ActivityGroupBuyingDiscount> _Discounts = new List<ActivityGroupBuyingDiscount>();

		private int _TotalCount;

		private int _MyCount;

		private int _ItemType;

		private int _CostType;

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

		[ProtoMember(2, IsRequired = false, Name = "ItemID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ItemID
		{
			get
			{
				return this._ItemID;
			}
			set
			{
				this._ItemID = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "ItemCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ItemCount
		{
			get
			{
				return this._ItemCount;
			}
			set
			{
				this._ItemCount = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "Diamond", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Diamond
		{
			get
			{
				return this._Diamond;
			}
			set
			{
				this._Diamond = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "Coupon", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Coupon
		{
			get
			{
				return this._Coupon;
			}
			set
			{
				this._Coupon = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "Limit", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Limit
		{
			get
			{
				return this._Limit;
			}
			set
			{
				this._Limit = value;
			}
		}

		[ProtoMember(7, Name = "Discounts", DataFormat = DataFormat.Default)]
		public List<ActivityGroupBuyingDiscount> Discounts
		{
			get
			{
				return this._Discounts;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "TotalCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int TotalCount
		{
			get
			{
				return this._TotalCount;
			}
			set
			{
				this._TotalCount = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "MyCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MyCount
		{
			get
			{
				return this._MyCount;
			}
			set
			{
				this._MyCount = value;
			}
		}

		[ProtoMember(10, IsRequired = false, Name = "ItemType", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ItemType
		{
			get
			{
				return this._ItemType;
			}
			set
			{
				this._ItemType = value;
			}
		}

		[ProtoMember(11, IsRequired = false, Name = "CostType", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int CostType
		{
			get
			{
				return this._CostType;
			}
			set
			{
				this._CostType = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
