using Valmar.Database;

namespace Valmar.Domain;

public interface IEventsDomainService
{
    Task<EventEntity> GetEventById(int id);
    Task<List<EventEntity>> GetAllEvents();
    Task<EventDropEntity> GetEventDropById(int id);
    Task<List<EventDropEntity>> GetAllEventDrops();
    Task<List<EventDropEntity>> GetEventDropsOfEvent(int id);
    Task<EventEntity> GetCurrentEvent();
}