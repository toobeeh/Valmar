using Valmar.Domain.Classes;

namespace Valmar.Domain;

public interface IEventsDomainService
{
    Task<EventDdo> GetEventById(int id);
    Task<List<EventDdo>> GetAllEvents();
    Task<EventDropDdo> GetEventDropById(int id);
    Task<EventDdo> GetCurrentEvent();
    Task<List<EventDropDdo>> GetEventDrops(int? eventId = null);
}