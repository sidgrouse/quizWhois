﻿using Domain.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories.Interfaces
{
    public interface IBaseRepository<TDbModel> where TDbModel : Base
    {

    }
}
