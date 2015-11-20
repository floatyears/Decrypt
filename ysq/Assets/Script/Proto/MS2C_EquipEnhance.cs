using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_EquipEnhance")]
	[Serializable]
	public class MS2C_EquipEnhance : IExtensible
	{
		private int _Result;

		private int _TotalMoney;

		private int _TotalCrit;

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

		[ProtoMember(2, IsRequired = false, Name = "TotalMoney", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int TotalMoney
		{
			get
			{
				return this._TotalMoney;
			}
			set
			{
				this._TotalMoney = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "TotalCrit", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int TotalCrit
		{
			get
			{
				return this._TotalCrit;
			}
			set
			{
				this._TotalCrit = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
