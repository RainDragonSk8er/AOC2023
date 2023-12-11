fn = "test1.txt"
lines = readlines(fn)
for i in eachindex(lines)
    lines[i] = string('.', lines[i], '.')
end
L = length(lines)

pushfirst!(lines, join(['.' for i in 1:L+2]))
push!(lines, join(['.' for i in 1:L+2]))



pattern = integer_pattern = r"\d+"
tot = 0
for i in 2:L+1
    line = lines[i]
    matches = eachmatch(pattern, line)
    for match in matches
        add = false
        expidx = (match.offset-1):(match.offset + length(match.match))
        overline = lines[i-1][expidx]
        underline = lines[i+1][expidx]
        sides = lines[i][[match.offset-1, match.offset + length(match.match)]]
        surroundings = join([overline, underline, sides])
        if any(collect(surroundings) .!= '.')
            add = true
            num = parse(Int, match.match)
            global tot += num
        end

    end


end

tot