JONS JSON Parser
================

I small JSON parser, that uses the C# 4.0 dynamic to mimic javascript Objects.

Example:
--------

	var jsonString =    @"{""Person"":
							{
								""firstName"": ""John"",
								""lastName"": ""Smith"",
								""age"": 25,
								""Address"": 
								{
									""streetAddress"" : ""21 2nd Street"",
									""city"":""New York"",
									""state"":""NY"",
									""postalCode"":""10021""
								},
								""PhoneNumbers"": 
								{
									""home"":""212 555-1234"",
									""fax"":""646 555-4567""
								}
							}
						}";

	// parse the string
	var json = JONSParser.JSON.Parse(jsonString);

	// get the name
	var name = json.Person.firstName + " " + json.Person.lastName;

	// print the name
	Console.WriteLine(name);

	// print all the entries in the address
	var address = json.Person.Address;
	foreach (var item in address)
		Console.WriteLine(item);


	Console.Read();
	
Additional Stuff:
-----------------

	// define a property on the object
	// its used in the parser to build the objects, but it is publicly available, so you can use it too.
	json.__defineProperty__(string key, string value)
	
	// test if the object has the specified property
	// returns true or false
	json.hasOwnProperty(string name)
	// shorthand
	json.__has(string name)
	
	// get the value with the specified key
	// same as json.key
	json[string key]
	
	// get the value at the specified index (arrays only)
	json[int index]
	
	// get the length (arrays only)
	json.length	