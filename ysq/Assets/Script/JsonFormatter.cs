using System;
using System.Collections.Generic;
using System.Text;

public class JsonFormatter
{
	private enum JsonContextType
	{
		Object,
		Array
	}

	private const string Space = " ";

	private const int DefaultIndent = 0;

	private const string Indent = "    ";

	private static readonly string NewLine = "\n";

	private static bool inDoubleString = false;

	private static bool inSingleString = false;

	private static bool inVariableAssignment = false;

	private static char prevChar = '\0';

	private static Stack<JsonFormatter.JsonContextType> context = new Stack<JsonFormatter.JsonContextType>();

	private static void BuildIndents(int indents, StringBuilder output)
	{
		for (indents = indents; indents > 0; indents--)
		{
			output.Append("    ");
		}
	}

	private static bool InString()
	{
		return JsonFormatter.inDoubleString || JsonFormatter.inSingleString;
	}

	public static string PrettyPrint(string input)
	{
		JsonFormatter.inDoubleString = false;
		JsonFormatter.inSingleString = false;
		JsonFormatter.inVariableAssignment = false;
		JsonFormatter.prevChar = '\0';
		JsonFormatter.context.Clear();
		StringBuilder stringBuilder = new StringBuilder(input.Length * 2);
		for (int i = 0; i < input.Length; i++)
		{
			char c = input[i];
			char c2 = c;
			switch (c2)
			{
			case ':':
				if (!JsonFormatter.InString())
				{
					JsonFormatter.inVariableAssignment = true;
					stringBuilder.Append(" ");
					stringBuilder.Append(c);
					stringBuilder.Append(" ");
				}
				else
				{
					stringBuilder.Append(c);
				}
				goto IL_2A6;
			case ';':
			case '<':
				IL_5A:
				switch (c2)
				{
				case ' ':
					if (JsonFormatter.InString())
					{
						stringBuilder.Append(c);
					}
					goto IL_2A6;
				case '!':
					IL_6F:
					switch (c2)
					{
					case '[':
						goto IL_AE;
					case '\\':
						IL_84:
						switch (c2)
						{
						case '{':
							goto IL_AE;
						case '|':
							IL_99:
							if (c2 == '\'')
							{
								if (!JsonFormatter.inDoubleString && JsonFormatter.prevChar != '\\')
								{
									JsonFormatter.inSingleString = !JsonFormatter.inSingleString;
								}
								stringBuilder.Append(c);
								goto IL_2A6;
							}
							if (c2 != ',')
							{
								stringBuilder.Append(c);
								goto IL_2A6;
							}
							stringBuilder.Append(c);
							if (!JsonFormatter.InString())
							{
								JsonFormatter.BuildIndents(JsonFormatter.context.Count, stringBuilder);
								stringBuilder.Append(JsonFormatter.NewLine);
								JsonFormatter.BuildIndents(JsonFormatter.context.Count, stringBuilder);
								JsonFormatter.inVariableAssignment = false;
							}
							goto IL_2A6;
						case '}':
							goto IL_13F;
						}
						goto IL_99;
					case ']':
						goto IL_13F;
					}
					goto IL_84;
					IL_AE:
					if (!JsonFormatter.InString())
					{
						if (JsonFormatter.inVariableAssignment || (JsonFormatter.context.Count > 0 && JsonFormatter.context.Peek() != JsonFormatter.JsonContextType.Array))
						{
							stringBuilder.Append(JsonFormatter.NewLine);
							JsonFormatter.BuildIndents(JsonFormatter.context.Count, stringBuilder);
						}
						stringBuilder.Append(c);
						JsonFormatter.context.Push(JsonFormatter.JsonContextType.Object);
						stringBuilder.Append(JsonFormatter.NewLine);
						JsonFormatter.BuildIndents(JsonFormatter.context.Count, stringBuilder);
					}
					else
					{
						stringBuilder.Append(c);
					}
					goto IL_2A6;
					IL_13F:
					if (!JsonFormatter.InString())
					{
						stringBuilder.Append(JsonFormatter.NewLine);
						JsonFormatter.context.Pop();
						JsonFormatter.BuildIndents(JsonFormatter.context.Count, stringBuilder);
						stringBuilder.Append(c);
					}
					else
					{
						stringBuilder.Append(c);
					}
					goto IL_2A6;
				case '"':
					if (!JsonFormatter.inSingleString && JsonFormatter.prevChar != '\\')
					{
						JsonFormatter.inDoubleString = !JsonFormatter.inDoubleString;
					}
					stringBuilder.Append(c);
					goto IL_2A6;
				}
				goto IL_6F;
			case '=':
				stringBuilder.Append(c);
				goto IL_2A6;
			}
			goto IL_5A;
			IL_2A6:
			JsonFormatter.prevChar = c;
		}
		return stringBuilder.ToString();
	}
}
