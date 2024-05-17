using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CZbor.model;
using CZbor.repository;
using log4net;

namespace CZbor.repository
{
    public class TuristRepoH
    {
        private readonly ISessionFactory sessionFactory;
        public TuristRepoH()
        {
            this.sessionFactory = SessionFactory.BuildSessionFactory();
        }

        public Turist findOneByName(string name)
        {
            using (var session = sessionFactory.OpenSession())
            {
                return session.QueryOver<Turist>()
                              .Where(t => t.TouristName == name)
                              .SingleOrDefault();
            }
        }


        public void save(Turist entity)
        {
            using (var session = sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.Save(entity);
                    transaction.Commit();
                }
            }
        }

        public Turist findOne(int id)
        {
            using (var session = sessionFactory.OpenSession())
            {
                return session.Get<Turist>(id);
            }
        }


    }
}