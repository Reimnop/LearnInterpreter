var numSteps = 10;

var last = 1;
var factArr = {};

void factorial(var i) { // calculates fibonacci sequence and put them in an array
	last = last * i;

	arrayAppend(factArr, last);

	if (i < numSteps) {
		factorial(i + 1);
	};
};

void printArr(var i) { // print entire array
	println(i, "! = ", factArr[i - 1]);

	if (i < numSteps) {
		printArr(i + 1); // no loop yet so emulate with recursion :P
	};
};

factorial(1);

println("Factorial:");
printArr(1);