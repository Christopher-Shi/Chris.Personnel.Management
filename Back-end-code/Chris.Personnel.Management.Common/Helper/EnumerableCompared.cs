using System.Collections.Generic;
using System.Linq;
using Chris.Personnel.Management.Common.EntityModel;

namespace Chris.Personnel.Management.Common.Helper
{
    public class EnumerableCompared<T> where T : RootEntity
    {
        public List<T> DeletedItems { get; }
        public List<T> AddedItems { get; }

        public EnumerableCompared(IEnumerable<T> oldList, IEnumerable<T> newList)
        {
            var entityBases = oldList.ToList();
            DeletedItems = entityBases.Where(p => newList.All(p2 => p2.Id != p.Id)).ToList();
            AddedItems = newList.Where(p => entityBases.All(p2 => p2.Id != p.Id)).ToList();
        }
    }
}
