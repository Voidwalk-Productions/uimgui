using UnityEngine;
using System;
using System.Collections.Generic;

namespace UImGui
{
	using FieldFunc = Func<string, object, object>;

	public static partial class UGui
    {
        private static readonly Dictionary<string, string> s_tempFieldStates = new();

        public static T Field<T>(string fieldId, T value)
        {
            using (new GUILayout.HorizontalScope())
            {
                object val = GetFieldDrawer<T>()?.Invoke(fieldId, value);

                GUILayout.Label(fieldId);

                return val != null ? (T)val : value;
            }
        }

        private static FieldFunc GetFieldDrawer<T>()
        {
            Type fieldType = typeof(T);

			if (fieldType == typeof(bool))
			{
				return DrawField_Checkbox;
			}
            else if (fieldType.IsPrimitive || fieldType == typeof(string))
            {
				return DrawField_Generic<T>;
            }
            else
            {
                return null;
            }
        }

        private static object DrawField_Checkbox(string _, object value)
        {
            return GUILayout.Toggle(Convert.ToBoolean(value), "", GUILayout.ExpandWidth(false));
        }

        private static object DrawField_Generic<T>(string fieldID, object value)
        {
			string parsedVal = value?.ToString() ?? "";
            s_tempFieldStates.TryAdd(fieldID, parsedVal);

            GUI.SetNextControlName(fieldID);

			string fieldVal = GUILayout.TextField(s_tempFieldStates[fieldID]);

			if (GUI.GetNameOfFocusedControl() != fieldID && fieldVal != parsedVal)
			{
				try
                {
                    object result = Convert.ChangeType(fieldVal, typeof(T));
                    s_tempFieldStates.Remove(fieldID);
                    return result;
                }
                catch (Exception)
                {
					s_tempFieldStates.Remove(fieldID);
					return null;
                }
			}

            s_tempFieldStates[fieldID] = fieldVal;

			return value;
		}
    }
}
