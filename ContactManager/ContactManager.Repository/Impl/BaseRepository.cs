using ContactManager.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManager.Repository.Impl
{
    public class BaseRepository : IMNPRepository
    {
        public BaseRepository()
        { }

        public virtual string BuildConnectionString()
        {
            throw new NotImplementedException();
        }
    }
}
