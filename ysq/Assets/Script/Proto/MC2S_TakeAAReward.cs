using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_TakeAAReward")]
	[Serializable]
	public class MC2S_TakeAAReward : IExtensible
	{
		private int _ActivityID;

		private int _AAItemID;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "ActivityID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ActivityID
		{
			get
			{
				return this._ActivityID;
			}
			set
			{
				this._ActivityID = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "AAItemID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int AAItemID
		{
			get
			{
				return this._AAItemID;
			}
			set
			{
				this._AAItemID = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
