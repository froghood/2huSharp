using System;
using System.Threading;

namespace Testing {
	class Program {
		static void Main(string[] args) {
			A a = new A();
			Console.WriteLine(a.Foo);

			Thread.Sleep(5000);
		}
	}

	public class A {

		public int Foo { get => b.Foo; set => b.Foo = value; }

		private B b;

		

		public A() {
			b = new B() { Foo = 5 };
		}

		private class B {
			public int Foo;
		}
	}
}
