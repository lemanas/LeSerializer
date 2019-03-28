using System;
using System.Collections.Generic;
using JsonSerializer.TestObjects;

namespace JsonSerializer
{
    class Program
    {
        static void Main()
        {
            // Block of test objects =============================================
            int[] arr = { 1, 2, 3, 4 };
            NameId nameId = new NameId
            {
                Id = 0,
                Name = "Test1"
            };
            NameId nameId2 = new NameId
            {
                Id = 1,
                Name = "Test2"
            };
            List<NameId> nameIds = new List<NameId> { nameId, nameId2 };
            var container = new Container
            {
                ContainerId = 0,
                ContainerName = "Name",
                NameId = nameId
            };
            var collectionContainer = new CollectionContainer
            {
                ContainerId = 0,
                ContainerName = "Name",
                NameId = nameId,
                NameIds = nameIds
            };
            var nameNumbers = new NameIdWithNumber
            {
                Id = 1,
                Name = "Test2",
                Array = arr
            };
            DateTime date = DateTime.Now;

            // currently broken, fix later
            DateName dateName = new DateName
            {
                Id = 0,
                Name = "Test1",
                Date = date
            };
            // ===================================================================

            LeSerializer serializer = new LeSerializer(collectionContainer);
            Console.Write(serializer.Serialize());

            Console.ReadKey();
        }
    }
}
