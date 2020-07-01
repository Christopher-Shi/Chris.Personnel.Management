using System;
using Microsoft.EntityFrameworkCore;

namespace Chris.Personnel.Management.Repository
{
    public interface IDbContextProvider : IDisposable
    {
        DbContext GetDbContext();
    }
}