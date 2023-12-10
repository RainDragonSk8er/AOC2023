lines = readlines("input.txt")
for i in eachindex(lines)
    lines[i] = string('.', lines[i], '.')
end

pushfirst!(lines, join(['.' for i in 1:142]))
push!(lines, join(['.' for i in 1:142]))



pattern = integer_pattern = r"\d+"
tot = 0
for i in 2:141
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