using UnityEngine;

namespace UImGui
{
	public static class GUIUtils
	{
		public static Texture2D CreateBackgroundTexture(Color color)
		{
			Texture2D texture = new(1, 1);
			texture.SetPixel(0, 0, color);
			texture.Apply();
			return texture;
		}
	}
}
