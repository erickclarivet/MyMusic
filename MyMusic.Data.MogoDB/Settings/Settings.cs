using System;
using System.Collections.Generic;
using System.Text;

namespace MyMusic.Data.MogoDB.Settings
{
    // We define here the connection string and db name
    public class Settings
    {
        public string ConnectionString { get; set; }  
        public string Database { get; set; }
    }
}
