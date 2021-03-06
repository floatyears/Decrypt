using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_BuyActivityPayShopItem")]
	[Serializable]
	public class MS2C_BuyActivityPayShopItem : IExtensible
	{
		private int _Result;

		private int _ActivityID;

		private int _ItemID;

		private int _BuyCount;

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

		[ProtoMember(2, IsRequired = false, Name = "ActivityID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ActivityID
		{
			get
			{
				return this._ActivityID;
			}
			set
			{
				this._ActivityID = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "ItemID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(4, IsRequired = false, Name = "BuyCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
