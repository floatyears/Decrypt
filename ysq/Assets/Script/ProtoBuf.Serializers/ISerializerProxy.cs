using System;

namespace ProtoBuf.Serializers
{
	internal interface ISerializerProxy
	{
		IProtoSerializer Serializer
		{
			get;
		}
	}
}
