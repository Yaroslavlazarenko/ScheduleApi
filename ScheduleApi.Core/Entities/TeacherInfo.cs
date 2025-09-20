namespace ScheduleApi.Core.Entities;

public class TeacherInfo
{
    public int TeacherId { get; set; }
    public int InfoTypeId { get; set; }
    public string Value { get; set; }

    public Teacher Teacher { get; set; }
    public InfoType InfoType { get; set; }
}