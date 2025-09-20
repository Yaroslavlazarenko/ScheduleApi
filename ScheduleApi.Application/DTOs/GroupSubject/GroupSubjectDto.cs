namespace ScheduleApi.Application.DTOs.GroupSubject;

public class GroupSubjectDto
{
    public int SubjectId { get; set; }
    public string SubjectName { get; set; }

    public int TeacherId { get; set; }
    public string TeacherFullName { get; set; }
}