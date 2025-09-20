using AutoMapper;
using MediatR;
using ScheduleApi.Application.DTOs.Teacher;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.Teacher.Commands;

public static class CreateTeacher
{
    public record Command(CreateTeacherDto Model) : IRequest<int>;

    private class Handler : IRequestHandler<Command, int>
    {
        private readonly ScheduleContext _ctx;
        private readonly IMapper _mapper;

        public Handler(ScheduleContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<int> Handle(Command request, CancellationToken cancellationToken)
        {
            var teacherEntity = _mapper.Map<Core.Entities.Teacher>(request.Model);

            await _ctx.Teachers.AddAsync(teacherEntity, cancellationToken);
            await _ctx.SaveChangesAsync(cancellationToken);

            return teacherEntity.Id;
        }
    }
}