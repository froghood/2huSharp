using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.System;
using SFML.Graphics;

using Touhou.IO;



namespace Touhou.UI {
	internal class MenuOption : Menu {

		public Text Text { get; set; } = new();
		public Sprite Sprite { get; set; } = new();

		//public MenuOption(Menu parent, string id) : base(parent, id) { }
		//public MenuOption(Menu parent, string id, Vector2f position) : base(parent, id) { Position = position; }

		public override void Update(float time, float delta) {
			OnUpdate?.Invoke(time, delta);
		}

		public override void Render(float time, float delta) {
			//Console.WriteLine("render");
			OnRender?.Invoke(time, delta);
			Game.Window.Draw(0, Text);
			Game.Window.Draw(0, Sprite);
		}

		public override void InputPressed(InputData inputData) {
			OnInputPressed?.Invoke(inputData);
		}

		public override void InputReleased(InputData inputData) {
			OnInputReleased?.Invoke(inputData);
		}
		
	}
}
