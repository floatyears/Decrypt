using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "ActivitySpecifyPayItem")]
	[Serializable]
	public class ActivitySpecifyPayItem : IExtensible
	{
		private int _ProductID;

		private int _GiftID;

		private readonly List<RewardData> _Data = new List<RewardData>();

		private int _PayCount;

		private int _RewardCount;

		private int _MaxCount;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "ProductID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ProductID
		{
			get
			{
				return this._ProductID;
			}
			set
			{
				this._ProductID = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "GiftID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int GiftID
		{
			get
			{
				return this._GiftID;
			}
			set
			{
				this._GiftID = value;
			}
		}

		[ProtoMember(3, Name = "Data", DataFormat = DataFormat.Default)]
		public List<RewardData> Data
		{
			get
			{
				return this._Data;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "PayCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int PayCount
		{
			get
			{
				return this._PayCount;
			}
			set
			{
				this._PayCount = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "RewardCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int RewardCount
		{
			get
			{
				return this._RewardCount;
			}
			set
			{
				this._RewardCount = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "MaxCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MaxCount
		{
			get
			{
				return this._MaxCount;
			}
			set
			{
				this._MaxCount = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
