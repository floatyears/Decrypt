using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Att
{
	[ProtoContract(Name = "MagicLoveInfo")]
	[Serializable]
	public class MagicLoveInfo : IExtensible
	{
		private int _ID;

		private readonly List<int> _Pet = new List<int>();

		private int _LoveValue;

		private int _Fragment;

		private readonly List<int> _Rate = new List<int>();

		private int _RewardLoveValue;

		private int _Cost;

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

		[ProtoMember(2, Name = "Pet", DataFormat = DataFormat.TwosComplement)]
		public List<int> Pet
		{
			get
			{
				return this._Pet;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "LoveValue", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int LoveValue
		{
			get
			{
				return this._LoveValue;
			}
			set
			{
				this._LoveValue = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "Fragment", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Fragment
		{
			get
			{
				return this._Fragment;
			}
			set
			{
				this._Fragment = value;
			}
		}

		[ProtoMember(5, Name = "Rate", DataFormat = DataFormat.TwosComplement)]
		public List<int> Rate
		{
			get
			{
				return this._Rate;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "RewardLoveValue", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int RewardLoveValue
		{
			get
			{
				return this._RewardLoveValue;
			}
			set
			{
				this._RewardLoveValue = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "Cost", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Cost
		{
			get
			{
				return this._Cost;
			}
			set
			{
				this._Cost = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
