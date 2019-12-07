using Newtonsoft.Json;

namespace TeisterMask.DataProcessor.ImportDto
{
    public class EmployeeDto
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        [JsonProperty("Tasks")]
        public int[] EmployeesTasks { get; set; }
    }
}