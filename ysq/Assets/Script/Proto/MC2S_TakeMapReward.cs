using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_TakeMapReward")]
	[Serializable]
	public class MC2S_TakeMapReward : IExtensible
	{
		private int _MapID;

		private int _Index;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "MapID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MapID
		{
			get
			{
				return this._MapID;
			}
			set
			{
				this._MapID = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "Index", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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
