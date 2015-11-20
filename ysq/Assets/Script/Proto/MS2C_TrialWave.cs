using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_TrialWave")]
	[Serializable]
	public class MS2C_TrialWave : IExtensible
	{
		private int _Result;

		private int _Key;

		private int _Wave;

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

		[ProtoMember(2, IsRequired = false, Name = "Key", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Key
		{
			get
			{
				return this._Key;
			}
			set
			{
				this._Key = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "Wave", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Wave
		{
			get
			{
				return this._Wave;
			}
			set
			{
				this._Wave = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
