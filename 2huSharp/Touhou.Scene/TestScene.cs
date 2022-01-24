using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.System;
using SFML.Graphics;

using Touhou.IO;
using Touhou.UI;

namespace Touhou.Scene {
	internal class TestScene : IScene {

		public TestScene() {
			_baseMenuSelector.Position = (Vector2f)Game.Window.Size / 2f;

			// define input events for base selector
			_baseMenuSelector.OnInputPressed += (InputData inputData) => {
				if (inputData.Type == Input.Type.Joystick) return;
				switch (inputData.Action) {
					case Input.Action.Down: _baseMenuSelector.Index++; break;
					case Input.Action.Up: _baseMenuSelector.Index--; break;
				}
			};

			_baseMenuSelector.OnInputPressed += (InputData inputData) => {
				//if (inputData.Type != Input.Type.Joystick) return;
				//switch (inputData.Direction) {
				//	case Input.JoystickDirection.South: _baseMenuSelector.Index++; break;
				//	case Input.JoystickDirection.North: _baseMenuSelector.Index--; break;
				//}
			};

			// create all menus
			var menuSelectorX = _baseMenuSelector.AddMenu<MenuSelector>("X", new Vector2f(0f, -40f));
			var menuSelectorY = _baseMenuSelector.AddMenu<MenuSelector>("Y");
			var menuSelectorZ = _baseMenuSelector.AddMenu<MenuSelector>("Z", new Vector2f(0f, 40f));

			var menuOptionA = menuSelectorX.AddMenu<MenuOption>("A", new Vector2f(-40f, 0f));
			var menuOptionB = menuSelectorX.AddMenu<MenuOption>("B");
			var menuOptionC = menuSelectorX.AddMenu<MenuOption>("C", new Vector2f(40f, 0f));
			var menuOptionD = menuSelectorY.AddMenu<MenuOption>("D", new Vector2f(-40f, 0f));
			var menuOptionE = menuSelectorY.AddMenu<MenuOption>("E");
			var menuOptionF = menuSelectorY.AddMenu<MenuOption>("F", new Vector2f(40f, 0f));
			var menuOptionG = menuSelectorZ.AddMenu<MenuOption>("G", new Vector2f(-40f, 0f));
			var menuOptionH = menuSelectorZ.AddMenu<MenuOption>("H");
			var menuOptionI = menuSelectorZ.AddMenu<MenuOption>("I", new Vector2f(40f, 0f));

			// loop through sub selectors
			foreach (MenuSelector menuSelector in _baseMenuSelector.GetMenus<MenuSelector>()) {

				// define input for sub selector
				menuSelector.OnInputPressed += (InputData inputData) => {
					if (inputData.Type == Input.Type.Joystick) return;
					switch (inputData.Action) {
						case Input.Action.Right: menuSelector.Index++; break;
						case Input.Action.Left: menuSelector.Index--; break;
					}
				};

				menuSelector.OnInputPressed += (InputData inputData) => {
					if (inputData.Type != Input.Type.Joystick) return;
					//switch (inputData.Direction) {
					//	case Input.JoystickDirection.East: menuSelector.Index++; break;
					//	case Input.JoystickDirection.West: menuSelector.Index--; break;
					//}
				};

				// define sub selector hover events
				menuSelector.OnHover += () => {
					MenuOption menu = menuSelector.GetMenuByIndex<MenuOption>();
					if (menu is not null) menu.Text.FillColor = Color.White;
				};

				menuSelector.OnHoverEnd += () => {
					MenuOption menu = menuSelector.GetMenuByIndex<MenuOption>();
					if (menu is not null) menu.Text.FillColor = new Color(160, 160, 160);
				};

				// loop through options in sub selector
				foreach (MenuOption menuOption in menuSelector.GetMenus<MenuOption>()) {
					menuOption.Text.Font = Game.FontManager.GetFont("arial");
					menuOption.Text.FillColor = new Color(80, 80, 80);
					menuOption.Text.DisplayedString = menuOption.Id;

					// define events for option
					menuOption.OnInputPressed += (inputData) => {
						switch (inputData.Action) {
							case Input.Action.NonA: Console.WriteLine(menuOption.Id); break;
						}

					};

					menuOption.OnRender += (time, delta) => {
						menuOption.Text.Position = menuOption.GetGlobalPosition();
					};

					menuOption.OnHover += () => {
						menuOption.Text.FillColor = Color.White;
					};

					menuOption.OnHoverEnd += () => {
						menuOption.Text.FillColor = new Color(80, 80, 80);
					};
				}

				// initial colors
				menuOptionA.Text.FillColor = Color.White;
				menuOptionD.Text.FillColor = new Color(160, 160, 160);
				menuOptionG.Text.FillColor = new Color(160, 160, 160);

			}
		}

		public void Update(float time, float delta) {
			_baseMenuSelector.Update(time, delta);
		}

		public void Render(float time, float delta) {
			//Console.WriteLine(_baseMenuSelector.Position);
			_baseMenuSelector.Render(time, delta);
		}

		public void InputPressed(InputData inputData) {
			_baseMenuSelector.InputPressed(inputData);

			//Console.WriteLine(inputData.ToString());
		}

		public void InputReleased(InputData inputData) {
			_baseMenuSelector.InputReleased(inputData);
		}

		private MenuSelector _baseMenuSelector = new("main");
		private Player _player = new(400f, 400f);
	}
}
