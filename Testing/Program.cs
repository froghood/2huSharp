using System;

namespace Testing {
	class Program {
		static void Main(string[] args) {
			A testA = new() { foo = 5 };
			ref A testB = ref testA;

			testA = new() { foo = 8 };
			testA = new() { foo = 20 };

			Console.WriteLine(testB.foo);

			while (true) ;
		}
	}

	class A {
		public int foo;
	}
}
