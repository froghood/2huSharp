using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.System;

using SharpDX.XInput;

namespace Touhou {
	public static partial class Input {

		const float JOYSTICK_MAX = 32767f;
		const float DEADZONE = 0.25f;

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

		public static Action GetAction(GamepadButtonFlags button) {
			_buttonToActionMap.TryGetValue(button, out Action action);
			return action;
		}

		public static void SetJoystickMode(JoystickMode joyMode) {
			_joystickMode = joyMode;
		}

		private static void _pollGamepads() {
			foreach (Controller controller in _controllers) {
				if (!controller.IsConnected) continue; // skip if not connected

				Gamepad gamepad = controller.GetState().Gamepad;
				int userIndex = (int)controller.UserIndex;

				// buttons
				var currentButtons = gamepad.Buttons;
				ref var previousButtons = ref _prevButtons[userIndex];
				var changedButtons = previousButtons ^ currentButtons;

				foreach (GamepadButtonFlags button in Enum.GetValues(typeof(GamepadButtonFlags))) {
					if (button == GamepadButtonFlags.None) continue; // ignore None
					if (!changedButtons.HasFlag(button)) continue; // skip unchanged flags

					if (currentButtons.HasFlag(button)) {
						Game.State.InputPressed(new InputData(userIndex, button)); // pressed
					} else {
						Game.State.InputReleased(new InputData(userIndex, button)); // released
					}
					previousButtons = currentButtons;
				}

				// joystick
				JoystickDirection joystickDirection = _getDirection(gamepad);
				ref JoystickDirection prevJoystickDirection = ref _prevDirections[userIndex];
				if (joystickDirection != prevJoystickDirection) {
					Game.State.InputPressed(new InputData(userIndex, joystickDirection));
					prevJoystickDirection = joystickDirection;
				}
			}
		}

		private static Controller[] _controllers = {
			new Controller(UserIndex.One), new Controller(UserIndex.Two)
		};
		private static GamepadButtonFlags[] _prevButtons = new GamepadButtonFlags[_controllers.Length];
		private static JoystickDirection[] _prevDirections = new JoystickDirection[_controllers.Length];

		private static Dictionary<GamepadButtonFlags, Action> _buttonToActionMap = new() {
			{ GamepadButtonFlags.DPadRight, Action.Right },
			{ GamepadButtonFlags.DPadLeft, Action.Left },
			{ GamepadButtonFlags.DPadDown, Action.Down },
			{ GamepadButtonFlags.DPadUp, Action.Up },
			{ GamepadButtonFlags.X, Action.NonA },
			{ GamepadButtonFlags.A, Action.NonB },
			{ GamepadButtonFlags.B, Action.SpellA },
			{ GamepadButtonFlags.Y, Action.SpellB },
			{ GamepadButtonFlags.LeftShoulder, Action.Focus },
			{ GamepadButtonFlags.RightShoulder, Action.Bomb },
			{ GamepadButtonFlags.LeftThumb, Action.Taunt },
		};

		private static Dictionary<JoystickDirection, Vector2f> _joystickVectors = new() {
			{ JoystickDirection.Center, new Vector2f(0f, 0f) },
			{ JoystickDirection.East, new Vector2f(1f, 0f) },
			{ JoystickDirection.NorthEast, new Vector2f(1, -1) },
			{ JoystickDirection.North, new Vector2f(0f, -1f) },
			{ JoystickDirection.NorthWest, new Vector2f(-1, -1) },
			{ JoystickDirection.West, new Vector2f(-1f, 0f) },
			{ JoystickDirection.SouthWest, new Vector2f(-1, 1) },
			{ JoystickDirection.South, new Vector2f(0f, 1f) },
			{ JoystickDirection.SouthEast, new Vector2f(1, 1) },
		};

		private static JoystickMode _joystickMode = JoystickMode.Simple;

		private static JoystickDirection _getDirection(Gamepad gamepad) {
			switch (_joystickMode) {
				case JoystickMode.Accurate:
					float joystickMagnitude = MathF.Min(JOYSTICK_MAX, MathF.Sqrt(
						gamepad.LeftThumbX * gamepad.LeftThumbX +
						gamepad.LeftThumbY * gamepad.LeftThumbY));
					if (joystickMagnitude >= JOYSTICK_MAX * DEADZONE) {
						float joystickAngle = (MathF.Atan2(gamepad.LeftThumbY, gamepad.LeftThumbX) + MathF.Tau) % MathF.Tau;
						return (JoystickDirection)(MathF.Round(joystickAngle * 8f / MathF.Tau) % 8f + 1f);
					}
					break;

				case JoystickMode.Simple:
					float xDir = MathF.Round(gamepad.LeftThumbX / JOYSTICK_MAX);
					float yDir = MathF.Round(gamepad.LeftThumbY / JOYSTICK_MAX);
					if (xDir != 0 || yDir != 0) {
						float joystickAngle = (MathF.Atan2(yDir, xDir) + MathF.Tau) % MathF.Tau;
						return (JoystickDirection)(MathF.Round(joystickAngle * 8f / MathF.Tau) % 8f + 1f);
					}
					break;
			}
			return JoystickDirection.Center;
		}
	}
}
