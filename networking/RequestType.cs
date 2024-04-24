using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZbor.networking
{
    public enum RequestType
    {
        LOGIN,
        LOGOUT,
        SET_OBSERVER,
        GET_DESTINATIONS, 
        GET_ZBORURI, 
        FILTER_ZBORURI, 
        BUY_BILET, 
        FIND_ADD_TURIST, 
        UPDATE_ZBOR
    }
}
