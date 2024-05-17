using NHibernate;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Cfg;

namespace CZbor.repository
{
    public class SessionFactory
    {
        public static ISessionFactory BuildSessionFactory()
        {
            var configuration = new NHibernate.Cfg.Configuration();
            configuration.Configure("C:\\Users\\adria\\source\\repos\\CZbor\\repository\\App.config"); // Load NHibernate configuration from a file
            var mappingFilePath = "C:\\Users\\adria\\source\\repos\\CZbor\\repository\\Turist.hbm.xml";

            // Add the mapping file to NHibernate configuration
            configuration.AddFile(mappingFilePath);
            var sessionFactory = configuration.BuildSessionFactory();
            return sessionFactory;
        }
    }
}