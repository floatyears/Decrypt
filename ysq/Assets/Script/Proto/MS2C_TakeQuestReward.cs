using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_TakeQuestReward")]
	[Serializable]
	public class MS2C_TakeQuestReward : IExtensible
	{
		private int _Result;

		private int _QuestID;

		private uint _SceneVersion;

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

		[ProtoMember(3, IsRequired = false, Name = "SceneVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint SceneVersion
		{
			get
			{
				return this._SceneVersion;
			}
			set
			{
				this._SceneVersion = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
