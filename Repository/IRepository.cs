using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TodoApi.Repository
{
    public interface IRepository<T1,T2> where T1:class
    {
        Task<ActionResult<IEnumerable<T1>>> GetAll();
        Task<ActionResult<T1>> GetById(T2 id);
        Task<ActionResult<T1>> Create(T1 entity);
        Task<ActionResult<T1>> Delete (T2 id);
        Task<ActionResult<T1>> Update(T1 entity, T2 id);
        Task Save();
        bool IsExist(long id);
    }
}
