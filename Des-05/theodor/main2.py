inp, *blocks = open(0).read().strip().split("\n\n")

seeds = list(map(int, inp.split(": ")[1].split()))
seeds = [(seeds[i], seeds[i] + seeds[i + 1]) for i in range(0, len(seeds), 2)]
blocks = [block.split("\n") for block in blocks]


for block in blocks:
    source_ranges = [list(map(int, line.split())) for line in block[1:]]
    temp_seeds = []
    while len(seeds) > 0:
        s, e = seeds.pop()
        for a, b, c in source_ranges:
            os = max(s, b)
            oe = min(e, b + c)
            if os < oe:
                temp_seeds.append((os - b + a, oe - b + a))
                if os > s:
                    seeds.append((s, os))
                if e > oe:
                    seeds.append((oe, e))
                break
        else:
            temp_seeds.append((s, e))
    seeds = temp_seeds
print(min(seeds)[0])
