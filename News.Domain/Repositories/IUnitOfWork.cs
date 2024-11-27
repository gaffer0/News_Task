﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.Domain.Repositories
{
    public interface IUnitOfWork:IDisposable
    {
        INewsRepository News  { get; }
        int Complete();
    }
}
