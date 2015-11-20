using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Att
{
	[ProtoContract(Name = "LuckyRollInfo")]
	[Serializable]
	public class LuckyRollInfo : IExtensible
	{
		private int _ID;

		private readonly List<int> _InfoID = new List<int>();

		private readonly List<int> _Rate = new List<int>();

		private int _VipLevel;

		private readonly List<int> _Count = new List<int>();

		private readonly List<int> _Pet = new List<int>();

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

		[ProtoMember(2, Name = "InfoID", DataFormat = DataFormat.TwosComplement)]
		public List<int> InfoID
		{
			get
			{
				return this._InfoID;
			}
		}

		[ProtoMember(3, Name = "Rate", DataFormat = DataFormat.TwosComplement)]
		public List<int> Rate
		{
			get
			{
				return this._Rate;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "VipLevel", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int VipLevel
		{
			get
			{
				return this._VipLevel;
			}
			set
			{
				this._VipLevel = value;
			}
		}

		[ProtoMember(5, Name = "Count", DataFormat = DataFormat.TwosComplement)]
		public List<int> Count
		{
			get
			{
				return this._Count;
			}
		}

		[ProtoMember(6, Name = "Pet", DataFormat = DataFormat.TwosComplement)]
		public List<int> Pet
		{
			get
			{
				return this._Pet;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
