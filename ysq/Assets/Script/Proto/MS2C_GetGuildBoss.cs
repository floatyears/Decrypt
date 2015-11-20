using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_GetGuildBoss")]
	[Serializable]
	public class MS2C_GetGuildBoss : IExtensible
	{
		private int _Result;

		private readonly List<GuildBossData> _Data = new List<GuildBossData>();

		private int _GMOpenTime;

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

		[ProtoMember(2, Name = "Data", DataFormat = DataFormat.Default)]
		public List<GuildBossData> Data
		{
			get
			{
				return this._Data;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "GMOpenTime", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int GMOpenTime
		{
			get
			{
				return this._GMOpenTime;
			}
			set
			{
				this._GMOpenTime = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
