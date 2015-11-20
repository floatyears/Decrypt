using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_AchievementUpdate")]
	[Serializable]
	public class MS2C_AchievementUpdate : IExtensible
	{
		private readonly List<AchievementData> _Data = new List<AchievementData>();

		private uint _AchievementVersion;

		private IExtension extensionObject;

		[ProtoMember(1, Name = "Data", DataFormat = DataFormat.Default)]
		public List<AchievementData> Data
		{
			get
			{
				return this._Data;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "AchievementVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
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
