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
	internal class MenuInput : Menu {

		public Text Text { get; set; } = new();
		public Sprite Sprite { get; set; } = new();

		public string InputString { get; set; }
		public Text InputText { get; set; } = new();
		public Vector2f InputPosition { get; set; }

		public Action OnTypingBegin;
		public Action OnTypingEnd;
		public bool IsTyping { get; set; } = false;

		public RectangleShape Cursor;
		private float _cursorVisibility = 0f;

		public MenuInput() { 
			InputString = "";
			Cursor = new RectangleShape();
		}


		

		//public MenuInput(Menu parent, string id) : base(parent, id) { }
		//public MenuInput(Menu parent, string id, Vector2f position) : base(parent, id) { Position = position; }

		public void Input(string unicode) {
			Console.WriteLine(unicode == "\u0016");
			_cursorVisibility = 0f;
			switch (unicode) {
				case "\u0008": // backspace
					if (InputString.Length > 0) InputString = InputString.Remove(InputString.Length - 1, 1);
					break; 
				case "\u000D": // return
					EndTyping();
					break;
				case "\u0016": // paste

					break;
				default:
					InputString += unicode;
					break;
			}
		}

		public void BeginTyping() {
			OnTypingBegin?.Invoke();
			_cursorVisibility = 0f;
			IsTyping = true;
			Game.InputHandler.BeginTyping();
		}

		public void EndTyping() {
			OnTypingEnd?.Invoke();
			IsTyping = false;
			Game.InputHandler.EndTyping();
		}

		public Vector2f GetGlobalInputPosition() {
			if (Parent is null) return InputPosition;
			else return InputPosition + Parent.GetGlobalPosition();
		}

		public override void Update(float time, float delta) {
			OnUpdate?.Invoke(time, delta);
		}

		public override void Render(float time, float delta) {
			//Console.WriteLine("render");
			_cursorVisibility = (_cursorVisibility + delta) % 1f;

			OnRender?.Invoke(time, delta);
			Game.Window.Draw(0, Text);
			Game.Window.Draw(0, InputText);
			Game.Window.Draw(0, Sprite);
			if (IsTyping && _cursorVisibility < 0.5f) Game.Window.Draw(0, Cursor);
		}

		public override void InputPressed(InputData inputData) {
			OnInputPressed?.Invoke(inputData);
		}

		public override void InputReleased(InputData inputData) {
			OnInputReleased?.Invoke(inputData);
		}
		
	}
}
