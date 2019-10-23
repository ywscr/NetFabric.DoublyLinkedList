﻿using System;
using System.Collections.Generic;
using System.Linq;
using NetFabric.Assertive;
using Xunit;

namespace NetFabric.Tests
{
    public class AddLastTests
    {
        [Fact]
        void AddNullEnumerable()
        {
            // Arrange
            var list = new DoublyLinkedList<int>();

            // Act
            Action action = () => list.AddLast((IEnumerable<int>)null);

            // Assert
            action.Must()
                .Throw<ArgumentNullException>()
                .EvaluatesTrue(exception =>
                    exception.ParamName == "collection");
        }

        [Fact]
        void AddNullCollection()
        {
            // Arrange
            var list = new DoublyLinkedList<int>();

            // Act
            Action action = () => list.AddLast((IReadOnlyList<int>)null);

            // Assert
            action.Must()
                .Throw<ArgumentNullException>()
                .EvaluatesTrue(exception =>
                    exception.ParamName == "collection");
        }

        [Fact]
        void AddNullList()
        {
            // Arrange
            var list = new DoublyLinkedList<int>();

            // Act
            Action action = () => list.AddLast((DoublyLinkedList<int>)null);

            // Assert
            action.Must()
                .Throw<ArgumentNullException>()
                .EvaluatesTrue(exception =>
                    exception.ParamName == "list");
        }

        public static TheoryData<IReadOnlyList<int>, int, IReadOnlyList<int>> ItemData =>
            new TheoryData<IReadOnlyList<int>, int, IReadOnlyList<int>>
            {
                { new int[] { },            1, new int[] { 1 } }, 
                { new int[] { 1 },          2, new int[] { 1, 2 } },
                { new int[] { 1, 2, 3, 4 }, 5, new int[] { 1, 2, 3, 4, 5 } },
            };

        [Theory]
        [MemberData(nameof(ItemData))]
        void AddItem(IReadOnlyList<int> collection, int item, IReadOnlyList<int> expected)
        {
            // Arrange
            var list = new DoublyLinkedList<int>(collection);
            var version = list.Version;

            // Act
            list.AddLast(item);

            // Assert
            list.Version.Must()
                .BeNotEqualTo(version);
            list.EnumerateForward().Must()
                .BeEnumerableOf<int>()
                .BeEqualTo(expected);
            list.EnumerateReversed().Must()
                .BeEnumerableOf<int>()
                .BeEqualTo(expected.Reverse());
        }

        public static TheoryData<IReadOnlyList<int>, IReadOnlyList<int>, bool, IReadOnlyList<int>> CollectionData =>
            new TheoryData<IReadOnlyList<int>, IReadOnlyList<int>, bool, IReadOnlyList<int>>
            {
                { new int[] { },                new int[] { },                  false,  new int[] { } },
                { new int[] { },                new int[] { 1 },                true,   new int[] { 1 } },
                { new int[] { },                new int[] { 1, 2, 3, 4, 5 },    true,   new int[] { 1, 2, 3, 4, 5 } },
                { new int[] { 1 },              new int[] { },                  false,  new int[] { 1 } },
                { new int[] { 1 },              new int[] { 2 },                true,   new int[] { 1, 2 } },
                { new int[] { 1 },              new int[] { 2, 3, 4, 5 },       true,   new int[] { 1, 2, 3, 4, 5 } },
                { new int[] { 1, 2, 3, 4, 5 },  new int[] { },                  false,  new int[] { 1, 2, 3, 4, 5 } },
                { new int[] { 1, 2, 3, 4 },     new int[] { 5 },                true,   new int[] { 1, 2, 3, 4, 5 } },
                { new int[] { 1, 2, 3 },        new int[] { 4, 5 },             true,   new int[] { 1, 2, 3, 4, 5 } },
            };

        [Theory]
        [MemberData(nameof(CollectionData))]
        void AddEnumerable(IReadOnlyList<int> collection, IEnumerable<int> items, bool isMutated, IReadOnlyList<int> expected)
        {
            // Arrange
            var list = new DoublyLinkedList<int>(collection);
            var version = list.Version;

            // Act
            list.AddLast(items);

            // Assert
            if (isMutated)
                list.Version.Must().BeNotEqualTo(version);
            else
                list.Version.Must().BeEqualTo(version);
            list.EnumerateForward().Must()
                .BeEnumerableOf<int>()
                .BeEqualTo(expected);
            list.EnumerateReversed().Must()
                .BeEnumerableOf<int>()
                .BeEqualTo(expected.Reverse());
        }

        [Theory]
        [MemberData(nameof(CollectionData))]
        void AddCollection(IReadOnlyList<int> collection, IReadOnlyList<int> items, bool isMutated, IReadOnlyList<int> expected)
        {
            // Arrange
            var list = new DoublyLinkedList<int>(collection);
            var version = list.Version;

            // Act
            list.AddLast(items);

            // Assert
            if (isMutated)
                list.Version.Must().BeNotEqualTo(version);
            else
                list.Version.Must().BeEqualTo(version);
            list.EnumerateForward().Must()
                .BeEnumerableOf<int>()
                .BeEqualTo(expected);
            list.EnumerateReversed().Must()
                .BeEnumerableOf<int>()
                .BeEqualTo(expected.Reverse());
        }

        public static TheoryData<IReadOnlyList<int>, IReadOnlyList<int>, bool, bool, IReadOnlyList<int>> ListData =>
            new TheoryData<IReadOnlyList<int>, IReadOnlyList<int>, bool, bool, IReadOnlyList<int>>
            {
                { new int[] { },                new int[] { },                  false,  false,  new int[] { } },
                { new int[] { },                new int[] { 1 },                false,  true,   new int[] { 1 } },
                { new int[] { },                new int[] { 1, 2, 3, 4, 5 },    false,  true,   new int[] { 1, 2, 3, 4, 5 } },
                { new int[] { 1 },              new int[] { },                  false,  false,  new int[] { 1 } },
                { new int[] { 1 },              new int[] { 2 },                false,  true,   new int[] { 1, 2 } },
                { new int[] { 1 },              new int[] { 2, 3, 4, 5 },       false,  true,   new int[] { 1, 2, 3, 4, 5 } },
                { new int[] { 1, 2, 3, 4, 5 },  new int[] { },                  false,  false,  new int[] { 1, 2, 3, 4, 5 } },
                { new int[] { 1, 2, 3, 4 },     new int[] { 5 },                false,  true,   new int[] { 1, 2, 3, 4, 5 } },
                { new int[] { 1, 2, 3 },        new int[] { 4, 5 },             false,  true,   new int[] { 1, 2, 3, 4, 5 } },
                { new int[] { },                new int[] { },                  true,   false,  new int[] { } },
                { new int[] { },                new int[] { 1 },                true,   true,   new int[] { 1 } },
                { new int[] { },                new int[] { 1, 2, 3, 4, 5 },    true,   true,   new int[] { 5, 4, 3, 2, 1 } },
                { new int[] { 1 },              new int[] { },                  true,   false,  new int[] { 1 } },
                { new int[] { 1 },              new int[] { 2 },                true,   true,   new int[] { 1, 2 } },
                { new int[] { 1 },              new int[] { 2, 3, 4, 5 },       true,   true,   new int[] { 1, 5, 4, 3, 2 } },
                { new int[] { 1, 2, 3, 4, 5 },  new int[] { },                  true,   false,  new int[] { 1, 2, 3, 4, 5 } },
                { new int[] { 1, 2, 3, 4 },     new int[] { 5 },                true,   true,   new int[] { 1, 2, 3, 4, 5 } },
                { new int[] { 1, 2, 3 },        new int[] { 4, 5 },             true,   true,   new int[] { 1, 2, 3, 5, 4 } },
            };

        [Theory]
        [MemberData(nameof(ListData))]
        void AddList(IReadOnlyList<int> collection, IReadOnlyList<int> items, bool reverse, bool isMutated, IReadOnlyList<int> expected)
        {
            // Arrange
            var left = new DoublyLinkedList<int>(collection);
            var version = left.Version;
            var right = new DoublyLinkedList<int>(items);

            // Act
            left.AddLast(right, reverse);

            // Assert
            if (isMutated)
                left.Version.Must().BeNotEqualTo(version);
            else
                left.Version.Must().BeEqualTo(version);
            left.EnumerateForward().Must()
                .BeEnumerableOf<int>()
                .BeEqualTo(expected);
            left.EnumerateReversed().Must()
                .BeEnumerableOf<int>()
                .BeEqualTo(expected.Reverse());
        }

        [Theory]
        [MemberData(nameof(ListData))]
        void AddListFrom(IReadOnlyList<int> collection, IReadOnlyList<int> items, bool reverse, bool isMutated, IReadOnlyList<int> expected)
        {
            // Arrange
            var left = new DoublyLinkedList<int>(collection);
            var version = left.Version;
            var right = new DoublyLinkedList<int>(items);

            // Act
            left.AddLastFrom(right, reverse);

            // Assert
            if (isMutated)
                left.Version.Must().BeNotEqualTo(version);
            else
                left.Version.Must().BeEqualTo(version);
            left.EnumerateForward().Must()
                .BeEnumerableOf<int>()
                .BeEqualTo(expected);
            left.EnumerateReversed().Must()
                .BeEnumerableOf<int>()
                .BeEqualTo(expected.Reverse());
            right.EnumerateForward().Must()
                .BeEnumerableOf<int>()
                .BeEmpty();
        }
    }
}
