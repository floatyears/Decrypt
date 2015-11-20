using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_LopetSetCombat")]
	[Serializable]
	public class MS2C_LopetSetCombat : IExtensible
	{
		private ELopetResult _Result;

		private uint _Version;

		private ulong _LopetID;

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

		[ProtoMember(3, IsRequired = false, Name = "LopetID", DataFormat = DataFormat.TwosComplement), DefaultValue(0f)]
		public ulong LopetID
		{
			get
			{
				return this._LopetID;
			}
			set
			{
				this._LopetID = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
