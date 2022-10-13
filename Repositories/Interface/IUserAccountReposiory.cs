using Monolithic.Models.Entities;

namespace Monolithic.Repositories.Interface;

public interface IUserAccountReposiory
{
    Task<UserAccountEntity> GetByEmail(string email);

    Task<UserAccountEntity> GetById(int id);

    Task<UserAccountEntity> Create(UserAccountEntity userAccountEntity);
}