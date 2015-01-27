#PI.Net.Http.Formatting.CsvFormatter

MediaTypeFormatter to deliver a CSV file over Web API 2 from an object or objects that inherits from the ICsvFormat interface

##How to implement

###Step 1
Create an object that applies the ICsvFormat interace such as below.

```

    public class AccountCsvView : ICsvFormat
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

		/// implementing the ICsvFormat interface method for building the header
        public string BuildCsvHeader()
        {
            string header = "Id, Name, Email";
            return header;
        }

		/// implementing the ICsvFormat interface method for building the item
        public string BuildCsvItem()
        {
            string item = String.Format("{0},{1},{2}"
                , PI.Net.Http.Formatting.CsvFormatter.Models.CsvFormatItem.Escape(Id)
                , PI.Net.Http.Formatting.CsvFormatter.Models.CsvFormatItem.Escape(Name)               
                , PI.Net.Http.Formatting.CsvFormatter.Models.CsvFormatItem.Escape(Email));
				
			//PI.Net.Http.Formatting.CsvFormatter.Models.CsvFormatItem will take the value and call ToString() 
			//it will then clean the string for a CSV file removing line spaces etc
            return item;
        }
    }

```

###Step 2

Register the formatter within your WebApiConfig.

```
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
			//************** THIS IS THE LINE YOU NEED TO ADD *******************
            // Web API configuration and services
            config.Formatters.Add(new PI.Net.Http.Formatting.CsvFormatter.Formatters.CsvFormatter()); 
			//************** THIS IS THE LINE YOU NEED TO ADD *******************

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
```

###Step 3

Create a normal Web API route method that builds a single or an IEnumerable of your objects to return

```
		[HttpGet]
		[Route("api/accounts")]
		public IHttpActionResult GetAccountsAsCsv()
        {
            try
            {
                //create a list of accounts
                var accounts = List<AccountCsvView>();
				//add an account
				accounts.Add(new AccountCsvView {
					Id = '1234',
					Name = 'Pete',
					Email = 'pete@pete.com'
				});
				//add another account
				accounts.Add(new AccountCsvView {
					Id = '5678',
					Name = 'Bob',
					Email = 'bob@bob.com'
				});

				//return the accounts
                return Ok(accounts);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
```

###Step 4
To call the API just append ``?type=csv`` for example ``http://mysite.com/api/accounts?type=csv``


***************************************************

# License

Copyright (c) 2015 Pi & Mash Limited

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.