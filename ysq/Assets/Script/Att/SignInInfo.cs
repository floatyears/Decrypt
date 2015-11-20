using ProtoBuf;
using System;
using System.ComponentModel;

namespace Att
{
	[ProtoContract(Name = "SignInInfo")]
	[Serializable]
	public class SignInInfo : IExtensible
	{
		private int _ID;

		private int _RewardType;

		private int _RewardValue1;

		private int _RewardValue2;

		private int _VipLevel;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = true, Name = "ID", DataFormat = DataFormat.TwosComplement)]
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

		[ProtoMember(2, IsRequired = false, Name = "RewardType", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int RewardType
		{
			get
			{
				return this._RewardType;
			}
			set
			{
				this._RewardType = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "RewardValue1", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int RewardValue1
		{
			get
			{
				return this._RewardValue1;
			}
			set
			{
				this._RewardValue1 = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "RewardValue2", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int RewardValue2
		{
			get
			{
				return this._RewardValue2;
			}
			set
			{
				this._RewardValue2 = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "VipLevel", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int VipLevel
		{
			get
			{
				return this._VipLevel;
			}
			set
			{
				this._VipLevel = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
