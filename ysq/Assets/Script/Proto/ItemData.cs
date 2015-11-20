using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "ItemData")]
	[Serializable]
	public class ItemData : IExtensible
	{
		private ulong _ID;

		private int _InfoID;

		private int _Value1;

		private int _Value2;

		private int _Value3;

		private int _Value4;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "ID", DataFormat = DataFormat.TwosComplement), DefaultValue(0f)]
		public ulong ID
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

		[ProtoMember(2, IsRequired = false, Name = "InfoID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int InfoID
		{
			get
			{
				return this._InfoID;
			}
			set
			{
				this._InfoID = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "Value1", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Value1
		{
			get
			{
				return this._Value1;
			}
			set
			{
				this._Value1 = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "Value2", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Value2
		{
			get
			{
				return this._Value2;
			}
			set
			{
				this._Value2 = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "Value3", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Value3
		{
			get
			{
				return this._Value3;
			}
			set
			{
				this._Value3 = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "Value4", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Value4
		{
			get
			{
				return this._Value4;
			}
			set
			{
				this._Value4 = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
