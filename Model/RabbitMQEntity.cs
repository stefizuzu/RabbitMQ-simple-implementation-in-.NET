using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMq.Model
{
    public class RabbitMQEntity
    {
        public RabbitMQEntity()
        {
        }

        public RabbitMQEntity(
            string hostname,
            int port,
            string username,
            string password,
            bool automaticRecoveryEnabled
            )
        {
            this.hostname = hostname;
            this.port = port;
            this.username = username;
            this.password = password;
            this.automaticRecoveryEnabled = automaticRecoveryEnabled;
        }

        public string hostname { get; set; }
        public int port { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public bool automaticRecoveryEnabled { get; set; }
    }
}
