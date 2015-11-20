using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_ShopBuyItem")]
	[Serializable]
	public class MS2C_ShopBuyItem : IExtensible
	{
		private int _Result;

		private int _ID;

		private uint _BuyCount;

		private uint _ShopVersion;

		private int _ShopType;

		private int _TimeStamp;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Result", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Result
		{
			get
			{
				return this._Result;
			}
			set
			{
				this._Result = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "ID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(3, IsRequired = false, Name = "BuyCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
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

		[ProtoMember(4, IsRequired = false, Name = "ShopVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint ShopVersion
		{
			get
			{
				return this._ShopVersion;
			}
			set
			{
				this._ShopVersion = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "ShopType", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ShopType
		{
			get
			{
				return this._ShopType;
			}
			set
			{
				this._ShopType = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "TimeStamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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
