using DataStructures

mutable struct Hand
    cards::String
    bet::Float64
    type::Int
    values::Vector{Int8}
end
function ltHand(H1::Hand, H2::Hand)
    if H1.type == H2.type
        return H1.values < H2.values
    else
        return H1.type < H2.type
    end
end
function getValue(c;joker=false)
    # println(c)
    # println(typeof(c))
    if isdigit(c)
        return parse(Int8, c)
    elseif c == 'T'
        return 10
    elseif c == 'J'
        return joker ? 1 : 11
    elseif c == 'Q'
        return 12
    elseif c == 'K'
        return 13
    elseif c == 'A'
        return 14
    else
        error("not valid card")
    end
end
function jokerMod(cards)
    if cards == "JJJJJ"
        return "AAAAA"
    end
    jokers = length(filter(x -> x=='J', cards))
    nonJokers = filter(x -> x != 'J', cards)
    pairs = collect(counter(nonJokers))
    pairs = sort(pairs, by=x->getValue(x[1]), rev=true)
    counts = sort(pairs, by=x->x[2], rev=true)
    chosencard = counts[1][1]
    newcards = replace(cards, 'J' => chosencard)
    return newcards    
end

function getHandType(cards)
    counts = counter(cards)
    countvals = sort(collect(values(counts)), rev=true)
    differentValues = length(counts)
    if differentValues == 1
        return 7 #fem like
    elseif differentValues == 2
        if countvals[1] == 4
            return 6 #fire like
        else
            return 5 #fullt hus
        end
    elseif differentValues == 3
        if countvals[1] == 3
            return 4 #tre like
        else
            return 3 #to like   
        end
    elseif differentValues == 4
        return 2 #ett par

    else
        return 1 #høyt kort
    end
end


function getWinnings(filename;joker=false)
    lines = readlines(filename)
    
    hands = Vector{Hand}(undef, length(lines))
    for (i, line) in enumerate(lines)
        parts = split(line, ' ')
        cards = parts[1]
        handvalues = getValue.(collect(cards), joker=joker)
        if joker
            cards = jokerMod(cards)
        end
        bet = parse(Float64, parts[2])
        type = getHandType(cards)
        hand = Hand(cards, bet, type, handvalues)
        hands[i] = hand
    end
    
    sort!(hands, lt=ltHand)
    
    tot = 0
    
    for (i, hand) in enumerate(hands)
        tot += i*hand.bet
    end
    return tot
end



filename = "test1.txt"
filename = "input.txt"
getWinnings(filename, joker=true)


a = "AC223"
c = counter(a)
cc = collect(c)
scc = sort(cc, by=x->x[2], rev=true)
scc[2]