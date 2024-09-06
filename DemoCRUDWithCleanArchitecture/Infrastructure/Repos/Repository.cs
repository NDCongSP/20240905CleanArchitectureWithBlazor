using Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repos
{
    public class Repository
    {
        public IProduct SProduct { get; set; }

        public Repository(IProduct sProduct = null)
        {
            SProduct = sProduct;
        }
    }
}
