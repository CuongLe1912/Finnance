using System.Collections.Generic;

namespace URF.Core.EF.Trackable.NestedSets
{
    public interface INestedSet<T>
        where T : INestedSet<T>
    {
        int Id { get; set; }
        int Level { get; set; }
        int? ParentId { get; set; }

        T Parent { get; set; }
        List<T> Childrens { get; set; }
    }
}
