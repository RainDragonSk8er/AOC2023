fn = "test1.txt"
fn = "input.txt"

lines = readlines(fn)

# readuntil(fn, ' ')

blocks = []

block = []
i = 1
while i <= length(lines)
    line = lines[i]
    if isempty(line)
        push!(blocks, reduce(vcat, permutedims.(collect.(block))))
        block = []
        i+=1
    else
        push!(block, line)
        i += 1

    end
end
push!(blocks, reduce(vcat, permutedims.(collect.(block))))
blocks

tot = 0

for block in blocks
    rowreflection = FindReflection(block)
    colreflection = FindReflection(permutedims(block, [2,1]))
    tot += 100*rowreflection
    tot += colreflection
end
tot

function FindReflection(block)
    m, n = size(block)
    for i in 1:(m-1)
        if block[i, :] == block[i+1, :]
            reflectionlength = min(i, m-i)
            if block[i+1-reflectionlength:i, :] == reverse(block[i+1:i+reflectionlength, :], dims=1)
                return i
            end
        end
    end
    return 0
end
function FindReflections(block)
    rows = []
    m, n = size(block)
    found = false
    for i in 1:(m-1)
        if block[i, :] == block[i+1, :]
            reflectionlength = min(i, m-i)
            if block[i+1-reflectionlength:i, :] == reverse(block[i+1:i+reflectionlength, :], dims=1)
                push!(rows, i)
                found = true
            end
        end
    end
    return rows
end

#part 2
tot = 0

for (b, block) in enumerate(blocks)
    rowreflection = FindReflection(block)
    colreflection = FindReflection(permutedims(block, [2,1]))
    m, n = size(block)
    firstpair = [rowreflection, colreflection]
    foundnewreflection = false
    for i in 1:m
        for j in 1:n
            M = copy(block)
            if block[i,j] == '.'
                M[i,j] = '#'
            else
                M[i,j] = '.'
            end
            newrowreflections = FindReflections(M)
            newcolreflections = FindReflections(permutedims(M, [2,1]))

            newrowreflection = setdiff(newrowreflections, rowreflection)
            newcolreflection = setdiff(newcolreflections, colreflection)

            newrowreflection = isempty(newrowreflection) ? 0 : newrowreflection[1]
            newcolreflection = isempty(newcolreflection) ? 0 : newcolreflection[1]

            newpair = [newrowreflection, newcolreflection]
            if newpair == [0,0] || newpair==firstpair
                continue
            end
            foundnewreflection = true
            println("b:$b orig.: $firstpair, new: $newpair, i=$i, j=$j")
            if newpair[1] == 0 || newpair[2] == 0
                tot += 100*newpair[1] + newpair[2]
            elseif firstpair[1] != 0#if original rowreflection, use the colreflection
                tot += newpair[2]
            else
                tot += 100*newpair[1] #if original colreflection, use rowreflection
            end
            if foundnewreflection
                break
            end
        end
        if foundnewreflection
            break
        end       
    end
    if !foundnewreflection
        println("b:$b orig.: $firstpair, new: $newpair, i=$i, j=$j ERROR")
    end

end
tot