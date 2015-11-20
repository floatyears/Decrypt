using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_PvpPillageResult")]
	[Serializable]
	public class MS2C_PvpPillageResult : IExtensible
	{
		private int _Result;

		private int _ElapsedTime;

		private int _ItemID;

		private int _Money;

		private int _Exp;

		private int _ExtraMoney;

		private int _ExtraDiamond;

		private int _ExtraItemID;

		private int _ExtraItemCount;

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

		[ProtoMember(2, IsRequired = false, Name = "ElapsedTime", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ElapsedTime
		{
			get
			{
				return this._ElapsedTime;
			}
			set
			{
				this._ElapsedTime = value;
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

		[ProtoMember(4, IsRequired = false, Name = "Money", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Money
		{
			get
			{
				return this._Money;
			}
			set
			{
				this._Money = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "Exp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Exp
		{
			get
			{
				return this._Exp;
			}
			set
			{
				this._Exp = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "ExtraMoney", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ExtraMoney
		{
			get
			{
				return this._ExtraMoney;
			}
			set
			{
				this._ExtraMoney = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "ExtraDiamond", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ExtraDiamond
		{
			get
			{
				return this._ExtraDiamond;
			}
			set
			{
				this._ExtraDiamond = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "ExtraItemID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ExtraItemID
		{
			get
			{
				return this._ExtraItemID;
			}
			set
			{
				this._ExtraItemID = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "ExtraItemCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ExtraItemCount
		{
			get
			{
				return this._ExtraItemCount;
			}
			set
			{
				this._ExtraItemCount = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
