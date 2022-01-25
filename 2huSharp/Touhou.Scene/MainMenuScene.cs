using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.System;
using SFML.Window;
using SFML.Graphics;

using Touhou.IO;
using Touhou.UI;
using Touhou.Extensions;

namespace Touhou.Scene {
	internal class MainMenuScene : IScene {

		public MainMenuScene() {
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
				if (inputData.Type != Input.Type.Joystick) return;
				switch (inputData.Direction) {
					case Input.JoystickDirection.South: _baseMenuSelector.Index++; break;
					case Input.JoystickDirection.North: _baseMenuSelector.Index--; break;
				}
			};

			// create all menus
			var menuOptionConnect = _baseMenuSelector.AddMenu<MenuOption>("Connect", new Vector2f(0f, -20f));
			var menuOptionHost = _baseMenuSelector.AddMenu<MenuOption>("Host", new Vector2f(0f, 20f));
			var menuOptionExit = _baseMenuSelector.AddMenu<MenuOption>("Exit", new Vector2f(0f, 80f));

			// initial colors
			menuOptionConnect.Text.FillColor = Color.White;
			menuOptionHost.Text.FillColor = new Color(160, 160, 160);
			menuOptionExit.Text.FillColor = new Color(160, 160, 160);

			menuOptionExit.OnInputPressed += (inputData) => {
				if (inputData.Action == Input.Action.NonA) {
					Game.Window.Close();
				}
			};

			// loop through options
			foreach (MenuOption menuOption in _baseMenuSelector.GetMenus()) {

				menuOption.Text.Font = Game.FontManager.GetFont("HanaMinA");
				menuOption.Text.DisplayedString = menuOption.Id;			
				menuOption.Text.Align(Alignment.Center, Alignment.Center);
				menuOption.Text.Position = menuOption.GetGlobalPosition();

				//menuOption.OnRender += (time, delta) => {
				//	menuOption.Text.Position = menuOption.GetGlobalPosition();
				//};


				// define sub menu hover events
				menuOption.OnHover += () => { menuOption.Text.FillColor = Color.White; };
				menuOption.OnHoverEnd += () => { menuOption.Text.FillColor = new Color(160, 160, 160); };

				
				

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

			Console.WriteLine(inputData);
		}

		public void InputReleased(InputData inputData) {
			_baseMenuSelector.InputReleased(inputData);
		}

		private MenuSelector _baseMenuSelector = new("main");
		private Player _player = new(400f, 400f);
	}
}
