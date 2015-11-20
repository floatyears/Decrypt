using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_OrePillageResult")]
	[Serializable]
	public class MS2C_OrePillageResult : IExtensible
	{
		private int _Result;

		private int _ElapsedTime;

		private int _Amount1;

		private int _Amount2;

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

		[ProtoMember(2, IsRequired = false, Name = "ElapsedTime", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ElapsedTime
		{
			get
			{
				return this._ElapsedTime;
			}
			set
			{
				this._ElapsedTime = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "Amount1", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Amount1
		{
			get
			{
				return this._Amount1;
			}
			set
			{
				this._Amount1 = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "Amount2", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Amount2
		{
			get
			{
				return this._Amount2;
			}
			set
			{
				this._Amount2 = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
