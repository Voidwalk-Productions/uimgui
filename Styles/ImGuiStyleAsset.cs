using UnityEngine;
using UnityEngine.Rendering;

namespace UImGui
{
    [CreateAssetMenu(fileName = "ImGuiStyleAsset", menuName = "Scriptable Objects/ImGuiStyleAsset")]
    public class ImGuiStyleAsset : ScriptableObject
    {
        public Texture2D rightArrowhead;
        public Texture2D downArrowhead;
        public Texture2D close;
    }
}
