using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_BossRespawn")]
	[Serializable]
	public class MS2C_BossRespawn : IExtensible
	{
		private int _Slot;

		private int _InfoID;

		private long _MaxHP;

		private int _Scale;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Slot", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Slot
		{
			get
			{
				return this._Slot;
			}
			set
			{
				this._Slot = value;
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

		[ProtoMember(3, IsRequired = false, Name = "MaxHP", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
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

		[ProtoMember(4, IsRequired = false, Name = "Scale", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Scale
		{
			get
			{
				return this._Scale;
			}
			set
			{
				this._Scale = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
