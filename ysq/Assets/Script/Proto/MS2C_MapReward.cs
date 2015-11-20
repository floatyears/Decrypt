using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_MapReward")]
	[Serializable]
	public class MS2C_MapReward : IExtensible
	{
		private int _MapID;

		private int _Mask;

		private uint _MapRewardVersion;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "MapID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MapID
		{
			get
			{
				return this._MapID;
			}
			set
			{
				this._MapID = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "Mask", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Mask
		{
			get
			{
				return this._Mask;
			}
			set
			{
				this._Mask = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "MapRewardVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint MapRewardVersion
		{
			get
			{
				return this._MapRewardVersion;
			}
			set
			{
				this._MapRewardVersion = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
