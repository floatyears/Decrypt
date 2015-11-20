using ProtoBuf;
using System;
using System.ComponentModel;

namespace Att
{
	[ProtoContract(Name = "KRRewardInfo")]
	[Serializable]
	public class KRRewardInfo : IExtensible
	{
		private int _ID;

		private int _MagicSoul;

		private int _Money;

		private int _Diamond;

		private int _ItemID;

		private int _Count;

		private int _KingMedal;

		private int _LopetSoul;

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

		[ProtoMember(2, IsRequired = false, Name = "MagicSoul", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MagicSoul
		{
			get
			{
				return this._MagicSoul;
			}
			set
			{
				this._MagicSoul = value;
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

		[ProtoMember(5, IsRequired = false, Name = "Diamond", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(6, IsRequired = false, Name = "ItemID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(7, IsRequired = false, Name = "Count", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(8, IsRequired = false, Name = "KingMedal", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int KingMedal
		{
			get
			{
				return this._KingMedal;
			}
			set
			{
				this._KingMedal = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "LopetSoul", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int LopetSoul
		{
			get
			{
				return this._LopetSoul;
			}
			set
			{
				this._LopetSoul = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
