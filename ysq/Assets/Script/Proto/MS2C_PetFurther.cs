using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_PetFurther")]
	[Serializable]
	public class MS2C_PetFurther : IExtensible
	{
		private int _Result;

		private ulong _PetID;

		private uint _Further;

		private uint _PetVersion;

		private readonly List<ulong> _Pets = new List<ulong>();

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

		[ProtoMember(2, IsRequired = false, Name = "PetID", DataFormat = DataFormat.TwosComplement), DefaultValue(0f)]
		public ulong PetID
		{
			get
			{
				return this._PetID;
			}
			set
			{
				this._PetID = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "Further", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint Further
		{
			get
			{
				return this._Further;
			}
			set
			{
				this._Further = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "PetVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint PetVersion
		{
			get
			{
				return this._PetVersion;
			}
			set
			{
				this._PetVersion = value;
			}
		}

		[ProtoMember(5, Name = "Pets", DataFormat = DataFormat.TwosComplement)]
		public List<ulong> Pets
		{
			get
			{
				return this._Pets;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
