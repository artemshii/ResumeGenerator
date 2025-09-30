using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Razor.TagHelpers;

public class ResponseBase
{
    [Required]
    public int StatusCode { get; set; }
    public string Message { get; set; }
    
    

    public ResponseBase(int statusCode, string message)
    {
        StatusCode = statusCode;
        Message = message;
    }
}
