using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_DoGuildBossDamage")]
	[Serializable]
	public class MC2S_DoGuildBossDamage : IExtensible
	{
		private int _ID;

		private int _ResultKey;

		private long _Damage;

		private int _Count;

		private int _RecvStartTime;

		private int _SendResultTime;

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

		[ProtoMember(2, IsRequired = false, Name = "ResultKey", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ResultKey
		{
			get
			{
				return this._ResultKey;
			}
			set
			{
				this._ResultKey = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "Damage", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public long Damage
		{
			get
			{
				return this._Damage;
			}
			set
			{
				this._Damage = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "Count", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Count
		{
			get
			{
				return this._Count;
			}
			set
			{
				this._Count = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "RecvStartTime", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int RecvStartTime
		{
			get
			{
				return this._RecvStartTime;
			}
			set
			{
				this._RecvStartTime = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "SendResultTime", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int SendResultTime
		{
			get
			{
				return this._SendResultTime;
			}
			set
			{
				this._SendResultTime = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
