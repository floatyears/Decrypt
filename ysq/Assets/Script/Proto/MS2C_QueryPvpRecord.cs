using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_QueryPvpRecord")]
	[Serializable]
	public class MS2C_QueryPvpRecord : IExtensible
	{
		private uint _RecordVersion;

		private readonly List<PvpRecord> _Data = new List<PvpRecord>();

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

		[ProtoMember(2, Name = "Data", DataFormat = DataFormat.Default)]
		public List<PvpRecord> Data
		{
			get
			{
				return this._Data;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
