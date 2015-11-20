using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_RemoveGuest")]
	[Serializable]
	public class MS2C_RemoveGuest : IExtensible
	{
		private int _Result;

		private ulong _PlayerID;

		private ulong _MasterID;

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

		[ProtoMember(2, IsRequired = false, Name = "PlayerID", DataFormat = DataFormat.TwosComplement), DefaultValue(0f)]
		public ulong PlayerID
		{
			get
			{
				return this._PlayerID;
			}
			set
			{
				this._PlayerID = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "MasterID", DataFormat = DataFormat.TwosComplement), DefaultValue(0f)]
		public ulong MasterID
		{
			get
			{
				return this._MasterID;
			}
			set
			{
				this._MasterID = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
