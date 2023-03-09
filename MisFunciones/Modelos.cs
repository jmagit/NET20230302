using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MisFunciones {
    public class Mensaje {
        public string PartitionKey { get; set; } = "Msg";
        public string RowKey { get; set; } = Guid.NewGuid().ToString();
        public string Texto { get; set; }
    }


    public class Person {
        public string PartitionKey { get; set; } = "Test";
        public string RowKey { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
    }

    public class Fichero {
        public string PartitionKey { get; set; } = "Fich";
        public string RowKey { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public long Size { get; set; }
    }

}
