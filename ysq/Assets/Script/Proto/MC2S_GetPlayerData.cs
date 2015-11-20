using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_GetPlayerData")]
	[Serializable]
	public class MC2S_GetPlayerData : IExtensible
	{
		private uint _StatsVersion;

		private uint _SocketVersion;

		private uint _ItemVersion;

		private uint _FashionVersion;

		private uint _PetVersion;

		private uint _SceneVersion;

		private uint _MapRewardVersion;

		private uint _AchievementVersion;

		private uint _MailVersion;

		private uint _BuyDataVersion;

		private uint _SevenDayVersion;

		private uint _ShareVersion;

		private uint _ActivityAchievementVersion;

		private uint _ActivityValueVersion;

		private uint _ActivityShopVersion;

		private uint _LopetVersion;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "StatsVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint StatsVersion
		{
			get
			{
				return this._StatsVersion;
			}
			set
			{
				this._StatsVersion = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "SocketVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint SocketVersion
		{
			get
			{
				return this._SocketVersion;
			}
			set
			{
				this._SocketVersion = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "ItemVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint ItemVersion
		{
			get
			{
				return this._ItemVersion;
			}
			set
			{
				this._ItemVersion = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "FashionVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint FashionVersion
		{
			get
			{
				return this._FashionVersion;
			}
			set
			{
				this._FashionVersion = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "PetVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint PetVersion
		{
			get
			{
				return this._PetVersion;
			}
			set
			{
				this._PetVersion = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "SceneVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint SceneVersion
		{
			get
			{
				return this._SceneVersion;
			}
			set
			{
				this._SceneVersion = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "MapRewardVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint MapRewardVersion
		{
			get
			{
				return this._MapRewardVersion;
			}
			set
			{
				this._MapRewardVersion = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "AchievementVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint AchievementVersion
		{
			get
			{
				return this._AchievementVersion;
			}
			set
			{
				this._AchievementVersion = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "MailVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint MailVersion
		{
			get
			{
				return this._MailVersion;
			}
			set
			{
				this._MailVersion = value;
			}
		}

		[ProtoMember(10, IsRequired = false, Name = "BuyDataVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint BuyDataVersion
		{
			get
			{
				return this._BuyDataVersion;
			}
			set
			{
				this._BuyDataVersion = value;
			}
		}

		[ProtoMember(11, IsRequired = false, Name = "SevenDayVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint SevenDayVersion
		{
			get
			{
				return this._SevenDayVersion;
			}
			set
			{
				this._SevenDayVersion = value;
			}
		}

		[ProtoMember(12, IsRequired = false, Name = "ShareVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint ShareVersion
		{
			get
			{
				return this._ShareVersion;
			}
			set
			{
				this._ShareVersion = value;
			}
		}

		[ProtoMember(13, IsRequired = false, Name = "ActivityAchievementVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint ActivityAchievementVersion
		{
			get
			{
				return this._ActivityAchievementVersion;
			}
			set
			{
				this._ActivityAchievementVersion = value;
			}
		}

		[ProtoMember(14, IsRequired = false, Name = "ActivityValueVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint ActivityValueVersion
		{
			get
			{
				return this._ActivityValueVersion;
			}
			set
			{
				this._ActivityValueVersion = value;
			}
		}

		[ProtoMember(15, IsRequired = false, Name = "ActivityShopVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint ActivityShopVersion
		{
			get
			{
				return this._ActivityShopVersion;
			}
			set
			{
				this._ActivityShopVersion = value;
			}
		}

		[ProtoMember(16, IsRequired = false, Name = "LopetVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint LopetVersion
		{
			get
			{
				return this._LopetVersion;
			}
			set
			{
				this._LopetVersion = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
