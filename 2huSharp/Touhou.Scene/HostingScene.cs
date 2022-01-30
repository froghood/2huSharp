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
	internal class HostingScene : IScene {

		private Task _hostingTask;
		private Text _hostingText;

		public HostingScene(string address, string port) {
			_hostingTask = new Task(() => { Game.Network.Host(port); });


			_hostingText = new Text() {
				Font = Game.FontManager.GetFont("redressed"),
				DisplayedString = "Waiting for opponent...",
				Position = Game.Window.GetCenter(),			
			};
			_hostingText.Origin = new Vector2f(_hostingText.GetLocalBounds().Width / 2f, _hostingText.CharacterSize * 0.75f);


			_hostingTask.Start();
		}

		public void InputPressed(InputData inputData) {
			
		}

		public void InputReleased(InputData inputData) {
			
		}

		public void Update(float time, float delta) {
			if (_hostingTask.IsCompleted) {
				Game.SceneManager.PushScene<CharacterSelectScene>(1);
			}
		}

		public void Render(float time, float delta) {
			Game.Window.Draw(0, _hostingText);
		}
	}
}
