using Monolithic.Models.ReqParams;
using Monolithic.Models.Entities;
using Monolithic.Models.Common;

namespace Monolithic.Repositories.Interface;

public interface INotificationRepository
{
    Task<PagedList<NotificationEntity>> GetNotifications(int userId, NotificationParams notificationParams);

    Task<NotificationEntity> GetById(int id);

    Task<Tuple<int, int>> CountUnreadNotification(int userId);

    Task<bool> CreateNotification(NotificationEntity notificationEntity);

    Task<bool> UpdateNotification(NotificationEntity notificationEntity);

    Task<bool> SetAllNotyHasRead(int userId);
}