using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.Semester;
using ScheduleApi.Core.Exceptions;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.Semester.Queries;

public static class GetSemesterById
{
    public record Query(int Id) : IRequest<SemesterDto>;

    private class Handler : IRequestHandler<Query, SemesterDto>
    {
        private readonly ScheduleContext _ctx;
        private readonly IMapper _mapper;

        public Handler(ScheduleContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<SemesterDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var semesterDto = await _ctx.Semesters
                .AsNoTracking()
                .Where(s => s.Id == request.Id)
                .ProjectTo<SemesterDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            if (semesterDto is null)
            {
                throw new NotFoundException($"Semester with ID {request.Id} not found.");
            }

            return semesterDto;
        }
    }
}