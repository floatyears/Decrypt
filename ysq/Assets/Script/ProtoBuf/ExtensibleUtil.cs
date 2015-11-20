using ProtoBuf.Meta;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace ProtoBuf
{
	internal static class ExtensibleUtil
	{
		[DebuggerHidden]
		internal static IEnumerable<TValue> GetExtendedValues<TValue>(IExtensible instance, int tag, DataFormat format, bool singleton, bool allowDefinedTag)
		{
        return null;
            //ExtensibleUtil.<GetExtendedValues>c__Iterator9<TValue> <GetExtendedValues>c__Iterator = new ExtensibleUtil.<GetExtendedValues>c__Iterator9<TValue>();
            //<GetExtendedValues>c__Iterator.instance = instance;
            //<GetExtendedValues>c__Iterator.tag = tag;
            //<GetExtendedValues>c__Iterator.format = format;
            //<GetExtendedValues>c__Iterator.singleton = singleton;
            //<GetExtendedValues>c__Iterator.allowDefinedTag = allowDefinedTag;
            //<GetExtendedValues>c__Iterator.<$>instance = instance;
            //<GetExtendedValues>c__Iterator.<$>tag = tag;
            //<GetExtendedValues>c__Iterator.<$>format = format;
            //<GetExtendedValues>c__Iterator.<$>singleton = singleton;
            //<GetExtendedValues>c__Iterator.<$>allowDefinedTag = allowDefinedTag;
            //ExtensibleUtil.<GetExtendedValues>c__Iterator9<TValue> expr_4F = <GetExtendedValues>c__Iterator;
            //expr_4F.$PC = -2;
            //return expr_4F;
		}

		[DebuggerHidden]
		internal static IEnumerable GetExtendedValues(TypeModel model, Type type, IExtensible instance, int tag, DataFormat format, bool singleton, bool allowDefinedTag)
		{
            return null;
            //ExtensibleUtil.<GetExtendedValues>c__IteratorA <GetExtendedValues>c__IteratorA = new ExtensibleUtil.<GetExtendedValues>c__IteratorA();
            //<GetExtendedValues>c__IteratorA.instance = instance;
            //<GetExtendedValues>c__IteratorA.tag = tag;
            //<GetExtendedValues>c__IteratorA.model = model;
            //<GetExtendedValues>c__IteratorA.format = format;
            //<GetExtendedValues>c__IteratorA.type = type;
            //<GetExtendedValues>c__IteratorA.singleton = singleton;
            //<GetExtendedValues>c__IteratorA.<$>instance = instance;
            //<GetExtendedValues>c__IteratorA.<$>tag = tag;
            //<GetExtendedValues>c__IteratorA.<$>model = model;
            //<GetExtendedValues>c__IteratorA.<$>format = format;
            //<GetExtendedValues>c__IteratorA.<$>type = type;
            //<GetExtendedValues>c__IteratorA.<$>singleton = singleton;
            //ExtensibleUtil.<GetExtendedValues>c__IteratorA expr_5F = <GetExtendedValues>c__IteratorA;
            //expr_5F.$PC = -2;
            //return expr_5F;
		}

		internal static void AppendExtendValue(TypeModel model, IExtensible instance, int tag, DataFormat format, object value)
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			IExtension extensionObject = instance.GetExtensionObject(true);
			if (extensionObject == null)
			{
				throw new InvalidOperationException("No extension object available; appended data would be lost.");
			}
			bool commit = false;
			Stream stream = extensionObject.BeginAppend();
			try
			{
				using (ProtoWriter protoWriter = new ProtoWriter(stream, model, null))
				{
					model.TrySerializeAuxiliaryType(protoWriter, null, format, tag, value, false);
					protoWriter.Close();
				}
				commit = true;
			}
			finally
			{
				extensionObject.EndAppend(stream, commit);
			}
		}
	}
}
