
using System;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Collections.Generic;

using SFML.Window;
using SFML.System;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using Touhou.IO;

namespace Touhou {
	internal class Config {
		public ConfigGraphics Graphics { get; set; }
		public ConfigInput Input { get; set; }
		public ConfigNetwork Network { get; set; }

		public static Config Load(string path) {	

			return JsonConvert.DeserializeObject<Config>(File.ReadAllText(path), new StringEnumConverter());

			//var json = JsonConvert.DeserializeObject<JToken>(File.ReadAllText(path));
			//var configProperties = typeof(Config).GetProperties(BindingFlags.Public | BindingFlags.Static);
			//foreach (JProperty jsonProperty in json) {

			//	// get the corresponding config field by comparing its name to the json property's name
			//	var configProperty = configProperties
			//		.SingleOrDefault(p => p.Name.Equals(jsonProperty.Name, StringComparison.OrdinalIgnoreCase));

			//	// instantiate the config property by converting the json property's value to the corresponding object
			//	configProperty.SetValue(null, jsonProperty.Value.ToObject(configProperty.PropertyType));
			//}
		}

		public void Save(string path) {

			var jsonText = JsonConvert.SerializeObject(this, Formatting.Indented, new StringEnumConverter());
			File.WriteAllText(path, jsonText);

			//var configProperties = typeof(Config).GetProperties(BindingFlags.Public | BindingFlags.Static);
			//var jsonDictionary = new Dictionary<string, object>();
			//foreach (var property in configProperties) {
			//	jsonDictionary[property.Name] = property.GetValue(null);
			//}
			//var jsonText = JsonConvert.SerializeObject(jsonDictionary, Formatting.Indented);
			//File.WriteAllText(path, jsonText);
		}

		public class ConfigGraphics {
			public bool Fullscreen;
			public int Left;
			public int Top;
			public int Width;
			public int Height;
		}

		public class ConfigInput {
			public ConfigInputKeyboard Keyboard { get; set; }
			public ConfigInputGamepad Gamepad { get; set; }

			public class ConfigInputKeyboard {
				public Keyboard.Key Right;
				public Keyboard.Key Left;
				public Keyboard.Key Down;
				public Keyboard.Key Up;
				public Keyboard.Key NonA;
				public Keyboard.Key NonB;
				public Keyboard.Key SpellA;
				public Keyboard.Key SpellB;
				public Keyboard.Key Focus;
				public Keyboard.Key Bomb;
				public Keyboard.Key Taunt;
			}

			public class ConfigInputGamepad {
				public Input.Button Right;
				public Input.Button Left;
				public Input.Button Down;
				public Input.Button Up;
				public Input.Button NonA;
				public Input.Button NonB;
				public Input.Button SpellA;
				public Input.Button SpellB;
				public Input.Button Focus;
				public Input.Button Bomb;
				public Input.Button Taunt;
			}
		}

		public class ConfigNetwork {
			public string DefaultConnectAddress;
			public string DefaultConnectPort;
			public string DefaultHostAddress;
			public string DefaultHostPort;
		}
	}	
}
