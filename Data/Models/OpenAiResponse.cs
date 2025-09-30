using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class OpenAiResponse
{
    [Required]
    public List<OutputItem> output { get; set; }
}

public class OutputItem
{
    [Required]
    public List<ContentItem> content { get; set; }
}

public class ContentItem
{
    [Required]
    public string type { get; set; }
    [Required]
    public string text { get; set; }
}
