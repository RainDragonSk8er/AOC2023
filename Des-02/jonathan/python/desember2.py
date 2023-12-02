
PATH: str = "Des2/jonathan/input/puzzle_input.txt"
TEST_PATH_1: str = "Des2/jonathan/input/test1_input.txt"
TEST_PATH_2: str = "Des2/jonathan/input/test2_input.txt"

LIMITS_DICT_1: dict[str, int] = {
    "red": 12,
    "green": 13,
    "blue": 14
}

def product(int_list: list[int]):
    return_product: int = 1
    for val in int_list:
        return_product *= val
    return return_product

class game_results:
    game_id: int 
    game_states: dict[int, dict[str, int]]
    def __init__(self, game_id: int, s_game_nr: int):
        self.game_id: int = game_id
        self.__post_init__(s_game_nr)

    def __post_init__(self, s_game_nr):
        self.set_game_state(s_game_nr)
        self.set_power()
    
    
    def set_game_state(self, s_game_nr):
        if s_game_nr < 1:
            self.game_states: dict[int, dict[str, int]] = {
                0: {
                    "green": 0,
                    "red": 0,
                    "blue": 0
                }
            }
        else:
            self.game_states: dict[int, dict[str, int]] = {
                game_nr: {
                    "green": 0,
                    "red": 0,
                    "blue": 0
                } for game_nr in range(s_game_nr)
            }

    def set_power(self):
        min_colour_dict: dict[str, int] = {}
        for colour in self.game_states[0].keys():
            min_colour_dict[colour] = max([self.game_states[s_game][colour] for s_game in self.game_states.keys()])
        self.power: int = product(min_colour_dict.values()) 
        
def check_game(limit_dict: dict[str, int], results: game_results):
    for s_game in results.game_states.keys():
        for key in limit_dict.keys():
            if results.game_states[s_game][key] > limit_dict[key]:
                return False
    return True

def func1(path: str, limits_dict: dict[str, int]):
    return_sum: int = 0
    with open(path) as i_file:
        for line in i_file.readlines():
            game_n_sgames: list[str] = line.strip().split(":")
            line_id: int = int(game_n_sgames[0].strip().split(" ")[-1])
            s_games: list[str] = game_n_sgames[-1].strip().split(";")
            line_results: game_results = game_results(line_id, len(s_games))
            for s_game_idx, s_game in enumerate(s_games):
                for result in s_game.split(","):
                    value_key: list[str] = result.strip().split(" ")
                    key: str = value_key[-1]
                    value:int = int(value_key[0])
                    line_results.game_states[s_game_idx][key] = value
            if check_game(limit_dict=limits_dict, results=line_results):
                return_sum += line_results.game_id
    return return_sum

def func2(path: str):
    return_product_sum: int = 0
    with open(path) as i_file:
        for line in i_file.readlines():
            game_n_sgames: list[str] = line.strip().split(":")
            line_id: int = int(game_n_sgames[0].strip().split(" ")[-1])
            s_games: list[str] = game_n_sgames[-1].strip().split(";")
            line_results: game_results = game_results(line_id, len(s_games))
            for s_game_idx, s_game in enumerate(s_games):
                for result in s_game.split(","):
                    value_key: list[str] = result.strip().split(" ")
                    key: str = value_key[-1]
                    value:int = int(value_key[0])
                    line_results.game_states[s_game_idx][key] = value
            line_results.set_power()
            return_product_sum += line_results.power
    return return_product_sum



if __name__=="__main__":
    my_game:game_results = game_results(2, 1)
    my_game.game_states = {
            0: {
                "green": 2,
                "red": 2,
                "blue": 2
            }
    }
    print("Example game:", my_game.game_states)
    my_game.set_power()
    print("Example powers:", my_game.power)

    game1_sum: int = func1(PATH, LIMITS_DICT_1)
    print("For puzzle 1 the sum of correct games is:", game1_sum)

    game2_sum: int = func2(PATH)
    print("For puzzle 2 the product sum of all games:", game2_sum)

