import re
from collections import defaultdict
lines = open(0).read().strip().splitlines()

t = 0
gearnums = defaultdict(list)

for i, row in enumerate(lines):
  for adjnum in re.finditer(r'(\d+)', row):
    for r in range(i-1, i+2):
      for c in range(adjnum.start()-1, adjnum.end()+1):
        if r >= 0 and r < len(lines) and c >= 0 and c < len(lines[r]):
          if lines[r][c] not in '.0123456789':
            if lines[r][c] == '*':
              gearnums[(r,c)].append(int(adjnum.group(0)))
            t += int(adjnum.group(0))
        
print(t)

t = 0

for k,v in gearnums.items():
  if len(v) == 2:
    t += v[0] * v[1]

print(t)