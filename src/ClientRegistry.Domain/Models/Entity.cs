using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientRegistry.Domain.Models
{
    public abstract class Entity
    {
        protected Entity()
        {
            Id = Guid.NewGuid();
            RegisterDateTime = DateTime.Now;
        }

        public Guid Id { get; set; }

        public DateTime RegisterDateTime { get; set; }
    }
}
