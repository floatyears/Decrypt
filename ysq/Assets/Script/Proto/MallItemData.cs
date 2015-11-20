using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MallItemData")]
	[Serializable]
	public class MallItemData : IExtensible
	{
		private int _ID;

		private int _InfoID;

		private uint _Price;

		private uint _OffPrice;

		private uint _Count;

		private uint _VipLevel;

		private uint _BuyCount;

		private int _TimeStamp;

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

		[ProtoMember(2, IsRequired = false, Name = "InfoID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int InfoID
		{
			get
			{
				return this._InfoID;
			}
			set
			{
				this._InfoID = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "Price", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint Price
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

		[ProtoMember(4, IsRequired = false, Name = "OffPrice", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint OffPrice
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

		[ProtoMember(5, IsRequired = false, Name = "Count", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint Count
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

		[ProtoMember(6, IsRequired = false, Name = "VipLevel", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint VipLevel
		{
			get
			{
				return this._VipLevel;
			}
			set
			{
				this._VipLevel = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "BuyCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint BuyCount
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

		[ProtoMember(8, IsRequired = false, Name = "TimeStamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int TimeStamp
		{
			get
			{
				return this._TimeStamp;
			}
			set
			{
				this._TimeStamp = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
