using AutoMapper;
using MediatR;
using ScheduleApi.Application.DTOs.Semester;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.Semester.Commands;

public static class CreateSemester
{
    public record Command(CreateSemesterDto Model) : IRequest<int>;

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
            var semesterEntity = _mapper.Map<Core.Entities.Semester>(request.Model);
            
            semesterEntity.StartDate = semesterEntity.StartDate.Date;
            semesterEntity.EndDate = semesterEntity.EndDate.Date;

            await _ctx.Semesters.AddAsync(semesterEntity, cancellationToken);
            await _ctx.SaveChangesAsync(cancellationToken);

            return semesterEntity.Id;
        }
    }
}