using Monolithic.Models.ReqParams;
using Monolithic.Models.Entities;
using Monolithic.Models.Common;

namespace Monolithic.Repositories.Interface;

public interface IUserAccountReposiory
{
    Task<PagedList<UserAccountEntity>> GetAllUsers(UserParams userParams);

    Task<UserAccountEntity> GetByEmail(string email);

    Task<UserAccountEntity> GetById(int id);

    Task<UserAccountEntity> Create(UserAccountEntity userAccountEntity);

    Task<bool> Update(int id, UserAccountEntity userAccountEntity);
}