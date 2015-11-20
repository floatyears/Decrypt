using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_PveResult")]
	[Serializable]
	public class MS2C_PveResult : IExtensible
	{
		private int _Result;

		private int _LootMoney;

		private int _LootExp;

		private readonly List<OpenLootData> _Items = new List<OpenLootData>();

		private int _LootDiamond;

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

		[ProtoMember(2, IsRequired = false, Name = "LootMoney", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int LootMoney
		{
			get
			{
				return this._LootMoney;
			}
			set
			{
				this._LootMoney = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "LootExp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int LootExp
		{
			get
			{
				return this._LootExp;
			}
			set
			{
				this._LootExp = value;
			}
		}

		[ProtoMember(4, Name = "Items", DataFormat = DataFormat.Default)]
		public List<OpenLootData> Items
		{
			get
			{
				return this._Items;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "LootDiamond", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int LootDiamond
		{
			get
			{
				return this._LootDiamond;
			}
			set
			{
				this._LootDiamond = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
