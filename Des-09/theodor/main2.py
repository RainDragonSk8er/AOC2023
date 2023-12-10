def lastElementOfNextRow(a):
    if all(x == 0 for x in a):
        return 0

    diff = lastElementOfNextRow([y - x for x, y in zip(a, a[1:])])
    return a[0] - diff


print(sum(lastElementOfNextRow(list(map(int, line.split()))) for line in open(0)))
