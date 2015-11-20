using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_TakeScoreReward")]
	[Serializable]
	public class MC2S_TakeScoreReward : IExtensible
	{
		private int _Index;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Index", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Index
		{
			get
			{
				return this._Index;
			}
			set
			{
				this._Index = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
