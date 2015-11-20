using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_LopetLevelup")]
	[Serializable]
	public class MC2S_LopetLevelup : IExtensible
	{
		private ulong _LopetID;

		private int _Level;

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

		[ProtoMember(2, IsRequired = false, Name = "Level", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Level
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
