using Memoization

@memoize function gofast(springs, groups)
    result = 0

    if isempty(groups)
        if '#' ∉ springs
            return 1
        else
            return 0
        end
    end
    if isempty(springs)
        return 0
    end

    character = springs[1]
    if character == '#'
        result = sharp(springs, groups)
    elseif character == '.'
        result = dot(springs, groups)
    elseif character == '?'
        result = dot(springs, groups) + sharp(springs, groups)
    else
        error("weir character")
    end
    return result
end

function sharp(springs, groups)
    grouplength = groups[1]

    currentgroup = springs[1:grouplength]
    currentgroup = replace(currentgroup, '?' => '#')

    if currentgroup != '#'^grouplength
        return 0
    elseif length(springs) == grouplength
        if length(groups) == 1
            return 1
        else 
            return 0
        end
    elseif springs[grouplength+1] != '#'
        return gofast(springs[grouplength+2:end], groups[2:end])
    else
        return 0
    end
end
function dot(springs, groups)
    return gofast(springs[2:end], groups)
end

fn = "test1.txt"
lines = readlines(fn)
multiplier = 0
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
    springs = parts[1]
    groups = config
    result = gofast(springs, groups)
    tot += result
end