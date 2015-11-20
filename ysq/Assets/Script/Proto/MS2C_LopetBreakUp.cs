using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_LopetBreakUp")]
	[Serializable]
	public class MS2C_LopetBreakUp : IExtensible
	{
		private ELopetResult _Result;

		private uint _Version;

		private readonly List<ulong> _LopetID = new List<ulong>();

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Result", DataFormat = DataFormat.TwosComplement), DefaultValue(ELopetResult.ELR_Success)]
		public ELopetResult Result
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

		[ProtoMember(3, Name = "LopetID", DataFormat = DataFormat.TwosComplement)]
		public List<ulong> LopetID
		{
			get
			{
				return this._LopetID;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
