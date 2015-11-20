using ProtoBuf;
using System;
using System.Collections.Generic;

namespace Att
{
	[ProtoContract(Name = "WorldBossInfoDict")]
	[Serializable]
	public class WorldBossInfoDict : IExtensible
	{
		private readonly List<WorldBossInfo> _Data = new List<WorldBossInfo>();

		private IExtension extensionObject;

		[ProtoMember(1, Name = "Data", DataFormat = DataFormat.Default)]
		public List<WorldBossInfo> Data
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
