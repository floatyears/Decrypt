using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Att
{
	[ProtoContract(Name = "AwakeInfo")]
	[Serializable]
	public class AwakeInfo : IExtensible
	{
		private int _ID;

		private readonly List<int> _ItemID = new List<int>();

		private int _PetCount;

		private int _ItemCount;

		private int _Money;

		private int _Attack;

		private int _Defense;

		private int _MaxHP;

		private int _AttPct;

		private int _PlayerItemCount;

		private int _PlayerAttack;

		private int _PlayerDefense;

		private int _PlayerMaxHP;

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

		[ProtoMember(2, Name = "ItemID", DataFormat = DataFormat.TwosComplement)]
		public List<int> ItemID
		{
			get
			{
				return this._ItemID;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "PetCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int PetCount
		{
			get
			{
				return this._PetCount;
			}
			set
			{
				this._PetCount = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "ItemCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(5, IsRequired = false, Name = "Money", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(6, IsRequired = false, Name = "Attack", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Attack
		{
			get
			{
				return this._Attack;
			}
			set
			{
				this._Attack = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "Defense", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Defense
		{
			get
			{
				return this._Defense;
			}
			set
			{
				this._Defense = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "MaxHP", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MaxHP
		{
			get
			{
				return this._MaxHP;
			}
			set
			{
				this._MaxHP = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "AttPct", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int AttPct
		{
			get
			{
				return this._AttPct;
			}
			set
			{
				this._AttPct = value;
			}
		}

		[ProtoMember(10, IsRequired = false, Name = "PlayerItemCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int PlayerItemCount
		{
			get
			{
				return this._PlayerItemCount;
			}
			set
			{
				this._PlayerItemCount = value;
			}
		}

		[ProtoMember(11, IsRequired = false, Name = "PlayerAttack", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int PlayerAttack
		{
			get
			{
				return this._PlayerAttack;
			}
			set
			{
				this._PlayerAttack = value;
			}
		}

		[ProtoMember(12, IsRequired = false, Name = "PlayerDefense", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int PlayerDefense
		{
			get
			{
				return this._PlayerDefense;
			}
			set
			{
				this._PlayerDefense = value;
			}
		}

		[ProtoMember(13, IsRequired = false, Name = "PlayerMaxHP", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int PlayerMaxHP
		{
			get
			{
				return this._PlayerMaxHP;
			}
			set
			{
				this._PlayerMaxHP = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
