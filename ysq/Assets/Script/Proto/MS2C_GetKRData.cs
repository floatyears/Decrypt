using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_GetKRData")]
	[Serializable]
	public class MS2C_GetKRData : IExtensible
	{
		private int _Result;

		private int _Count;

		private readonly List<int> _QuestID = new List<int>();

		private readonly List<int> _RewardID = new List<int>();

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

		[ProtoMember(2, IsRequired = false, Name = "Count", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(3, Name = "QuestID", DataFormat = DataFormat.TwosComplement)]
		public List<int> QuestID
		{
			get
			{
				return this._QuestID;
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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
