using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_UpdateSevenDayReward")]
	[Serializable]
	public class MS2C_UpdateSevenDayReward : IExtensible
	{
		private readonly List<SevenDayRewardData> _Data = new List<SevenDayRewardData>();

		private uint _SevenDayVersion;

		private IExtension extensionObject;

		[ProtoMember(1, Name = "Data", DataFormat = DataFormat.Default)]
		public List<SevenDayRewardData> Data
		{
			get
			{
				return this._Data;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "SevenDayVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
