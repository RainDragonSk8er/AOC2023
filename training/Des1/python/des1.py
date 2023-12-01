import argparse

class Elf:
    name: int
    calories: int
    def __init__(self, name, calories):
        name: int = name
        calories: int = calories


def part1() -> Elf:
    elf_list: list[Elf] = [Elf(0,0) for i in range(2)]
    print(elf_list)
    with open("input/input_num.txt") as i_file:
        total_cals: int = 0
        elf_number:int = 0
        for line in i_file.readlines():
            if (line == "\n"):
                for i, elf in enumerate(elf_list):
                    print(elf_list[i])
                    if (total_cals >= elf_list[i].calories):
                        elf_list[i].calories = total_cals
                        elf_list[i].name = elf_number
                    elf_number += 1
                    total_cals = 0
            elif (line.strip().isdigit()):
                total_cals += int(line)
            else:
                print("Something has gone wrong!")
    return elf_list


if __name__=="__main__":
    bestElfs: list[Elf] = part1()
    print(bestElfs)
    print(list(map(lambda x: x.name, bestElfs)), "\n", list(map(lambda x: x.name, bestElfs)))