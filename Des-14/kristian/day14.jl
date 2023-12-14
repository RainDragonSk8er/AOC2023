
function moverocks(lines)
    for (i, line) in enumerate(lines[1:end])
        if i == 1
            continue
        end
        for (j, ch) in enumerate(line)
            if ch == 'O'
                # println("i=$i, j=$j")
                upi = i
                while upi > 1 && lines[upi-1][j] == '.'
                    upi -= 1
                end
                # println("upi=$upi")
                if lines[upi][j] == '.'
                    # println("moves rock! j=$j, i=$i, to $upi")
                    lines[i] = lines[i][1:j-1]*'.'*lines[i][j+1:end]
                    lines[upi] = lines[upi][1:j-1]*'O'*lines[upi][j+1:end]
                end
            end
        end
    end
    return lines
end
function loadcalc(lines)
    tot = 0
    L = length(lines)
    for (i, line) in enumerate(lines)
        for (j, ch) in enumerate(line)
            if ch == 'O'
                tot += L-i+1
            end
        end
    end
    return tot
end



fn = "input.txt"
fn = "test1.txt"

lines = readlines(fn)
oldlines = lines
newlines = moverocks(oldlines)
while newlines != oldlines
    println("moves again!")
    oldlines = newlines
    newlines = moverocks(oldlines)
end
newlines
loadcalc(newlines)


#part 2
fn = "input.txt"
fn = "test1.txt"

lines = readlines(fn)
cycles = 3
cycles = 1_000_000_000
@time bruteforcecycle(lines, 100_001)




function bruteforcecycle(lines, cycles)
    rotations = 4*cycles
    oldlines = lines
    for c in 1:cycles
        for r in 1:4
            lines = moverocks(lines)
            M = reduce(vcat, permutedims.(collect.(lines)))
            M = rotr90(M)
            lines = [join(M[row, :]) for row in 1:size(M,1)]
            # println(oldlines)
        end
        # println("lines:$lines \n oldlines=$oldlines")
        if lines == oldlines
            println("no change after $c cycles")
            return lines
        end
        oldlines = copy(lines)
        # println("$oldlines")
    end
    return lines
end