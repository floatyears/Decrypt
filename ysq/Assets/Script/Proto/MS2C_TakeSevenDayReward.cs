using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_TakeSevenDayReward")]
	[Serializable]
	public class MS2C_TakeSevenDayReward : IExtensible
	{
		private int _Result;

		private int _ID;

		private uint _SevenDayVersion;

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

		[ProtoMember(3, IsRequired = false, Name = "SevenDayVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint SevenDayVersion
		{
			get
			{
				return this._SevenDayVersion;
			}
			set
			{
				this._SevenDayVersion = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
