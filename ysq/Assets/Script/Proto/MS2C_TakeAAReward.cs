using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_TakeAAReward")]
	[Serializable]
	public class MS2C_TakeAAReward : IExtensible
	{
		private int _Result;

		private int _ActivityID;

		private int _AAItemID;

		private uint _AAVersion;

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

		[ProtoMember(2, IsRequired = false, Name = "ActivityID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(3, IsRequired = false, Name = "AAItemID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(4, IsRequired = false, Name = "AAVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint AAVersion
		{
			get
			{
				return this._AAVersion;
			}
			set
			{
				this._AAVersion = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
