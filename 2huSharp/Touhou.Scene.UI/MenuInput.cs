using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.System;
using SFML.Graphics;

using Touhou.IO;
using Touhou.Extensions;

namespace Touhou.UI {
	internal class MenuInput : Menu {

		public Text Text { get; set; }
		public Sprite Sprite { get; set; }
		public Text InputText { get; set; }
		public MenuInputCursor Cursor { get; set; }

		public Action OnTypingBegin;
		public Action OnTypingEnd;

		public Vector2f InputPosition { get; set; }

		public string InputString {
			get => _inputString;
			set {
				_inputString = value;
				InputText.DisplayedString = _inputString;
				//CursorPosition = _cursorPosition;
			}
		}
		public bool IsTyping { get; set; } = false;


		private string _inputString;
		private float _cursorVisibility = 0f;
		//private int _cursorPosition;

		public MenuInput() {
			Text = new Text();
			Sprite = new Sprite();
			InputText = new Text();
			Cursor = new MenuInputCursor(this);
		}


		

		//public MenuInput(Menu parent, string id) : base(parent, id) { }
		//public MenuInput(Menu parent, string id, Vector2f position) : base(parent, id) { Position = position; }

		public void Input(string unicode) {
			var prevLength = InputString.Length;

			switch (unicode) {
				case "\u0008": // backspace
					if (InputString.Length > 0 && Cursor.Index > 0) InputString = InputString.Remove(Cursor.Index - 1, 1);
					break; 

				case "\u000D": // return
					EndTyping();
					break;

				case "\u0016": // paste
					break;

				default:
					if (unicode != null) InputString = InputString.Insert(Cursor.Index, unicode);
					break;
			}

			Cursor.Index += InputString.Length - prevLength;
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
		
		public class MenuInputCursor : Drawable {

			public int Index {
				get => _index;
				set {
					int prevIndex = _index;				
					_index = Math.Min(Math.Max(value, 0), _parent.InputString.Length);
					if ((_index - prevIndex) != 0) _parent._cursorVisibility = 0f;
				}
			}

			public Vector2f Size { 
				get => _shape.Size; 
				set => _shape.Size = value;
			}

			public Vector2f Origin {
				get => _shape.Origin;
				set => _shape.Origin = value;
			}

			public Color Color {
				get => _shape.FillColor;
				set => _shape.FillColor = value;
			}

			
			

			private MenuInput _parent;
			private RectangleShape _shape;
			private int _index;

			public MenuInputCursor(MenuInput parent) {
				_parent = parent;
				_shape = new RectangleShape();
			}

			public void Draw(RenderTarget target, RenderStates states) {
				_shape.Position = _parent.InputText.Position + _parent.InputText.FindCharacterPos((uint)Index);
				_shape.Draw(target, states);
			}
		}

	}
}
