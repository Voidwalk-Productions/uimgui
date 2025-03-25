using UnityEngine;

namespace UImGui
{
	public static class ImGuiStyles
	{
		public static readonly GUIStyle WindowPanel = new()
		{
			alignment = TextAnchor.MiddleLeft,
			normal = { background = GUIUtils.CreateBackgroundTexture(new Color(0.0f, 0.0f, 0.0f, 0.6f)), textColor = Color.white },
			fontSize = 20,
			padding = new(5, 5, 5, 5)
		};

		public static readonly GUIStyle Header = new()
		{
			alignment = TextAnchor.MiddleLeft,
			stretchHeight = true,
			normal = { background = GUIUtils.CreateBackgroundTexture(new Color(0.16f, 0.29f, 0.48f, 1f)), textColor = Color.white },
			fontSize = 14,
			padding = new(2, 2, 2, 2)
		};

		public static readonly GUIStyle MenuBar = new()
		{
			alignment = TextAnchor.MiddleLeft,
			normal = { background = GUIUtils.CreateBackgroundTexture(new Color(0.14f, 0.14f, 0.14f, 1f)), textColor = Color.white },
			fontSize = 20,
			padding = new(2, 2, 2, 2)
		};

		public static readonly GUIStyle Button = new()
		{
			alignment = TextAnchor.MiddleCenter,
			stretchHeight = true,
			normal = { background = GUIUtils.CreateBackgroundTexture(new Color(0.14f, 0.14f, 0.14f, 1f)), textColor = Color.white },
			hover = { background = GUIUtils.CreateBackgroundTexture(new Color(0.24f, 0.5f, 0.81f, 1f)), textColor = Color.white },
			fontSize = 12,
			padding = new(10, 10, 0, 0)
		};

		public static readonly GUIStyle TitleButton = new()
		{
			alignment = TextAnchor.MiddleCenter,
			stretchHeight = true,
			normal = { background = GUIUtils.CreateBackgroundTexture(new Color(0.0f, 0.0f, 0.0f, 0.0f)) },
			hover = { background = GUIUtils.CreateBackgroundTexture(new Color(0.24f, 0.5f, 0.81f, 1f)) },
			padding = new(6, 6, 6, 6)
		};
	}
}
