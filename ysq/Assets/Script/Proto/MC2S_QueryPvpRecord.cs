using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_QueryPvpRecord")]
	[Serializable]
	public class MC2S_QueryPvpRecord : IExtensible
	{
		private uint _RecordVersion;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "RecordVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint RecordVersion
		{
			get
			{
				return this._RecordVersion;
			}
			set
			{
				this._RecordVersion = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
