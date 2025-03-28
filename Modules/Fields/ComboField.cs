using System;
using System.Collections.Generic;
using UnityEngine;

namespace UImGui
{
    public static partial class UGui
    {
        private static readonly HashSet<string> s_openPopups = new();

        public static bool ComboField(string fieldId, ref int currentSelection, params string[] options)
        {
            if (currentSelection < 0 || currentSelection >= options.Length)
            {
                Debug.LogError("[UGui.DropdownField] currentSelection out of bounds");
                return false;
            }

            Rect dropdownRect = Rect.zero;
            using (new GUILayout.HorizontalScope())
            {
				if (GUILayout.Button(options[currentSelection], GUILayout.ExpandWidth(false)))
				{
                    if (s_openPopups.Contains(fieldId))
                    {
                        s_openPopups.Remove(fieldId);
                    }
                    else
                    {
                        s_openPopups.Add(fieldId);
                    }
				}
				dropdownRect = GUILayoutUtility.GetLastRect();

				GUILayout.Label(fieldId);
			}

            if (s_openPopups.Contains(fieldId))
            {
                const float ButtonSize = ImGuiStyles.DefaultButtonHeight;

				dropdownRect.y += dropdownRect.height;
                dropdownRect.height = options.Length * ButtonSize;

                GUI.Box(dropdownRect, "");
                
                for (int i = 0; i < options.Length; ++i)
                {
                    string option = options[i];
                    Rect optionRect = new(dropdownRect.x, dropdownRect.y + (i * ButtonSize), dropdownRect.width, ButtonSize);
                    if (GUI.Button(optionRect, option))
                    {
                        s_openPopups.Remove(fieldId);
                        currentSelection = Array.IndexOf(options, option);
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
