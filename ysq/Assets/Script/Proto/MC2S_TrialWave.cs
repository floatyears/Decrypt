using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_TrialWave")]
	[Serializable]
	public class MC2S_TrialWave : IExtensible
	{
		private int _ResultKey;

		private int _Wave;

		private int _RecvStartTime;

		private int _SendResultTime;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "ResultKey", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ResultKey
		{
			get
			{
				return this._ResultKey;
			}
			set
			{
				this._ResultKey = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "Wave", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(3, IsRequired = false, Name = "RecvStartTime", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int RecvStartTime
		{
			get
			{
				return this._RecvStartTime;
			}
			set
			{
				this._RecvStartTime = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "SendResultTime", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int SendResultTime
		{
			get
			{
				return this._SendResultTime;
			}
			set
			{
				this._SendResultTime = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
