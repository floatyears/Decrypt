using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Att
{
	[ProtoContract(Name = "D2MInfo")]
	[Serializable]
	public class D2MInfo : IExtensible
	{
		private int _ID;

		private int _D2MCost;

		private int _D2MValue;

		private readonly List<int> _D2MRate = new List<int>();

		private readonly List<int> _VipD2MRate = new List<int>();

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

		[ProtoMember(2, IsRequired = false, Name = "D2MCost", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int D2MCost
		{
			get
			{
				return this._D2MCost;
			}
			set
			{
				this._D2MCost = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "D2MValue", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int D2MValue
		{
			get
			{
				return this._D2MValue;
			}
			set
			{
				this._D2MValue = value;
			}
		}

		[ProtoMember(4, Name = "D2MRate", DataFormat = DataFormat.TwosComplement)]
		public List<int> D2MRate
		{
			get
			{
				return this._D2MRate;
			}
		}

		[ProtoMember(5, Name = "VipD2MRate", DataFormat = DataFormat.TwosComplement)]
		public List<int> VipD2MRate
		{
			get
			{
				return this._VipD2MRate;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
