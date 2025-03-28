using UnityEngine;

namespace UImGui
{
	public static class ImGuiStyles
	{
		public const float DefaultButtonHeight = 25.0f;

		public static readonly GUIStyle s_windowPanel = new()
		{
			alignment = TextAnchor.MiddleLeft,
			normal = { background = GUIUtils.CreateBackgroundTexture(new Color(0.0f, 0.0f, 0.0f, 0.6f)), textColor = Color.white },
			fontSize = 20,
			padding = new(5, 5, 5, 5)
		};

		public static readonly GUIStyle s_header = new()
		{
			alignment = TextAnchor.MiddleLeft,
			stretchHeight = true,
			normal = { background = GUIUtils.CreateBackgroundTexture(new Color(0.16f, 0.29f, 0.48f, 1f)), textColor = Color.white },
			fontSize = 14,
			padding = new(2, 2, 2, 2)
		};

		public static readonly GUIStyle s_menuBar = new()
		{
			alignment = TextAnchor.MiddleLeft,
			normal = { background = GUIUtils.CreateBackgroundTexture(new Color(0.14f, 0.14f, 0.14f, 1f)), textColor = Color.white },
			fontSize = 20,
			padding = new(2, 2, 2, 2)
		};

		public static readonly GUIStyle s_button = new()
		{
			alignment = TextAnchor.MiddleCenter,
			stretchHeight = true,
			normal = { background = GUIUtils.CreateBackgroundTexture(new Color(0.14f, 0.14f, 0.14f, 1f)), textColor = Color.white },
			hover = { background = GUIUtils.CreateBackgroundTexture(new Color(0.24f, 0.5f, 0.81f, 1f)), textColor = Color.white },
			fontSize = 12,
			padding = new(10, 10, 0, 0)
		};

		public static readonly GUIStyle s_titleButton = new()
		{
			alignment = TextAnchor.MiddleCenter,
			stretchHeight = true,
			normal = { background = GUIUtils.CreateBackgroundTexture(new Color(0.0f, 0.0f, 0.0f, 0.0f)) },
			hover = { background = GUIUtils.CreateBackgroundTexture(new Color(0.24f, 0.5f, 0.81f, 1f)) },
			padding = new(6, 6, 6, 6),
			fixedWidth = 24.0f,
			fixedHeight = 24.0f
		};

		public static readonly GUIStyle s_resizeButton = new()
		{
			normal = { background = GUIUtils.CreateBackgroundTexture(new Color(0.24f, 0.5f, 0.81f, 0.1f)) },
			hover = { background = GUIUtils.CreateBackgroundTexture(new Color(0.24f, 0.5f, 0.81f, 0.8f)) },
		};
	}
}
