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
				if (inputData.Action == Input.Action.NonA) {
					Game.SceneManager.PushScene<ConnectingScene>(menuInputAddress.InputString, menuInputPort.InputString);
				}
			};

			menuOptionBack.OnInputPressed += (InputData inputData) => {
				if (inputData.Action == Input.Action.NonA) Game.SceneManager.PopScene();
			};

			

			foreach (var menuInput in _baseMenuSelector.GetMenus<MenuInput>()) {
				menuInput.Text.Font = Game.FontManager.GetFont("redressed");
				menuInput.Text.FillColor = new Color(160, 160, 160);
				menuInput.Text.DisplayedString = menuInput.Id;
				menuInput.Text.Origin = new Vector2f(menuInput.Text.GetLocalBounds().Width, menuInput.Text.CharacterSize * 0.75f);
				menuInput.Text.Position = menuInput.GetGlobalPosition();
				menuInput.InputText.Font = Game.FontManager.GetFont("redressed");
				menuInput.InputText.FillColor = new Color(160, 160, 160);
				menuInput.InputText.Origin = new Vector2f(0f, menuInput.InputText.CharacterSize * 0.75f);
				menuInput.InputText.Position = menuInput.GetGlobalInputPosition();
				menuInput.Cursor.Size = new Vector2f(2f, menuInput.InputText.CharacterSize * 1.5f);
				menuInput.Cursor.Origin = menuInput.InputText.Origin * 1.2f;
				menuInput.Cursor.Color = Color.White;

				menuInput.OnInputPressed += (InputData inputData) => {
					switch (inputData.Type) {
						case Input.Type.Text:
							menuInput.Input(inputData.Unicode);
							menuInput.Cursor.Index +=
								(inputData.Action == Input.Action.Right).Int() -
								(inputData.Action == Input.Action.Left).Int();
							break;
						case Input.Type.Key:
							if (inputData.Action == Input.Action.NonA) {
								menuInput.BeginTyping();
								menuInput.Cursor.Index = menuInput.InputString.Length;
							}
							break;
					}
				};

				menuInput.OnHover += () => {
					menuInput.Text.FillColor = Color.White;
					menuInput.Text.CharacterSize += 8;
					menuInput.Text.Origin = new Vector2f(menuInput.Text.GetLocalBounds().Width, menuInput.Text.CharacterSize * 0.75f);
				};

				menuInput.OnHoverEnd += () => {
					menuInput.Text.FillColor = new Color(160, 160, 160);
					menuInput.Text.CharacterSize -= 8;
					menuInput.Text.Origin = new Vector2f(menuInput.Text.GetLocalBounds().Width, menuInput.Text.CharacterSize * 0.75f);
				};

				menuInput.OnTypingBegin += () => {
					menuInput.OnHoverEnd.Invoke();
					menuInput.InputText.FillColor = Color.White;
					menuInput.InputText.CharacterSize += 8;
					menuInput.InputText.Origin = new Vector2f(0f, menuInput.InputText.CharacterSize * 0.75f);
				};

				menuInput.OnTypingEnd += () => {
					menuInput.OnHover.Invoke();
					menuInput.InputText.FillColor = new Color(160, 160, 160);
					menuInput.InputText.CharacterSize -= 8;
					menuInput.InputText.Origin = new Vector2f(0f, menuInput.InputText.CharacterSize * 0.75f);
				};
			}

			menuInputAddress.OnTypingEnd += () => Game.Config.Network.DefaultConnectAddress = menuInputAddress.InputString;
			menuInputPort.OnTypingEnd += () => Game.Config.Network.DefaultConnectPort = menuInputPort.InputString;

			foreach (var menuOption in _baseMenuSelector.GetMenus<MenuOption>()) {
				menuOption.Text.Font = Game.FontManager.GetFont("redressed");
				menuOption.Text.FillColor = new Color(160, 160, 160);
				menuOption.Text.DisplayedString = menuOption.Id;
				menuOption.Text.Origin = new Vector2f(menuOption.Text.GetLocalBounds().Width / 2, menuOption.Text.CharacterSize * 0.75f);
				menuOption.Text.Position = menuOption.GetGlobalPosition();

				menuOption.OnHover += () => {
					menuOption.Text.FillColor = Color.White;
					menuOption.Text.CharacterSize += 8;
					menuOption.Text.Origin = new Vector2f(menuOption.Text.GetLocalBounds().Width / 2, menuOption.Text.CharacterSize * 0.75f);
				};

				menuOption.OnHoverEnd += () => {
					menuOption.Text.FillColor = new Color(160, 160, 160);
					menuOption.Text.CharacterSize -= 8;
					menuOption.Text.Origin = new Vector2f(menuOption.Text.GetLocalBounds().Width / 2, menuOption.Text.CharacterSize * 0.75f);
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
