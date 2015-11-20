using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "GuildBossData")]
	[Serializable]
	public class GuildBossData : IExtensible
	{
		private int _ID;

		private int _InfoID;

		private float _HealthPct;

		private long _MaxHP;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "ID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				this._ID = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "InfoID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int InfoID
		{
			get
			{
				return this._InfoID;
			}
			set
			{
				this._InfoID = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "HealthPct", DataFormat = DataFormat.FixedSize), DefaultValue(0f)]
		public float HealthPct
		{
			get
			{
				return this._HealthPct;
			}
			set
			{
				this._HealthPct = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "MaxHP", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public long MaxHP
		{
			get
			{
				return this._MaxHP;
			}
			set
			{
				this._MaxHP = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
