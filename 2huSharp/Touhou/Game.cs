
using System;
using System.Collections.Generic;
//using System.Text.Json;

using SFML.System;
using SFML.Graphics;
using SFML.Window;

using Touhou.IO;
using Touhou.Graphics;
using Touhou.Scene;
using Touhou.Net;


namespace Touhou {
	internal static class Game {

		public static Config Config;
		public static InputHandler InputHandler;
		public static GameWindow Window;
		public static SceneManager SceneManager;
		public static TextureManager TextureManager;
		public static FontManager FontManager;
		public static Network Network;
		
		private static Queue<Action> _commandBuffer = new();
		private static Clock _clock = new();

		public static void Init() {

			Config = Config.Load(@"config.json");

			InputHandler = new InputHandler();
			InputHandler.LoadConfig(Config.Input);

			_initWindow();

			TextureManager = new TextureManager(@"assets\textures");
			FontManager = new FontManager(@"assets\fonts");

			SceneManager = new SceneManager();

			Network = new Network();

			SceneManager.PushScene<LoadingScene>(new Action(() => {
				TextureManager.LoadTexture("Reimu");
				FontManager.LoadFont("arial");
				FontManager.LoadFont("HanaMinA");
				FontManager.LoadFont("simsun");
				FontManager.LoadFont("redressed");
			}), new Action(() => {
				SceneManager.PopScene();
				SceneManager.PushScene<MainMenuScene>();
			}));
		}

		public static void Start() {

			float prevTime = 0f;

			while (Window.IsOpen) {

				float time = _clock.ElapsedTime.AsSeconds();
				float deltaTime = time - prevTime;

				for (int i = _commandBuffer.Count; i > 0; i--) {
					_commandBuffer.Dequeue().Invoke();
				}

				InputHandler.Poll();
				SceneManager.Update(time, deltaTime);
				SceneManager.Render(time, deltaTime);
				Window.Clear();
				Window.Display();

				prevTime = time;
			}

		}

		public static void Close() {
			Config.Save(@"config.json");
			Window.Close();
		}

		public static void Command(Action action) {
			_commandBuffer.Enqueue(action);
		}

		private static void _initWindow() {
			var width = (uint)Config.Graphics.Width;
			var height = (uint)Config.Graphics.Height;
			var videoMode = new VideoMode(width, height);
			_createWindow(videoMode, "MnS (Mad & Sad)", Styles.Default);
		}

		private static void _createWindow(VideoMode videoMode, string title, Styles styles) {
			Window = new GameWindow(videoMode, title, styles);
			Window.SetKeyRepeatEnabled(false);
			InputHandler.BindEvents(Window);
			Window.ClearColor = new Color(30, 30, 30);
			
		}
	}
}
