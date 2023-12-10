from re import findall

instructions, maps = open(0).read().strip().split("\n\n")

maps = maps.splitlines()

d = {a: (b, c) for a, b, c, in (findall(r"\w+", m) for m in maps)}

cnt = 0
mover = "AAA"

while mover != "ZZZ":
    for char in instructions:
        cnt += 1
        mover = d[mover][0 if char == "L" else 1]

print(cnt)
