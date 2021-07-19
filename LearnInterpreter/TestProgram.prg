// this is my test program

var a = 1;
var b = 1;

println("Fibonacci:");

void foo() {
	var c = a + b;

	b = a;
	a = c;

	println(c);

	if (c < 255) {
		foo();
	};
};

foo();