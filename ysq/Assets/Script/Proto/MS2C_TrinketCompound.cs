using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_TrinketCompound")]
	[Serializable]
	public class MS2C_TrinketCompound : IExtensible
	{
		private int _Result;

		private ulong _TrinketID;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Result", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Result
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

		[ProtoMember(2, IsRequired = false, Name = "TrinketID", DataFormat = DataFormat.TwosComplement), DefaultValue(0f)]
		public ulong TrinketID
		{
			get
			{
				return this._TrinketID;
			}
			set
			{
				this._TrinketID = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
