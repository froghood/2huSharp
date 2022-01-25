using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.Window;

namespace Touhou.IO {
	internal static class Input {

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

		public enum Type {
			Unknown,
			Key,
			Text,
			Button,
			Joystick,
		}

		public enum JoystickDirection {
			Center,
			East,
			NorthEast,
			North,
			NorthWest,
			West,
			SouthWest,
			South,
			SouthEast
		}

		public enum JoystickMode {
			Accurate,
			Simple
		}

		public enum Button {
			Unknown,
			A,
			B,
			X,
			Y,
			LShoulder,
			RShoulder,
			Select,
			Start,
			LThumb,
			RThumb,
			Right,
			Left,
			Down,
			Up
		}
	}
}
