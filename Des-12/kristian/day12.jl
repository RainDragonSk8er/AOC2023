fn = "input.txt"
fn = "test1.txt"
lines = readlines(fn)

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
    return tot
end

tot = 0
@time for (l, line) in enumerate(lines)
    println(l)
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

using Profile
using PProf
Profile.clear()
@profile FindCombinations(lines, multiplier=2)
pprof()
#springs = filter(x-> !isempty(x), split(parts[1], '.'))