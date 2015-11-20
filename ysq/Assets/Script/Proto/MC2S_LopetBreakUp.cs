using ProtoBuf;
using System;
using System.Collections.Generic;

namespace Proto
{
	[ProtoContract(Name = "MC2S_LopetBreakUp")]
	[Serializable]
	public class MC2S_LopetBreakUp : IExtensible
	{
		private readonly List<ulong> _LopetID = new List<ulong>();

		private IExtension extensionObject;

		[ProtoMember(1, Name = "LopetID", DataFormat = DataFormat.TwosComplement)]
		public List<ulong> LopetID
		{
			get
			{
				return this._LopetID;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
