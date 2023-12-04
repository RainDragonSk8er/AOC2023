import re
lines = open(0).read().strip().splitlines()

rmax, gmax, bmax = 12, 13, 14

t = 0

for i, line in enumerate(lines):
    invalid = False
    for x, game in enumerate(line.split(';')):
        r = sum(map(int,re.findall('(\\d+) red', game)))
        g = sum(map(int,re.findall('(\\d+) green', game)))
        b = sum(map(int,re.findall('(\\d+) blue', game)))
        print(r, g, b)
        if r > rmax or g > gmax or b > bmax:
            invalid = True
        if r < rmax and g < gmax and b < bmax and game == line.split(';')[-1] and invalid == False:
            print(i+1)
            t+= i+1
print(t)

t = 0

for i, line in enumerate(lines):
    gid = re.findall('Game (\\d+)', line)
    r = max(map(int,re.findall('(\\d+) red', line)))
    g = max(map(int,re.findall('(\\d+) green', line)))
    b = max(map(int,re.findall('(?=(\\d+) blue)', line)))
    t += r*g*b

print(t)