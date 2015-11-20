using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Att
{
	[ProtoContract(Name = "CostumePartyInfo")]
	[Serializable]
	public class CostumePartyInfo : IExtensible
	{
		private int _ID;

		private int _Time;

		private int _Money;

		private int _IRate;

		private int _PRate;

		private int _InteractionCost;

		private readonly List<int> _IRewardType = new List<int>();

		private readonly List<int> _IRewardValue1 = new List<int>();

		private readonly List<int> _IRewardValue2 = new List<int>();

		private readonly List<int> _PRewardType = new List<int>();

		private readonly List<int> _PRewardValue1 = new List<int>();

		private readonly List<int> _PRewardValue2 = new List<int>();

		private int _Count;

		private int _Diamond;

		private int _PetID;

		private readonly List<int> _ItemID = new List<int>();

		private readonly List<int> _ItemCount = new List<int>();

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = true, Name = "ID", DataFormat = DataFormat.TwosComplement)]
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

		[ProtoMember(2, IsRequired = false, Name = "Time", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Time
		{
			get
			{
				return this._Time;
			}
			set
			{
				this._Time = value;
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

		[ProtoMember(5, IsRequired = false, Name = "IRate", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int IRate
		{
			get
			{
				return this._IRate;
			}
			set
			{
				this._IRate = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "PRate", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int PRate
		{
			get
			{
				return this._PRate;
			}
			set
			{
				this._PRate = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "InteractionCost", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int InteractionCost
		{
			get
			{
				return this._InteractionCost;
			}
			set
			{
				this._InteractionCost = value;
			}
		}

		[ProtoMember(8, Name = "IRewardType", DataFormat = DataFormat.TwosComplement)]
		public List<int> IRewardType
		{
			get
			{
				return this._IRewardType;
			}
		}

		[ProtoMember(9, Name = "IRewardValue1", DataFormat = DataFormat.TwosComplement)]
		public List<int> IRewardValue1
		{
			get
			{
				return this._IRewardValue1;
			}
		}

		[ProtoMember(10, Name = "IRewardValue2", DataFormat = DataFormat.TwosComplement)]
		public List<int> IRewardValue2
		{
			get
			{
				return this._IRewardValue2;
			}
		}

		[ProtoMember(11, Name = "PRewardType", DataFormat = DataFormat.TwosComplement)]
		public List<int> PRewardType
		{
			get
			{
				return this._PRewardType;
			}
		}

		[ProtoMember(12, Name = "PRewardValue1", DataFormat = DataFormat.TwosComplement)]
		public List<int> PRewardValue1
		{
			get
			{
				return this._PRewardValue1;
			}
		}

		[ProtoMember(13, Name = "PRewardValue2", DataFormat = DataFormat.TwosComplement)]
		public List<int> PRewardValue2
		{
			get
			{
				return this._PRewardValue2;
			}
		}

		[ProtoMember(14, IsRequired = false, Name = "Count", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Count
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

		[ProtoMember(15, IsRequired = false, Name = "Diamond", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(16, IsRequired = false, Name = "PetID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int PetID
		{
			get
			{
				return this._PetID;
			}
			set
			{
				this._PetID = value;
			}
		}

		[ProtoMember(17, Name = "ItemID", DataFormat = DataFormat.TwosComplement)]
		public List<int> ItemID
		{
			get
			{
				return this._ItemID;
			}
		}

		[ProtoMember(18, Name = "ItemCount", DataFormat = DataFormat.TwosComplement)]
		public List<int> ItemCount
		{
			get
			{
				return this._ItemCount;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
