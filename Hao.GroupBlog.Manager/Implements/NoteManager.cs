using AutoMapper;
using Hao.GroupBlog.Domain.Interfaces;
using Hao.GroupBlog.Manager.Basic;
using Hao.GroupBlog.Persistence.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hao.GroupBlog.Manager.Implements
{
    public class NoteManager: BaseManager, INoteManager
    {
        private readonly ILogger _logger;

        public NoteManager(GbDbContext dbContext, 
            IMapper mapper,
            ILogger<NoteManager> logger,
            IConfiguration configuration, 
            IHttpContextAccessor httpContextAccessor) 
            : base(dbContext, mapper, configuration, httpContextAccessor)
        {
            _logger = logger;
        }

        public async Task<bool> IsOpen(string id)
        {
            throw new NotImplementedException();
        }
    }
}
