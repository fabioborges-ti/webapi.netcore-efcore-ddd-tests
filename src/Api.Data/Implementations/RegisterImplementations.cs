using Api.Data.Context;
using Api.Domain.Entities;
using Api.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Data.Implementations
{
    [ExcludeFromCodeCoverage]
    public class RegisterImplementations : IRegisterRepository
    {
        private readonly DataContext _context;

        public RegisterImplementations(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserEntity>> GetRegisters()
        {
            return await _context.Users
                            .AsNoTracking()
                            .Include(a => a.Addresses)
                            .Include(p => p.Phones)
                            .AsSplitQuery()
                            .ToListAsync(); 
        }

        public async Task<UserEntity> GetRegisterByUserId(Guid id)
        {
            return await _context.Users
                            .AsNoTracking()
                            .Include(a => a.Addresses)
                            .Include(p => p.Phones)
                            .AsSplitQuery()
                            .Where(u => u.Id.Equals(id))
                            .FirstOrDefaultAsync();
        }
    }
}
