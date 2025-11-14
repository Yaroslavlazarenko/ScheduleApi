namespace ScheduleApi.Core.Entities;

/// <summary>
/// Содержит всю необходимую временную информацию для выполнения запроса по расписанию.
/// Вся логика опирается на время в "авторитетном" часовом поясе (Украина).
/// </summary>
public record ScheduleTimeContext
{
    /// <summary>
    /// Целевое время в часовом поясе Украины. Используется для всей бизнес-логики (день недели, четность и т.д.).
    /// </summary>
    public DateTime AuthoritativeTime { get; init; }

    /// <summary>
    /// Начало целевого дня в часовом поясе Украины, сконвертированное в UTC. Используется для запросов к БД.
    /// </summary>
    public DateTime StartOfDayUtc { get; init; }

    /// <summary>
    /// Конец целевого дня (начало следующего) в часовом поясе Украины, сконвертированный в UTC. Используется для запросов к БД.
    /// </summary>
    public DateTime EndOfDayUtc { get; init; }
}