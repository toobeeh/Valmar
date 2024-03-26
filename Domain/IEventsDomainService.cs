using Valmar.Database;
using Valmar.Domain.Classes;

namespace Valmar.Domain;

public interface IEventsDomainService
{
    Task<EventDdo> GetEventById(int id);
    Task<List<EventDdo>> GetAllEvents();
    Task<EventDropEntity> GetEventDropById(int id);
    Task<List<EventDropEntity>> GetAllEventDrops();
    Task<List<EventDropEntity>> GetEventDropsOfEvent(int id);
    Task<EventDdo> GetCurrentEvent();
}