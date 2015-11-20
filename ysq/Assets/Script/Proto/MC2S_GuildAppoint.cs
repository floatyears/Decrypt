using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_GuildAppoint")]
	[Serializable]
	public class MC2S_GuildAppoint : IExtensible
	{
		private ulong _ID;

		private int _Rank;

		private IExtension extensionObject;

		[ProtoMember(2, IsRequired = false, Name = "ID", DataFormat = DataFormat.TwosComplement), DefaultValue(0f)]
		public ulong ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				this._ID = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "Rank", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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
