using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_LuckyRoll")]
	[Serializable]
	public class MS2C_LuckyRoll : IExtensible
	{
		private int _Result;

		private readonly List<OpenLootData> _Data = new List<OpenLootData>();

		private readonly List<ulong> _PetIDs = new List<ulong>();

		private int _Type;

		private int _LuckyDrawFreeTimestamp;

		private int _LuckyDrawScore;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Result", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Result
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

		[ProtoMember(2, Name = "Data", DataFormat = DataFormat.Default)]
		public List<OpenLootData> Data
		{
			get
			{
				return this._Data;
			}
		}

		[ProtoMember(3, Name = "PetIDs", DataFormat = DataFormat.TwosComplement)]
		public List<ulong> PetIDs
		{
			get
			{
				return this._PetIDs;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "Type", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(5, IsRequired = false, Name = "LuckyDrawFreeTimestamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int LuckyDrawFreeTimestamp
		{
			get
			{
				return this._LuckyDrawFreeTimestamp;
			}
			set
			{
				this._LuckyDrawFreeTimestamp = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "LuckyDrawScore", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int LuckyDrawScore
		{
			get
			{
				return this._LuckyDrawScore;
			}
			set
			{
				this._LuckyDrawScore = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
