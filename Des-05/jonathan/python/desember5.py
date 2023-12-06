import re
import numpy as np

# Really bad solution!
# -> rather check if the number is within each source range, and then calculate the destination!
# The size of the hasmaps would be massive, should have looked at the full input!


PATH: str = "Des-05/jonathan/input/puzzle_input.txt"
TEST_PATH_1: str = "Des-05/jonathan/input/test1_input.txt"
TEST_PATH_2: str = "Des-05/jonathan/input/test2_input.txt"

class RangeMap:
    map_name: str
    rangeMapNums: list[list[int, int, int]]
    def __init__(self, i_name: str):
        self.map_name = i_name
        self.rangeMapNums: list[tuple[int, int, int, int]] = []
    def add_to_map(self, start_dest: int, start_src:int,  range_len: int):
        # store the source range (start and stop), and difference (i.e. the transformation to perform) 
        self.rangeMapNums.append(tuple([start_src, start_src + range_len, start_dest - start_src]))
    def convertList(self, list_values: list[int]) -> list[int]:
        outList: list[int] = []
        for value in list_values:
            mapped: bool = False
            for range in self.rangeMapNums:
                if range[0] <= value < range[1]:
                    outList.append(value + range[2])
                    mapped = True
                    break
            if mapped == False:
                outList.append(value)
        return outList



def read_in_chunks(file_object, chunk_size=1024):
    """Lazy function (generator) to read a file piece by piece.
    Default chunk size: 1k."""
    while True:
        data = file_object.read(chunk_size)
        if not data:
            break
        yield data

def runThroughRangeMapList(rangeMapList: list[RangeMap], in_array: list, idx: int = 0, out_list:list[list, list] = []):
    if idx == 0:
        out_list: list[list[str], list[list[int]]] = [list(["seeds"]), list([in_array])]
    try:
        rangeMap: RangeMap = rangeMapList[idx]
    except IndexError:
        # print("Got to the exit!")
        # print("\t\toutlist:", out_list)
        # print("answer:", min(out_list[1][-1]))
        return out_list
    else:
        # print("idx:", idx)
        # print(RangeMap[idx].map_name)
        # print(RangeMap[idx].rangeMapNums)
        new_list: list[int] = []
        new_list = rangeMap.convertList(in_array)

        out_list[0].append(rangeMapList[idx].map_name)
        out_list[1].append(new_list)
        return runThroughRangeMapList(rangeMapList=rangeMapList, in_array=out_list[1][-1], idx=idx+1, out_list=out_list)
    

def func1(path):
    my_seeds: list
    my_maps: list[RangeMap] = []
    with open(path, "r") as i_file:
        for line in i_file.readlines():
            line = line.strip()
            if line[:5] == "seeds":
                
                my_seeds: list = list(
                    map(
                        lambda x: int(x), 
                        filter(
                            lambda x: x.isdigit(),
                            line.split(" ")
                        )
                    )
                )
                # print("found seeds:", my_seeds)
            if line[-4:] == "map:":
                map_name: str =line.split(" ")[0]
                my_maps.append(RangeMap(map_name))
                # print("found map_name:", map_name)
            if line.split(" ")[0].isdigit():
                map_init: list = list(
                    map(
                        lambda x: int(x), 
                        line.split(" ")
                    )
                )
                # print("found map part:", map_init)
                my_maps[-1].add_to_map(map_init[0], map_init[1], map_init[2])
    out_list = runThroughRangeMapList(my_maps, my_seeds)
    print(out_list)
    # print("my maps:\n", my_maps[0].rangeMapNums)
    min_loc: int = min(out_list[1][-1])
    return min_loc


def func2(path: str):
    my_seeds: list
    seedRanges: list[tuple[int, int]]
    my_maps: list[RangeMap] = []
    with open(path, "r") as i_file:
        for line in i_file.readlines():
            line = line.strip()
            if line[:5] == "seeds":
                
                my_seeds: list = list(
                    map(
                        lambda x: int(x), 
                        filter(
                            lambda x: x.isdigit(),
                            line.split(" ")
                        )
                    )
                )
                seedRanges: list[tuple[int]] = []
                for i in range(0, len(my_seeds), 2):
                    seedRanges.append(tuple([my_seeds[i], my_seeds[i+1]]))
                print("found seed ranges:", seedRanges)
            if line[-4:] == "map:":
                map_name: str =line.split(" ")[0]
                my_maps.append(RangeMap(map_name))
                # print("found map_name:", map_name)
            if line.split(" ")[0].isdigit():
                map_init: list = list(
                    map(
                        lambda x: int(x), 
                        line.split(" ")
                    )
                )
                # print("found map part:", map_init)
                my_maps[-1].add_to_map(map_init[0], map_init[1], map_init[2])
    min_seed: int
    for i, seedRange in enumerate(seedRanges):
        print("running through range with start:", seedRange[0], ", of length:", seedRange[1])
        for j, seed in enumerate(range(seedRange[0], seedRange[0]+seedRange[1])):
            if j % 100000 == 0:
                print("milestone:", j)
            out_list = runThroughRangeMapList(my_maps, [seed])
            new_num = out_list[1][-1][0]
            if i == 0:
                min_seed = new_num
            if new_num < min_seed:
                min_seed = new_num
                
        print("currently lowest:", min_seed)
    # print(out_list)
    # print("my maps:\n", my_maps[0].rangeMapNums)
    # min_loc: int = min(out_list[1][-1])
    return min_seed

if __name__=="__main__": 
    # answer1 = func1(PATH)
    # print("answer to question 1:", answer1)
    answer2 = func2(PATH)
    print("answer to question 2:", answer2)