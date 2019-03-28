using System.Collections.Generic;

namespace JsonSerializer.TestObjects
{
    class Container
    {
        public int ContainerId { get; set; }
        public string ContainerName { get; set; }
        public NameId NameId { get; set; }
    }

    class CollectionContainer : Container
    {
        public List<NameId> NameIds { get; set; }
    }
}
