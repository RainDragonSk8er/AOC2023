from math import lcm
from re import findall

instructions, maps = open(0).read().strip().split("\n\n")

maps = maps.splitlines()

d = {a: (b, c) for a, b, c, in (findall(r"\w+", m) for m in maps)}


movers = [x for x in d if x.endswith("A")]
cycles = []

for mover in movers:
    cycle = []

    current_steps = instructions
    step_count = 0
    first_z = None

    while True:
        while step_count == 0 or not mover.endswith("Z"):
            step_count += 1
            mover = d[mover][0 if current_steps[0] == "L" else 1]
            current_steps = current_steps[1:] + current_steps[0]

        cycle.append(step_count)

        if first_z is None:
            first_z = mover
            step_count = 0
        elif mover == first_z:
            break

    cycles.append(cycle)

print(lcm(*[cycle[0] for cycle in cycles]))
