lines = readlines("input.txt")
tot = 0
for game in lines
    split1 = split(game, ':')
    id = parse(Int, filter(isdigit, split1[1]))
    hands = split(split1[2], ';')
    gamestats = Dict("blue" => 0, "red" => 0, "green" => 0) # this is highest occurence of color in each hand in game
    for hand in hands
        colors = split(hand, ',')
        for entry in colors
            num = parse(Int, filter(isdigit, entry))
            color = filter(isletter, entry)
            gamestats[color] = max(gamestats[color], num)
        end
        
    end
    smallestpower = values(gamestats) |> collect |> prod
    tot += smallestpower
end
println(tot)