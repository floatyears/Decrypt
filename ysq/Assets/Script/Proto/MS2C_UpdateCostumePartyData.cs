using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_UpdateCostumePartyData")]
	[Serializable]
	public class MS2C_UpdateCostumePartyData : IExtensible
	{
		private int _CDType;

		private int _CD;

		private int _HasReward;

		private int _CarnivalType;

		private int _Count;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "CDType", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int CDType
		{
			get
			{
				return this._CDType;
			}
			set
			{
				this._CDType = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "CD", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int CD
		{
			get
			{
				return this._CD;
			}
			set
			{
				this._CD = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "HasReward", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int HasReward
		{
			get
			{
				return this._HasReward;
			}
			set
			{
				this._HasReward = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "CarnivalType", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int CarnivalType
		{
			get
			{
				return this._CarnivalType;
			}
			set
			{
				this._CarnivalType = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "Count", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
