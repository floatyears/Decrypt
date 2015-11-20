using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_LopetAdd")]
	[Serializable]
	public class MS2C_LopetAdd : IExtensible
	{
		private readonly List<LopetData> _Data = new List<LopetData>();

		private uint _Version;

		private IExtension extensionObject;

		[ProtoMember(1, Name = "Data", DataFormat = DataFormat.Default)]
		public List<LopetData> Data
		{
			get
			{
				return this._Data;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "Version", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint Version
		{
			get
			{
				return this._Version;
			}
			set
			{
				this._Version = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
