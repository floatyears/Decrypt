using ProtoBuf;
using System;
using System.Collections.Generic;

namespace Proto
{
	[ProtoContract(Name = "MS2C_GetGuildApplication")]
	[Serializable]
	public class MS2C_GetGuildApplication : IExtensible
	{
		private readonly List<GuildApplication> _Data = new List<GuildApplication>();

		private IExtension extensionObject;

		[ProtoMember(1, Name = "Data", DataFormat = DataFormat.Default)]
		public List<GuildApplication> Data
		{
			get
			{
				return this._Data;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
