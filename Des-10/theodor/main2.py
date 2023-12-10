from collections import deque

grid = open(0).read().strip().splitlines()

for r, row in enumerate(grid):
    for c, char in enumerate(row):
        if char == "S":
            sr = r
            sc = c
            break
    else:
        continue
    break

seen = {(sr, sc)}
queue = deque([(sr, sc)])
s_cand = set("|JL-F7")


while queue:
    r, c = queue.popleft()
    ch = grid[r][c]
    if r > 0 and ch in "S|JL" and grid[r - 1][c] in "|F7" and (r - 1, c) not in seen:
        seen.add((r - 1, c))
        queue.append((r - 1, c))
        if ch == "S":
            s_cand &= set("|JL")

    if (
        r < len(grid)
        and ch in "S|F7"
        and grid[r + 1][c] in "|JL"
        and (r + 1, c) not in seen
    ):
        seen.add((r + 1, c))
        queue.append((r + 1, c))
        if ch == "S":
            s_cand &= set("|7F")

    if c > 0 and ch in "S-J7" and grid[r][c - 1] in "-FL" and (r, c - 1) not in seen:
        seen.add((r, c - 1))
        queue.append((r, c - 1))
        if ch == "S":
            s_cand &= set("-J7")

    if (
        c < len(grid[r])
        and ch in "S-LF"
        and grid[r][c + 1] in "-J7"
        and (r, c + 1) not in seen
    ):
        seen.add((r, c + 1))
        queue.append((r, c + 1))
        if ch == "S":
            s_cand &= set("-LF")

(S,) = s_cand
# print(S)
grid = [row.replace("S", S) for row in grid]
grid = [
    "".join(ch if (r, c) in seen else "." for c, ch in enumerate(row))
    for r, row in enumerate(grid)
]

# for r, row in enumerate(grid):
#     for c, ch in enumerate(row):
#         print(ch, end='')
#     print()

outside = set()


for r, row in enumerate(grid):
    within = False
    up = None
    for c, ch in enumerate(row):
        if ch == "." or ch == "-":
            pass
        elif ch == "|":
            within = not within
        elif ch in "FL":
            up = ch == "L"
        elif ch in "7J":
            if ch == ("7" if up else "J"):
                within = not within
            up = None
        if not within:
            outside.add((r, c))

print(len(grid) * len(grid[0]) - len(outside | seen))
