using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_LopetLevelup")]
	[Serializable]
	public class MS2C_LopetLevelup : IExtensible
	{
		private ELopetResult _Result;

		private uint _Version;

		private ulong _LopetID;

		private uint _Exp;

		private uint _Level;

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

		[ProtoMember(4, IsRequired = false, Name = "Exp", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint Exp
		{
			get
			{
				return this._Exp;
			}
			set
			{
				this._Exp = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "Level", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint Level
		{
			get
			{
				return this._Level;
			}
			set
			{
				this._Level = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
