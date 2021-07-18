using Api.Data.Context;
using Api.Data.Repositories;
using Api.Domain.Entities;
using Api.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Api.Data.Implementations
{
    [ExcludeFromCodeCoverage]
    public class UserImplementations : BaseRepository<UserEntity>, IUserRepository
    {
        private readonly DbSet<UserEntity> _dataSet;

        public UserImplementations(ILogger logger, DataContext context) : base(logger, context)
        {
            _dataSet = context.Set<UserEntity>();
        }

        public async Task<UserEntity> GetByEmail(string email)
        {
            return await _dataSet.AsNoTracking().FirstOrDefaultAsync(u => u.Email.Equals(email));
        }
    }
}
