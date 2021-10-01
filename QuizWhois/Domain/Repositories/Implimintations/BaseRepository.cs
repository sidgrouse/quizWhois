using Domain.Entity.Base;
using Domain.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories.Implimintations
{
    public class BaseRepository<TDbModel> : IBaseRepository<TDbModel> where TDbModel : Base
    {

    }
}
