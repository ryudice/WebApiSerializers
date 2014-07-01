using System;

namespace WebApiSerializers
{
    public interface IRelationShipBuilder<T> where T : class
    {
        IRelationShipBuilder<T> Map(Func<T, object> map);
    }

    class RelationShipBuilder<T> : IRelationShipBuilder<T> where T : class
    {
        private readonly HasManyAttribute<T> _attribute;

        public RelationShipBuilder(HasManyAttribute<T> attribute )
        {
            _attribute = attribute;
        }

        public IRelationShipBuilder<T> Map(Func<T, object> map)
        {
            _attribute.Map = map;
            return this;
        }
    }
}