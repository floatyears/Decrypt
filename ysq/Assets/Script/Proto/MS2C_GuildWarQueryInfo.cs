using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_GuildWarQueryInfo")]
	[Serializable]
	public class MS2C_GuildWarQueryInfo : IExtensible
	{
		private EGuildResult _Result;

		private EGuildWarState _Status = EGuildWarState.EGWS_Normal;

		private int _Timestamp;

		private readonly List<GuildWarClientCity> _Citys = new List<GuildWarClientCity>();

		private readonly List<GuildWarClient> _WarData = new List<GuildWarClient>();

		private int _Version;

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

		[ProtoMember(2, IsRequired = false, Name = "Status", DataFormat = DataFormat.TwosComplement), DefaultValue(EGuildWarState.EGWS_Normal)]
		public EGuildWarState Status
		{
			get
			{
				return this._Status;
			}
			set
			{
				this._Status = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "Timestamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Timestamp
		{
			get
			{
				return this._Timestamp;
			}
			set
			{
				this._Timestamp = value;
			}
		}

		[ProtoMember(4, Name = "Citys", DataFormat = DataFormat.Default)]
		public List<GuildWarClientCity> Citys
		{
			get
			{
				return this._Citys;
			}
		}

		[ProtoMember(5, Name = "WarData", DataFormat = DataFormat.Default)]
		public List<GuildWarClient> WarData
		{
			get
			{
				return this._WarData;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "Version", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Version
		{
			get
			{
				return this._Version;
			}
			set
			{
				this._Version = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
