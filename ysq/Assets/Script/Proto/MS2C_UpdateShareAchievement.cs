using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_UpdateShareAchievement")]
	[Serializable]
	public class MS2C_UpdateShareAchievement : IExtensible
	{
		private readonly List<ShareAchievementData> _Data = new List<ShareAchievementData>();

		private uint _ShareVersion;

		private IExtension extensionObject;

		[ProtoMember(1, Name = "Data", DataFormat = DataFormat.Default)]
		public List<ShareAchievementData> Data
		{
			get
			{
				return this._Data;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "ShareVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
