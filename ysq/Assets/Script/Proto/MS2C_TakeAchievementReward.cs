using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_TakeAchievementReward")]
	[Serializable]
	public class MS2C_TakeAchievementReward : IExtensible
	{
		private int _Result;

		private int _AchievementID;

		private int _TimeStamp;

		private uint _AchievementVersion;

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

		[ProtoMember(2, IsRequired = false, Name = "AchievementID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int AchievementID
		{
			get
			{
				return this._AchievementID;
			}
			set
			{
				this._AchievementID = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "TimeStamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int TimeStamp
		{
			get
			{
				return this._TimeStamp;
			}
			set
			{
				this._TimeStamp = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "AchievementVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
