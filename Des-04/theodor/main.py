lines = open(0).read().strip().splitlines()

t = 0

for line in lines:
    c = 0
    line = line.split(': ')[1]
    winning, card = line.split(' | ')
    winning = list(map(int, winning.split()))
    card = list(map(int, card.split()))

    for num in card:
        if num in winning:
            if not c:
                c = 1
            else:
                c *= 2
    t += c
print(t)