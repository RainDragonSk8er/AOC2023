
PATH: str = "input/puzzle_input.txt"
TEST_PATH_1: str = "input/test1_input.txt"
TEST_PATH_2: str = "input/test2_input.txt"


def func1(path: str):
    cum_sum: int = 0
    with open(path) as i_file:
        for line in i_file.readlines():
            char_list: list[str] = line
            num_filter_char_list: list[str] = list(filter(str.isdigit, char_list))
            str_of_cal: str = num_filter_char_list[0] + num_filter_char_list[-1]
            int_of_cal: int = int(str_of_cal)
            cum_sum += int_of_cal
    return cum_sum

def read_str_to_digit(string:str):
    return_string:str = string
    strToInt_dict: dict[str, str] = {
        "one": "1",
        "two": "2",
        "three": "3",
        "four": "4",
        "five": "5",
        "six": "6",
        "seven": "7",
        "eight": "8",
        "nine": "9"
    }
    for char_idx in range(len(return_string)):
        if string[char_idx].isdigit():
            break
        for key in strToInt_dict:
            test_str = string[char_idx : min(char_idx + len(key), len(string) - 1)]
            if (key == test_str):
                # print("replace:", test_str, "->", strToInt_dict[key])
                return_string = string.replace(key, strToInt_dict[key], 1)
                break
            else:
                continue
        if (key == test_str):
            break
        else:
            continue
    for char_idx in range(len(return_string)):
        if return_string[::-1][char_idx].isdigit():
            break
        for key in strToInt_dict:
            test_str = string[::-1][char_idx : min(char_idx + len(key), len(string) - 1)]
            # print(test_str.strip(), ":", key[::-1] )
            if (key[::-1] == test_str):
                # print("replace:", test_str, "->", strToInt_dict[key])
                return_string = return_string[::-1].replace(key[::-1], strToInt_dict[key][::-1], 1)[::-1]
                # print(return_string)
                break
            else:
                continue
        if (key[::-1] == test_str):
            break
        else:
            continue

    return return_string

def func2(path: str):
    cum_sum: int = 0
    with open(path) as i_file:
        for line in i_file.readlines():
            char_list: list[str] = read_str_to_digit(line)
            # print("DEBUG:", char_list)
            num_filter_char_list: list[str] = list(filter(str.isdigit, char_list))
            str_of_cal: str = num_filter_char_list[0] + num_filter_char_list[-1]
            # print(str_of_cal)
            int_of_cal: int = int(str_of_cal)
            cum_sum += int_of_cal
    return cum_sum

if __name__=="__main__":
    problem1_result:int = func1(PATH)
    print(problem1_result)
    problem2_result:int = func2(PATH)
    print(problem2_result)