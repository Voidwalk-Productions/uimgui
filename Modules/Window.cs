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

	[Flags]
	public enum WindowFlags
	{
		None = 0,
		Titlebar = 1,
		Toolbar = 2,
	}

	public class Window : IWindow
	{
		public bool IsOpen { get; set; }

		private readonly string _windowName				= string.Empty;
		private readonly WindowFlags _windowFlags		= WindowFlags.None;
		private readonly List<MenuItem> _menuItems		= new();
		private readonly ImGuiStyleAsset _styleAsset	= null;

		private Rect _windowRect						= new(20.0f, 20.0f, 256.0f, 256.0f);
		private Rect _windowFoldedRect					= new(20.0f, 20.0f, 256.0f, 28.0f);
		private Rect _titlebarRect						= new(0.0f, 0.0f, 256.0f, 28.0f);

		private Vector2 _windowDragOffset				= Vector2.zero;
		private bool _isDragging						= false;
		private bool _isFolded							= false;

		Window(string name, in Rect windowRect, WindowFlags flags, in ImGuiStyleAsset styleAsset)
		{
			_windowName = name;
			_windowRect = windowRect;
			_windowFlags = flags;
			_styleAsset = styleAsset;
		}

		public static Window Create(string name, in Rect windowRect, WindowFlags flags, in ImGuiStyleAsset styleAsset)
		{ 
			return new Window(name, windowRect, flags, styleAsset);
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

			GUILayout.BeginArea(_isFolded ? _windowFoldedRect : _windowRect, ImGuiStyles.WindowPanel);
			{
				HandleWindowDragEvent();

				DrawTitlebar();

				if (!_isFolded)
				{
					DrawToolbar();
					// Draw user elements here
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
					Vector2 newRectPos = mouseScreenPos - _windowDragOffset;
					_windowRect.position = newRectPos;
					_windowFoldedRect.position = newRectPos;
				}
				else if (e.type == EventType.MouseUp)
				{
					_isDragging = false;
				}
			}
		}

		private void DrawTitlebar()
		{
			if (_windowFlags.HasFlag(WindowFlags.Titlebar))
			{
				GUILayout.BeginArea(_titlebarRect, ImGuiStyles.Header);
				{
					GUILayout.BeginHorizontal();
					{
						if (GUILayout.Button(_isFolded ? _styleAsset.rightArrowhead : _styleAsset.downArrowhead, ImGuiStyles.TitleButton, GUILayout.Width(24), GUILayout.Height(24)))
						{
							_isFolded = !_isFolded;
						}

						GUILayout.Label(_windowName, ImGuiStyles.Header);

						if (GUILayout.Button(_styleAsset.close, ImGuiStyles.TitleButton, GUILayout.Width(24), GUILayout.Height(24)))
						{
							IsOpen = false;
						}
					}

					GUILayout.EndHorizontal();
				}
				GUILayout.EndArea();
			}
		}

		private void DrawToolbar()
		{
			if (_windowFlags.HasFlag(WindowFlags.Toolbar))
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
	}
}
