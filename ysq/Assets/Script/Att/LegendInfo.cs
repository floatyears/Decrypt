using ProtoBuf;
using System;
using System.Collections.Generic;

namespace Att
{
	[ProtoContract(Name = "LegendInfo")]
	[Serializable]
	public class LegendInfo : IExtensible
	{
		private int _ID;

		private readonly List<string> _Desc = new List<string>();

		private readonly List<int> _RefineLevel = new List<int>();

		private readonly List<int> _EffectType = new List<int>();

		private readonly List<int> _Value1 = new List<int>();

		private readonly List<int> _Value2 = new List<int>();

		private readonly List<int> _Value3 = new List<int>();

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

		[ProtoMember(2, Name = "Desc", DataFormat = DataFormat.Default)]
		public List<string> Desc
		{
			get
			{
				return this._Desc;
			}
		}

		[ProtoMember(3, Name = "RefineLevel", DataFormat = DataFormat.TwosComplement)]
		public List<int> RefineLevel
		{
			get
			{
				return this._RefineLevel;
			}
		}

		[ProtoMember(4, Name = "EffectType", DataFormat = DataFormat.TwosComplement)]
		public List<int> EffectType
		{
			get
			{
				return this._EffectType;
			}
		}

		[ProtoMember(5, Name = "Value1", DataFormat = DataFormat.TwosComplement)]
		public List<int> Value1
		{
			get
			{
				return this._Value1;
			}
		}

		[ProtoMember(6, Name = "Value2", DataFormat = DataFormat.TwosComplement)]
		public List<int> Value2
		{
			get
			{
				return this._Value2;
			}
		}

		[ProtoMember(7, Name = "Value3", DataFormat = DataFormat.TwosComplement)]
		public List<int> Value3
		{
			get
			{
				return this._Value3;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
