using ProtoBuf;
using System;
using System.Collections.Generic;

namespace Att
{
	[ProtoContract(Name = "KRInfoDict")]
	[Serializable]
	public class KRInfoDict : IExtensible
	{
		private readonly List<KRInfo> _Data = new List<KRInfo>();

		private IExtension extensionObject;

		[ProtoMember(1, Name = "Data", DataFormat = DataFormat.Default)]
		public List<KRInfo> Data
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
