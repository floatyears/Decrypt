using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "ActivityNationalDayItem")]
	[Serializable]
	public class ActivityNationalDayItem : IExtensible
	{
		private int _ID;

		private readonly List<RewardData> _SrcItem = new List<RewardData>();

		private readonly List<RewardData> _Rewards = new List<RewardData>();

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "ID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(2, Name = "SrcItem", DataFormat = DataFormat.Default)]
		public List<RewardData> SrcItem
		{
			get
			{
				return this._SrcItem;
			}
		}

		[ProtoMember(3, Name = "Rewards", DataFormat = DataFormat.Default)]
		public List<RewardData> Rewards
		{
			get
			{
				return this._Rewards;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
