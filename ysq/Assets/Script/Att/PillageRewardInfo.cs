using ProtoBuf;
using System;
using System.ComponentModel;

namespace Att
{
	[ProtoContract(Name = "PillageRewardInfo")]
	[Serializable]
	public class PillageRewardInfo : IExtensible
	{
		private int _ID;

		private int _Type;

		private int _Value;

		private int _MinCount;

		private int _MaxCount;

		private int _Rate;

		private int _Level;

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

		[ProtoMember(2, IsRequired = false, Name = "Type", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Type
		{
			get
			{
				return this._Type;
			}
			set
			{
				this._Type = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "Value", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Value
		{
			get
			{
				return this._Value;
			}
			set
			{
				this._Value = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "MinCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MinCount
		{
			get
			{
				return this._MinCount;
			}
			set
			{
				this._MinCount = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "MaxCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MaxCount
		{
			get
			{
				return this._MaxCount;
			}
			set
			{
				this._MaxCount = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "Rate", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Rate
		{
			get
			{
				return this._Rate;
			}
			set
			{
				this._Rate = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "Level", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Level
		{
			get
			{
				return this._Level;
			}
			set
			{
				this._Level = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
