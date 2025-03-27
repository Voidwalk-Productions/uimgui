using UnityEngine;
using System;
using System.Collections.Generic;

namespace UImGui
{
	using FieldFunc = Func<string, object, object>;

	public static partial class UGui
    {
        private static Dictionary<string, string> _tempFieldStates = new();

        public static T Field<T>(string fieldId, T value)
        {
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label(fieldId);

                object val = GetFieldDrawer<T>()?.Invoke(fieldId, value);
                return val != null ? (T)val : value;
            }
        }

        private static FieldFunc GetFieldDrawer<T>()
        {
            Type fieldType = typeof(T);

			if (fieldType == typeof(bool))
			{
				return DrawField_Boolean;
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

        private static object DrawField_Boolean(string _, object value)
        {
            return GUILayout.Toggle(Convert.ToBoolean(value), "");
        }

        private static object DrawField_Generic<T>(string fieldID, object value)
        {
			string parsedVal = value?.ToString() ?? "";
            _tempFieldStates.TryAdd(fieldID, parsedVal);

            GUI.SetNextControlName(fieldID);

			string fieldVal = GUILayout.TextField(_tempFieldStates[fieldID]);

			if (GUI.GetNameOfFocusedControl() != fieldID && fieldVal != parsedVal)
			{
				try
                {
                    object result = Convert.ChangeType(fieldVal, typeof(T));
                    _tempFieldStates.Remove(fieldID);
                    return result;
                }
                catch (Exception)
                {
					_tempFieldStates.Remove(fieldID);
					return null;
                }
			}

            _tempFieldStates[fieldID] = fieldVal;

			return value;
		}
    }
}
