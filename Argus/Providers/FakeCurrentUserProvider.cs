using Argus.Enums;
using Argus.Providers.Interfaces;

namespace Argus.Providers
{
    public class FakeCurrentUserProvider: ICurrentUserProvider
    {
        // Вставь сюда любой реальный Guid пользователя из твоей базы данных, под которым ты хочешь тестировать систему.
        public Guid? UserId => Guid.Parse("019f3743-d881-7218-b651-50ba3133b82d");

        // Временно наделяем нашего фейкового юзера правами Админа
        public UserRole? Role => UserRole.Admin;
    }
}
