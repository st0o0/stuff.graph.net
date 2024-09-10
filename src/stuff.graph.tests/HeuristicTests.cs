using stuff.graph.astar.net;
using stuff.graph.net;

namespace stuff.graph.tests
{
    public class HeuristicTests
    {
        [Fact]
        public void MaxDXDYDZ_ShouldReturnCorrectValue()
        {
            var nodeA = Node.Create(1, 1, 2, 3);
            var nodeB = Node.Create(2, 4, 6, 8);

            var result = Heuristic.MaxDXDYDZ(nodeA, nodeB);

            Assert.Equal(5, result);
        }

        [Fact]
        public void DiagonalShortCut_ShouldReturnCorrectValue()
        {
            var nodeA = Node.Create(1, 1, 2, 3);
            var nodeB = Node.Create(2, 4, 6, 8);

            var result = Heuristic.DiagonalShortCut(nodeA, nodeB);

            Assert.Equal(10.243, result, 3);
        }

        [Fact]
        public void Euclidean_ShouldReturnCorrectValue()
        {
            var nodeA = Node.Create(1, 1, 2, 3);
            var nodeB = Node.Create(2, 4, 6, 8);

            var result = Heuristic.Euclidean(nodeA, nodeB);

            Assert.Equal(Math.Sqrt(50), result, 5);
        }

        [Fact]
        public void Manhattan_ShouldReturnCorrectValue()
        {
            var nodeA = Node.Create(1, 1, 2, 3);
            var nodeB = Node.Create(2, 4, 6, 8);
            var result = Heuristic.Manhatten(nodeA, nodeB);

            Assert.Equal(12, result);
        }
    }
}
