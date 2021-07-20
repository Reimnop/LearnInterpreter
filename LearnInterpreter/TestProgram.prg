function factorial(var a) { // calculates factorial
	if (a == 1) {
		return 1;
	};

	return factorial(a - 1) * a;
};

println("20! = ", factorial(20));