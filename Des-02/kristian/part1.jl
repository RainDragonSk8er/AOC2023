lines = readlines("input.txt")
tot = 0
for game in lines
    possiblegame = true
    split1 = split(game, ':')
    id = parse(Int, filter(isdigit, split1[1]))
    hands = split(split1[2], ';')
    for hand in hands
        maxreds, maxgreens, maxblues = 12, 13, 14
        gamestats = Dict("blue" => 0, "red" => 0, "green" => 0)
        colors = split(hand, ',')
        for entry in colors
            num = parse(Int, filter(isdigit, entry))
            color = filter(isletter, entry)
            gamestats[color] += num
        end
        if gamestats["blue"] > maxblues || gamestats["red"] > maxreds || gamestats["green"]  > maxgreens
            possiblegame = false
        end
    end
    if possiblegame
        tot+=id
    end
end
println(tot)