using System.ComponentModel.DataAnnotations;

namespace ScheduleApi.Application.DTOs.Pair;

public class CreatePairDto
{
    public int Number { get; set; }
    public TimeOnly StartTime { get; set; }
    
    [CustomValidation(typeof(CreatePairDto), nameof(ValidateEndTime))]
    public TimeOnly EndTime { get; set; }
    
    public static ValidationResult? ValidateEndTime(TimeOnly endTime, ValidationContext context)
    {
        var dto = (CreatePairDto)context.ObjectInstance;
        
        if (endTime <= dto.StartTime)
        {
            return new ValidationResult("Time cannot be before start time");
        }
        
        return ValidationResult.Success;
    }
}