﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace FlatFiles.Scrutinizer.TypeMapping
{
    internal class MemberLookup
    {
        private readonly Dictionary<string, IMemberMapping> lookup;
        private readonly Dictionary<Type, object> factories;
        private int ignoredCount;

        public MemberLookup()
        {
            this.lookup = new Dictionary<string, IMemberMapping>();
            this.factories = new Dictionary<Type, object>();
        }

        public int WorkCount
        {
            get { return lookup.Count - ignoredCount; }
        }

        public TMemberMapping GetOrAddMember<TMemberMapping>(IMemberAccessor member, Func<int, int, TMemberMapping> factory)
            where TMemberMapping : IMemberMapping
        {
            if (lookup.TryGetValue(member.Name, out var mapping))
            {
                return (TMemberMapping)mapping;
            }
            else
            {
                int fileIndex = lookup.Count;
                int workIndex = lookup.Count - ignoredCount;
                var newMapping = factory(fileIndex, workIndex);
                lookup.Add(member.Name, newMapping);
                return newMapping;
            }
        }

        public IgnoredMapping AddIgnored()
        {
            var column = new IgnoredColumn();
            var mapping = new IgnoredMapping(column, lookup.Count);
            string key = $"@Ignored_{mapping.FileIndex}";
            lookup.Add(key, mapping);
            ++ignoredCount;
            return mapping;
        }

        public IMemberMapping[] GetMappings()
        {
            return lookup.Values.OrderBy(m => m.FileIndex).ToArray();
        }

        public Func<TEntity> GetFactory<TEntity>()
        {
            if (factories.TryGetValue(typeof(TEntity), out var factory))
            {
                if (factory is Func<TEntity> entityFactory)
                {
                    return entityFactory;
                }
                else if (factory is Func<object> objectFactory)
                {
                    return () => (TEntity)objectFactory();
                }
            }
            return null;
        }

        public void SetFactory<TEntity>(Func<TEntity> factory)
        {
            factories.Add(typeof(TEntity), factory);
        }

        public void SetFactory(Type entityType, Func<object> factory)
        {
            factories.Add(entityType, factory);
        }
    }
}
