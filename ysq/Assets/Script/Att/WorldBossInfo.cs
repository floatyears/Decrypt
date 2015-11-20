using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Att
{
	[ProtoContract(Name = "WorldBossInfo")]
	[Serializable]
	public class WorldBossInfo : IExtensible
	{
		private int _ID;

		private int _HighRank;

		private int _LowRank;

		private readonly List<int> _RewardType = new List<int>();

		private readonly List<int> _RewardValue1 = new List<int>();

		private readonly List<int> _RewardValue2 = new List<int>();

		private readonly List<int> _KillRewardType = new List<int>();

		private readonly List<int> _KillRewardValue1 = new List<int>();

		private readonly List<int> _KillRewardValue2 = new List<int>();

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

		[ProtoMember(2, IsRequired = false, Name = "HighRank", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int HighRank
		{
			get
			{
				return this._HighRank;
			}
			set
			{
				this._HighRank = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "LowRank", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int LowRank
		{
			get
			{
				return this._LowRank;
			}
			set
			{
				this._LowRank = value;
			}
		}

		[ProtoMember(4, Name = "RewardType", DataFormat = DataFormat.TwosComplement)]
		public List<int> RewardType
		{
			get
			{
				return this._RewardType;
			}
		}

		[ProtoMember(5, Name = "RewardValue1", DataFormat = DataFormat.TwosComplement)]
		public List<int> RewardValue1
		{
			get
			{
				return this._RewardValue1;
			}
		}

		[ProtoMember(6, Name = "RewardValue2", DataFormat = DataFormat.TwosComplement)]
		public List<int> RewardValue2
		{
			get
			{
				return this._RewardValue2;
			}
		}

		[ProtoMember(7, Name = "KillRewardType", DataFormat = DataFormat.TwosComplement)]
		public List<int> KillRewardType
		{
			get
			{
				return this._KillRewardType;
			}
		}

		[ProtoMember(8, Name = "KillRewardValue1", DataFormat = DataFormat.TwosComplement)]
		public List<int> KillRewardValue1
		{
			get
			{
				return this._KillRewardValue1;
			}
		}

		[ProtoMember(9, Name = "KillRewardValue2", DataFormat = DataFormat.TwosComplement)]
		public List<int> KillRewardValue2
		{
			get
			{
				return this._KillRewardValue2;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
