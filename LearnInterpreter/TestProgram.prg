function factorial(var a) { // calculates factorial
	if (a == 1) {
		return 1;
	};

	return factorial(a - 1) * a;
};

var a = {};

var i = 1;

while (i <= 1000000) {
	arrayAppend(a, i * i);
	i = i + 1;
};