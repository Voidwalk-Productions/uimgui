using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace UImGui
{
	public delegate void MenuItemSelectedCallback();
	public delegate void LayoutOnDrawCallback();

	public struct MenuItem
	{
		public string name;
		public MenuItemSelectedCallback onPressed;
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
		private readonly float _titleBtnSize			= 24.0f;
		private readonly float _resizeBtnSize			= 18.0f;

		private Rect _windowRect						= new(20.0f, 20.0f, 256.0f, 256.0f);
		private Rect _windowFoldedRect					= new(20.0f, 20.0f, 256.0f, 28.0f);
		private Rect _titlebarRect						= new(0.0f, 0.0f, 256.0f, 28.0f);
		private Rect _toolbarRect						= new(0.0f, 28.0f, 256.0f, 28.0f);

		private Vector2 _windowDragOffset				= Vector2.zero;
		private Vector2 _windowResizeOffset				= Vector2.zero;

		private bool _isDragging						= false;
		private bool _isResizing						= false;
		private bool _isFolded							= false;

		private List<LayoutOnDrawCallback> _layoutOnDrawCallbacks = new();

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

		public Window AddLayout(params LayoutOnDrawCallback[] layoutCallbacks)
		{
			_layoutOnDrawCallbacks.AddRange(layoutCallbacks);
			return this;
		}

		private Vector2 scrollViewPosition = Vector2.zero;

		public void OnGui()
		{
			GUI.skin = _styleAsset.skin;

			if (!IsOpen)
			{
				return;
			}

			GUILayout.BeginArea(_isFolded ? _windowFoldedRect : _windowRect, ImGuiStyles.WindowPanel);
			{
				DrawTitlebar();

				if (!_isFolded)
				{
					DrawToolbar();

					const float spacing = 2.0f;
					Rect rect = new(
						_toolbarRect.xMin + spacing, 
						_toolbarRect.yMax + spacing, 
						_windowRect.width - spacing, 
						_windowRect.height - (_titlebarRect.height + _toolbarRect.height + _resizeBtnSize)
					);
					GUILayout.BeginArea(rect);

					scrollViewPosition = GUILayout.BeginScrollView(scrollViewPosition, GUILayout.Width(rect.width), GUILayout.Height(rect.height - _resizeBtnSize));

					foreach (var callback in _layoutOnDrawCallbacks)
					{
						callback?.Invoke();
					}

					GUILayout.EndScrollView();

					GUILayout.EndArea();
				}

				HandleWindowResizeEvent();
				HandleWindowDragEvent();
			}
			GUILayout.EndArea();
		}

		private void HandleWindowResizeEvent()
		{
			Event e = Event.current;
			Vector2 mouseScreenPos = GUIUtility.GUIToScreenPoint(e.mousePosition);

			Rect resizeButtonRect = new (_windowRect.width - _resizeBtnSize, _windowRect.height - _resizeBtnSize, _resizeBtnSize, _resizeBtnSize);

			GUI.Box(resizeButtonRect, "", ImGuiStyles.ResizeButton);

			if (resizeButtonRect.Contains(e.mousePosition))
			{
				if (e.type == EventType.MouseDown && e.button == 0)
				{
					_isResizing = true;
					_windowResizeOffset = mouseScreenPos - _windowRect.max;
				}
				else if (e.type == EventType.MouseUp && e.button == 0)
				{
					_isResizing = false;
				}
			}

			if (_isResizing)
			{
				Vector2 newRectMax = mouseScreenPos - _windowResizeOffset;

				newRectMax.x = MathF.Max(newRectMax.x, _windowRect.min.y + _resizeBtnSize);
				newRectMax.y = Mathf.Max(newRectMax.y, _windowRect.min.y + _toolbarRect.max.y);

				_windowRect.max = newRectMax;
				_titlebarRect.xMax = _windowRect.width;
				_toolbarRect.xMax = _windowRect.width;
			}
		}

		private void HandleWindowDragEvent()
		{
			if (_isResizing)
			{
				return;
			}

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

						if (GUILayout.Button(_styleAsset.close, ImGuiStyles.TitleButton, GUILayout.Width(_titleBtnSize), GUILayout.Height(_titleBtnSize)))
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
				GUILayout.BeginArea(_toolbarRect, ImGuiStyles.MenuBar);
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
