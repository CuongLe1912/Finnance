using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace URF.Core.EF.Trackable.NestedSets
{
    public class NestedSetManager<TDbContext, T> where T : class, INestedSet<T>
        where TDbContext : DbContext
    {
        private readonly DbContext _db;
        private readonly DbSet<T> _nodesSet;

        public NestedSetManager(TDbContext dbContext, Expression<Func<TDbContext, DbSet<T>>> nodesSourceExpression)
        {
            _db = dbContext;
            var propertyInfo = new PropertySelectorVisitor(nodesSourceExpression).Property;
            _nodesSet = (DbSet<T>)propertyInfo.GetValue(dbContext);
        }

        public T InsertRoot(T node)
        {
            return Insert(default, new List<T> { node }).First();
        }

        public int GetMaxLevel(int nodeId)
        {
            var level = 0;
            var deptNumber = 1000;
            var parentIds = new List<int>();
            var itemIds = new List<int>() { nodeId };
            var query = _nodesSet.Where(c => c.ParentId.HasValue);
            for (int i = 0; i < deptNumber; i++)
            {
                var ids = query.Where(c => itemIds.Contains(c.ParentId.Value))
                    .Where(c => c.Id != c.ParentId.Value)
                    .Select(c => c.Id)
                    .ToList();
                if (ids != null && ids.Count > 0)
                {
                    level += 1;
                    itemIds = ids;
                    parentIds.AddRange(ids);
                }
                else break;
            }
            return level;
        }

        public T Insert(int parentId, T node)
        {
            return Insert(parentId, new List<T> { node }).First();
        }

        public void Move(int nodeId, int? toParentId)
        {
            T node = GetNode(nodeId);
            T parent = toParentId.HasValue
                ? GetNode(toParentId.Value)
                : null;
            if (node != null)
            {
                var prevLevel = node.Level;
                node.ParentId = parent?.Id;
                node.Level = parent != null
                    ? parent.Level + 1
                    : 1;
                _nodesSet.Update(node);
                var childs = GetDescendants(node.Id).ToList();
                if (childs != null && childs.Count > 0)
                {
                    foreach (var item in childs)
                    {
                        item.Level = (item.Level - prevLevel) + node.Level;
                        _nodesSet.Update(item);
                    }
                }
                _db.SaveChanges();
            }
        }

        public List<T> Insert(int parentId, List<T> nodes)
        {
            T parent = null;
            if (parentId > 0)
                parent = GetNode(parentId);
            foreach (var node in nodes)
            {
                node.Level = 1;
                if (parent != null)
                {
                    node.ParentId = parent.Id;
                    node.Level = parent.Level + 1;
                }
                _nodesSet.Add(node);
            }
            _db.SaveChanges();
            return nodes;
        }

        public IQueryable<T> GetDescendants(int nodeId, int? depth = null)
        {
            var deptNumber = depth.HasValue && depth.Value > 0
                ? depth.Value
                : 1000;
            var parentIds = new List<int>();
            var itemIds = new List<int>() { nodeId };
            var query = _nodesSet.Where(c => c.ParentId.HasValue);
            for (int i = 0; i < deptNumber; i++)
            {
                var ids = query.Where(c => itemIds.Contains(c.ParentId.Value))
                    .Where(c => c.Id != c.ParentId.Value)
                    .Select(c => c.Id)
                    .ToList();
                if (ids != null && ids.Count > 0)
                {
                    itemIds = ids;
                    parentIds.AddRange(ids);
                }
                else break;
            }
            var queryItem = _nodesSet
                .Where(c => c.ParentId.HasValue)
                .Where(c => parentIds.Contains(c.Id));
            return queryItem;
        }

        public List<int> GetPathToNode(int nodeId, int? parentId = default)
        {
            if (!parentId.HasValue)
                parentId = GetParentNodeId(nodeId);
            var nodeIds = new List<int>() { nodeId };
            while (parentId.HasValue)
            {
                nodeIds.Add(parentId.Value);
                parentId = GetParentNodeId(parentId.Value);
            }
            return nodeIds.Distinct().ToList();
        }

        public IQueryable<T> GetDescendantsByLevel(int nodeId, int childLevel)
        {
            var level = 0;
            var deptNumber = 1000;
            var parentIds = new List<int>();
            var itemIds = new List<int>() { nodeId };
            var query = _nodesSet.Where(c => c.ParentId.HasValue);
            for (int i = 0; i < deptNumber; i++)
            {
                var ids = query.Where(c => itemIds.Contains(c.ParentId.Value))
                    .Where(c => c.Id != c.ParentId.Value)
                    .Select(c => c.Id)
                    .ToList();
                if (ids != null && ids.Count > 0)
                {
                    level += 1;
                    itemIds = ids;
                    if (level == childLevel)
                    {
                        parentIds = ids;
                        break;
                    }
                }
                else break;
            }
            var queryItem = _nodesSet
                .Where(c => c.ParentId.HasValue)
                .Where(c => parentIds.Contains(c.Id));
            return queryItem;
        }

        public IQueryable<T> GetDescendants(List<int> nodeIds, int? depth = null)
        {
            var deptNumber = depth.HasValue && depth.Value > 0
                ? depth.Value
                : 1000;
            var itemIds = nodeIds;
            var parentIds = new List<int>();
            var query = _nodesSet.Where(c => c.ParentId.HasValue);
            for (int i = 0; i < deptNumber; i++)
            {
                var ids = query.Where(c => itemIds.Contains(c.ParentId.Value))
                    .Where(c => c.Id != c.ParentId.Value)
                    .Select(c => c.Id)
                    .ToList();
                if (ids != null && ids.Count > 0)
                {
                    itemIds = ids;
                    parentIds.AddRange(ids);
                }
                else break;
            }
            var queryItem = _nodesSet
                .Where(c => c.ParentId.HasValue)
                .Where(c => parentIds.Contains(c.Id));
            return queryItem;
        }

        private T GetNode(int id)
        {
            return _nodesSet.Single(c => c.Id.Equals(id));
        }
        private int? GetParentNodeId(int id)
        {
            return _nodesSet.Where(c => c.Id.Equals(id)).Select(c => c.ParentId).FirstOrDefault();
        }
    }
}
