lines = readlines("input.txt")
numbers = zeros(Int8, length(lines))
zero, one, two, three, four, five, six, seven, eight, nine = "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine"
words_to_char = Dict(
        "one" => '1', 
        "two" => '2', 
        "three" => '3', 
        "four" => '4', 
        "five" => '5',
        "six" => '6', 
        "seven" => '7', 
        "eight" => '8', 
        "nine" => '9'
    )
for (i, line) in enumerate(lines)
    digs = filter(isdigit, line)
    firstdigindx = findfirst(digs[1], line)
    lastdigindx = findlast(digs[end], line)
    pattern = Regex("($zero|$one|$two|$three|$four|$five|$six|$seven|$eight|$nine)")
    matches = eachmatch(pattern, line, overlap=true) 
    indices = [(words_to_char[m.match], m.offset) for m in matches]
    push!(indices, (digs[1], firstdigindx))
    push!(indices, (digs[end], lastdigindx))
    sort!(indices, by=x -> x[2])

    num1 = indices[1][1]
    num2 = indices[end][1]
    num = parse(Int, num1*num2)
    numbers[i] = num
end
tot = sum(numbers)