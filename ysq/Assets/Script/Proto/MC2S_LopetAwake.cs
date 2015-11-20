using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_LopetAwake")]
	[Serializable]
	public class MC2S_LopetAwake : IExtensible
	{
		private ulong _LopetID;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "LopetID", DataFormat = DataFormat.TwosComplement), DefaultValue(0f)]
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
