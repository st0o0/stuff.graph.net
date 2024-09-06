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

            var heuristicArgs = new Heuristic.HeuristicArgs(nodeA, nodeB);
            var result = Heuristic.MaxDXDYDZ(heuristicArgs);

            Assert.Equal(5, result);  // Max difference in any single dimension
        }

        [Fact]
        public void DiagonalShortCut_ShouldReturnCorrectValue()
        {
            var nodeA = Node.Create(1, 1, 2, 3);
            var nodeB = Node.Create(2, 4, 6, 8);

            var heuristicArgs = new Heuristic.HeuristicArgs(nodeA, nodeB);
            var result = Heuristic.DiagonalShortCut(heuristicArgs);

            Assert.Equal(10.243, result, 3);
        }

        [Fact]
        public void Euclidean_ShouldReturnCorrectValue()
        {
            var nodeA = Node.Create(1, 1, 2, 3);
            var nodeB = Node.Create(2, 4, 6, 8);

            var heuristicArgs = new Heuristic.HeuristicArgs(nodeA, nodeB);
            var result = Heuristic.Euclidean(heuristicArgs);

            Assert.Equal(Math.Sqrt(50), result, 5);  // √((4-1)² + (6-2)² + (8-3)²)
        }

        [Fact]
        public void Manhattan_ShouldReturnCorrectValue()
        {
            var nodeA = Node.Create(1, 1, 2, 3);
            var nodeB = Node.Create(2, 4, 6, 8);

            var heuristicArgs = new Heuristic.HeuristicArgs(nodeA, nodeB);
            var result = Heuristic.Manhatten(heuristicArgs);

            Assert.Equal(12, result);  // |4-1| + |6-2| + |8-3|
        }
    }
}
