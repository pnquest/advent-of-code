namespace Day062022;

internal class Program
{
    static void Main(string[] args)
    {
        const string input = "bfdbbngnvnsvshhhrvrbrtbrrhqrqgrrmmdfmmqttptltntrntrnrcrdcrrctctdtwtrwwmlltcltcllmpprvvtbbmbvbsvvqwvvscswsqqgzqqppvzppnddjwddlrdlllmwllfccdfccswwrhhndhdhfdftdfdcdcllcjjbsbgssvlvrrhfrfjfpjffvnfvfwfzftztwwrhwrhwhwddpjjmhmgmsssstzszqzfqzzprpzpnndttphtpphvvcpcjppdtdwwqgwwwnhwnnvhvsvqqtrrbsscwssgwglgwlldjdzjjbsjbsbvvpbvppvfvqvbqbzzwtwbwpbbbcffdgggpnpfptpfphfhfthfthffzrrbzbcblbmmzmhhnvvqvlvwvzvtztgzznbbcqbccwhcwcnwnhndhhgcghhlthllplvppgngrgsswfsfnfjjswstsrtrhthhqzqggdtggzvvjljddqdzzrmrjmjcmjmrrwlldttrhhfjjgbglgjlldrllppnwpnwnndrddtdqdhhpccbgcgzzswzztgzzrwwzzmtztvvrtrgttvddwvwvmwvmmdjdrdtthqhgqhggldgdfdnnzjnjffmrrnprnpnssjrrbrjjrrzvrvhvllfvlvnlvvhlhrhzrzrsrswrssjvjdjfjwwjmjvjjthhnjnmnppsccmlcclfclcbllldwldwdlwddbddmnmrmgrmmtsszgssqjsqscswwzmwmbwwvqqbtbtwwpbbwdbdbhhqmqgqddflfmfnmffbtfbbsdsffspsjpsspzszccrsrstrtjrjppcrrpqqvttsbbjtbjjpbbccmzznvvzzvnvmmwmhmvmsstlssqtssvggtbgttgvtggfhggbcgcmcrmcmhhbttdlttfrfprrvttrtggcpplccqggcwwdjjjplprrbsrbbjsjbbfhbhphtttpffdgdgjddzldzlzplpnplljttdqqlmlllhzhjhccpwcwhwwbbqtqffjpfpfpjptplltbtzbbwwvggpcpprdprpbpvvgzgczzfffzcfzccjffrmrhmrhhchvhddsvswscsnnpfppdrdcrdrgdgmdgmgllhggjwggvqvmmvrrzpprhhqfhfvffvlvnlngnffbbtccfbfhfwhhhhwcwzwjjbffvrffqrrnsnwswrsrvrjvrvprpsptspttbztzfzlfzlflcflclqlplglmlqqvlvvhccmmddqvqzvvmlllvnlnhnfnwnlndnntrrhvvgjjnpnggfqgqdqqzvvclvclvlfvlvvlzlssqffmrfmfmjjczcmmmnttnfnmfnmmmhmggjgdjjqmjjvsslszlzrrpdpcpfcpfpnpssvtsvtvllbttzzljzzhrrnsnddjgdjgjljzzvhvddmdvvdtdmmpmqmnnwcwfwbfwwsvvbpvvgvnvcvdvtvrrcppwbppcqpccwqqzmqmmzczppwbbrzrjzzcrcvrrpttrjjcdjcjfcclscstthqhfqhffdcfdfqdqldddsswbblppdmdnnjvvhwvhvjhvjhjhlhcclrrcqcgcjgcjjtbtctbcbsccfwwltlrlplpfpjphplhhgtghhrppwhhrprwpwhhdqdpdwpwwtccvncvvvrpvphpssfpplcczttltgltldlmlqmmbsszdszznpprsprsprsppsbppjddjhhnrrfsrffmlmglgmlglccddpssdpsddhhfmfsfrfsfnnlggfrgghmgmrmprmprrnvrnnbqnqddhhfwfmfwfmfpfdfzdzppsbpsbpprfrvvwvtwtltvlttrhhgvhhjttmssdggnzzvmvcczmzbzwzttvtpvpcphcctmmhshjsjbssglsglgtltslscchhhsjjpljjdtdsdpppptlpprmppwdppglgmggnddztdzddzppswwmhfbpqzffjqgmsntwsnrwqrqwgpwgrbpbjwrhbcdcvqjnwslsnwhglcsjbwjhswjvzssfqgwbbdgbwfrblfmmlmsndhtlbwzfwsspqlncspqbgbnzshbwpvrmjqjzbcbzzdgssbtqdzffjphqjvrspfrjhpspbwcjwbfhqzsdnjwqjzjtjgnbrdbwqhzffphzppvlmsmppqcfjbjbdsnbwtvthwqcfrtfrwchnmqmhnwfcjtbwqwwvlnpmwlrvzwljrljzqstzglqwbzfdftzltlcbvmmfwcjqglvznztwnvzvftpndqmngqswppsnqhdbgthrddfbcfpflpndrhmcqwvnbfztsvnjjdwqgpmvdwvdftgbtvrwbnvvrwsdfzhwbwdhlpzbcqdzhbfqtpjqcmrpvcsrmcwvgghqrclfzpfgnppzmhvdhvdfrcrnjbdcwbftcqjhhfdsnfnwjzjllzzqftzsjrqnsbpjdcswhhmwwdzmvmqcjtqnczjcvzmmqwzjhjpcczgpbmcvbwmpmvnghlrgcmrrdnmjvmvnhtpfpgwgmdfzvlbclzjzwdqqcvfhhgfzdhzpdvfmwjlzzrpdgzmttmvvcplbwfzqftcgcwcgcpgwvnmlqsplpqwfnhwvqtlwcspqdzshsqnlcpqhpcpbwdhdjsmvtbqdwbcrscqfjcrcjhbjbpzbshpbmlcthmbjfwhzfphgbfqfnfztptzvdnwrpslmdtpmzmpbsszqshwdghrbtvhwzhcmpcgfqggpgzwmhhdrlhlvnpzvwwhzqvgvhrzngttcnqgjjhnblncnqnjzlwnmwnrtvwjtnrbhthncwmzwqdbdgtwrncljddnbhmphgjzfrrgmmcwfwjwjlcrhvcdtvsrvsfhmlgmzsgjchhrfmqslmdgtdtrlbhdffddvbsdbdwlwdcmcmmpvzpdtmbthjdzlpwftptpfsggmhjfjwvbwljsfhfwtbfwmczwhbmhvzllqtcqqfbrcdqqsrcpfmnswnfzfqghcmcbqwgvzqpwvvmbpddlhgjgzvgmpljznrhqphwcztqzpnhzqdgpwwclmsgpwnwtvtsjsdmcnvmjbqglttrhbzqdbgwnbsqzmsmztndrtmlpszhzgjbbftbsdwwdrlftrbbnrsqshfhdpdrwmztcqzdjlnthnjhppwntmbqdgzpmfmfnccblsdwljqhjfgtlgvpzpjbsndmwzfwrbmdhpnmbchqlwqtbhhhqqbsfnvscjwrzvjdtvbsqwzvfhwbbjgqpzcwqjdrlfmggzmhbcrhtbqdjntbtqdvmvpqflmccfpnbmnmtqbdflsgczpbsqpfphlzqgvwbjlmsgshrhpcljzdvwvdvlqqwchtjmjgtqjhgwtnddmhphwhvwhtrhfbjjjzfgrcqngnnddctzdzlqjlbdwmjqzccwrvctrzgtzqsswggbqdnplclhtdslcvzhppcjjslnshtwjnbrwdprqhdtfqmqpgfgnqtdnnhrnzfrsqhlftpdslgmmvqhvpjqjwpwgtnmgrbhwntdjftfwtjzjtprctbtsjmqmpcbbtrjvsgqgsfjprqmsmdztbhnbgzldqfzgwqwnnccgcfclctrwqmqpgvfglgsmmpjszqnphnzcnvswpsfsrmnsnlqnpmvfdvdtfgzrdmbftdrrrbfsvzfgmnffvjpcpnndrwhtjjrrvnztlfhcvfqjgfrhtbnhmwnrmwdhzmmtvjmsqmghtbtfjwdnvdcqtqjrfhrwscjftmbgjmcsrbpdpttlmvfmnfjhnptqvggnshzqnlqqdpqqsqssppbwpblhgfrwrblpzwvqphpsgfmbpqtqqpjpgnbblzstgcjhqntgpbfwlzzctqbnbvpgwsdsdldqzhvznqcsrrghpwllshqpdlqnqgzfwrnhwsvhftzplspcbqmclplprlthvwjhdndrjblqdgwvgjlbmblbmcnbzwzdlnpnhhppvrtngvqqwsttgwlvtcqmtrvpbnvcnfqdtqrsrsmhclmtgbdwwdvhwgfcqpmprcpdhqwftcchbwvstcdqrlwtgbcfqfgzprgvpbbzlqfzbqtcrlzscnqpqwtgzbbbdvsvmhggdr";
        Part1(input);
        Part2(input);
    }

    private static void Part2(string input)
    {
        int found = FindFirstUniqueChain(input, 14);
        Console.WriteLine($"Part 2: {found}");
    }

    private static void Part1(string input)
    {
        int curEnd = FindFirstUniqueChain(input, 4);

        Console.WriteLine($"Part 1: {curEnd}");
    }

    private static int FindFirstUniqueChain(string input, int size)
    {
        bool found = false;
        int curIndex = 0;
        int curEnd = size;
        var pairs = new HashSet<char>(size);
        ReadOnlySpan<char> inputSpan = input.AsSpan();
        while (!found)
        {
            pairs.Clear();
            ReadOnlySpan<char> spn = inputSpan[curIndex..curEnd];
            foreach (char c in spn)
            {
                if(!pairs.Add(c))
                {
                    break;
                }
            }

            if (pairs.Count == size)
            {
                found = true;
            }

            curIndex++;
            curEnd++;
        }

        return curEnd - 1;
    }
}
