using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CZbor.model;

namespace CZbor.repository
{
    public interface IRepository<ID, E> where E : Entity<ID>
    {
        E findOne(ID id);
        IEnumerable<E> findAll();

        void save(E entity);

        void delete(ID id);

        void update(E entity);

    }
}
