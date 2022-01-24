
using SFML.Window;

namespace Touhou.IO {
	internal interface IInput {

		void InputPressed(InputData inputData);
		void InputReleased(InputData inputData);

	}
}
