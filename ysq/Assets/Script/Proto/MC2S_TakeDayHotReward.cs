using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_TakeDayHotReward")]
	[Serializable]
	public class MC2S_TakeDayHotReward : IExtensible
	{
		private int _ID;

		private int _Index;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "ID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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
