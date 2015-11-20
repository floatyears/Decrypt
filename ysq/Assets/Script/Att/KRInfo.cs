using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Att
{
	[ProtoContract(Name = "KRInfo")]
	[Serializable]
	public class KRInfo : IExtensible
	{
		private int _ID;

		private int _MinLevel;

		private int _MaxLevel;

		private readonly List<int> _RewardID = new List<int>();

		private readonly List<int> _Rate = new List<int>();

		private readonly List<int> _QuestID = new List<int>();

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

		[ProtoMember(3, IsRequired = false, Name = "MaxLevel", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MaxLevel
		{
			get
			{
				return this._MaxLevel;
			}
			set
			{
				this._MaxLevel = value;
			}
		}

		[ProtoMember(4, Name = "RewardID", DataFormat = DataFormat.TwosComplement)]
		public List<int> RewardID
		{
			get
			{
				return this._RewardID;
			}
		}

		[ProtoMember(5, Name = "Rate", DataFormat = DataFormat.TwosComplement)]
		public List<int> Rate
		{
			get
			{
				return this._Rate;
			}
		}

		[ProtoMember(6, Name = "QuestID", DataFormat = DataFormat.TwosComplement)]
		public List<int> QuestID
		{
			get
			{
				return this._QuestID;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
