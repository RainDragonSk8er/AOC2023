# filename = "test1.txt"
filename = "input.txt"
lines = readlines(filename)

tot = 0

function completeVector(v)
    d = diff(v)
    if all(d .== 0)
        return v[1]
    else
        return v[1] - completeVector(d)
    end
end
for line in lines
    tot += completeVector(parse.(Float64, split(line, ' ')))
end
tot