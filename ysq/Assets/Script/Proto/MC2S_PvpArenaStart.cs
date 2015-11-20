using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_PvpArenaStart")]
	[Serializable]
	public class MC2S_PvpArenaStart : IExtensible
	{
		private ulong _TargetID;

		private int _Rank;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "TargetID", DataFormat = DataFormat.TwosComplement), DefaultValue(0f)]
		public ulong TargetID
		{
			get
			{
				return this._TargetID;
			}
			set
			{
				this._TargetID = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "Rank", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Rank
		{
			get
			{
				return this._Rank;
			}
			set
			{
				this._Rank = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
