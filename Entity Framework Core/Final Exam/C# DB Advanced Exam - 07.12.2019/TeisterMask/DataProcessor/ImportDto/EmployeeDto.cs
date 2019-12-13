namespace TeisterMask.DataProcessor.ImportDto
{
    using Newtonsoft.Json;

    public class EmployeeDto
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        [JsonProperty("Tasks")]
        public int[] TaskIds { get; set; }
    }
}