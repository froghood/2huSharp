using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.System;
using SFML.Window;

using SharpDX.XInput;

namespace Touhou.IO {

	// TODO: finish input, gamepad data

	internal readonly struct InputData {

		public Input.Type Type { get; init; }
		
		public Keyboard.Key Key { get => _key ?? Keyboard.Key.Unknown; init => _key = value; }
		public Input.Action Action { get; init; }
		public string Unicode { get; init; }	
		public int User { get; init; }
		public Input.Button Button { get; init; }
		public Input.JoystickDirection Direction { get; init; }
		public Vector2f Vector { get; init; }

		private readonly Keyboard.Key? _key;

		//public UserIndex User { get; }
		//public JoystickDirection Direction { get; }
		//public Vector2f Vector { get; }

		public override string ToString() {
			return
				$"Type: {Type} | " +
				$"Action: {Action} | " +
				$"Key: {Key} | " +
				$"Unicode: {Unicode} | " +
				$"User: {User} | " +
				$"Button: {Button} | " +
				$"Direction: {Direction} | " +
				$"Vector: {Vector}";
		}

		//// key
		//public InputData(Keyboard.Key key) : this() {
		//	InputType = Type.Key;
		//	_key = key;
		//	Action = Game.InputHandler.GetAction(key);
		//}

		//// text
		//public InputData(string unicode) : this() {
		//	InputType = InputType.Text;
		//	Unicode = unicode;
		//}

		//// button
		//public InputData(int user, GamepadButtonFlags button) : this() {
		//	InputType = InputType.Button;
		//	User = user;
		//	Button = button;
		//	Action = GetAction(button);
		//}

		//// joystick
		//public InputData(int user, JoystickDirection direction) : this() {
		//	InputType = InputType.Joystick;
		//	User = user;
		//	Direction = direction;
		//	Vector = _joystickVectors[direction];
		//}

		//public InputData(int user, Action action) : this() {
		//	InputType = InputType.Joystick;
		//	User = user;
		//	Action = action;
		//}






	}

	
}
