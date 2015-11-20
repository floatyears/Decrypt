using ProtoBuf;
using System;
using System.Collections.Generic;

namespace Proto
{
	[ProtoContract(Name = "MS2C_RecommendFriend")]
	[Serializable]
	public class MS2C_RecommendFriend : IExtensible
	{
		private readonly List<FriendData> _Data = new List<FriendData>();

		private IExtension extensionObject;

		[ProtoMember(1, Name = "Data", DataFormat = DataFormat.Default)]
		public List<FriendData> Data
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
