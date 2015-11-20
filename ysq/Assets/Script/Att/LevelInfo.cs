using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Att
{
	[ProtoContract(Name = "LevelInfo")]
	[Serializable]
	public class LevelInfo : IExtensible
	{
		private int _ID;

		private uint _PExp;

		private readonly List<uint> _Exp = new List<uint>();

		private uint _MaxEnergy;

		private uint _GiveEnergy;

		private readonly List<uint> _EnhanceCost = new List<uint>();

		private readonly List<uint> _RefineExp = new List<uint>();

		private readonly List<uint> _EnhanceExp = new List<uint>();

		private uint _MaxMP;

		private readonly List<uint> _LPExp = new List<uint>();

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

		[ProtoMember(2, IsRequired = false, Name = "PExp", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint PExp
		{
			get
			{
				return this._PExp;
			}
			set
			{
				this._PExp = value;
			}
		}

		[ProtoMember(3, Name = "Exp", DataFormat = DataFormat.TwosComplement)]
		public List<uint> Exp
		{
			get
			{
				return this._Exp;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "MaxEnergy", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint MaxEnergy
		{
			get
			{
				return this._MaxEnergy;
			}
			set
			{
				this._MaxEnergy = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "GiveEnergy", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint GiveEnergy
		{
			get
			{
				return this._GiveEnergy;
			}
			set
			{
				this._GiveEnergy = value;
			}
		}

		[ProtoMember(6, Name = "EnhanceCost", DataFormat = DataFormat.TwosComplement)]
		public List<uint> EnhanceCost
		{
			get
			{
				return this._EnhanceCost;
			}
		}

		[ProtoMember(7, Name = "RefineExp", DataFormat = DataFormat.TwosComplement)]
		public List<uint> RefineExp
		{
			get
			{
				return this._RefineExp;
			}
		}

		[ProtoMember(8, Name = "EnhanceExp", DataFormat = DataFormat.TwosComplement)]
		public List<uint> EnhanceExp
		{
			get
			{
				return this._EnhanceExp;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "MaxMP", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint MaxMP
		{
			get
			{
				return this._MaxMP;
			}
			set
			{
				this._MaxMP = value;
			}
		}

		[ProtoMember(10, Name = "LPExp", DataFormat = DataFormat.TwosComplement)]
		public List<uint> LPExp
		{
			get
			{
				return this._LPExp;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
