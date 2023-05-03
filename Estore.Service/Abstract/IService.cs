using Estore.Core.Entities;
using Estore.Data.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estore.Service.Abstract
{
    public interface IService<T> : IRepository<T> where T : class, IEntity, new()  
    {

    }
}
