using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_OpenItem")]
	[Serializable]
	public class MS2C_OpenItem : IExtensible
	{
		private int _Result;

		private readonly List<OpenLootData> _Data = new List<OpenLootData>();

		private readonly List<int> _PetInfoIDs = new List<int>();

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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
