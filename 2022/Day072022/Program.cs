namespace Day072022;

internal class Program
{
    static void Main(string[] args)
    {
        string[] lines = File.ReadAllLines("./input.txt");

        FileSystemNode? current = null;
        Dictionary<string, FileSystemNode> nodes = new();
        current = BuildFileTree(lines, current, nodes);

        var root = nodes["/"];
        Part1(root);
        Part2(nodes, root);
    }

    private static void Part2(Dictionary<string, FileSystemNode> nodes, FileSystemNode root)
    {
        const long capacity = 70000000;
        long cur = root.FullSize.Value;
        long available = capacity - cur;
        long needed = 30000000 - available;
        long toDelete = nodes.Values.Select(v => v.FullSize.Value).OrderBy(v => v).First(v => v >= needed);
        Console.WriteLine($"Part 2: {toDelete}");
    }

    private static void Part1(FileSystemNode root)
    {
        long totalSize = 0;

        Stack<FileSystemNode> nodeStack = new();
        nodeStack.Push(root);

        while (nodeStack.Count > 0)
        {
            FileSystemNode node = nodeStack.Pop();
            long fullSize = node.FullSize.Value;

            if (fullSize <= 100000)
            {
                totalSize += fullSize;
            }

            foreach (FileSystemNode child in node.Children.Where(c => c.Type == FileSystemType.Directory))
            {
                nodeStack.Push(child);
            }
        }

        Console.WriteLine($"Part 1: {totalSize}");
    }

    private static FileSystemNode? BuildFileTree(string[] lines, FileSystemNode? current, Dictionary<string, FileSystemNode> nodes)
    {
        foreach (string lin in lines)
        {
            if (lin.StartsWith('$'))
            {
                ReadOnlySpan<char> cmdTxt = lin.AsSpan()[2..4];
                Command cmd = cmdTxt switch {
                    ['c', 'd'] => new Command(CommandType.cd, lin[5..]),
                    ['l', 's'] => new Command(CommandType.ls, null),
                    _ => throw new InvalidOperationException("Not supported")
                };

                if (cmd.type == CommandType.cd)
                {
                    if (cmd.Argument == null)
                    {
                        throw new InvalidOperationException("Cant cd to nowhere");
                    }
                    HandleCd(ref current, nodes, cmd.Argument);
                }
            }
            else
            {
                if (current == null)
                {
                    throw new InvalidOperationException("Can't ls from nowhere");
                }
                HandleLs(current, nodes, lin);
            }
        }

        return current;
    }

    public static void HandleLs(FileSystemNode current, Dictionary<string, FileSystemNode> nodes, string line)
    {
        if(line.StartsWith("dir"))
        {
            string dirName = ResolveFullName(current, line[4..]);

            if (!nodes.ContainsKey(dirName))
            {
                var newNode = new FileSystemNode {
                    Name = dirName,
                    Type = FileSystemType.Directory,
                    Parent = current
                };

                current.Children.Add(newNode);
                nodes.Add(dirName, newNode);
            }
        }
        else
        {
            int spaceIdx = line.IndexOf(' ') + 1;
            int size = int.Parse(line.AsSpan()[..(spaceIdx)]);
            string baseName = line[spaceIdx..];

            string fullPath = ResolveFullName(current, baseName);

            if(!nodes.ContainsKey(fullPath))
            {
                var file = new FileSystemNode {
                    Name = fullPath,
                    Type = FileSystemType.File,
                    Parent = current,
                    Size = size
                };
                current.Children.Add(file);
                nodes.Add(fullPath, file);
            }
        }
    }

    private static string ResolveFullName(FileSystemNode current, string baseName)
    {
        return current.Name switch {
            "/" => "/" + baseName,
            _ => current.Name + "/" + baseName
        };
    }

    public static void HandleCd(ref FileSystemNode? current, Dictionary<string, FileSystemNode> nodes, string target)
    {
        if(current == null && target == "/")
        {
            var node = new FileSystemNode 
            {
                Type = FileSystemType.Directory,
                Name = target
            };

            nodes.Add("/", node);
            current = node;
        }
        else if(current != null)
        {
            string? fullPath = target switch 
            {
                "/" => "/",
                ".." => current.Parent?.Name,
                _ => ResolveFullName(current, target)
            };
            if(fullPath == null || !nodes.TryGetValue(fullPath, out FileSystemNode? node)) 
            {
                throw new InvalidOperationException("Unknown directory");
            }

            current = node;
        }
    }

    internal enum CommandType
    {
        ls = 0,
        cd = 1
    }

    internal enum FileSystemType
    {
        File = 0,
        Directory = 1,
    }

    internal readonly record struct Command(CommandType type, string? Argument);
    internal class FileSystemNode
    {
        public required FileSystemType Type { get; init; }
        public required string Name { get; init; }
        public int? Size { get; init; }
        public List<FileSystemNode> Children { get; } = new ();
        public FileSystemNode? Parent { get; set; }
        public Lazy<long> FullSize { get; }

        public FileSystemNode()
        {
            FullSize = new Lazy<long>(() => ComputeDirectorySize(this));
        }

        private static long ComputeDirectorySize(FileSystemNode dir)
        {
            long size = 0;

            foreach (FileSystemNode child in dir.Children)
            {
                if (child.Type == FileSystemType.File)
                {
                    size += child.Size.Value;
                }
                else
                {
                    size += child.FullSize.Value;
                }
            }

            return size;
        }
    }
    
}
