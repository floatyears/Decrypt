using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "GuildWarAddScore")]
	[Serializable]
	public class GuildWarAddScore : IExtensible
	{
		private int _StrongholdID;

		private int _Score;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "StrongholdID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int StrongholdID
		{
			get
			{
				return this._StrongholdID;
			}
			set
			{
				this._StrongholdID = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "Score", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Score
		{
			get
			{
				return this._Score;
			}
			set
			{
				this._Score = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
