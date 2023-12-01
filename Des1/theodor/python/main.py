import re
lines = open(0).read().strip().splitlines()

t=0

for line in lines:
  nums = re.findall('(\\d)', line)
  t += int(nums[0] + nums[-1])

print(t)

t2=0

n = "one two three four five six seven eight nine".split()
parse = lambda x: x if x.isdigit() else str(n.index(x)+1)

for line in lines:
  nums = re.findall('(?=(\\d|'+'|'.join(n)+'))', line)
  t2 += int(parse(nums[0]) + parse(nums[-1]))

print(t2)
