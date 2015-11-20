using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "ActivityGroupBuyingCount")]
	[Serializable]
	public class ActivityGroupBuyingCount : IExtensible
	{
		private int _ID;

		private int _TotalCount;

		private int _MyCount;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "ID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(2, IsRequired = false, Name = "TotalCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int TotalCount
		{
			get
			{
				return this._TotalCount;
			}
			set
			{
				this._TotalCount = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "MyCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MyCount
		{
			get
			{
				return this._MyCount;
			}
			set
			{
				this._MyCount = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
