using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_ShareAchievement")]
	[Serializable]
	public class MC2S_ShareAchievement : IExtensible
	{
		private int _ID;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "ID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ID
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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
