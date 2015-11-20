using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_ActivityGroupBuyingBuy")]
	[Serializable]
	public class MS2C_ActivityGroupBuyingBuy : IExtensible
	{
		private int _Result;

		private int _ID;

		private int _Score;

		private int _TotalCount;

		private int _MyCount;

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

		[ProtoMember(2, IsRequired = false, Name = "ID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(3, IsRequired = false, Name = "Score", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Score
		{
			get
			{
				return this._Score;
			}
			set
			{
				this._Score = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "TotalCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(5, IsRequired = false, Name = "MyCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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
