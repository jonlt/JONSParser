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
	var json = JSON.Parse(jsonString);

	// get the first name
	var firstName = json.Person.firstName;
	var city = json.Person.Address.city;

	// print
	Console.WriteLine("Hi "+firstName+" from "+city);
	Console.Read();