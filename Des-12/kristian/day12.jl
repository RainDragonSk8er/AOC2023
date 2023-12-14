using Combinatorics
using Memoization

#M = reduce(vcat, permutedims.(collect.(lines)))
function CheckSprings(springs, config)
    groups = filter(x-> !isempty(x), split(springs, '.'))
    grouplengths = length.(groups)
    answer = false
    if length(grouplengths) != length(config)
        answer =  false
    elseif config == grouplengths
        answer =  true
    else
        answer =  false
    end
    return answer
end
function FindCombinations(lines; multiplier=0)
    tot = 0
    for (l, line) in enumerate(lines)
        # println(l)
        parts = split(line, ' ')
        if multiplier > 0
            parts[1] = ((parts[1]*'?')^multiplier)[1:end-1]
            parts[2] = ((parts[2]*',')^multiplier)[1:end-1]
        end
        config = parse.(Int64, split(parts[2], ','))
        numhashtags = sum(collect(parts[1]) .== '#')
        totalsprings = sum(config)
        unknowns = length(filter(x -> x =='?', parts[1]))
        qpos  = collect(parts[1]) .== '?'
        
        numberOfNewHashes = totalsprings - numhashtags
        s = ("1"^numberOfNewHashes)*("0"^(unknowns-numberOfNewHashes))
        icomb = 0
        for combinationvec in multiset_permutations(s, length(s))
            icomb += 1
            # println(combinationvec)
            combString = join(combinationvec)
            # combination = digits(i-1, base=2, pad=unknowns) |> reverse
            combination = digits(parse(Int, combString, base=2), base=2, pad=length(s))
            if sum(combination) !== (totalsprings - numhashtags)
                continue
            end
            hashtags, dots = copy(qpos), copy(qpos)
            # println("$hashtags, $qpos, $combination")
            hashtags[qpos] = combination
            dots[qpos] = (combination .==0)
            vector = collect(parts[1])
            vector[hashtags] .= '#'
            vector[dots] .= '.'
            springs = join(vector)
            valid = CheckSprings(springs, config)
            # if valid
            #     println("valid: i =$icomb, $springs")
            # end
            tot += valid
        end
    end
    return tot
end

fn = "input.txt"
fn = "test1.txt"


lines = readlines(fn)
tot = 0
@time for (l, line) in enumerate(lines)
    # println(l)
    parts = split(line, ' ')
    config = parse.(Int64, split(parts[2], ','))
    numhashtags = sum(collect(parts[1]) .== '#')
    totalsprings = sum(config)
    unknowns = length(filter(x -> x =='?', parts[1]))
    qpos  = collect(parts[1]) .== '?'
    for i in 1:2^unknowns
        combination = digits(i-1, base=2, pad=unknowns) |> reverse
        if sum(combination) !== (totalsprings - numhashtags)
            continue
        end
        hashtags, dots = copy(qpos), copy(qpos)
        hashtags[qpos] = combination
        dots[qpos] = (combination .==0)
        vector = collect(parts[1])
        vector[hashtags] .= '#'
        vector[dots] .= '.'
        springs = join(vector)
        valid = CheckSprings(springs, config)
        # if valid
        #     println("valid: i =$i, $springs")
        # end
        tot += valid
    end
end
tot


@time FindCombinations(lines, multiplier=3)

#springs = filter(x-> !isempty(x), split(parts[1], '.'))