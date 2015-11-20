using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Att
{
	[ProtoContract(Name = "MGInfo")]
	[Serializable]
	public class MGInfo : IExtensible
	{
		private int _ID;

		private int _MinLevel;

		private int _MaxValue;

		private int _CombatValue;

		private int _MinFragmentCount;

		private readonly List<int> _RewardType = new List<int>();

		private readonly List<int> _RewardValue1 = new List<int>();

		private readonly List<int> _RewardValue2 = new List<int>();

		private int _TowerID1;

		private int _TowerID2;

		private int _TowerID3;

		private readonly List<int> _RespawnID = new List<int>();

		private readonly List<float> _Delay = new List<float>();

		private int _MaxFragmentCount;

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

		[ProtoMember(2, IsRequired = false, Name = "MinLevel", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MinLevel
		{
			get
			{
				return this._MinLevel;
			}
			set
			{
				this._MinLevel = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "MaxValue", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MaxValue
		{
			get
			{
				return this._MaxValue;
			}
			set
			{
				this._MaxValue = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "CombatValue", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int CombatValue
		{
			get
			{
				return this._CombatValue;
			}
			set
			{
				this._CombatValue = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "MinFragmentCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MinFragmentCount
		{
			get
			{
				return this._MinFragmentCount;
			}
			set
			{
				this._MinFragmentCount = value;
			}
		}

		[ProtoMember(6, Name = "RewardType", DataFormat = DataFormat.TwosComplement)]
		public List<int> RewardType
		{
			get
			{
				return this._RewardType;
			}
		}

		[ProtoMember(7, Name = "RewardValue1", DataFormat = DataFormat.TwosComplement)]
		public List<int> RewardValue1
		{
			get
			{
				return this._RewardValue1;
			}
		}

		[ProtoMember(8, Name = "RewardValue2", DataFormat = DataFormat.TwosComplement)]
		public List<int> RewardValue2
		{
			get
			{
				return this._RewardValue2;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "TowerID1", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int TowerID1
		{
			get
			{
				return this._TowerID1;
			}
			set
			{
				this._TowerID1 = value;
			}
		}

		[ProtoMember(10, IsRequired = false, Name = "TowerID2", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int TowerID2
		{
			get
			{
				return this._TowerID2;
			}
			set
			{
				this._TowerID2 = value;
			}
		}

		[ProtoMember(11, IsRequired = false, Name = "TowerID3", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int TowerID3
		{
			get
			{
				return this._TowerID3;
			}
			set
			{
				this._TowerID3 = value;
			}
		}

		[ProtoMember(12, Name = "RespawnID", DataFormat = DataFormat.TwosComplement)]
		public List<int> RespawnID
		{
			get
			{
				return this._RespawnID;
			}
		}

		[ProtoMember(13, Name = "Delay", DataFormat = DataFormat.FixedSize)]
		public List<float> Delay
		{
			get
			{
				return this._Delay;
			}
		}

		[ProtoMember(14, IsRequired = false, Name = "MaxFragmentCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MaxFragmentCount
		{
			get
			{
				return this._MaxFragmentCount;
			}
			set
			{
				this._MaxFragmentCount = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
