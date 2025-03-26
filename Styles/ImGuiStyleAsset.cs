using UnityEngine;

namespace UImGui
{
    [CreateAssetMenu(fileName = "ImGuiStyleAsset", menuName = "Scriptable Objects/ImGuiStyleAsset")]
    public class ImGuiStyleAsset : ScriptableObject
    {
        public Texture2D rightArrowhead;
        public Texture2D downArrowhead;
        public Texture2D close;
        public GUISkin skin;

        public Rect defaultRectPos = new Rect(0, 0, 200, 200);
        public Rect rectPos = new Rect(0, 0, 200, 200);
	}
}
