using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using MatchFM.Models;

namespace MatchFM.Repositories
{
    /// <summary>
    /// Parent of all repository used
    /// </summary>
    public class BaseRepository
    {
        protected readonly ApplicationDbContext _context;

        public BaseRepository(ApplicationDbContext context)
        {
            this._context = context;
        }
    }
}