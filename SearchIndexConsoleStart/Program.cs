using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Azure.Search.Documents.Models;
using SearchIndexConsoleEnd.Data;
using System.Reflection.Metadata;

// Den här appen skapar ett Hotels-index som du läser in med hotelldata och kör frågor mot.
// https://learn.microsoft.com/sv-se/azure/search/search-get-started-dotnet

class Program
{
    static void Main(string[] args)
    {
        // Info from my Azure Portal
        string serviceName = "azure-search-demo";
        string apiKey = "BnKvEo0tIarB0UGWTuD14M0gwm0FJgGERVH6kzANHxAzSeB59L99";
        string indexName = "hotels-search";

        // ///////////////////////////////////////////////////////////////////////////////////
        // Skapa två klienter:
        //
        // 1. SearchIndexClient skapar indexet och
        // 2. SearchClient läser in och frågar ett befintligt index.
        // Båda behöver tjänstslutpunkten och en API-administratörsnyckel för autentisering med
        // behörighet att skapa/ta bort.

        // Create a SearchIndexClient to send create/delete index commands
        Uri serviceEndpoint = new Uri($"https://{serviceName}.search.windows.net/");
        AzureKeyCredential credential = new AzureKeyCredential(apiKey);
        SearchIndexClient adminClient = new SearchIndexClient(serviceEndpoint, credential);

        // Create a SearchClient to load and query documents
        SearchClient srchclient = new SearchClient(serviceEndpoint, indexName, credential);

        // Delete index if it exists /////////////////////////////////////////////////////////
        Console.WriteLine("{0}", "Deleting index...\n");
        DeleteIndexIfExists(indexName, adminClient);

        // ///////////////////////////////////////////////////////////////////////////////////
        // Create index
        Console.WriteLine("{0}", "Creating index...\n");
        CreateIndex(indexName, adminClient);

        // ///////////////////////////////////////////////////////////////////////////////////
        // Load documents

        // An ingestor is a program that reads data entering the system through a communications port. 
        SearchClient ingesterClient = adminClient.GetSearchClient(indexName);
        Console.WriteLine("{0}", "Uploading documents...\n");
        UploadDocuments(ingesterClient);

        // Eftersom det här är en konsolapp som kör alla kommandon sekventiellt lägger du till en
        // väntetid på 2 sekunder mellan indexering och frågor.
        // (for demo and console-app purposes only)
        Console.WriteLine("Waiting for indexing...\n");
        System.Threading.Thread.Sleep(2000);

        // Call the RunQueries method to invoke a series of queries
        Console.WriteLine("Starting queries...\n");
        RunQueries(srchclient);

        // End the program
        Console.WriteLine("{0}", "Complete. Press any key to end this program...\n");
        Console.ReadKey();
    }

    // End of Main - End of Main - End of Main - End of Main - End of Main - End of Main - End of Main -
    // End of Main - End of Main - End of Main - End of Main - End of Main - End of Main - End of Main -
    // End of Main - End of Main - End of Main - End of Main - End of Main - End of Main - End of Main -
    // End of Main - End of Main - End of Main - End of Main - End of Main - End of Main - End of Main -

    // Methods - Methods - Methods - Methods - Methods - Methods - Methods - Methods - Methods - Methods -
    // Methods - Methods - Methods - Methods - Methods - Methods - Methods - Methods - Methods - Methods -
    // Methods - Methods - Methods - Methods - Methods - Methods - Methods - Methods - Methods - Methods -
    // Methods - Methods - Methods - Methods - Methods - Methods - Methods - Methods - Methods - Methods -


    // Delete the hotels-quickstart index to reuse its name /////////////////////////////////
    static void DeleteIndexIfExists(string indexName, SearchIndexClient adminClient)
    {
        adminClient.GetIndexNames();
        {
            adminClient.DeleteIndex(indexName);
        }
    }
    // Create hotels-quickstart index ////////////////////////////////////////////////////////
    static void CreateIndex(string indexName, SearchIndexClient adminClient)
    {
        FieldBuilder fieldBuilder = new FieldBuilder();
        var searchFields = fieldBuilder.Build(typeof(Hotel));

        var definition = new SearchIndex(indexName, searchFields);

        var suggester = new SearchSuggester("sg", new[] { "HotelName", "Category", "Address/City", "Address/StateProvince" });
        definition.Suggesters.Add(suggester);

        adminClient.CreateOrUpdateIndex(definition);
    }

    // Upload documents in a single Upload request. //////////////////////////////////////////
    static void UploadDocuments(SearchClient searchClient)
    {
        IndexDocumentsBatch<Hotel> batch = IndexDocumentsBatch.Create(
            IndexDocumentsAction.Upload(
                new Hotel()
                {
                    HotelId = "1",
                    HotelName = "Secret Point Motel",
                    Description = "The hotel is ideally located on the main commercial artery of the city in the heart of New York. A few minutes away is Time's Square and the historic centre of the city, as well as other places of interest that make New York one of America's most attractive and cosmopolitan cities.",
                    DescriptionFr = "L'hôtel est idéalement situé sur la principale artère commerciale de la ville en plein cœur de New York. A quelques minutes se trouve la place du temps et le centre historique de la ville, ainsi que d'autres lieux d'intérêt qui font de New York l'une des villes les plus attractives et cosmopolites de l'Amérique.",
                    Category = "Boutique",
                    Tags = new[] { "pool", "air conditioning", "concierge" },
                    ParkingIncluded = false,
                    LastRenovationDate = new DateTimeOffset(1970, 1, 18, 0, 0, 0, TimeSpan.Zero),
                    Rating = 3.6,
                    Address = new Address()
                    {
                        StreetAddress = "677 5th Ave",
                        City = "New York",
                        StateProvince = "NY",
                        PostalCode = "10022",
                        Country = "USA"
                    }
                }),
            IndexDocumentsAction.Upload(
                new Hotel()
                {
                    HotelId = "2",
                    HotelName = "Twin Dome Motel",
                    Description = "The hotel is situated in a  nineteenth century plaza, which has been expanded and renovated to the highest architectural standards to create a modern, functional and first-class hotel in which art and unique historical elements coexist with the most modern comforts.",
                    DescriptionFr = "L'hôtel est situé dans une place du XIXe siècle, qui a été agrandie et rénovée aux plus hautes normes architecturales pour créer un hôtel moderne, fonctionnel et de première classe dans lequel l'art et les éléments historiques uniques coexistent avec le confort le plus moderne.",
                    Category = "Boutique",
                    Tags = new[] { "pool", "free wifi", "concierge" },
                    ParkingIncluded = false,
                    LastRenovationDate = new DateTimeOffset(1979, 2, 18, 0, 0, 0, TimeSpan.Zero),
                    Rating = 3.60,
                    Address = new Address()
                    {
                        StreetAddress = "140 University Town Center Dr",
                        City = "Sarasota",
                        StateProvince = "FL",
                        PostalCode = "34243",
                        Country = "USA"
                    }
                }),
            IndexDocumentsAction.Upload(
                new Hotel()
                {
                    HotelId = "3",
                    HotelName = "Triple Landscape Hotel",
                    Description = "The Hotel stands out for its gastronomic excellence under the management of William Dough, who advises on and oversees all of the Hotel’s restaurant services.",
                    DescriptionFr = "L'hôtel est situé dans une place du XIXe siècle, qui a été agrandie et rénovée aux plus hautes normes architecturales pour créer un hôtel moderne, fonctionnel et de première classe dans lequel l'art et les éléments historiques uniques coexistent avec le confort le plus moderne.",
                    Category = "Resort and Spa",
                    Tags = new[] { "air conditioning", "bar", "continental breakfast" },
                    ParkingIncluded = true,
                    LastRenovationDate = new DateTimeOffset(2015, 9, 20, 0, 0, 0, TimeSpan.Zero),
                    Rating = 4.80,
                    Address = new Address()
                    {
                        StreetAddress = "3393 Peachtree Rd",
                        City = "Atlanta",
                        StateProvince = "GA",
                        PostalCode = "30326",
                        Country = "USA"
                    }
                }),
            IndexDocumentsAction.Upload(
                new Hotel()
                {
                    HotelId = "4",
                    HotelName = "Sublime Cliff Hotel",
                    Description = "Sublime Cliff Hotel is located in the heart of the historic center of Sublime in an extremely vibrant and lively area within short walking distance to the sites and landmarks of the city and is surrounded by the extraordinary beauty of churches, buildings, shops and monuments. Sublime Cliff is part of a lovingly restored 1800 palace.",
                    DescriptionFr = "Le sublime Cliff Hotel est situé au coeur du centre historique de sublime dans un quartier extrêmement animé et vivant, à courte distance de marche des sites et monuments de la ville et est entouré par l'extraordinaire beauté des églises, des bâtiments, des commerces et Monuments. Sublime Cliff fait partie d'un Palace 1800 restauré avec amour.",
                    Category = "Boutique",
                    Tags = new[] { "concierge", "view", "24-hour front desk service" },
                    ParkingIncluded = true,
                    LastRenovationDate = new DateTimeOffset(1960, 2, 06, 0, 0, 0, TimeSpan.Zero),
                    Rating = 4.60,
                    Address = new Address()
                    {
                        StreetAddress = "7400 San Pedro Ave",
                        City = "San Antonio",
                        StateProvince = "TX",
                        PostalCode = "78216",
                        Country = "USA"
                    }
                })
            );

        try
        {
            IndexDocumentsResult result = searchClient.IndexDocuments(batch);
        }
        catch (Exception)
        {
            // If for some reason any documents are dropped during indexing, you can compensate by delaying and
            // retrying. This simple demo just logs the failed document keys and continues.
            Console.WriteLine("Failed to index some of the documents: {0}");
        }
    }

    // Run queries, use WriteDocuments to print output /////////////////////////////////////////////////
    private static void RunQueries(SearchClient srchclient)
    {
        SearchOptions options;
        SearchResults<Hotel> response;

        // Query 1 ///////////////////////////////////////////////////////////////////
        Console.WriteLine("Query #1: Search on empty term '*' to return all documents, " +
            "showing a subset of fields...\n");

        options = new SearchOptions()
        {
            IncludeTotalCount = true,
            Filter = "",
            OrderBy = { "" }
        };

        options.Select.Add("HotelId");
        options.Select.Add("HotelName");
        options.Select.Add("Rating");

        response = srchclient.Search<Hotel>("*", options);
        WriteDocuments(response);

        // Query 2 ///////////////////////////////////////////////////////////////////
        // söker du efter en term, lägger till ett filter som väljer dokument där
        // Klassificering är större än 4 och sorterar sedan efter Klassificering i
        // fallande ordning. Filter är ett booleskt uttryck som utvärderas över
        // IsFilterable-fält i ett index. Filterfrågor inkluderar eller exkluderar värden. 
        Console.WriteLine("Query #2: Search on 'hotels', filter on 'Rating gt 4', " +
            "sort by Rating in descending order...\n");

        options = new SearchOptions()
        {
            Filter = "Rating gt 4",
            OrderBy = { "Rating desc" }
        };

        options.Select.Add("HotelId");
        options.Select.Add("HotelName");
        options.Select.Add("Rating");

        response = srchclient.Search<Hotel>("hotels", options);
        WriteDocuments(response);

        // Query 3 ////////////////////////////////////////////////////////////////////
        //  visar searchFields, som används för att begränsa en fulltextsökningsåtgärd
        //  till specifika fält.
        Console.WriteLine("Query #3: Limit search to specific fields " +
            "(pool in Tags field)...\n");

        options = new SearchOptions()
        {
            SearchFields = { "Tags" }
        };

        options.Select.Add("HotelId");
        options.Select.Add("HotelName");
        options.Select.Add("Tags");

        response = srchclient.Search<Hotel>("pool", options);
        WriteDocuments(response);

        // Query 4 /////////////////////////////////////////////////////////////////////
        // visar faser, som kan användas för att strukturera en aspektbaserad
        // navigeringsstruktur.
        Console.WriteLine("Query #4: Facet on 'Category'...\n");

        options = new SearchOptions()
        {
            Filter = ""
        };

        options.Facets.Add("Category");

        options.Select.Add("HotelId");
        options.Select.Add("HotelName");
        options.Select.Add("Category");

        response = srchclient.Search<Hotel>("*", options);
        WriteDocuments(response);

        // Query 5 ///////////////////////////////////////////////////////////////////////
        // Returnerar du ett specifikt dokument. En dokumentsökning är ett typiskt svar
        // på OnClick-händelsen i en resultatuppsättning.
        Console.WriteLine("Query #5: Look up a specific document...\n");

        Response<Hotel> lookupResponse;
        lookupResponse = srchclient.GetDocument<Hotel>("3");

        Console.WriteLine(lookupResponse.Value.HotelId);

        // Query 6 ///////////////////////////////////////////////////////////////////////
        // Visar syntaxen för automatisk komplettering, vilket simulerar en partiell
        // användarinmatning av "sa" som matchar två möjliga matchningar i de sourceFields som är associerade med den förslagsanvändare som du definierade i indexet.
        Console.WriteLine("Query #6: Call Autocomplete on HotelName that starts with " +
            "'sa'...\n");

        var autoresponse = srchclient.Autocomplete("sa", "sg");
        WriteDocuments(autoresponse);
    }

    // Write search results to console //////////////////////////////////////////////////////////
    private static void WriteDocuments(SearchResults<Hotel> searchResults)
    {
        foreach (SearchResult<Hotel> result in searchResults.GetResults())
        {
            Console.WriteLine(result.Document);
        }

        Console.WriteLine();
    }

    private static void WriteDocuments(AutocompleteResults autoResults)
    {
        foreach (AutocompleteItem result in autoResults.Results)
        {
            Console.WriteLine(result.Text);
        }

        Console.WriteLine();
    }
}