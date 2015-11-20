using ProtoBuf;
using System;
using System.Collections.Generic;

namespace Att
{
	[ProtoContract(Name = "SayInfoDict")]
	[Serializable]
	public class SayInfoDict : IExtensible
	{
		private readonly List<SayInfo> _Data = new List<SayInfo>();

		private IExtension extensionObject;

		[ProtoMember(1, Name = "Data", DataFormat = DataFormat.Default)]
		public List<SayInfo> Data
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
