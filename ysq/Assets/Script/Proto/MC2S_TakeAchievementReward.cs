using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_TakeAchievementReward")]
	[Serializable]
	public class MC2S_TakeAchievementReward : IExtensible
	{
		private int _AchievementID;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "AchievementID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
