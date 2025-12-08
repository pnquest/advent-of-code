// See https://aka.ms/new-console-template for more information
using Core;

IntPoint3[] points = File.ReadAllLines("./input.txt")
    .Select(l => IntPoint3.Parse(l))
    .ToArray();

List<Distance> distances = [];

for (int i = 0; i < points.Length; i++)
{
    for (int j = i + 1; j < points.Length; j++)
    {
        distances.Add(new Distance(points[i], points[j], points[i].ComputeDistance(points[j])));
    }
}

Part1(points, distances);

long result = Part2(points, distances);
Console.WriteLine($"Part 2: {result}");

static void Part1(IntPoint3[] points, List<Distance> distances)
{
    Dictionary<IntPoint3, Junction> junctions = points.ToDictionary(d => d, d => new Junction { Location = d });
    int nextId = 0;
    List<HashSet<Junction>> circuits = new();

    foreach (var pair in distances.OrderBy(d => d.Length).Take(1000))
    {
        Junction left = junctions[pair.First];
        Junction right = junctions[pair.Second];

        if (left.CircuitId != null && right.CircuitId == null)
        {
            right.CircuitId = left.CircuitId;
            circuits[left.CircuitId.Value].Add(right);
        }
        else if (left.CircuitId == null && right.CircuitId != null)
        {
            left.CircuitId = right.CircuitId;
            circuits[right.CircuitId.Value].Add(left);
        }
        else if (left.CircuitId == null && right.CircuitId == null)
        {
            int circuitId = nextId++;
            left.CircuitId = circuitId;
            right.CircuitId = circuitId;
            circuits.Add([left, right]);
        }
        else if (left.CircuitId != right.CircuitId)
        {
            HashSet<Junction> leftCuircuit = circuits[left.CircuitId!.Value];
            HashSet<Junction> rightCuircuit = circuits[right.CircuitId!.Value];

            foreach (Junction r in rightCuircuit)
            {
                r.CircuitId = left.CircuitId;
                leftCuircuit.Add(r);
            }

            rightCuircuit.Clear();
        }
    }

    long answer = circuits.Where(c => c.Count > 0).OrderByDescending(c => c.Count).Take(3).Aggregate(1L, (t, j) => t *= (long)j.Count);

    Console.WriteLine($"Part 1: {answer}");
}

static long Part2(IntPoint3[] points, List<Distance> distances)
{
    Dictionary<IntPoint3, Junction> junctions = points.ToDictionary(d => d, d => new Junction { Location = d });

    int nextId = 0;
    List<HashSet<Junction>> circuits = new();

    foreach (var pair in distances.OrderBy(d => d.Length))
    {
        Junction left = junctions[pair.First];
        Junction right = junctions[pair.Second];

        if (left.CircuitId != null && right.CircuitId == null)
        {
            right.CircuitId = left.CircuitId;
            circuits[left.CircuitId.Value].Add(right);

            if (circuits[left.CircuitId.Value].Count == points.Length)
            {
                return (long)left.Location.X * (long)right.Location.X;
            }
        }
        else if (left.CircuitId == null && right.CircuitId != null)
        {
            left.CircuitId = right.CircuitId;
            circuits[right.CircuitId.Value].Add(left);

            if (circuits[left.CircuitId.Value].Count == points.Length)
            {
                return (long)left.Location.X * (long)right.Location.X;
            }
        }
        else if (left.CircuitId == null && right.CircuitId == null)
        {
            int circuitId = nextId++;
            left.CircuitId = circuitId;
            right.CircuitId = circuitId;
            circuits.Add([left, right]);

            if (circuits[left.CircuitId.Value].Count == points.Length)
            {
                return (long)left.Location.X * (long)right.Location.X;
            }
        }
        else if (left.CircuitId != right.CircuitId)
        {
            HashSet<Junction> leftCuircuit = circuits[left.CircuitId!.Value];
            HashSet<Junction> rightCuircuit = circuits[right.CircuitId!.Value];

            foreach (Junction r in rightCuircuit)
            {
                r.CircuitId = left.CircuitId;
                leftCuircuit.Add(r);
            }

            rightCuircuit.Clear();

            if (circuits[left.CircuitId.Value].Count == points.Length)
            {
                return (long)left.Location.X * (long)right.Location.X;
            }
        }
    }

    throw new InvalidOperationException("Shouln't have made it here");
}

record Distance(IntPoint3 First, IntPoint3 Second, double Length);

class Junction
{
    public IntPoint3 Location { get; set; }
    public int? CircuitId { get; set; }

    public override bool Equals(object? obj) => obj is Junction junction && Location.Equals(junction.Location);
    public override int GetHashCode() => HashCode.Combine(Location);
}
