using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_CreateOrder")]
	[Serializable]
	public class MS2C_CreateOrder : IExtensible
	{
		private int _Result;

		private int _CDTime;

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

		[ProtoMember(2, IsRequired = false, Name = "CDTime", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int CDTime
		{
			get
			{
				return this._CDTime;
			}
			set
			{
				this._CDTime = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
