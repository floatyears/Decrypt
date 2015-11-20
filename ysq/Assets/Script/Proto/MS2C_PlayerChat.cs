using ProtoBuf;
using System;
using System.Collections.Generic;

namespace Proto
{
	[ProtoContract(Name = "MS2C_PlayerChat")]
	[Serializable]
	public class MS2C_PlayerChat : IExtensible
	{
		private readonly List<ChatMessage> _Data = new List<ChatMessage>();

		private IExtension extensionObject;

		[ProtoMember(1, Name = "Data", DataFormat = DataFormat.Default)]
		public List<ChatMessage> Data
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
