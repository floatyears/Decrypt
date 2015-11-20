using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_PetLevelup")]
	[Serializable]
	public class MS2C_PetLevelup : IExtensible
	{
		private int _Result;

		private ulong _PetID;

		private uint _Exp;

		private uint _Level;

		private readonly List<ulong> _Pets = new List<ulong>();

		private uint _PetVersion;

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

		[ProtoMember(3, IsRequired = false, Name = "Exp", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint Exp
		{
			get
			{
				return this._Exp;
			}
			set
			{
				this._Exp = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "Level", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint Level
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

		[ProtoMember(5, Name = "Pets", DataFormat = DataFormat.TwosComplement)]
		public List<ulong> Pets
		{
			get
			{
				return this._Pets;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "PetVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
