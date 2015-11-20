using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XUPorterJSON;

namespace NtUniSdk.Unity3d
{
	public class GamePadMgr : MonoBehaviour
	{
		public delegate void VoidDelegate();

		public delegate void BoolDelegate(bool state);

		public const int KEYCODE_BUTTON_A = 1;

		public const int KEYCODE_BUTTON_B = 2;

		public const int KEYCODE_BUTTON_X = 4;

		public const int KEYCODE_BUTTON_Y = 8;

		public const int KEYCODE_BUTTON_L1 = 16;

		public const int KEYCODE_BUTTON_R1 = 32;

		public const int KEYCODE_BUTTON_L2 = 64;

		public const int KEYCODE_BUTTON_R2 = 128;

		public const int KEYCODE_DPAD_UP = 256;

		public const int KEYCODE_DPAD_DOWN = 512;

		public const int KEYCODE_DPAD_LEFT = 1024;

		public const int KEYCODE_DPAD_RIGHT = 2048;

		public const int KEYCODE_BUTTON_BACK = 4096;

		public const int KEYCODE_BUTTON_START = 8192;

		public const int KEYCODE_BUTTON_THUMBL = 16384;

		public const int KEYCODE_BUTTON_THUMBR = 32768;

		public const int KEYCODE_BUTTON_HELP = 65536;

		public const int KEYCODE_BUTTON_KEYBOARD = 131072;

		public const int KEYCODE_LEFT_STICK = 262144;

		public const int KEYCODE_RIGHT_STICK = 524288;

		public const int KEYCODE_BUTTON_ANY = 268435455;

		public const int ACTION_DOWN = 0;

		public const int ACTION_UP = 1;

		public const int ACTION_PRESSURE = -1;

		public const int ACTION_DISCONNECTED = 0;

		public const int ACTION_CONNECTED = 1;

		public const int ACTION_CONNECTING = 2;

		private const string SDK_JAVA_CLASS_GAMEPAD = "com.netease.ntunisdk.SdkXinyou";

		private static Dictionary<int, GamePadMgr.VoidDelegate> onClickDict = new Dictionary<int, GamePadMgr.VoidDelegate>();

		private static Dictionary<int, GamePadMgr.BoolDelegate> onPressDict = new Dictionary<int, GamePadMgr.BoolDelegate>();

		private static Dictionary<int, GamePadMgr.BoolDelegate> onDragDict = new Dictionary<int, GamePadMgr.BoolDelegate>();

		private static HashSet<int> isKeyDownSet = new HashSet<int>();

		private static bool downFlag;

		private static bool upFlag;

		public static bool isConnnected
		{
			get;
			private set;
		}

		public static bool isMove
		{
			get;
			private set;
		}

		public static bool isLeftScroll
		{
			get;
			private set;
		}

		public static float moveX
		{
			get;
			private set;
		}

		public static float moveY
		{
			get;
			private set;
		}

		private void Start()
		{
			this.Reset();
		}

		private void LateUpdate()
		{
			foreach (int current in GamePadMgr.isKeyDownSet)
			{
				if (GamePadMgr.IsKeyMove())
				{
					if (GamePadMgr.onDragDict.ContainsKey(current))
					{
						GamePadMgr.onDragDict[current](true);
					}
				}
			}
		}

		private void Reset()
		{
			GamePadMgr.isConnnected = false;
			GamePadMgr.isMove = false;
			GamePadMgr.isLeftScroll = false;
			GamePadMgr.downFlag = false;
			GamePadMgr.upFlag = false;
			GamePadMgr.moveX = 0f;
			GamePadMgr.moveY = 0f;
		}

		public void OnKeyDownCallback(string jsonstr)
		{
			Hashtable hashtable = (Hashtable)MiniJSON.jsonDecode(jsonstr);
			int num = Convert.ToInt32(hashtable["keyCode"]);
			global::Debug.Log(new object[]
			{
				"OnKeyDownCallback, KeyCode = " + num
			});
			GamePadMgr.isConnnected = true;
			this.OnKeyDown(num);
		}

		public void OnKeyUpCallback(string jsonstr)
		{
			Hashtable hashtable = (Hashtable)MiniJSON.jsonDecode(jsonstr);
			int num = Convert.ToInt32(hashtable["keyCode"]);
			global::Debug.Log(new object[]
			{
				"OnKeyUpCallback, KeyCode = " + num
			});
			GamePadMgr.isConnnected = true;
			this.OnKeyUp(num);
		}

		public void OnKeyPressureCallback(string jsonstr)
		{
			Hashtable hashtable = (Hashtable)MiniJSON.jsonDecode(jsonstr);
			int num = Convert.ToInt32(hashtable["keyCode"]);
			float num2 = Convert.ToSingle(hashtable["pressure"]);
			global::Debug.Log(new object[]
			{
				string.Concat(new object[]
				{
					"OnKeyPressureCallback, KeyCode = ",
					num,
					"Pressure = ",
					num2
				})
			});
			GamePadMgr.isConnnected = true;
		}

		public void OnLeftStickCallback(string jsonstr)
		{
			Hashtable hashtable = (Hashtable)MiniJSON.jsonDecode(jsonstr);
			float num = Convert.ToSingle(hashtable["x"]);
			float num2 = Convert.ToSingle(hashtable["y"]);
			global::Debug.Log(new object[]
			{
				string.Concat(new object[]
				{
					"OnLeftStickCallback, X = ",
					num,
					"Y = ",
					num2
				})
			});
			GamePadMgr.isConnnected = true;
			this.OnLeftStickScroll(num, num2);
		}

		public void OnRightStickCallback(string jsonstr)
		{
			Hashtable hashtable = (Hashtable)MiniJSON.jsonDecode(jsonstr);
			float num = Convert.ToSingle(hashtable["x"]);
			float num2 = Convert.ToSingle(hashtable["y"]);
			global::Debug.Log(new object[]
			{
				string.Concat(new object[]
				{
					"OnRightStickCallback, X = ",
					num,
					"Y = ",
					num2
				})
			});
			GamePadMgr.isConnnected = true;
		}

		public void OnStateEventCallback(string jsonstr)
		{
			Hashtable hashtable = (Hashtable)MiniJSON.jsonDecode(jsonstr);
			int num = Convert.ToInt32(hashtable["state"]);
			global::Debug.Log(new object[]
			{
				"OnStateEventCallback, State = " + num
			});
			this.Reset();
			GamePadMgr.isConnnected = (num == 1);
		}

		public static void RegClickDelegate(int keyCode, GamePadMgr.VoidDelegate callback)
		{
			if (GamePadMgr.onClickDict.ContainsKey(keyCode))
			{
				Dictionary<int, GamePadMgr.VoidDelegate> dictionary;
				Dictionary<int, GamePadMgr.VoidDelegate> expr_15 = dictionary = GamePadMgr.onClickDict;
				GamePadMgr.VoidDelegate a = dictionary[keyCode];
				expr_15[keyCode] = (GamePadMgr.VoidDelegate)Delegate.Combine(a, callback);
			}
			else
			{
				GamePadMgr.onClickDict.Add(keyCode, callback);
			}
		}

		public static void UnRegClickDelegate(int keyCode, GamePadMgr.VoidDelegate callback)
		{
			if (GamePadMgr.onClickDict.ContainsKey(keyCode))
			{
				GamePadMgr.VoidDelegate source = GamePadMgr.onClickDict[keyCode];
				if ((GamePadMgr.VoidDelegate)Delegate.Remove(source, callback) == null)
				{
					GamePadMgr.onClickDict.Remove(keyCode);
				}
			}
		}

		public static void RegPressDelegate(int keyCode, GamePadMgr.BoolDelegate callback)
		{
			if (GamePadMgr.onPressDict.ContainsKey(keyCode))
			{
				Dictionary<int, GamePadMgr.BoolDelegate> dictionary;
				Dictionary<int, GamePadMgr.BoolDelegate> expr_15 = dictionary = GamePadMgr.onPressDict;
				GamePadMgr.BoolDelegate a = dictionary[keyCode];
				expr_15[keyCode] = (GamePadMgr.BoolDelegate)Delegate.Combine(a, callback);
			}
			else
			{
				GamePadMgr.onPressDict.Add(keyCode, callback);
			}
		}

		public static void UnRegPressDelegate(int keyCode, GamePadMgr.BoolDelegate callback)
		{
			if (GamePadMgr.onPressDict.ContainsKey(keyCode))
			{
				GamePadMgr.BoolDelegate source = GamePadMgr.onPressDict[keyCode];
				if ((GamePadMgr.BoolDelegate)Delegate.Remove(source, callback) == null)
				{
					GamePadMgr.onPressDict.Remove(keyCode);
				}
			}
		}

		public static void RegDragDelegate(int keyCode, GamePadMgr.BoolDelegate callback)
		{
			if (GamePadMgr.onDragDict.ContainsKey(keyCode))
			{
				Dictionary<int, GamePadMgr.BoolDelegate> dictionary;
				Dictionary<int, GamePadMgr.BoolDelegate> expr_15 = dictionary = GamePadMgr.onDragDict;
				GamePadMgr.BoolDelegate a = dictionary[keyCode];
				expr_15[keyCode] = (GamePadMgr.BoolDelegate)Delegate.Combine(a, callback);
			}
			else
			{
				GamePadMgr.onDragDict.Add(keyCode, callback);
			}
		}

		public static void UnRegDragDelegate(int keyCode, GamePadMgr.BoolDelegate callback)
		{
			if (GamePadMgr.onDragDict.ContainsKey(keyCode))
			{
				GamePadMgr.BoolDelegate source = GamePadMgr.onDragDict[keyCode];
				if ((GamePadMgr.BoolDelegate)Delegate.Remove(source, callback) == null)
				{
					GamePadMgr.onDragDict.Remove(keyCode);
				}
			}
		}

		public void OnKeyDown(int keyCode)
		{
			int num = 0;
			int num2 = 0;
			if (keyCode == 2048)
			{
				num++;
			}
			if (keyCode == 1024)
			{
				num--;
			}
			if (keyCode == 256)
			{
				num2++;
			}
			if (keyCode == 512)
			{
				num2--;
			}
			bool flag = false;
			if (num != 0)
			{
				GamePadMgr.moveX = (float)num;
				flag = true;
			}
			if (num2 != 0)
			{
				GamePadMgr.moveY = (float)num2;
				flag = true;
			}
			if (flag)
			{
				GamePadMgr.isMove = true;
				GamePadMgr.isLeftScroll = false;
				GamePadMgr.downFlag = true;
			}
			else
			{
				GamePadMgr.isKeyDownSet.Add(keyCode);
				if (GamePadMgr.onPressDict.ContainsKey(keyCode))
				{
					GamePadMgr.onPressDict[keyCode](true);
				}
			}
		}

		public void OnKeyUp(int keyCode)
		{
			int num = -1;
			int num2 = -1;
			if (keyCode == 2048)
			{
				num = 0;
			}
			if (keyCode == 1024)
			{
				num = 0;
			}
			if (keyCode == 256)
			{
				num2 = 0;
			}
			if (keyCode == 512)
			{
				num2 = 0;
			}
			bool flag = false;
			if (num == 0)
			{
				GamePadMgr.moveX = (float)num;
				flag = true;
			}
			if (num2 == 0)
			{
				GamePadMgr.moveY = (float)num2;
				flag = true;
			}
			if (flag)
			{
				if (Mathf.Approximately(GamePadMgr.moveX, 0f) && Mathf.Approximately(GamePadMgr.moveY, 0f))
				{
					GamePadMgr.isMove = false;
				}
				GamePadMgr.upFlag = true;
			}
			else
			{
				GamePadMgr.isKeyDownSet.Remove(keyCode);
				if (GamePadMgr.onDragDict.ContainsKey(keyCode))
				{
					GamePadMgr.onDragDict[keyCode](false);
				}
				if (GamePadMgr.onPressDict.ContainsKey(keyCode))
				{
					GamePadMgr.onPressDict[keyCode](false);
				}
				if (GamePadMgr.onClickDict.ContainsKey(268435455))
				{
					GamePadMgr.onClickDict[268435455]();
				}
				else if (GamePadMgr.onClickDict.ContainsKey(keyCode))
				{
					GamePadMgr.onClickDict[keyCode]();
				}
			}
		}

		public void OnLeftStickScroll(float stickX, float stickY)
		{
			float num = 0f;
			float num2 = 0f;
			if (Mathf.Abs(stickX) > 0.2f)
			{
				num = stickX;
			}
			if (Mathf.Abs(stickY) > 0.2f)
			{
				num2 = stickY;
			}
			bool flag = false;
			if (!Mathf.Approximately(num, 0f))
			{
				GamePadMgr.moveX = num;
				flag = true;
			}
			else if (!GamePadMgr.isMove)
			{
				GamePadMgr.moveX = 0f;
				GamePadMgr.upFlag = true;
			}
			if (!Mathf.Approximately(num2, 0f))
			{
				GamePadMgr.moveY = num2;
				flag = true;
			}
			else if (!GamePadMgr.isMove)
			{
				GamePadMgr.moveY = 0f;
				GamePadMgr.upFlag = true;
			}
			if (flag)
			{
				GamePadMgr.isLeftScroll = true;
				GamePadMgr.isMove = false;
				GamePadMgr.downFlag = true;
			}
			else
			{
				GamePadMgr.isLeftScroll = false;
			}
		}

		public static bool GetKeyButtonDown()
		{
			if (GamePadMgr.isKeyDownSet.Count != 0)
			{
				return false;
			}
			bool result = GamePadMgr.downFlag && (GamePadMgr.isMove || GamePadMgr.isLeftScroll);
			GamePadMgr.downFlag = false;
			return result;
		}

		public static bool IsKeyMove()
		{
			return GamePadMgr.isMove || GamePadMgr.isLeftScroll;
		}

		public static bool GetKeyButton()
		{
			return GamePadMgr.isKeyDownSet.Count == 0 && GamePadMgr.IsKeyMove();
		}

		public static bool GetKeyButtonUp()
		{
			if (GamePadMgr.isKeyDownSet.Count != 0)
			{
				return false;
			}
			bool result = GamePadMgr.upFlag && !GamePadMgr.isMove && !GamePadMgr.isLeftScroll;
			GamePadMgr.upFlag = false;
			return result;
		}

		public static void setCallbackModule(string module)
		{
			GamePadMgr.callSdkApi("setCallbackModule", new object[]
			{
				module
			});
		}

		public static void init()
		{
		}

		private static void callSdkApi(string apiName, params object[] args)
		{
			global::Debug.Log(new object[]
			{
				"callSdkApi GamePad " + apiName + " calling..."
			});
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.netease.ntunisdk.SdkXinyou"))
			{
				using (AndroidJavaObject androidJavaObject = androidJavaClass.CallStatic<AndroidJavaObject>("getInst", new object[0]))
				{
					androidJavaObject.Call(apiName, args);
				}
			}
		}
	}
}
