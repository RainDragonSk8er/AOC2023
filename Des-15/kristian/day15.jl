fn = "input.txt"
fn = "test1.txt"
fn = "test2.txt"

function HASH(s)
    result = 0
    for ch in s
        val = Int(ch)
        result += val
        result = result*17
        result = mod(result, 256)
    end
    return result
end
lines = readlines(fn)

#M = reduce(vcat, permutedims.(collect.(lines)))
tot = 0
line = lines[1]
parts = split(line, ',')
for part in parts
    result = 0
    # for ch in part
    #     val = Int(ch)
    #     result += val
    #     result = result*17
    #     result = mod(result, 256)
    # end
    result = HASH(part)
    println("$part -> $result")
    tot += result
end
tot

#part2
fn = "input.txt"
fn = "test1.txt"
fn = "test2.txt"
lines = readlines(fn)

line = lines[1]
parts = split(line, ',')

boxes = Vector{Any}(undef, 256)
for i in 0:255
    boxes[i+1] = [[],[]]
end
for part in parts
    println("part: $part")
    result = 0
    i = 1
    while part[i] ∉ ['=', '-']
        i+=1
    end
    label = part[1:i-1]
    println("Label: $label")
    boxnum = HASH(label) +1
    println("Boxnum: $boxnum")

    if part[i] == '='
        FL = parse(Int, part[i+1:end])
        println("FL: $FL")
        j = findfirst(x -> x==label, boxes[boxnum][1])

        if isnothing(j)
            push!(boxes[boxnum][1], label)
            push!(boxes[boxnum][2], FL)
        else
            boxes[boxnum][2][j] = FL
        end
    
    elseif part[i] == '-'
        j = findfirst(x -> x==label, boxes[boxnum][1])
        if !isnothing(j)
            deleteat!(boxes[boxnum][1], j)
            deleteat!(boxes[boxnum][2], j)
        end
    end
end

tot = 0
for (boxi, box) in enumerate(boxes)
    if !isempty(box[2])
        for (slot, FL) in enumerate(box[2])
            FP = boxi*slot*FL
            tot += FP
        end
    end
end
tot