using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.System;
using SFML.Graphics;

using Touhou.IO;

namespace Touhou.Scene {
	internal class CharacterSelectScene : IScene {

		private Text _charSelectText;

		public CharacterSelectScene(int player) {
			_charSelectText = new Text() {
				Font = Game.FontManager.GetFont("redressed"),
				DisplayedString = "Character Select",
				Position = Game.Window.GetCenter()
			};
			_charSelectText.Origin =
				new Vector2f(_charSelectText.GetLocalBounds().Width / 2f, _charSelectText.CharacterSize * 0.75f);
		}

		public void InputPressed(InputData inputData) {	}

		public void InputReleased(InputData inputData) { }

		public void Update(float time, float delta) { }

		public void Render(float time, float delta) { 
			Game.Window.Draw(0, _charSelectText);
		}
	}
}
