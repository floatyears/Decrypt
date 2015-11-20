using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_ChangeSocket")]
	[Serializable]
	public class MC2S_ChangeSocket : IExtensible
	{
		private int _Slot1;

		private int _Slot2;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Slot1", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Slot1
		{
			get
			{
				return this._Slot1;
			}
			set
			{
				this._Slot1 = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "Slot2", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Slot2
		{
			get
			{
				return this._Slot2;
			}
			set
			{
				this._Slot2 = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
