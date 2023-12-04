import re
import numpy as np

PATH: str = "Des-04/jonathan/input/puzzle_input.txt"
TEST_PATH_1: str = "Des-04/jonathan/input/test1_input.txt"
TEST_PATH_2: str = "Des-04/jonathan/input/test2_input.txt"

class ScratchCard:
    card_id: int
    winNum_list: list[str]
    cardNum_list: list[str]
    points: int
    matchingNums: int
    def __init__(self, i_card_id: int, i_winNum_list: list[str], i_cardNum_list: list[str]) -> None:
        self.card_id = i_card_id
        self.winNum_list = i_winNum_list
        self.cardNum_list = i_cardNum_list
        self.points = 0
        self.matchingNums = 0
    def calculatePionts(self):
        for my_number in self.cardNum_list:
            if my_number in self.winNum_list:
                if self.points == 0:
                    self.points = 1
                else:
                    self.points *= 2
    def caluculateWonCopies(self):
        for my_number in self.cardNum_list:
            if my_number in self.winNum_list:
                self.matchingNums += 1
    def __str__(self) -> str:
        out_str: str = f"Card: {self.card_id}; Points: {self.points}; Matches: {self.matchingNums}"
        return out_str

def read_in_chunks(file_object, chunk_size=1024):
    """Lazy function (generator) to read a file piece by piece.
    Default chunk size: 1k."""
    while True:
        data = file_object.read(chunk_size)
        if not data:
            break
        yield data
 

def func1(path: str):
    sum: int = 0
    with open(path, "r") as i_file:
        line_length = len(i_file.readline())
    with open(path, "r") as i_file:
        generator = read_in_chunks(i_file, line_length)
        while generator:
            try:
                line = next(generator) 
            except StopIteration:
                break
            card_id: int = int(line.split(":")[0].split(" ")[-1].strip())
            winner_nums: list[str] = list(filter(lambda x: x.strip(), line.split("|")[0].split(":")[-1].strip().split(" ")))
            card_nums: list[str] = list(filter(lambda x: x.strip(), line.split("|")[-1].strip().split(" ")))
            
            sCard = ScratchCard(card_id, winner_nums, card_nums)
            sCard.calculatePionts()
            # print(sCard)
            sum += sCard.points
            # print(card_id, ":", winner_nums, "@", card_nums)
    return sum

def func2(path):
    my_cards: dict[int, int] = {}
    with open(path, "r") as i_file:
        line_length = len(i_file.readline())
    with open(path, "r") as i_file:
        generator = read_in_chunks(i_file, line_length)
        while generator:
            try:
                line = next(generator) 
            except StopIteration:
                break
            card_id: int = int(line.split(":")[0].split(" ")[-1].strip())
            winner_nums: list[str] = list(filter(lambda x: x.strip(), line.split("|")[0].split(":")[-1].strip().split(" ")))
            card_nums: list[str] = list(filter(lambda x: x.strip(), line.split("|")[-1].strip().split(" ")))
            sCard = ScratchCard(card_id, winner_nums, card_nums)
            sCard.caluculateWonCopies()
            
            if sCard.card_id in my_cards:
                my_cards[sCard.card_id] += 1
            else:
                my_cards[sCard.card_id] = 1     
            # print(sCard)
            for copies in range(my_cards[sCard.card_id]):
                for i in range(sCard.matchingNums):
                    if sCard.card_id + i + 1 not in my_cards:
                        my_cards[sCard.card_id + i + 1] = 0    
                    my_cards[sCard.card_id + i + 1] += 1
                    # print(sCard.card_id, ":", sCard.card_id + i + 1)
            
            # print(card_id, ":", winner_nums, "@", card_nums)
    return my_cards

if __name__=="__main__":
    answer1 = func1(PATH)
    print("This is answer 1:", answer1)
    answer2 = func2(PATH)
    print("This is answer 2:", sum(answer2.values()))