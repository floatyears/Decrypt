using ProtoBuf;
using System;
using System.ComponentModel;

namespace Att
{
	[ProtoContract(Name = "PvpInfo")]
	[Serializable]
	public class PvpInfo : IExtensible
	{
		private int _ID;

		private int _ArenaHighRank;

		private int _ArenaLowRank;

		private int _ArenaRewardDiamond;

		private int _ArenaRewardMoney;

		private int _ArenaRewardHonor;

		private int _ArenaRewardItemID;

		private int _ArenaRewardItemCount;

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

		[ProtoMember(2, IsRequired = false, Name = "ArenaHighRank", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ArenaHighRank
		{
			get
			{
				return this._ArenaHighRank;
			}
			set
			{
				this._ArenaHighRank = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "ArenaLowRank", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ArenaLowRank
		{
			get
			{
				return this._ArenaLowRank;
			}
			set
			{
				this._ArenaLowRank = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "ArenaRewardDiamond", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ArenaRewardDiamond
		{
			get
			{
				return this._ArenaRewardDiamond;
			}
			set
			{
				this._ArenaRewardDiamond = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "ArenaRewardMoney", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ArenaRewardMoney
		{
			get
			{
				return this._ArenaRewardMoney;
			}
			set
			{
				this._ArenaRewardMoney = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "ArenaRewardHonor", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ArenaRewardHonor
		{
			get
			{
				return this._ArenaRewardHonor;
			}
			set
			{
				this._ArenaRewardHonor = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "ArenaRewardItemID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ArenaRewardItemID
		{
			get
			{
				return this._ArenaRewardItemID;
			}
			set
			{
				this._ArenaRewardItemID = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "ArenaRewardItemCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ArenaRewardItemCount
		{
			get
			{
				return this._ArenaRewardItemCount;
			}
			set
			{
				this._ArenaRewardItemCount = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
