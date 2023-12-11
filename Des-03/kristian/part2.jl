fn = "test1.txt"
fn = "input.txt"
lines = readlines(fn)
for i in eachindex(lines)
    lines[i] = string('.', lines[i], '.')
end
L = length(lines)

pushfirst!(lines, join(['.' for i in 1:L+2]))
push!(lines, join(['.' for i in 1:L+2]))

M = reduce(vcat, permutedims.(collect.(lines)))
multipliermatrix = ones(Int64, (L+2, L+2))
gearmatrix = 100*(M .== '*')

pattern = integer_pattern = r"\d+"
tot = 0
for i in 2:L+1
    line = lines[i]
    matches = eachmatch(pattern, line)
    for match in matches
        expidx = (match.offset-1):(match.offset + length(match.match))
        partnumber = parse(Int64, match.match)
        #do stuff on left on left
        gearmatrix[i, expidx.start] += 1 #add a number to signal adjacent partnumber
        multipliermatrix[i, expidx.start] *= partnumber
        #right
        gearmatrix[i, expidx.stop] += 1 #add a number to signal adjacent partnumber
        multipliermatrix[i, expidx.stop] *= partnumber

        #top
        gearmatrix[i-1, expidx] .+= 1 #add a number to signal adjacent partnumber
        multipliermatrix[i-1, expidx] .*= partnumber

        #bottom
        gearmatrix[i+1, expidx] .+= 1 #add a number to signal adjacent partnumber
        multipliermatrix[i+1, expidx] .*= partnumber
    end
end

gears = gearmatrix .== 102
gearcoords = findall(x->x==1, gears)
gearrations = multipliermatrix[gearcoords]
gearratiosum = sum(gearrations)