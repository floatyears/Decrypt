using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_GuildWarGetSupportInfo")]
	[Serializable]
	public class MS2C_GuildWarGetSupportInfo : IExtensible
	{
		private EGuildResult _Result;

		private readonly List<GuildWarClientSupportInfo> _SupportInfo = new List<GuildWarClientSupportInfo>();

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Result", DataFormat = DataFormat.TwosComplement), DefaultValue(EGuildResult.EGR_Success)]
		public EGuildResult Result
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

		[ProtoMember(2, Name = "SupportInfo", DataFormat = DataFormat.Default)]
		public List<GuildWarClientSupportInfo> SupportInfo
		{
			get
			{
				return this._SupportInfo;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
