from collections import defaultdict

runs = defaultdict(int)

for i, line in enumerate(open(0)):
    runs[i] += 1

    line = line.split(': ')[1].strip()
    winning, card = [list(map(int, k.split())) for k in line.split(' | ')]
    c = sum(num in card for num in winning)
    for n in range(i+1, i+c+1):
        runs[n] += runs[i]


print(sum(runs.values()))