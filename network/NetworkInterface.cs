using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace network
{
    interface NetworkInterface
    {

        /* Establish a hosted connection, resulting in an
           available stream */
        void create_connection();

        /* Join a hosted connection resulting in an 
           available stream */
        void join_connection();

        /* send an item along the stream (adds it to send queue) */
        void send_along_connection(Object obj);

        /* retrieve an item from the recvd queue)
           return? or call apply methods etc.. */
        void recv_from_connection();
    }
}
