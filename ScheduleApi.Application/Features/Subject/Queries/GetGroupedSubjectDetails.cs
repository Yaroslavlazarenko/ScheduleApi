using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.Subject;
using ScheduleApi.Core.Exceptions;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.Subject.Queries;

public static class GetGroupedSubjectDetails
{
    public record Query(int SubjectNameId, int? GroupId) : IRequest<GroupedSubjectDetailsDto>;

    private class Handler : IRequestHandler<Query, GroupedSubjectDetailsDto>
    {
        private readonly ScheduleContext _ctx;
        private readonly IMapper _mapper;

        public Handler(ScheduleContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<GroupedSubjectDetailsDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var resultDto = await _ctx.SubjectNames
                .AsNoTracking()
                .Where(sn => sn.Id == request.SubjectNameId)
                // ProjectTo соберет все ваши профили и построит один SQL-запрос
                .ProjectTo<GroupedSubjectDetailsDto>(_mapper.ConfigurationProvider,
                    // Передаем параметр groupId в наш настроенный маппинг
                    new { groupId = request.GroupId })
                .FirstOrDefaultAsync(cancellationToken);

            if (resultDto == null)
            {
                throw new NotFoundException($"SubjectName with ID '{request.SubjectNameId}' not found");
            }

            if (!resultDto.Variants.Any())
            {
                throw new NotFoundException($"No Subject variants found for SubjectName with ID '{request.SubjectNameId}'");
            }
            
            if (request.GroupId.HasValue && !resultDto.Variants.Any(v => v.Teachers.Any()))
            {
                throw new NotFoundException($"Subject '{resultDto.Abbreviation}' has no assigned teachers for the specified group.");
            }

            return resultDto;
        }
    }
}