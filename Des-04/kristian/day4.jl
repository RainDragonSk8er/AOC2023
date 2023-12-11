fn = "test1.txt"
fn = "input.txt"
lines = readlines(fn)
L = length(lines)

tot = 0
winningnumbers = zeros(Int64, L)
for (i, line) in enumerate(lines)
    info = split(line, ':')[2]
    parts = split(info, '|')
    winnings = parse.(Int64, split(parts[1]))
    mynumbers = parse.(Int64, split(parts[2]))
    winningnumbers[i] = in.(mynumbers, Ref(winnings)) |> sum
    if winningnumbers[i] > 0
        tot += 2^(winningnumbers[i]-1)
    end
end
tot

#part 2

instances = ones(Int64, length(lines))
for i in 1:L
    winnum = winningnumbers[i]
    if winnum > 0
        instances[i+1:i+winnum] .+= 1*instances[i]
    end
end
sum(instances)