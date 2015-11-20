using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_GetLootReward")]
	[Serializable]
	public class MS2C_GetLootReward : IExtensible
	{
		private int _ID;

		private readonly List<RewardData> _LootReward = new List<RewardData>();

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

		[ProtoMember(2, Name = "LootReward", DataFormat = DataFormat.Default)]
		public List<RewardData> LootReward
		{
			get
			{
				return this._LootReward;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
