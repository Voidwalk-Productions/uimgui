using System;
using System.Collections.Generic;
using UnityEngine;

namespace UImGui
{
	public delegate void CallBack();

	public struct MenuItem
	{
		public string name;
		public CallBack onPressed;
	}
	
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
			normal = { background = GUIUtils.CreateBackgroundTexture(new Color(0.16f, 0.29f, 0.48f, 1f)), textColor = Color.white },
			fontSize = 18,
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
	}

	[Flags]
	public enum WindowFlags
	{
		None = 0,
		Titlebar = 1,
		Menubar = 2,
	}

	public class Window : IWindow
	{
		public bool IsOpen { get; set; }

		private readonly string _windowName			= string.Empty;
		private readonly WindowFlags _windowFlags	= WindowFlags.None;
		private readonly List<MenuItem> _menuItems	= new();

		private Rect _windowRect					= new(20.0f, 20.0f, 256.0f, 256.0f);
		private Vector2 _windowDragOffset			= Vector2.zero;
		private bool _isDragging					= false;

		Window(string name, in Rect windowRect, WindowFlags flags)
		{
			_windowName = name;
			_windowRect = windowRect;
			_windowFlags = flags;
		}

		public static Window Create(string name, in Rect windowRect, WindowFlags flags)
		{ 
			return new Window(name, windowRect, flags);
		}

		public Window AddMenu(params MenuItem[] menuItems)
		{
			_menuItems.AddRange(menuItems);
			return this;
		}

		public void OnGui()
		{
			if (!IsOpen)
			{
				return;
			}

			GUILayout.BeginArea(_windowRect, ImGuiStyles.WindowPanel);
			{
				HandleWindowDragEvent();

				if (_windowFlags.HasFlag(WindowFlags.Titlebar))
				{
					GUILayout.BeginArea(new Rect(0.0f, 0.0f, 256.0f, 28.0f), ImGuiStyles.Header);
					{
						GUILayout.BeginHorizontal();
						{
							if (GUILayout.Button("#", GUILayout.Width(24), GUILayout.Height(24)))
							{
								// Toggle panel fold
							}

							GUILayout.Label(_windowName, ImGuiStyles.Header);

							if (GUILayout.Button("#", GUILayout.Width(24), GUILayout.Height(24)))
							{
								IsOpen = false;
							}
						}

						GUILayout.EndHorizontal();
					}
					GUILayout.EndArea();
				}

				if (_windowFlags.HasFlag(WindowFlags.Menubar))
				{
					GUILayout.BeginArea(new Rect(0.0f, 28.0f, 256.0f, 28.0f), ImGuiStyles.MenuBar);
					GUILayout.BeginHorizontal();
					foreach (var item in _menuItems)
					{
						if (GUILayout.Button(item.name, ImGuiStyles.Button, GUILayout.ExpandWidth(false)))
						{
							item.onPressed?.Invoke();
						}
					}
					GUILayout.EndHorizontal();
					GUILayout.EndArea();
				}
			}
			GUILayout.EndArea();
		}

		private void HandleWindowDragEvent()
		{
			Event e = Event.current;

			Vector2 mouseScreenPos = GUIUtility.GUIToScreenPoint(e.mousePosition);
			if (_windowRect.Contains(mouseScreenPos) && e.type == EventType.MouseDown && e.button == 0)
			{
				_isDragging = true;
				_windowDragOffset = mouseScreenPos - _windowRect.position;
			}

			if (_isDragging)
			{
				if (e.type == EventType.MouseDrag)
				{
					_windowRect.position = mouseScreenPos - _windowDragOffset;
				}
				else if (e.type == EventType.MouseUp)
				{
					_isDragging = false;
				}
			}
		}
	}
}
