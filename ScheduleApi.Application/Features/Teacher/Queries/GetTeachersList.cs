using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.Teacher;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.Teacher.Queries;

public static class GetTeachersList
{
    public record Query() : IRequest<List<TeacherDto>>;

    private class Handler : IRequestHandler<Query, List<TeacherDto>>
    {
        private readonly ScheduleContext _ctx;
        private readonly IMapper _mapper;

        public Handler(ScheduleContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<List<TeacherDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var list = await _ctx.Teachers
                .AsNoTracking()
                .OrderBy(t => t.LastName).ThenBy(t => t.FirstName)
                .ProjectTo<TeacherDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return list;
        }
    }
}