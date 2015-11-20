using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MagicLoveData")]
	[Serializable]
	public class MagicLoveData : IExtensible
	{
		private readonly List<int> _PetID = new List<int>();

		private readonly List<int> _LoveValue = new List<int>();

		private int _MagicMatchCount;

		private int _MagicMatchBuyCount;

		private int _LoseCount;

		private int _Bout;

		private readonly List<int> _RewardFlag = new List<int>();

		private int _LastSelfMagicType;

		private int _LastTargetMagicType;

		private int _LastIndex;

		private int _Timestamp;

		private IExtension extensionObject;

		[ProtoMember(1, Name = "PetID", DataFormat = DataFormat.TwosComplement)]
		public List<int> PetID
		{
			get
			{
				return this._PetID;
			}
		}

		[ProtoMember(2, Name = "LoveValue", DataFormat = DataFormat.TwosComplement)]
		public List<int> LoveValue
		{
			get
			{
				return this._LoveValue;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "MagicMatchCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MagicMatchCount
		{
			get
			{
				return this._MagicMatchCount;
			}
			set
			{
				this._MagicMatchCount = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "MagicMatchBuyCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MagicMatchBuyCount
		{
			get
			{
				return this._MagicMatchBuyCount;
			}
			set
			{
				this._MagicMatchBuyCount = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "LoseCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int LoseCount
		{
			get
			{
				return this._LoseCount;
			}
			set
			{
				this._LoseCount = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "Bout", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Bout
		{
			get
			{
				return this._Bout;
			}
			set
			{
				this._Bout = value;
			}
		}

		[ProtoMember(7, Name = "RewardFlag", DataFormat = DataFormat.TwosComplement)]
		public List<int> RewardFlag
		{
			get
			{
				return this._RewardFlag;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "LastSelfMagicType", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int LastSelfMagicType
		{
			get
			{
				return this._LastSelfMagicType;
			}
			set
			{
				this._LastSelfMagicType = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "LastTargetMagicType", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int LastTargetMagicType
		{
			get
			{
				return this._LastTargetMagicType;
			}
			set
			{
				this._LastTargetMagicType = value;
			}
		}

		[ProtoMember(10, IsRequired = false, Name = "LastIndex", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int LastIndex
		{
			get
			{
				return this._LastIndex;
			}
			set
			{
				this._LastIndex = value;
			}
		}

		[ProtoMember(11, IsRequired = false, Name = "Timestamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Timestamp
		{
			get
			{
				return this._Timestamp;
			}
			set
			{
				this._Timestamp = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
