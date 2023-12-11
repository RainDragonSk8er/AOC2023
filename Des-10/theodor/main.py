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


while queue:
    r, c = queue.popleft()
    if (
        r > 0
        and grid[r][c] in "S|JL"
        and grid[r - 1][c] in "|F7"
        and (r - 1, c) not in seen
    ):
        seen.add((r - 1, c))
        queue.append((r - 1, c))

    if (
        r < len(grid)
        and grid[r][c] in "S|F7"
        and grid[r + 1][c] in "|JL"
        and (r + 1, c) not in seen
    ):
        seen.add((r + 1, c))
        queue.append((r + 1, c))

    if (
        c > 0
        and grid[r][c] in "S-J7"
        and grid[r][c - 1] in "-FL"
        and (r, c - 1) not in seen
    ):
        seen.add((r, c - 1))
        queue.append((r, c - 1))

    if (
        c < len(grid[r])
        and grid[r][c] in "S-LF"
        and grid[r][c + 1] in "-J7"
        and (r, c + 1) not in seen
    ):
        seen.add((r, c + 1))
        queue.append((r, c + 1))

print(len(seen) // 2)
