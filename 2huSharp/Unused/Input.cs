
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SFML.System;
using SFML.Window;
using SFML.Graphics;

using SharpDX.XInput;

using Touhou.Graphics;

namespace Touhou.IO {
	public static partial class Input {


		public void Poll() {
			Graphics.DispatchEvents();
			_pollGamepads();
		}

		public static Action GetAction(Keyboard.Key key) {
			_keyboardToActionMap.TryGetValue(key, out Action action);
			return action;

			
		}

		

		public static void BindEvents(RenderWindow context) {
			context.KeyPressed += (s, e) => { Game.State.InputPressed(new InputData(e.Code)); };
			context.KeyReleased += (s, e) => { Game.State.InputReleased(new InputData(e.Code)); };
			context.TextEntered += (s, e) => { if (_isTyping) Game.State.InputPressed(new InputData(e.Unicode)); };
			//context.JoystickMoved += (s, e) => { Game.State.InputPressed(new InputData(e.JoystickId, e.Axis, e.Position)); };
			//context.JoystickButtonPressed += (s, e) => { Game.State.InputPressed(new InputData(e.JoystickId, e.Button)); };
			//context.JoystickButtonReleased += (s, e) => { Game.State.InputReleased(new InputData(e.JoystickId, e.Button)); };
		}

		private static bool _isTyping = false;
		public static void BeginTyping() { _isTyping = true; }
		public static void EndTyping() { _isTyping = false; }

		//public static void SetInputMode(Mode inputMode) {
		//	_mode = inputMode;
		//}

		private static Dictionary<int, Action> _xAxisActionMap = new() { { 1, Action.Right }, { -1, Action.Left } };
		private static Dictionary<int, Action> _yAxisActionMap = new() { { 1, Action.Down }, { -1, Action.Up } };

		private static Dictionary<Keyboard.Key, Action> _keyboardToActionMap = new() {
			{ Keyboard.Key.Right, Action.Right },
			{ Keyboard.Key.Left, Action.Left },
			{ Keyboard.Key.Down, Action.Down },
			{ Keyboard.Key.Up, Action.Up },
			{ Keyboard.Key.Z, Action.NonA },
			{ Keyboard.Key.X, Action.NonB },
			{ Keyboard.Key.C, Action.SpellA },
			{ Keyboard.Key.V, Action.SpellB },
			{ Keyboard.Key.LShift, Action.Focus },
			{ Keyboard.Key.Space, Action.Bomb },
			{ Keyboard.Key.LControl, Action.Taunt }
		};

		public enum Action {
			Unknown,
			Right,
			Left,
			Down,
			Up,
			NonA,
			NonB,
			SpellA,
			SpellB,
			Focus,
			Bomb,
			Taunt
		}

		

		const float DIAGONAL = 0.70710678f;



	}
}
