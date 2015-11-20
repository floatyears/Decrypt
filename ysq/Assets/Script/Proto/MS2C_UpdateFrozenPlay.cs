using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_UpdateFrozenPlay")]
	[Serializable]
	public class MS2C_UpdateFrozenPlay : IExtensible
	{
		private int _Play;

		private int _Timestamp;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Play", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Play
		{
			get
			{
				return this._Play;
			}
			set
			{
				this._Play = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "Timestamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Timestamp
		{
			get
			{
				return this._Timestamp;
			}
			set
			{
				this._Timestamp = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
