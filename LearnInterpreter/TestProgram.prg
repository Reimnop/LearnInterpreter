// this is my test program

var last = 1;

println("Factorial:");

void factorial(var i) {
	last = last * i;

	print(last, ", ");

	if (i < 20) {
		factorial(i + 1);
	};
};

factorial(1);