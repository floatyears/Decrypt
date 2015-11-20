using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_OneKeyKR")]
	[Serializable]
	public class MC2S_OneKeyKR : IExtensible
	{
		private int _QuestID;

		private IExtension extensionObject;

		[ProtoMember(2, IsRequired = false, Name = "QuestID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int QuestID
		{
			get
			{
				return this._QuestID;
			}
			set
			{
				this._QuestID = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
