using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.Teacher;
using ScheduleApi.Core.Exceptions;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.Teacher.Queries;

public static class GetTeacherById
{
    public record Query(int Id) : IRequest<TeacherDto>;

    private class Handler : IRequestHandler<Query, TeacherDto>
    {
        private readonly ScheduleContext _ctx;
        private readonly IMapper _mapper;

        public Handler(ScheduleContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<TeacherDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var teacher = await _ctx.Teachers
                .AsNoTracking()
                .Include(t => t.TeacherInfos)
                .ThenInclude(ti => ti.InfoType)
                .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

            if (teacher is null)
            {
                throw new NotFoundException("Teacher not found");
            }

            return _mapper.Map<TeacherDto>(teacher);
        }
    }
}