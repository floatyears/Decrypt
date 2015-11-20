using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_GetScratchOffData")]
	[Serializable]
	public class MS2C_GetScratchOffData : IExtensible
	{
		private int _Result;

		private int _Diamond;

		private int _Count;

		private int _OverTime;

		private int _Cost;

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

		[ProtoMember(2, IsRequired = false, Name = "Diamond", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Diamond
		{
			get
			{
				return this._Diamond;
			}
			set
			{
				this._Diamond = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "Count", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Count
		{
			get
			{
				return this._Count;
			}
			set
			{
				this._Count = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "OverTime", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(5, IsRequired = false, Name = "Cost", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Cost
		{
			get
			{
				return this._Cost;
			}
			set
			{
				this._Cost = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
