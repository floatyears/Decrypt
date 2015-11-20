using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_PetCultivate")]
	[Serializable]
	public class MS2C_PetCultivate : IExtensible
	{
		private int _Result;

		private ulong _PetID;

		private int _AttackPreview;

		private int _PhysicDefensePreview;

		private int _MagicDefensePreview;

		private int _MaxHPPreview;

		private uint _PetVersion;

		private int _CultivateCount;

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

		[ProtoMember(3, IsRequired = false, Name = "AttackPreview", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int AttackPreview
		{
			get
			{
				return this._AttackPreview;
			}
			set
			{
				this._AttackPreview = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "PhysicDefensePreview", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int PhysicDefensePreview
		{
			get
			{
				return this._PhysicDefensePreview;
			}
			set
			{
				this._PhysicDefensePreview = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "MagicDefensePreview", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MagicDefensePreview
		{
			get
			{
				return this._MagicDefensePreview;
			}
			set
			{
				this._MagicDefensePreview = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "MaxHPPreview", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MaxHPPreview
		{
			get
			{
				return this._MaxHPPreview;
			}
			set
			{
				this._MaxHPPreview = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "PetVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
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

		[ProtoMember(8, IsRequired = false, Name = "CultivateCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int CultivateCount
		{
			get
			{
				return this._CultivateCount;
			}
			set
			{
				this._CultivateCount = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
