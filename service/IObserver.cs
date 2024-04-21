using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CZbor.model;

namespace CZbor.service
{
    public interface IObserver
    {
        void updateZbor();
    }
}
