using AutoMapper;
using AutoMapper.QueryableExtensions;
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
            // ОПТИМИЗИРОВАННЫЙ ВАРИАНТ:
            // .Include() и последующий .Map() заменены на один вызов .ProjectTo()
            var teacherDto = await _ctx.Teachers
                .AsNoTracking()
                .Where(t => t.Id == request.Id)
                .ProjectTo<TeacherDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            if (teacherDto is null)
            {
                throw new NotFoundException($"Teacher with ID {request.Id} not found.");
            }

            return teacherDto;
        }
    }
}