﻿using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace NetFabric.Tests
{
    public class AddFirstTests
    {
        public static TheoryData<IReadOnlyList<int>, int, IReadOnlyList<int>> ItemData =>
            new TheoryData<IReadOnlyList<int>, int, IReadOnlyList<int>>
            {
                { new int[] { },            1, new int[] { 1 } },
                { new int[] { 2 },          1, new int[] { 1, 2 } },
                { new int[] { 2, 3, 4, 5 }, 1, new int[] { 1, 2, 3, 4, 5 } },
            };

        [Theory]
        [MemberData(nameof(ItemData))]
        void AddItem(IReadOnlyList<int> collection, int item, IReadOnlyList<int> expected)
        {
            // Arrange
            var list = new DoubleLinkedList<int>(collection);
            var version = list.Version;

            // Act
            list.AddFirst(item);

            // Assert
            list.Count.Should().Be(expected.Count);
            list.Version.Should().NotBe(version);
            list.EnumerateForward().Should().Equal(expected);
            list.EnumerateReversed().Should().Equal(expected.Reverse());
        }

        public static TheoryData<IReadOnlyList<int>, IReadOnlyList<int>, bool, IReadOnlyList<int>> CollectionData =>
            new TheoryData<IReadOnlyList<int>, IReadOnlyList<int>, bool, IReadOnlyList<int>>
            {
                { new int[] { },                new int[] { },                  false,  new int[] { } },
                { new int[] { },                new int[] { 1 },                true,   new int[] { 1 } },
                { new int[] { },                new int[] { 1, 2, 3, 4, 5 },    true,   new int[] { 1, 2, 3, 4, 5 } },
                { new int[] { 1 },              new int[] { },                  false,  new int[] { 1 } },
                { new int[] { 2 },              new int[] { 1 },                true,   new int[] { 1, 2 } },
                { new int[] { 5 },              new int[] { 1, 2, 3, 4 },       true,   new int[] { 1, 2, 3, 4, 5 } },
                { new int[] { 1, 2, 3, 4, 5 },  new int[] { },                  false,  new int[] { 1, 2, 3, 4, 5 } },
                { new int[] { 2, 3, 4, 5 },     new int[] { 1 },                true,   new int[] { 1, 2, 3, 4, 5 } },
                { new int[] { 3, 4, 5 },        new int[] { 1, 2 },             true,   new int[] { 1, 2, 3, 4, 5 } },
            };

        [Theory]
        [MemberData(nameof(CollectionData))]
        void AddEnumerable(IReadOnlyList<int> collection, IEnumerable<int> items, bool isMutated, IReadOnlyList<int> expected)
        {
            // Arrange
            var list = new DoubleLinkedList<int>(collection);
            var version = list.Version;

            // Act
            list.AddFirst(items);

            // Assert
            list.Count.Should().Be(expected.Count);
            if (isMutated)
                list.Version.Should().NotBe(version);
            else
                list.Version.Should().Be(version);
            list.EnumerateForward().Should().Equal(expected);
            list.EnumerateReversed().Should().Equal(expected.Reverse());
        }

        [Theory]
        [MemberData(nameof(CollectionData))]
        void AddCollection(IReadOnlyList<int> collection, IReadOnlyList<int> items, bool isMutated, IReadOnlyList<int> expected)
        {
            // Arrange
            var list = new DoubleLinkedList<int>(collection);
            var version = list.Version;

            // Act
            list.AddFirst(items);

            // Assert
            list.Count.Should().Be(expected.Count);
            if (isMutated)
                list.Version.Should().NotBe(version);
            else
                list.Version.Should().Be(version);
            list.EnumerateForward().Should().Equal(expected);
            list.EnumerateReversed().Should().Equal(expected.Reverse());
        }

        public static TheoryData<IReadOnlyList<int>, IReadOnlyList<int>, bool, bool, IReadOnlyList<int>> ListData =>
            new TheoryData<IReadOnlyList<int>, IReadOnlyList<int>, bool, bool, IReadOnlyList<int>>
            {
                { new int[] { },                new int[] { },                  false,  false,  new int[] { } },
                { new int[] { },                new int[] { 1 },                false,  true,   new int[] { 1 } },
                { new int[] { },                new int[] { 1, 2, 3, 4, 5 },    false,  true,   new int[] { 1, 2, 3, 4, 5 } },
                { new int[] { 1 },              new int[] { },                  false,  false,  new int[] { 1 } },
                { new int[] { 2 },              new int[] { 1 },                false,  true,   new int[] { 1, 2 } },
                { new int[] { 5 },              new int[] { 1, 2, 3, 4 },       false,  true,   new int[] { 1, 2, 3, 4, 5 } },
                { new int[] { 1, 2, 3, 4, 5 },  new int[] { },                  false,  false,  new int[] { 1, 2, 3, 4, 5 } },
                { new int[] { 2, 3, 4, 5 },     new int[] { 1 },                false,  true,   new int[] { 1, 2, 3, 4, 5 } },
                { new int[] { 3, 4, 5 },        new int[] { 1, 2 },             false,  true,   new int[] { 1, 2, 3, 4, 5 } },
                { new int[] { },                new int[] { },                  true,   false,  new int[] { } },
                { new int[] { },                new int[] { 1 },                true,   true,   new int[] { 1 } },
                { new int[] { },                new int[] { 1, 2, 3, 4, 5 },    true,   true,   new int[] { 5, 4, 3, 2, 1 } },
                { new int[] { 1 },              new int[] { },                  true,   false,  new int[] { 1 } },
                { new int[] { 2 },              new int[] { 1 },                true,   true,   new int[] { 1, 2 } },
                { new int[] { 5 },              new int[] { 1, 2, 3, 4 },       true,   true,   new int[] { 4, 3, 2, 1, 5 } },
                { new int[] { 1, 2, 3, 4, 5 },  new int[] { },                  true,   false,  new int[] { 1, 2, 3, 4, 5 } },
                { new int[] { 2, 3, 4, 5 },     new int[] { 1 },                true,   true,   new int[] { 1, 2, 3, 4, 5 } },
                { new int[] { 3, 4, 5 },        new int[] { 1, 2 },             true,   true,   new int[] { 2, 1, 3, 4, 5 } },
            };

        [Theory]
        [MemberData(nameof(ListData))]
        void AddList(IReadOnlyList<int> collection, IReadOnlyList<int> items, bool reversed, bool isMutated, IReadOnlyList<int> expected)
        {
            // Arrange
            var left = new DoubleLinkedList<int>(collection);
            var version = left.Version;
            var right = new DoubleLinkedList<int>(items);

            // Act
            left.AddFirst(right, reversed);

            // Assert
            left.Count.Should().Be(expected.Count);
            if (isMutated)
                left.Version.Should().NotBe(version);
            else
                left.Version.Should().Be(version);
            left.EnumerateForward().Should().Equal(expected);
            left.EnumerateReversed().Should().Equal(expected.Reverse());
        }

        [Theory]
        [MemberData(nameof(ListData))]
        void AddListFrom(IReadOnlyList<int> collection, IReadOnlyList<int> items, bool reversed, bool isMutated, IReadOnlyList<int> expected)
        {
            // Arrange
            var left = new DoubleLinkedList<int>(collection);
            var version = left.Version;
            var right = new DoubleLinkedList<int>(items);

            // Act
            left.AddFirstFrom(right, reversed);

            // Assert
            left.Count.Should().Be(expected.Count);
            if (isMutated)
                left.Version.Should().NotBe(version);
            else
                left.Version.Should().Be(version);
            left.EnumerateForward().Should().Equal(expected);
            left.EnumerateReversed().Should().Equal(expected.Reverse());
            right.Count.Should().Be(0);
        }
    }
}
