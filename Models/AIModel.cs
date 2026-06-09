namespace StudySummarizer.Models;

public class AIModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ModelId { get; set; } = string.Empty;
    public bool IsActive { get; set; } = false;
    public int MaxTokens { get; set; } = 2000;
    public decimal Temperature { get; set; } = 0.7m;
    public string Tone { get; set; } = Constants.SummaryTone.Neutral;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
