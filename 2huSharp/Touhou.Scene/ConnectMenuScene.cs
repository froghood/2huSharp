using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.System;
using SFML.Window;
using SFML.Graphics;

using Touhou.UI;
using Touhou.IO;
using Touhou.Extensions;

namespace Touhou.Scene {
	internal class ConnectMenuScene : IScene {

		private MenuSelector _baseMenuSelector;

		public ConnectMenuScene() {
			_baseMenuSelector = new MenuSelector() {
				Id = "Main",
				Position = (Vector2f)Game.Window.Size / 2f
			};

			_baseMenuSelector.OnInputPressed += (InputData inputData) => {
				if (inputData.Type == Input.Type.Text) return;
				_baseMenuSelector.Index +=
					(inputData.Action == Input.Action.Down).Int() -
					(inputData.Action == Input.Action.Up).Int();
				if (inputData.Action == Input.Action.NonB) {
					_baseMenuSelector.Index = _baseMenuSelector.GetIndexById("Back");
				}
			};
			
			var menuInputAddress = new MenuInput() {
				Id = "Address",
				Position = new Vector2f(-10f, -20f),
				InputPosition = new Vector2f(10f, -20f),
				InputString = Game.Config.Network.DefaultConnectAddress
			};
			_baseMenuSelector.AddMenu(menuInputAddress);


			var menuInputPort = new MenuInput() {
				Id = "Port",
				Position = new Vector2f(-10f, 20f),
				InputPosition = new Vector2f(10f, 20f),
				InputString = Game.Config.Network.DefaultConnectPort
			};
			_baseMenuSelector.AddMenu(menuInputPort);

			var menuOptionConnect = new MenuOption() { Id = "Connect", Position = new Vector2f(0f, 80f) };
			_baseMenuSelector.AddMenu(menuOptionConnect);

			var menuOptionBack = new MenuOption() { Id = "Back", Position = new Vector2f(0f, 120f) };
			_baseMenuSelector.AddMenu(menuOptionBack);


			// initial selector index
			_baseMenuSelector.Index = _baseMenuSelector.GetIndexById("Connect");



			menuOptionConnect.OnInputPressed += (InputData inputData) => {
				if (inputData.Action == Input.Action.NonA) Console.WriteLine(menuOptionConnect.Id);
			};

			menuOptionBack.OnInputPressed += (InputData inputData) => {
				if (inputData.Action == Input.Action.NonA) Game.SceneManager.PopScene();
			};

			

			foreach (var menuInput in _baseMenuSelector.GetMenus<MenuInput>()) {
				menuInput.Text.Font = Game.FontManager.GetFont("redressed");
				menuInput.Text.FillColor = new Color(160, 160, 160);
				menuInput.Text.DisplayedString = menuInput.Id;
				menuInput.Text.Align(Alignment.Right, Alignment.Center);
				menuInput.Text.Position = menuInput.GetGlobalPosition();
				menuInput.InputText.Font = Game.FontManager.GetFont("redressed");
				menuInput.InputText.FillColor = new Color(160, 160, 160);
				menuInput.InputText.DisplayedString = menuInput.InputString;
				menuInput.InputText.Align(Alignment.Left, Alignment.Center);
				menuInput.InputText.Position = menuInput.GetGlobalInputPosition();
				menuInput.Cursor.Size = new Vector2f(2f, 40f);
				menuInput.Cursor.FillColor = Color.White;
				menuInput.Cursor.Align(Alignment.Left, Alignment.Center);

				menuInput.OnInputPressed += (InputData inputData) => {
					switch (inputData.Type) {
						case Input.Type.Text:
							menuInput.Input(inputData.Unicode);
							menuInput.InputText.DisplayedString = menuInput.InputString;
							menuInput.InputText.Align(Alignment.Left, Alignment.Center);
							menuInput.Cursor.Position = menuInput.GetGlobalInputPosition() +
								new Vector2f(menuInput.InputText.GetGlobalBounds().Width, 0f);
							//Console.WriteLine(inputData.Key);
							break;
						case Input.Type.Key:
							if (inputData.Action == Input.Action.NonA) menuInput.BeginTyping();
							menuInput.Cursor.Position = menuInput.GetGlobalInputPosition() +
								new Vector2f(menuInput.InputText.GetGlobalBounds().Width, 0f);

							break;
					}
				};

				menuInput.OnHover += () => {
					menuInput.Text.FillColor = Color.White;
					menuInput.Text.CharacterSize += 8;
					menuInput.Text.Align(Alignment.Right, Alignment.Center);
				};

				menuInput.OnHoverEnd += () => {
					menuInput.Text.FillColor = new Color(160, 160, 160);
					menuInput.Text.CharacterSize -= 8;
					menuInput.Text.Align(Alignment.Right, Alignment.Center);
				};

				menuInput.OnTypingBegin += () => {
					menuInput.OnHoverEnd.Invoke();
					menuInput.InputText.FillColor = Color.White;
					menuInput.InputText.CharacterSize += 8;
					menuInput.InputText.Align(Alignment.Left, Alignment.Center);
				};

				menuInput.OnTypingEnd += () => {
					menuInput.OnHover.Invoke();
					menuInput.InputText.FillColor = new Color(160, 160, 160);
					menuInput.InputText.CharacterSize -= 8;
					menuInput.InputText.Align(Alignment.Left, Alignment.Center);
				};
			}

			menuInputAddress.OnTypingEnd += () => Game.Config.Network.DefaultConnectAddress = menuInputAddress.InputString;
			menuInputPort.OnTypingEnd += () => Game.Config.Network.DefaultConnectPort = menuInputPort.InputString;

			foreach (var menuOption in _baseMenuSelector.GetMenus<MenuOption>()) {
				menuOption.Text.Font = Game.FontManager.GetFont("redressed");
				menuOption.Text.FillColor = new Color(160, 160, 160);
				menuOption.Text.DisplayedString = menuOption.Id;
				menuOption.Text.Align(Alignment.Center, Alignment.Center);
				menuOption.Text.Position = menuOption.GetGlobalPosition();
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

			_baseMenuSelector.GetMenuByIndex().OnHover.Invoke();

		}

		public void Update(float time, float delta) {
			_baseMenuSelector.Update(time, delta);
		}

		public void Render(float time, float delta) {
			_baseMenuSelector.Render(time, delta);
		}

		public void InputPressed(InputData inputData) {
			_baseMenuSelector.InputPressed(inputData);
		}

		public void InputReleased(InputData inputData) {
			_baseMenuSelector.InputReleased(inputData);
		}
	}
}
