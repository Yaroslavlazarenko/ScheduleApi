namespace ScheduleApi.Core.Entities;

public class SubjectInfo
{
    public int SubjectId { get; set; }
    public int InfoTypeId { get; set; }
    public string Value { get; set; }
    public string? Description { get; set; }

    public Subject Subject { get; set; }
    public InfoType InfoType { get; set; }
}