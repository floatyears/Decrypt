using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_GuildSign")]
	[Serializable]
	public class MS2C_GuildSign : IExtensible
	{
		private int _Result;

		private int _exp;

		private int _money;

		private int _prosperity;

		private int _reputation;

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

		[ProtoMember(2, IsRequired = false, Name = "exp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int exp
		{
			get
			{
				return this._exp;
			}
			set
			{
				this._exp = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "money", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int money
		{
			get
			{
				return this._money;
			}
			set
			{
				this._money = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "prosperity", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int prosperity
		{
			get
			{
				return this._prosperity;
			}
			set
			{
				this._prosperity = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "reputation", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int reputation
		{
			get
			{
				return this._reputation;
			}
			set
			{
				this._reputation = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
