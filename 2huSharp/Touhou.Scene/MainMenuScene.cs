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

		private MenuSelector _baseMenuSelector;

		public MainMenuScene() {

			_baseMenuSelector = new MenuSelector() {
				Id = "Main",
				Position = (Vector2f)Game.Window.Size / 2f
			};

			// define input events for base selector
			_baseMenuSelector.OnInputPressed += (InputData inputData) => {
				if (inputData.Type == Input.Type.Joystick) return;
				switch (inputData.Action) {
					case Input.Action.Down: _baseMenuSelector.Index++; break;
					case Input.Action.Up: _baseMenuSelector.Index--; break;
				}
				if (inputData.Action == Input.Action.NonB) {
					_baseMenuSelector.Index = _baseMenuSelector.GetIndexById("Exit");
				}
			};

			_baseMenuSelector.OnInputPressed += (InputData inputData) => {
				if (inputData.Type != Input.Type.Joystick) return;
				switch (inputData.Direction) {
					case Input.JoystickDirection.South: _baseMenuSelector.Index++; break;
					case Input.JoystickDirection.North: _baseMenuSelector.Index--; break;
				}
				if (inputData.Action == Input.Action.NonB) {
					_baseMenuSelector.Index = _baseMenuSelector.GetIndexById("Exit");
				}
			};

			// create all menus

			

			var menuOptionConnect = new MenuOption() { Id = "Connect", Position = new Vector2f(0f, -20f) };
			var menuOptionHost = new MenuOption() { Id = "Host", Position = new Vector2f(0f, 20f) };
			var menuOptionExit = new MenuOption() { Id = "Exit", Position = new Vector2f(0f, 80f) };

			// initial colors
			
			menuOptionHost.Text.FillColor = new Color(160, 160, 160);
			menuOptionExit.Text.FillColor = new Color(160, 160, 160);

			_baseMenuSelector.AddMenu(menuOptionConnect);
			_baseMenuSelector.AddMenu(menuOptionHost);
			_baseMenuSelector.AddMenu(menuOptionExit);

			menuOptionConnect.OnInputPressed += (inputData) => {
				if (inputData.Action == Input.Action.NonA) {
					Game.SceneManager.PushScene<ConnectMenuScene>();
				}
			};

			menuOptionHost.OnInputPressed += (inputData) => {
				if (inputData.Action == Input.Action.NonA) {
					Game.SceneManager.PushScene<HostMenuScene>();
				}
			};

			menuOptionExit.OnInputPressed += (inputData) => {
				if (inputData.Action == Input.Action.NonA) {
					Game.Close();
				}
			};

			// loop through options
			foreach (MenuOption menuOption in _baseMenuSelector.GetMenus()) {

				menuOption.Text.Font = Game.FontManager.GetFont("redressed");
				menuOption.Text.DisplayedString = menuOption.Id;			
				menuOption.Text.Align(Alignment.Center, Alignment.Center);
				menuOption.Text.Position = menuOption.GetGlobalPosition();

				//menuOption.OnRender += (time, delta) => {
				//	menuOption.Text.Position = menuOption.GetGlobalPosition();
				//};


				// define sub menu hover events
				menuOption.OnHover += () => { 
					menuOption.Text.FillColor = Color.White;
					menuOption.Text.CharacterSize += 8;
					menuOption.Text.Align(Alignment.Center, Alignment.Center);
				};
				menuOption.OnHoverEnd += () => { 
					menuOption.Text.FillColor = new Color(160, 160, 160);
					menuOption.Text.CharacterSize -= 8;
					menuOption.Text.Align(Alignment.Center, Alignment.Center);
				};

				
				

			}

			_baseMenuSelector.GetMenuByIndex(0).OnHover.Invoke();


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

		
		private Player _player = new(400f, 400f);
	}
}
