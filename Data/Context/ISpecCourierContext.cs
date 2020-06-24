using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SpeccourierApiV2.Models;

namespace SpeccourierApiV2.Data.Context
{
	public interface ISpecCourierContext
	{
        DbSet<Package> Packages { get; set; }

		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
	}
}
