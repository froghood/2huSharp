using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.System;
using SFML.Graphics;


using Touhou.IO;
using Touhou.Net;

namespace Touhou.Scene {
	internal class ConnectingScene : IScene {

		private Task<Network.Result> _connectingTask;
		private Text _connectingText;

		public ConnectingScene(string address, string port) {
			_connectingTask = new Task<Network.Result>(() => { return Game.Network.Connect(address, port); });


			_connectingText = new Text() {
				Font = Game.FontManager.GetFont("redressed"),
				DisplayedString = "Connecting...",
				Position = Game.Window.GetCenter(),
			};
			_connectingText.Origin = 
				new Vector2f(_connectingText.GetLocalBounds().Width / 2f, _connectingText.CharacterSize * 0.75f);


			_connectingTask.Start();
			
		}

		public void InputPressed(InputData inputData) {

		}

		public void InputReleased(InputData inputData) {
			
		}

		public void Update(float time, float delta) {
			if (_connectingTask.IsCompleted) {
				switch (_connectingTask.Result) {
					case Network.Result.Success:
						Game.SceneManager.PushScene<CharacterSelectScene>(2);
						break;
					case Network.Result.Failure:
						Game.SceneManager.PopScene();
						break;
				}
			}
		}

		public void Render(float time, float delta) {
			Game.Window.Draw(0, _connectingText);
		}
	}
}
