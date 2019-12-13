namespace BookShop.DataProcessor.ImportDto
{
    using Newtonsoft.Json;

    public class AuthorDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        [JsonProperty("Books")]
        public BookIdDto[] BookIds { get; set; }
    }

    public class BookIdDto
    {
        public int? Id { get; set; }
    }
}
