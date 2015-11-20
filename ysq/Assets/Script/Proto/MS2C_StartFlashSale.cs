using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_StartFlashSale")]
	[Serializable]
	public class MS2C_StartFlashSale : IExtensible
	{
		private int _Result;

		private readonly List<OpenLootData> _Data = new List<OpenLootData>();

		private readonly List<int> _PetInfoIDs = new List<int>();

		private readonly List<OpenLootData> _Pet2Items = new List<OpenLootData>();

		private int _Slot;

		private int _Count;

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

		[ProtoMember(3, Name = "PetInfoIDs", DataFormat = DataFormat.TwosComplement)]
		public List<int> PetInfoIDs
		{
			get
			{
				return this._PetInfoIDs;
			}
		}

		[ProtoMember(4, Name = "Pet2Items", DataFormat = DataFormat.Default)]
		public List<OpenLootData> Pet2Items
		{
			get
			{
				return this._Pet2Items;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "Slot", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(6, IsRequired = false, Name = "Count", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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
