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
	internal class HostMenuScene : IScene {

		private MenuSelector _baseMenuSelector;

		public HostMenuScene() {
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
				InputString = Game.Config.Network.DefaultHostAddress
			};
			_baseMenuSelector.AddMenu(menuInputAddress);


			var menuInputPort = new MenuInput() {
				Id = "Port",
				Position = new Vector2f(-10f, 20f),
				InputPosition = new Vector2f(10f, 20f),
				InputString = Game.Config.Network.DefaultHostPort
			};
			_baseMenuSelector.AddMenu(menuInputPort);

			var menuOptionHost = new MenuOption() { Id = "Host", Position = new Vector2f(0f, 80f) };
			_baseMenuSelector.AddMenu(menuOptionHost);

			var menuOptionBack = new MenuOption() { Id = "Back", Position = new Vector2f(0f, 120f) };
			_baseMenuSelector.AddMenu(menuOptionBack);


			// initial selector index
			_baseMenuSelector.Index = _baseMenuSelector.GetIndexById("Host");



			menuOptionHost.OnInputPressed += (InputData inputData) => {
				if (inputData.Action == Input.Action.NonA) {
					Game.SceneManager.PushScene<HostingScene>(menuInputAddress.InputString, menuInputPort.InputString);
				}
			};

			menuOptionBack.OnInputPressed += (InputData inputData) => {
				if (inputData.Action == Input.Action.NonA) Game.SceneManager.PopScene();
			};

			//TODO: finish input menu or something !! (cursor, alignment, paste, selection, other various text editing features ???)

			foreach (var menuInput in _baseMenuSelector.GetMenus<MenuInput>()) {
				menuInput.Text.Font = Game.FontManager.GetFont("redressed");
				menuInput.Text.FillColor = new Color(160, 160, 160);
				menuInput.Text.DisplayedString = menuInput.Id;
				//menuInput.Text.Align(Alignment.Right, Alignment.Center);
				menuInput.Text.Origin = new Vector2f(menuInput.Text.GetLocalBounds().Width, menuInput.Text.CharacterSize * 0.75f);
				menuInput.Text.Position = menuInput.GetGlobalPosition();
				menuInput.InputText.Font = Game.FontManager.GetFont("redressed");
				menuInput.InputText.FillColor = new Color(160, 160, 160);
				//menuInput.InputText.DisplayedString = menuInput.InputString;
				//menuInput.InputText.Align(Alignment.Left, Alignment.Center);
				menuInput.InputText.Origin = new Vector2f(0f, menuInput.InputText.CharacterSize * 0.75f);
				menuInput.InputText.Position = menuInput.GetGlobalInputPosition();
				menuInput.Cursor.Size = new Vector2f(2f, menuInput.InputText.CharacterSize * 1.5f);
				menuInput.Cursor.Origin = menuInput.InputText.Origin * 1.2f;
				menuInput.Cursor.Color = Color.White;
				//menuInput.Cursor.Align(Alignment.Left, Alignment.Center);

				menuInput.OnInputPressed += (InputData inputData) => {
					switch (inputData.Type) {
						case Input.Type.Text:
							menuInput.Input(inputData.Unicode);

							menuInput.Cursor.Index +=
								(inputData.Action == Input.Action.Right).Int() -
								(inputData.Action == Input.Action.Left).Int();
							//menuInput.InputText.DisplayedString = menuInput.InputString;
							//menuInput.InputText.Align(Alignment.Left, Alignment.Center);
							//menuInput.Cursor.Position = menuInput.GetGlobalInputPosition() +
							//new Vector2f(menuInput.InputText.GetGlobalBounds().Width, 0f);                           
							//Console.WriteLine(inputData.Key);
							break;
						case Input.Type.Key:

							if (inputData.Action == Input.Action.NonA) {
								menuInput.BeginTyping();
								menuInput.Cursor.Index = menuInput.InputString.Length;
							}

							//menuInput.Cursor.Position = menuInput.GetGlobalInputPosition() +
								//new Vector2f(menuInput.InputText.GetGlobalBounds().Width, 0f);

							break;
					}
				};

				menuInput.OnHover += () => {
					menuInput.Text.FillColor = Color.White;
					menuInput.Text.CharacterSize += 8;
					menuInput.Text.Origin = new Vector2f(menuInput.Text.GetLocalBounds().Width, menuInput.Text.CharacterSize * 0.75f);
					//menuInput.Text.Align(Alignment.Right, Alignment.Center);
					//menuInput.Text.Origin = new Vector2f(menuInput.Text.GetLocalBounds().Width, menuInput.Text.CharacterSize / 2f);
				};

				menuInput.OnHoverEnd += () => {
					menuInput.Text.FillColor = new Color(160, 160, 160);
					menuInput.Text.CharacterSize -= 8;
					menuInput.Text.Origin = new Vector2f(menuInput.Text.GetLocalBounds().Width, menuInput.Text.CharacterSize * 0.75f);
					//menuInput.Text.Align(Alignment.Right, Alignment.Center);
					//menuInput.Text.Origin = new Vector2f(menuInput.Text.GetLocalBounds().Width, menuInput.Text.CharacterSize / 2f);
				};

				menuInput.OnTypingBegin += () => {
					menuInput.OnHoverEnd.Invoke();
					menuInput.InputText.FillColor = Color.White;
					menuInput.InputText.CharacterSize += 8;
					menuInput.InputText.Origin = new Vector2f(0f, menuInput.InputText.CharacterSize * 0.75f);
					//menuInput.InputText.Align(Alignment.Left, Alignment.Center);
					//menuInput.Text.Origin = new Vector2f(menuInput.Text.GetLocalBounds().Width, menuInput.Text.CharacterSize);
				};

				menuInput.OnTypingEnd += () => {
					menuInput.OnHover.Invoke();
					menuInput.InputText.FillColor = new Color(160, 160, 160);
					menuInput.InputText.CharacterSize -= 8;
					menuInput.InputText.Origin = new Vector2f(0f, menuInput.InputText.CharacterSize * 0.75f);
					//menuInput.InputText.Align(Alignment.Left, Alignment.Center);
					//menuInput.Text.Origin = new Vector2f(menuInput.Text.GetLocalBounds().Width, menuInput.Text.CharacterSize);
				};


			}

			menuInputAddress.OnTypingEnd += () => Game.Config.Network.DefaultHostAddress = menuInputAddress.InputString;
			menuInputPort.OnTypingEnd += () => Game.Config.Network.DefaultHostPort = menuInputPort.InputString;

			foreach (var menuOption in _baseMenuSelector.GetMenus<MenuOption>()) {
				menuOption.Text.Font = Game.FontManager.GetFont("redressed");
				menuOption.Text.FillColor = new Color(160, 160, 160);
				menuOption.Text.DisplayedString = menuOption.Id;
				//menuOption.Text.Align(Alignment.Center, Alignment.Center);
				menuOption.Text.Origin = new Vector2f(menuOption.Text.GetLocalBounds().Width / 2, menuOption.Text.CharacterSize * 0.75f);
				menuOption.Text.Position = menuOption.GetGlobalPosition();
				menuOption.OnHover += () => { 
					menuOption.Text.FillColor = Color.White;
					menuOption.Text.CharacterSize += 8;
					menuOption.Text.Origin = new Vector2f(menuOption.Text.GetLocalBounds().Width / 2, menuOption.Text.CharacterSize * 0.75f);

					//menuOption.Text.Align(Alignment.Center, Alignment.Center);
				};
				menuOption.OnHoverEnd += () => { 
					menuOption.Text.FillColor = new Color(160, 160, 160);
					menuOption.Text.CharacterSize -= 8;
					menuOption.Text.Origin = new Vector2f(menuOption.Text.GetLocalBounds().Width / 2, menuOption.Text.CharacterSize * 0.75f);

					//menuOption.Text.Align(Alignment.Center, Alignment.Center);
				};

				//menuOption.OnRender += (time, delta) => {
				//	Game.Window.Draw(0, new CircleShape() {
				//		Position = menuOption.Text.Position,
				//		OutlineColor = Color.White,
				//		OutlineThickness = 1f,
				//		FillColor = Color.Transparent,
				//		Radius = 1f,
				//	}) ;
				//};
			}

			//_baseMenuSelector.Index = _baseMenuSelector.GetIndexById("Host");
			_baseMenuSelector.GetMenuByIndex().OnHover.Invoke();

			

			

			

			//_baseMenuSelector.GetMenuByIndex(0).OnHover.Invoke();

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
