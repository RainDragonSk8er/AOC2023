lines = [row.split("\n") for row in open(0).read().strip().split("\n\n")]

seeds = [int(z) for z in lines[0][0].split(": ")[1].split()]
for i, x in enumerate(lines, 1):
    dest_ranges = []
    source_ranges = []
    for y in x[1:]:
        dest, source, length = map(int, y.split())
        dest_ranges.append(range(dest, dest + length + 1))
        source_ranges.append(range(source, source + length + 1))
    for l, seed in enumerate(seeds):
        for k, source_range in enumerate(source_ranges):
            if seed in source_range:
                seeds[l] = dest_ranges[k][source_range.index(seed)]

print(min(seeds))
