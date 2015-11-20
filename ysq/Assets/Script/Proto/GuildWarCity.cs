using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "GuildWarCity")]
	[Serializable]
	public class GuildWarCity : IExtensible
	{
		private int _CityId;

		private EGuildWarCityState _Status = EGuildWarCityState.EGWCS_NoOwner;

		private ulong _OwnerId;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "CityId", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int CityId
		{
			get
			{
				return this._CityId;
			}
			set
			{
				this._CityId = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "Status", DataFormat = DataFormat.TwosComplement), DefaultValue(EGuildWarCityState.EGWCS_NoOwner)]
		public EGuildWarCityState Status
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

		[ProtoMember(3, IsRequired = false, Name = "OwnerId", DataFormat = DataFormat.TwosComplement), DefaultValue(0f)]
		public ulong OwnerId
		{
			get
			{
				return this._OwnerId;
			}
			set
			{
				this._OwnerId = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
