using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CodeCampRESTfulApi.Data {
    public class CodeCampContextFactory : IDesignTimeDbContextFactory<CodeCampContext> {
        public CodeCampContext CreateDbContext(string[] args) {
            var config = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json")
              .Build();

            return new CodeCampContext(new DbContextOptionsBuilder<CodeCampContext>().Options, config);
        }
    }
}
