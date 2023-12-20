using DataStructures
# using AStarSearch
# using Memoization

function SetupNodes(fn)
    lines = readlines(fn)
    nodes = Dict() #name -> [type, downstreamneighbors, upstreamneighbors, state]. upstream:dict(preneighbor -> mem)

    for i in 1:2
        for line in lines[1:end]
            node, nbs = split(line, " -> ")
            neighbors = split(nbs, ", ")
            type = node[1] #broadcaster getrs type b
            name = node[2:end]
            name == "roadcaster" ? name = "broadcaster" : nothing
            for nb in neighbors
                if haskey(nodes, nb)
                    nodes[nb][memix][name] = lowPulse
                end
            end
            if !haskey(nodes, name)
                nodes[name]= [type, neighbors, Dict{Any, Bool}(), 0]
            end
        end
    end
    return nodes
end

function SendPulse(nodes;stopon= (x,y) -> false)
    highs = 0
    lows = 1
    pulses = Deque{Tuple}()
    pushfirst!(pulses, ("broadcaster", 0))
    while !isempty(pulses)
        pulse = popfirst!(pulses)
        # if pulse == stopon
        #     return -1, -1
        # end
        from = pulse[1]
        pulseType = pulse[2]
        node = nodes[from]
        
        for nb in node[nbix]#[end:-1:1]
            debug ? println("$from -> $pulseType -> $nb") : nothing
            pulseType == 1 ? highs+=1 : lows += 1
            #que pulse to nb, to start of deque
            
            if !haskey(nodes, nb)
                continue
            elseif nodes[nb][typeix] == '%'
                if pulseType == highPulse
                    continue
                elseif nodes[nb][stateix] == 1#on, dont send pulse
                    nodes[nb][stateix] = 0
                    sendpulse = lowPulse
                else
                    nodes[nb][stateix] = 1
                    sendpulse = highPulse
                end
            elseif nodes[nb][typeix] == '&'
                nodes[nb][memix][from] = pulseType
                # debug ? println("$nb memory now $(nodes[nb][memix][target])") : nothing
                # nodes["ql"][memix][target]["mf"] == 1 ? println("$nb memory now $(nodes[nb][memix][target])") : nothing
                if all(values(nodes[nb][memix]) .== 1)#if all in memory are high, send low
                    sendpulse = lowPulse
                else
                    sendpulse = highPulse #else send high
                end
            end
            newpulse = (nb, sendpulse)
            if stopon(nodes, newpulse)
                return -1, -1, nodes
            end
            # debug ? println("Pushing pulse $newpulse") : nothing
            push!(pulses, newpulse)
        end
    end
    return highs, lows, nodes
end
fn = "input.txt"
fn = "test1.txt"
fn = "test2.txt"
lines = readlines(fn)
debug = false
#M = reduce(vcat, permutedims.(collect.(lines)))
nbix = 2
typeix = 1
memix = 3
stateix = 4
lowPulse = 0
highPulse = 1
# nodes= SetupNodes(fn)
# @run SendPulse(("", 2))

tothi = 0
totlo = 0
numpulses = 1000
nodes = SetupNodes(fn)
for i in 1:numpulses
    hi, lo, nodes = SendPulse(nodes)
    tothi += hi
    totlo += lo
end
tothi*totlo

#part 2 brute force
j = 0
nodes = SetupNodes(fn)
hi = 0
# stopfn = (n, p) -> p == ("rx", 0)
target = "sn"
from = "tn"
mem = copy(nodes[target][memix][from])
maxj = 100000
switches = 0
changesat = [0]
while hi != -1 && j< maxj && switches < 30
    j+=1
    hi, lo, nodes = SendPulse(nodes)
    newmem = copy(nodes[target][memix][from])
    if newmem != mem
        println("j:$j, target:$(nodes[target][memix])")
        switches +=1
        push!(changesat, j)
    end
    mem = copy(newmem)
end
diff(changesat)
unique(diff(changesat))
j
##
nodes = SetupNodes(fn)
n = 1024
for i in 1:n
    hi,lo,nodes = SendPulse(nodes)
    # println(nodes[target][memix][target])
end
println(nodes[target][memix])


#part 2 smart??
nodes = SetupNodes(fn)
# hi, lo = SendPulse(nodes)
target = "lr"
upstream = keys(nodes[target][memix])
done = false
maxi = 10000
i = 0
cycles = []
for up in upstream
    println(up)
    nodescopy = deepcopy(nodes)
    done = false
    i=0
    while !done && i< maxi
        i+=1
        hi, lo, nodescopy = SendPulse(nodescopy)
        if nodescopy[target][memix][up] == 1
            done = true
            push!(cycles, i)
        end
    end
    println("finished with $up. Done:$done, i:$i")
end
cycles
foldl(lcm, cycles)

#-------
god = []
nodes = SetupNodes(fn)
target = "ql"
upstream = keys(nodes[target][memix])
maxj = 100000
list = []
for up in upstream
    switches = 0
    changesat = []
    j=0
    mem = copy(nodes[target][memix][up])
    while  j < maxj && switches < 2
        j+=1
        hi, lo, nodes = SendPulse(nodes)
        newmem = copy(nodes[target][memix][up])
        if newmem != mem
            println("j:$j, target:$(nodes[target][memix])")
            switches +=1
            push!(changesat, j)
        end
        mem = copy(newmem)
    end
    push!(list, changesat)
end
target
list
starts = [el[1] for el in list]
n = foldl(lcm, starts)
push!(god, n)
anspls = foldl(lcm, god)


#----------
nodes = SetupNodes(fn)
debug = false
stopfn = (n,p) -> p == ("ss", 1)
maxi = 10000
hi = 0
i = 0
while hi!=-1 && i < maxi
    i+=1
    hi, lo, nodes = SendPulse(nodes, stopon=stopfn)
end
i

foldl(lcm, [3847, 3761, 3793, 3881])