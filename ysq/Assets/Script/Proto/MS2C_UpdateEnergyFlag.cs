using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_UpdateEnergyFlag")]
	[Serializable]
	public class MS2C_UpdateEnergyFlag : IExtensible
	{
		private ulong _GUID;

		private int _Flag;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "GUID", DataFormat = DataFormat.TwosComplement), DefaultValue(0f)]
		public ulong GUID
		{
			get
			{
				return this._GUID;
			}
			set
			{
				this._GUID = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "Flag", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Flag
		{
			get
			{
				return this._Flag;
			}
			set
			{
				this._Flag = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
