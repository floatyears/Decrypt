using ProtoBuf;
using System;
using System.Collections.Generic;

namespace Att
{
	[ProtoContract(Name = "DayHotInfo")]
	[Serializable]
	public class DayHotInfo : IExtensible
	{
		private int _ID;

		private readonly List<int> _BeginHour = new List<int>();

		private readonly List<int> _EndHour = new List<int>();

		private readonly List<int> _RewardType = new List<int>();

		private readonly List<int> _RewardValue1 = new List<int>();

		private readonly List<int> _RewardValue2 = new List<int>();

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

		[ProtoMember(2, Name = "BeginHour", DataFormat = DataFormat.TwosComplement)]
		public List<int> BeginHour
		{
			get
			{
				return this._BeginHour;
			}
		}

		[ProtoMember(3, Name = "EndHour", DataFormat = DataFormat.TwosComplement)]
		public List<int> EndHour
		{
			get
			{
				return this._EndHour;
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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
