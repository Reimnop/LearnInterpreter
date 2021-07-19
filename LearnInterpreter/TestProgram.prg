// this is my test program

void foo(var bar) {
	// do stuff here
	var foz = bar * bar;
	var baz = foz / 4;

	println("foz = ", foz);
	println("baz = ", baz);
};

var a = 8; 
var b = 2;

var c = a + b;
var d = a * b / (10 + a);

if (c > d) {
	var abc = 4;
	foo(abc);
};

var f;

f = a * b;
f = c + f;

println("a + b = ", a + b);
println("f = ", f);
println("c = ", c);
println("d = ", d);

if (f > c) {
	println("Hello, World!");
};