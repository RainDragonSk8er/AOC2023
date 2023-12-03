import typing
import re

PATH: str = "Des-03/jonathan/input/puzzle_input.txt"
TEST_PATH_1: str = "Des-03/jonathan/input/test1_input.txt"
TEST_PATH_2: str = "Des-03/jonathan/input/test2_input.txt"

class line_number:
    number: str
    start_idx: int
    stop_idx: int
    counted: bool
    
    def __init__(self, i_number: str, i_start_idx: int, i_stop_idx: int) -> None:
        self.number = i_number
        self.start_idx = i_start_idx
        self.stop_idx = i_stop_idx
        self.counted = False

class line_numbers:
    line_idx: int
    number_dict: dict[int, line_number]
    
    def __init__(self, i_line_idx: int) -> None:
        self.line_idx = i_line_idx
        self.number_dict = {}
    
    def add_line_number(self, number, start_idx, stop_idx):
        if self.number_dict == {}:
            number_idx:int = 0
        else:
            number_idx:int = max(self.number_dict.keys()) + 1
        self.number_dict[number_idx] = line_number(number, start_idx, stop_idx)
    def __str__(self) -> str:
        out_str: str = f"Line nr.: {self.line_idx} \n"
        for entry_id, line_number_obj in self.number_dict.items():
            out_str += f"\tEntry nr.: {entry_id}; start_idx: {line_number_obj.start_idx}; stop_idx: {line_number_obj.stop_idx}; number: {line_number_obj.number};\n"
            out_str += f"\t\tHas adjacent symbol: {'yes' if line_number_obj.counted else 'no'}\n"
        return out_str

def find_numbers_on_line(line: str, line_obj: line_numbers) -> line_numbers:
    # find numbers on line!
            
    add_string: str = ""
    start_idx: int = 0
    stop_idx: int = 0
    
    is_digit: bool = False
    last_was_digit: bool = None
    for char_idx in range(len(line)):
        # print(line[char_idx], end="")
        if str(line[char_idx]).isdigit():
            is_digit = True
            stop_idx = char_idx
            if last_was_digit == False and is_digit:
                start_idx = char_idx

            add_string += line[char_idx]
        else:
            is_digit = False
        if not is_digit and add_string != "":
            
            line_obj.add_line_number(int(add_string), start_idx, stop_idx)
            add_string = ""
            start_idx, stop_idx = 0, 0
        last_was_digit = is_digit
    return line_obj

def determine_surrounding_symbols(line_idx: int, generator: typing.Generator, line_obj: line_numbers) -> line_numbers:
    if line_idx == 0:
        line_before = None
        line_after = line_idx + 1
    elif line_idx == len(generator) - 1:
        line_before = line_idx - 1
        line_after = None
    else:
        line_before = line_idx - 1
        line_after = line_idx + 1
    search_lines: tuple[int] = (line_before, line_idx, line_after)

    # search for symbols around entries!
    for entry_nr, line_number_obj in line_obj.number_dict.items():
        for search_line_idx in search_lines:
            
            if search_line_idx == None:
                continue
            start_search_idx = max(0,line_number_obj.start_idx - 1)
            stop_search_idx = min(len(generator[search_line_idx]) - 1, line_number_obj.stop_idx + 1)
            search_slice: str = generator[search_line_idx][start_search_idx:stop_search_idx + 1] # .strip("\n")
            if line_idx in [21, 18]:
                print(f"relative searchline({line_number_obj.number}):", search_line_idx)
                print("\t search string:", search_slice, f"start_search: {start_search_idx}, stop_search: {stop_search_idx}")

            if re.fullmatch(
                r"^[\.\d]*([^\w\d\s\.]+[\.\d\n]*)+$", 
                search_slice
            ):
                line_number_obj.counted = True
                break 
    if line_idx in [21, 18]:
        print(line_obj)
    return line_obj

def func1(path: str):
    sum: int = 0
    with open(path) as i_file:
        generator: typing.Generator[str] = i_file.readlines()
        for idx, line in enumerate(generator):
            #initialize values
            this_line_numbers: line_numbers = line_numbers(idx)

            print(f"#  {idx}: {line.strip()}")
            
            # Find numbers in line
            this_line_numbers = find_numbers_on_line(line, this_line_numbers)
            # Determine if number should count
            this_line_numbers = determine_surrounding_symbols(idx, generator, this_line_numbers)
            # Add value of counted numbers to sum
            for line_object in this_line_numbers.number_dict.values():
                if line_object.counted:
                    # print(line_object.number)
                    sum += int(line_object.number)
    return sum

def read_in_chunks(file_object, chunk_size=1024):
    """Lazy function (generator) to read a file piece by piece.
    Default chunk size: 1k."""
    while True:
        data = file_object.read(chunk_size)
        if not data:
            break
        yield data

def func2(path:str) -> int:
    sum: int = 0
    with open(path, "r") as i_file:
        line_length = len(i_file.readline())
    with open(path, "r") as i_file:
        i: int = 1
        break_next: bool = False

        old_line: typing.Union[str, None] = None
        cur_line: typing.Union[str, None] = None
        next_line: typing.Union[str, None] = None
        while read_in_chunks(i_file, line_length):
            if break_next:
                break
            try:
                next_line = next(read_in_chunks(i_file, line_length))
            except StopIteration:
                next_line = None
                break_next = True 

            #Find "*" on line
            matc_nr = 0
            if cur_line != None:
                print("#old", old_line)
                print("#cur", cur_line)
                print("#new", next_line)
                for match_apostrophe in re.finditer(r'\*', cur_line):
                    matc_nr += 1
                    print(f"match_apostrophe[{matc_nr}]: '{match_apostrophe.group()}', start index: {match_apostrophe.start()}, End index: {match_apostrophe.end()}")
                    print(f"The line[{i}]:", cur_line.strip(), " | match_apostrophe:", cur_line[match_apostrophe.start():match_apostrophe.end()])
                    match_slicer = [match_apostrophe.start(), match_apostrophe.end()]
                    gear_numbers:list[str] = []
                    for j, time_line in enumerate([old_line, cur_line, next_line]):
                        if time_line != None:
                            for match_digit in re.finditer(r"\d+", time_line):
                                if (match_slicer[0] - 1 <= match_digit.start() <= match_slicer[0] + 1) or (match_slicer[0] - 1 <= match_digit.end() - 1 <= match_slicer[0] + 1): #  
                                    gear_numbers.append(match_digit.group())
                                    print(f"Found[{j-1}]:", match_digit.group())
                    print(gear_numbers)
                    if len(gear_numbers) == 2:
                        sum += int(gear_numbers[0]) * int(gear_numbers[1])
                i += 1
              
            old_line = cur_line
            cur_line = next_line
    return sum

            

if __name__=="__main__":
    # answer_1 = func1(PATH)
    # print("This is the question 1 answer:", answer_1)
    answer_2 = func2(PATH)
    print("This is the question 2 answer:", answer_2)