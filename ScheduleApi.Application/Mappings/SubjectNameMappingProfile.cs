using AutoMapper;
using ScheduleApi.Application.DTOs.Subject;
using ScheduleApi.Core.Entities;

namespace ScheduleApi.Application.Mappings;

public class SubjectNameMappingProfile : Profile
{
    public SubjectNameMappingProfile()
    {
        // Объявляем параметр, который сможем передавать в ProjectTo.
        // Он будет использоваться для условной фильтрации.
        int? groupId = null;

        // Главный маппинг для нашего сложного запроса.
        // Он связывает корневую сущность SubjectName с итоговой DTO.
        CreateMap<SubjectName, GroupedSubjectDetailsDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName))
            // AutoMapper автоматически сопоставит ShortName и Abbreviation по имени.

            // Здесь мы указываем, как маппить вложенную коллекцию Variants.
            // AutoMapper увидит, что src.Subjects - это коллекция Subject,
            // и автоматически применит маппинг Subject -> SubjectVariantDto,
            // который у вас уже определен в SubjectMappingProfile.
            .ForMember(dest => dest.Variants, opt => opt.MapFrom(src => src.Subjects));

        // ВАЖНАЯ ЧАСТЬ: Настраиваем маппинг для SubjectVariantDto,
        // чтобы он правильно фильтровал учителей.
        // Этот маппинг будет использоваться внутри маппинга выше.
        CreateMap<Subject, SubjectVariantDto>()
             // Все остальные поля (Id, SubjectType, Infos) AutoMapper сопоставит,
             // используя ваши существующие маппинги (Subject -> SubjectVariantDto,
             // SubjectType -> SubjectTypeDto и т.д.).
             // Нам нужно переопределить только логику для Teachers.
            .ForMember(dest => dest.Teachers, opt => opt.MapFrom(src =>
                src.TeacherSubjects
                    // УСЛОВИЕ: Если groupId не был передан (null), берем всех учителей.
                    // Если groupId был передан, фильтруем по нему.
                    .Where(ts => groupId == null || ts.GroupSubjects.Any(gs => gs.GroupId == groupId))
                    .Select(ts => ts.Teacher) // Выбираем сущность Teacher.
            ));
            // AutoMapper увидит, что результатом является коллекция Teacher,
            // и автоматически применит ваш существующий маппинг Teacher -> TeacherDto.
            
        CreateMap<SubjectName, SubjectNameDto>()
            .ForMember(dest => dest.SubjectNameId, opt => opt.MapFrom(src => src.Id));
    }
}