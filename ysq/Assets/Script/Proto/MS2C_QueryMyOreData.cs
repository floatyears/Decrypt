using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_QueryMyOreData")]
	[Serializable]
	public class MS2C_QueryMyOreData : IExtensible
	{
		private int _Result;

		private int _Amount1;

		private int _Amount2;

		private int _RevengeCount;

		private readonly List<OrePillageRecord> _Data = new List<OrePillageRecord>();

		private int _BuyRevengeCount;

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

		[ProtoMember(2, IsRequired = false, Name = "Amount1", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Amount1
		{
			get
			{
				return this._Amount1;
			}
			set
			{
				this._Amount1 = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "Amount2", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Amount2
		{
			get
			{
				return this._Amount2;
			}
			set
			{
				this._Amount2 = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "RevengeCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int RevengeCount
		{
			get
			{
				return this._RevengeCount;
			}
			set
			{
				this._RevengeCount = value;
			}
		}

		[ProtoMember(5, Name = "Data", DataFormat = DataFormat.Default)]
		public List<OrePillageRecord> Data
		{
			get
			{
				return this._Data;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "BuyRevengeCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int BuyRevengeCount
		{
			get
			{
				return this._BuyRevengeCount;
			}
			set
			{
				this._BuyRevengeCount = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
