using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_MGFarm")]
	[Serializable]
	public class MC2S_MGFarm : IExtensible
	{
		private int _MGID;

		private IExtension extensionObject;

		[ProtoMember(2, IsRequired = false, Name = "MGID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MGID
		{
			get
			{
				return this._MGID;
			}
			set
			{
				this._MGID = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
