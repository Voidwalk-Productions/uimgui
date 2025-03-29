using System;
using System.Reflection;
using UnityEngine;

namespace UImGui
{
    public static partial class UGui
    {
		public static void VectorField<T>(string fieldID, ref T vector) where T : struct
        {
			using (new GUILayout.HorizontalScope())
			{
				Type type = typeof(T);

				FieldInfo[] fields = typeof(T).GetFields();
				foreach (FieldInfo field in fields)
				{
					if (field.IsLiteral || field.IsStatic || !field.FieldType.Equals(typeof(float)))
					{
						continue;
					}

					string id = fieldID + field.Name;
					GUI.SetNextControlName(id);

					float value = (float)field.GetValue(vector);
					string inputVal = value.ToString();

					s_tempFieldStates.TryAdd(id, inputVal);

					string strValue = GUILayout.TextField(s_tempFieldStates[id]);

					if (GUI.GetNameOfFocusedControl() != id && strValue != inputVal)
					{
						if (float.TryParse(strValue, out float parsedValue))
						{
							object vec = vector;
							field.SetValue(vec, parsedValue);
							vector = (T)vec;
						}
					}

					s_tempFieldStates[id] = strValue;
				}

				GUILayout.Label(fieldID);
			}
        }
    }
}
