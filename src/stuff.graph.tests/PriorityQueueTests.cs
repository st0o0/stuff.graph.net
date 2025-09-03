using stuff.graph.algorithms.net;

namespace stuff.graph.tests;

public class PriorityQueueTests
{
    [Fact]
    public void Enqueue_And_Dequeue_ShouldReturnInCorrectOrder()
    {
        // Arrange
        var pq = new PriorityQueue<int>();

        // Act
        pq.Enqueue(5);
        pq.Enqueue(2);
        pq.Enqueue(8);
        pq.Enqueue(1);

        // Assert
        Assert.Equal(4, pq.Count);
        Assert.Equal(1, pq.Dequeue());
        Assert.Equal(2, pq.Dequeue());
        Assert.Equal(5, pq.Dequeue());
        Assert.Equal(8, pq.Dequeue());
        Assert.Equal(0, pq.Count);
    }

    [Fact]
    public void Dequeue_EmptyQueue_ShouldThrow()
    {
        // Arrange
        var pq = new PriorityQueue<int>();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => pq.Dequeue());
    }

    [Fact]
    public void Contains_ShouldWorkWithDefaultComparator()
    {
        // Arrange
        var pq = new PriorityQueue<int>();
        pq.Enqueue(5);

        // Act & Assert
        Assert.True(pq.Contains(5));
        Assert.False(pq.Contains(10));
    }

    [Fact]
    public void Contains_ShouldWorkWithCustomPredicate()
    {
        // Arrange
        var pq = new PriorityQueue<string>();
        pq.Enqueue("Apple");

        // Act & Assert
        Assert.True(pq.Contains("APPLE", (a, b) => a.Equals(b, StringComparison.OrdinalIgnoreCase)));
        Assert.False(pq.Contains("Banana", (a, b) => a.Equals(b, StringComparison.OrdinalIgnoreCase)));
    }

    [Fact]
    public void UpdatePriority_ShouldReorderElements()
    {
        // Arrange
        var pq = new PriorityQueue<TestItem>();
        var item1 = new TestItem { Value = 10, Id = 1 };
        var item2 = new TestItem { Value = 20, Id = 2 };
        pq.Enqueue(item1);
        pq.Enqueue(item2);

        // Act
        item2.Value = 5;
        pq.UpdatePriority(item2);

        // Assert
        Assert.Equal(item2, pq.Dequeue());
        Assert.Equal(item1, pq.Dequeue());
    }

    [Fact]
    public void UpdatePriority_NonExistingItem_ShouldDoNothing()
    {
        // Arrange
        var pq = new PriorityQueue<int>();
        pq.Enqueue(1);

        // Act
        pq.UpdatePriority(2);

        // Assert
        Assert.Equal(1, pq.Count);
    }

    private class TestItem : IComparable<TestItem>
    {
        public int Id { get; set; }
        public int Value { get; set; }

        public int CompareTo(TestItem? other)
        {
            if (other == null) return 1;
            return Value.CompareTo(other.Value);
        }

        public override bool Equals(object? obj)
        {
            if (obj is TestItem other) return Id == other.Id;
            return false;
        }

        public override int GetHashCode() => Id.GetHashCode();
    }
}
