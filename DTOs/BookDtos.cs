using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LibraryApi.DTOs
{
    public class BookDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("year")]
        public int Year { get; set; }

        [JsonPropertyName("author")]
        public AuthorDto? Author { get; set; }
    }

    public class CreateBookDto
    {
        [Required(AllowEmptyStrings = false)]
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [Range(1, 2100)]
        [JsonPropertyName("year")]
        public int Year { get; set; }

        [JsonPropertyName("authorId")]
        public int AuthorId { get; set; }
    }
}