namespace Monolithic.Models.DTO;

public class ReviewConfident
{
    public float Neg { get; set; }
    public float Pos { get; set; }
    public float Neu { get; set; }
}
public class AnalyseReviewResDTO
{
    public string Status { get; set; }
    public ReviewConfident Confident { get; set; }
}
