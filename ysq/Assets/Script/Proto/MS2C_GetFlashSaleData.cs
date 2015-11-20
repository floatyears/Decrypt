using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_GetFlashSaleData")]
	[Serializable]
	public class MS2C_GetFlashSaleData : IExtensible
	{
		private int _Result;

		private readonly List<int> _Count = new List<int>();

		private readonly List<string> _Detail = new List<string>();

		private readonly List<int> _PreCost = new List<int>();

		private readonly List<int> _CurCost = new List<int>();

		private readonly List<string> _Name = new List<string>();

		private int _OverTime;

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

		[ProtoMember(2, Name = "Count", DataFormat = DataFormat.TwosComplement)]
		public List<int> Count
		{
			get
			{
				return this._Count;
			}
		}

		[ProtoMember(3, Name = "Detail", DataFormat = DataFormat.Default)]
		public List<string> Detail
		{
			get
			{
				return this._Detail;
			}
		}

		[ProtoMember(4, Name = "PreCost", DataFormat = DataFormat.TwosComplement)]
		public List<int> PreCost
		{
			get
			{
				return this._PreCost;
			}
		}

		[ProtoMember(5, Name = "CurCost", DataFormat = DataFormat.TwosComplement)]
		public List<int> CurCost
		{
			get
			{
				return this._CurCost;
			}
		}

		[ProtoMember(6, Name = "Name", DataFormat = DataFormat.Default)]
		public List<string> Name
		{
			get
			{
				return this._Name;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "OverTime", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int OverTime
		{
			get
			{
				return this._OverTime;
			}
			set
			{
				this._OverTime = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
