from collections import Counter

letter_map = {"T": "A", "J": "B", "Q": "C", "K": "D", "A": "E"}


def classify(hand):
    count = Counter(hand).values()

    if 5 in count:
        return 6
    elif 4 in count:
        return 5
    elif 3 in count and 2 in count:
        return 4
    elif 3 in count:
        return 3
    elif list(count).count(2) == 2:
        return 2
    elif 2 in count:
        return 1
    return 0


def strength(hand):
    return (classify(hand), [letter_map.get(char, char) for char in hand])


plays = [(hand, int(bid)) for hand, bid in (line.split() for line in open(0))]

plays.sort(key=lambda play: strength(play[0]))

print(sum(rank * bid for rank, (_, bid) in enumerate(plays, 1)))
