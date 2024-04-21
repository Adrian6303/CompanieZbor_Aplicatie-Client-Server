using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZbor.model
{
    [Serializable]
    public class Entity<ID>
{
        private ID _id;

        public ID Id
        {
            get { return _id; }
            set
            {
                // Add any validation or logic here
                _id = value;
            }
        }

        public override bool Equals(object obj)
    {
        if (this == obj)
            return true;

        if (!(obj is Entity<ID>))
            return false;

        Entity<ID> entity = (Entity<ID>)obj;
        return Equals(Id, entity.Id);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public override string ToString()
    {
        return $"Entity{{id={Id}}}";
    }

}

}
