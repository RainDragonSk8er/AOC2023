function loopindex(i, n)
    if i == n
        return i
    else
        m = mod(i, n)
        return m == 0 ? n : m
    end
end
function solveproblem(filename)
    lines = readlines(filename)

    instructions = lines[1]

    global D = Dict()

    for line in lines[3:end]
        node = line[1:3]
        left = line[8:10]
        right = line[13:15]
        D[node] = (left, right)
    end
    foundzzz = false
    instructionlength = length(instructions)
    global i = 1
    global node = "AAA"
    # for (i, instruction) in enumerate(Iterators.cycle(instructions))
    #     node = D[node][(instruction == 'R') + 1]
    #     if node == "ZZZ"
    #         println("Found ZZZ! i = $i")
    #         return i
    #     end
    #     if i > 1_000_000
    #         return "TOO FAR!"
    #     end

    # end
    while  !foundzzz
        left, right = D[node]
        instructionindex = loopindex(i, instructionlength)
        # print("$instructionindex, ")
        #println("i=$i, index = $instructionindex")
        instruction = instructions[instructionindex]
        #println("$node = ($left, $right), going to  $instruction")
        if instruction == 'R'
            node = right
        else
            node = left
        end
        if node == "ZZZ"
            foundzzz = true
            println("Found ZZZ!, i = $i")
        else
            i += 1
        end
        if mod(i, 10000)==0
            println("i=$i !")
            if i == 100_000
                println("reached $i iterations!! stopping")
                foundzzz = true
            end
        end
    end
    return i + 1
end
function solveproblem2(filename;maxiter=Inf)
    lines = readlines(filename)

    instructions = lines[1]

    global D = Dict()

    for line in lines[3:end]
        node = line[1:3]
        left = line[8:10]
        right = line[13:15]
        D[node] = (left, right)
    end

    global i = 1
    global node = "AAA"
    nodes = collect(filter(x -> x[3]=='A', keys(D)))
    for (i, instruction) in enumerate(Iterators.cycle(instructions))
        nodes = [ D[node][(instruction == 'R') + 1] for node in nodes]
        if all([node[3]=='Z' for node in nodes])
            println("Found all __Z's i = $i")
            return i
        end
        if i > maxiter
            return "TOO FAR!"
        end
    end
    return i + 1
end

filename="testinput3.txt"
filename = "input.txt"
lines = readlines(filename)

instructions = lines[1]

global D = Dict()

for line in lines[3:end]
    node = line[1:3]
    left = line[8:10]
    right = line[13:15]
    D[node] = (left, right)
end

global i = 1
global node = "AAA"
startnodes = collect(filter(x -> x[3]=='A', keys(D)))

cycles = zeros(Int, length(startnodes))
L = length(instructions)
for (isn, startnode) in enumerate(startnodes)
    node = startnode
    for (i, instruction) in enumerate(Iterators.cycle(instructions))
        node = D[node][(instruction == 'R') + 1]
        if node[3] == 'Z'
            println("Startnode: $startnode, Z-node: $node ! i = $i")
            cycles[isn] = i
            break
        end
        if node == startnode
            println("reached startnode, i = $i")
        end
        if mod(i, L)==1 && node == startnode
            println("Startnode: $startnode Full cycle @ i = $i")
            break
        end
        if i > 100_000
            return "TOO FAR!, i = $i"
        end
    
    end
end

cycles
lcm(cycles...)


@time solveproblem("testinput1.txt")
@time solveproblem("testinput2.txt")
@time solveproblem("input.txt")

#B
@time solveproblem2("testinput3.txt")
@time solveproblem2("input.txt")