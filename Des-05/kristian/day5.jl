filename = "test1.txt"
filename = "input.txt"
seeds = parse.(Int64, split(readline(filename)[8:end], ' '))

lines = readlines(filename)

struct RangeMap
    sourceStart::Int
    destStart::Int
    rangelength::Int
end
maps = []
finished = false
i = 4
inputlength = length(lines)
while !finished
    rangemaps = []

    while i <= inputlength && !isempty(lines[i])
        vec = parse.(Int64, split(lines[i]))
        destStart = vec[1]
        sourceStart = vec[2]
        rangelength = vec[3]
        fn(n::Int) = ( sourceStart <= n < sourceStart + rangelength  ) ? (destStart + n - sourceStart) : n
        rmap = RangeMap(sourceStart, destStart, rangelength)
        push!(rangemaps, rmap)

        i += 1
    end
    function bigmap(n)
        mapped = false
        m = NaN
        for rangemap in rangemaps
            if n in rangemap.sourceStart:(rangemap.sourceStart + rangemap.rangelength -1)
                m = (rangemap.destStart + n - rangemap.sourceStart)
                mapped = true
                break
            end
        end
        if !mapped
            m = n
        end
        return m
    end
    push!(maps, bigmap)
    i += 2
    if i > inputlength
        finished = true
    end
end

function totalMap(n::Int)
    for map in maps
        n = map(n)
    end
    return n
end


@time totalMap(79)

seednumbers = length(seeds)
locations = zeros(length(seeds))
locations = totalMap.(seeds)
minimumlocation = minimum(locations)
minimumlocation


#part 2--------------------------------------------------------
# minLoc = Inf
# for i in 1:2:seednumbers
#     ranstart = seeds[i]
#     ranend = ranstart + seeds[i+1]
#     minLoc = min(minLoc, minimum(totalMap.(ranstart:ranend)))
# end
# minLoc
function mapRangeToLayer(inputrange, layer, mappedranges)
    mapped = false
    for (i, rm) in enumerate(layer)
        # println("Mapping $i")
        destStart = rm[1]
        sourceStart = rm[2]
        mapLength = rm[3]
        startin = (sourceStart <= inputrange[1] < (sourceStart + mapLength))
        endin = (sourceStart <= (inputrange[1] + inputrange[2]-1) < (sourceStart + mapLength))
        if startin && endin #hele interval inni
            fromstart = inputrange[1] - sourceStart
            mappedstart = destStart + fromstart
            mappedrange = [mappedstart, inputrange[2]]
            push!(mappedranges, mappedrange)
            mapped = true
        elseif startin && !endin
            fromstart = inputrange[1] - sourceStart
            mappedstart = destStart + fromstart
            mappedlength = mapLength-fromstart
            mappedrange = [mappedstart, mappedlength]
            push!(mappedranges, mappedrange)
            # map leftovers
            rangeleft = [inputrange[1] + mappedlength, inputrange[2]- mappedlength]
            mappedranges = mapRangeToLayer(rangeleft, layer, mappedranges)
            mapped = true
        elseif !startin && endin
            mappedstart = destStart
            rangeleft = [inputrange[1], sourceStart - inputrange[1]]
            mappedlength = inputrange[2] - rangeleft[2]
            mappedrange = [mappedstart, mappedlength]
            push!(mappedranges, mappedrange)
            mappedranges = mapRangeToLayer(rangeleft, layer, mappedranges)
            mapped = true
        elseif !startin && !endin
            if ( inputrange[1]< sourceStart ) && ( (inputrange[1] + inputrange[2]-1) > (sourceStart + mapLength-1) )
                #range is covering rangemap
                mappedrange = [destStart, mapLength]
                push!(mappedranges, mappedrange)
                rangeleft = [inputrange[1], sourceStart - inputrange[1]]
                mappedranges = mapRangeToLayer(rangeleft, layer, mappedranges)
                rangeleft = [sourceStart + mapLength , inputrange[2]- rangeleft[2] - mapLength]
                mappedranges = mapRangeToLayer(rangeleft, layer, mappedranges)
                mapped = true
            end
        end                
    end
    if !mapped
        # inputrange in Set( mappedranges) ? println("input $inputrange is in mappedranges, but is also pushed!") : println("mapping $inputrange to itself")
        push!(mappedranges, inputrange)
    end
    return unique(mappedranges)
end


layers = []
i = 4
finished = false
while !finished
    ranges = []
    
    while i <= inputlength && !isempty(lines[i])
        vec = parse.(Int64, split(lines[i]))
        push!(ranges, vec)
        i += 1
    end
    push!(layers, ranges)
    i += 2
    if i > inputlength
        finished = true
    end
end 
  

seedranges = []
for i in 1:2:length(seeds)
    push!(seedranges, [seeds[i], seeds[i+1]])
end
mappedranges = seedranges
for layer in layers
    inputranges = mappedranges
    mappedranges = []
    for inputrange in inputranges
        mappedranges = mapRangeToLayer(inputrange, layer, mappedranges)
    end
end
sort!(mappedranges)
firstloc = mappedranges[1][1]

# test med range med lengde 1
inputrange = [5, 3] #5, 6, 7
layer = [[0, 6, 1]] # maps to 0
sol = mapRangeToLayer(inputrange, layer, [])

#test stikker ut høyre
@time mapRangeToLayer([5, 3], [[104, 4, 3]], [])

#test stikker ut venstre
inputrange = [10,10]
rm = [0,15,5]
mapRangeToLayer([10,10], [[0, 15,5]], [])

#test inni range
inputrange = [5, 3]
rm = [100, 3, 20]
mapRangeToLayer(inputrange, [rm], [])