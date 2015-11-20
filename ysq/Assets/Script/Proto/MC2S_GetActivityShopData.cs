using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_GetActivityShopData")]
	[Serializable]
	public class MC2S_GetActivityShopData : IExtensible
	{
		private uint _Version;

		private int _ActivityID;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Version", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
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

		[ProtoMember(2, IsRequired = false, Name = "ActivityID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ActivityID
		{
			get
			{
				return this._ActivityID;
			}
			set
			{
				this._ActivityID = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
