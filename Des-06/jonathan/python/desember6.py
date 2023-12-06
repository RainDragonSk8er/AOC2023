import re
import numpy as np

# Really bad solution!
# -> rather check if the number is within each source range, and then calculate the destination!
# The size of the hasmaps would be massive, should have looked at the full input!


PATH: str = "Des-06/jonathan/input/puzzle_input.txt"
TEST_PATH_1: str = "Des-06/jonathan/input/test1_input.txt"
TEST_PATH_2: str = "Des-06/jonathan/input/test2_input.txt"

def distance_per_hold(time_hold: int, max_time: int):
    speed:int = 0
    speed = time_hold
    return speed * (max_time - time_hold)

print(distance_per_hold(10, 30))
print(distance_per_hold(20, 30))

def max_min_dist(time_max: int, max_dist: int):
    return_vals_list: list[int] = []
    for i in range(2):
        if i == 0:
            func = np.add
            round = np.ceil
            resolve = np.add
        else:
            func = np.subtract
            round = np.floor
            resolve = np.subtract
        result = np.divide(func(-time_max, np.sqrt(np.add(np.power(time_max, 2), np.multiply(-4, max_dist)))), -2)
        for i, result_num in enumerate(result):
            if result_num == np.rint(result_num):
                result[i] = resolve(result_num, 1)
        return_vals_list.append(round(result))
    
    return tuple(return_vals_list)
        

def read_in_chunks(file_object, chunk_size=1024):
    """Lazy function (generator) to read a file piece by piece.
    Default chunk size: 1k."""
    while True:
        data = file_object.read(chunk_size)
        if not data:
            break
        yield data
    

def func1(path):
    time_array: np.array
    dist_array: np.array
    with open(path, "r") as i_file:
        line_length = len(i_file.readline())
    with open(path, "r") as i_file:
        generator = read_in_chunks(i_file, line_length)
        while generator:
            try:
                line = next(generator) 
            except StopIteration:
                break
            if line[:5] == "Time:":
                time_list = list(
                    map(
                        lambda x: int(x.strip()),
                        filter(
                            lambda i: i.isdigit(),
                            line.strip().split(" ")[1:]
                        )
                    )
                )
                time_array = np.array(time_list)
            if line[:9] == "Distance:":
                dist_list = list(
                    map(
                        lambda x: int(x.strip()),
                        filter(
                            lambda i: i.isdigit(),
                            line.strip().split(" ")[1:]
                        )
                    )
                )
                dist_array = np.array(dist_list)
    out_array = max_min_dist(time_array, dist_array)
    print("This is the out:", list(out_array))
    range_list = []
    for i in range(len(out_array[0])):
        range_list.append(out_array[1][i] - out_array[0][i] + 1)
    print(range_list)
    return np.prod(range_list)


def func2(path: str):
    time_array: np.array
    dist_array: np.array
    with open(path, "r") as i_file:
        line_length = len(i_file.readline())
    with open(path, "r") as i_file:
        generator = read_in_chunks(i_file, line_length)
        while generator:
            try:
                line = next(generator) 
            except StopIteration:
                break
            if line[:5] == "Time:":
                time_list = list(
                    map(
                        lambda x: x.strip(),
                            filter(
                                lambda i: i.isdigit(),
                                line.strip().split(" ")[1:]
                        )
                    )
                )
                time_array = np.array([int("".join(time_list))])
                print("Time list:", list(time_array))
            if line[:9] == "Distance:":
                dist_list = list(
                    map(
                        lambda x: x.strip(),
                            filter(
                                lambda i: i.isdigit(),
                                line.strip().split(" ")[1:]
                        )
                    )
                )
                dist_array = np.array([int("".join(dist_list))])
                print("Time list:", list(dist_array))
    out_array = max_min_dist(time_array, dist_array)
    print("This is the out:", list(out_array))
    range_list = []
    for i in range(len(out_array[0])):
        range_list.append(out_array[1][i] - out_array[0][i] + 1)
    print(range_list)
    return np.prod(range_list)

if __name__=="__main__": 
    # answer1 = func1(PATH)
    # print("answer to question 1:", answer1)
    answer2 = func2(PATH)
    print("answer to question 2:", answer2)