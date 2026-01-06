using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LibraryApi.DTOs
{
    public class AuthorDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("first_name")]
        public string FirstName { get; set; } = string.Empty;

        [JsonPropertyName("last_name")]
        public string LastName { get; set; } = string.Empty;
    }

    public class CreateAuthorDto
    {
        [Required(AllowEmptyStrings = false)]
        [JsonPropertyName("first_name")]
        public string FirstName { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false)]
        [JsonPropertyName("last_name")]
        public string LastName { get; set; } = string.Empty;
    }
}