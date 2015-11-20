using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "D2MData")]
	[Serializable]
	public class D2MData : IExtensible
	{
		private int _Money;

		private int _Crit;

		private int _Diamond;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Money", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Money
		{
			get
			{
				return this._Money;
			}
			set
			{
				this._Money = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "Crit", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Crit
		{
			get
			{
				return this._Crit;
			}
			set
			{
				this._Crit = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "Diamond", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
