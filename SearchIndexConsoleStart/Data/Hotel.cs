using Azure.Search.Documents.Indexes.Models;
using Azure.Search.Documents.Indexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

// I klientbiblioteket Azure.Search.Documents kan du använda SearchableField och SimpleField
// för att effektivisera fältdefinitionerna.
// Båda är derivat av ett SearchField och kan potentiellt förenkla din kod:

// SimpleField
// ... kan vara valfri datatyp, är alltid icke-sökbar (den ignoreras för fulltextsökningsfrågor)
// och kan hämtas (den är inte dold).
// Andra attribut är inaktiverade som standard, men kan aktiveras.
// Du kan använda en SimpleField för dokument-ID eller fält som endast används i filter,
// fasetter eller bedömningsprofiler. I så fall måste du tillämpa eventuella attribut som är
// nödvändiga för scenariot, till exempel IsKey = true för ett dokument-ID.

// SearchableField
// ... måste vara en sträng och är alltid sökbar och kan hämtas.
// Andra attribut är inaktiverade som standard, men kan aktiveras.
// Eftersom den här fälttypen är sökbar stöder den synonymer och det fullständiga komplementet av
// analysverktygsegenskaper.

namespace SearchIndexConsoleEnd.Data
{
    public class Hotel
    {
        [SimpleField(IsKey = true, IsFilterable = true)]
        public string HotelId { get; set; }

        [SearchableField(IsSortable = true)]
        public string HotelName { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
        public string Description { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.FrLucene)]
        [JsonPropertyName("Description_fr")]
        public string DescriptionFr { get; set; }

        [SearchableField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
        public string Category { get; set; }

        [SearchableField(IsFilterable = true, IsFacetable = true)]
        public string[] Tags { get; set; }

        [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
        public bool? ParkingIncluded { get; set; }

        [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
        public DateTimeOffset? LastRenovationDate { get; set; }

        [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
        public double? Rating { get; set; }

        [SearchableField]
        public Address Address { get; set; }

        // ///////////////////////////////////////////////////////////////////////////////////////////////
        // This implementation of ToString() is only for the purposes of the sample console application.
        // You can override ToString() in your own model class if you want, but you don't need to in order
        // to use the Azure Search .NET SDK.
        public override string ToString()
        {
            var builder = new StringBuilder();

            if (!String.IsNullOrEmpty(HotelId))
            {
                builder.AppendFormat("HotelId: {0}\n", HotelId);
            }

            if (!String.IsNullOrEmpty(HotelName))
            {
                builder.AppendFormat("Name: {0}\n", HotelName);
            }

            if (!String.IsNullOrEmpty(Description))
            {
                builder.AppendFormat("Description: {0}\n", Description);
            }

            if (!String.IsNullOrEmpty(DescriptionFr))
            {
                builder.AppendFormat("Description (French): {0}\n", DescriptionFr);
            }

            if (!String.IsNullOrEmpty(Category))
            {
                builder.AppendFormat("Category: {0}\n", Category);
            }

            if (Tags != null && Tags.Length > 0)
            {
                builder.AppendFormat("Tags: [ {0} ]\n", String.Join(", ", Tags));
            }

            if (ParkingIncluded.HasValue)
            {
                builder.AppendFormat("Parking included: {0}\n", ParkingIncluded.Value ? "yes" : "no");
            }

            if (LastRenovationDate.HasValue)
            {
                builder.AppendFormat("Last renovated on: {0}\n", LastRenovationDate);
            }

            if (Rating.HasValue)
            {
                builder.AppendFormat("Rating: {0}\n", Rating);
            }

            if (Address != null && !Address.IsEmpty)
            {
                builder.AppendFormat("Address: \n{0}\n", Address.ToString());
            }

            return builder.ToString();
        }
    }
}
