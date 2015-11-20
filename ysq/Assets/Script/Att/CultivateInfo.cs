using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Att
{
	[ProtoContract(Name = "CultivateInfo")]
	[Serializable]
	public class CultivateInfo : IExtensible
	{
		private int _ID;

		private readonly List<int> _MaxCulAttack = new List<int>();

		private readonly List<int> _MaxCulPhysicDefense = new List<int>();

		private readonly List<int> _MaxCulMagicDefense = new List<int>();

		private readonly List<int> _MaxCulMaxHP = new List<int>();

		private int _MaxAttackPer;

		private int _MaxPhysicDefensePer;

		private int _MaxMagicDefensePer;

		private int _MaxMaxHPPer;

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

		[ProtoMember(2, Name = "MaxCulAttack", DataFormat = DataFormat.TwosComplement)]
		public List<int> MaxCulAttack
		{
			get
			{
				return this._MaxCulAttack;
			}
		}

		[ProtoMember(3, Name = "MaxCulPhysicDefense", DataFormat = DataFormat.TwosComplement)]
		public List<int> MaxCulPhysicDefense
		{
			get
			{
				return this._MaxCulPhysicDefense;
			}
		}

		[ProtoMember(4, Name = "MaxCulMagicDefense", DataFormat = DataFormat.TwosComplement)]
		public List<int> MaxCulMagicDefense
		{
			get
			{
				return this._MaxCulMagicDefense;
			}
		}

		[ProtoMember(5, Name = "MaxCulMaxHP", DataFormat = DataFormat.TwosComplement)]
		public List<int> MaxCulMaxHP
		{
			get
			{
				return this._MaxCulMaxHP;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "MaxAttackPer", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MaxAttackPer
		{
			get
			{
				return this._MaxAttackPer;
			}
			set
			{
				this._MaxAttackPer = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "MaxPhysicDefensePer", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MaxPhysicDefensePer
		{
			get
			{
				return this._MaxPhysicDefensePer;
			}
			set
			{
				this._MaxPhysicDefensePer = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "MaxMagicDefensePer", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MaxMagicDefensePer
		{
			get
			{
				return this._MaxMagicDefensePer;
			}
			set
			{
				this._MaxMagicDefensePer = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "MaxMaxHPPer", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MaxMaxHPPer
		{
			get
			{
				return this._MaxMaxHPPer;
			}
			set
			{
				this._MaxMaxHPPer = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
